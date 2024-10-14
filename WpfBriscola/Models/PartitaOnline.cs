using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static WpfBriscola.GameValues;

namespace WpfBriscola.Models
{
    internal class PartitaOnline : Partita
    {
        public PartitaOnline(string nomeGiocatore1, string nomeGiocatore2, ControllerView controller) :base()
        {
            Giocatore1 = new Giocatore(1, nomeGiocatore1, Mazzo);
            Giocatore2 = new AIGiocatore(2, nomeGiocatore2, Mazzo);
            controllerView = controller;
        }

        internal async Task CreaPartita()
        {
            if (!OnlineSettings!.PrincipalHost)
            {
                await OnlineSettings.WaitForDeck.Task;
                Mazzo = new(OnlineSettings.Mazzo);
                InizializzaPartitaPartiInComune();
            }
            else
            {
                OnlineSettings.GrandezzaMazzo = 40;
                Mazzo = new Mazzo();
                InizializzaPartitaPartiInComune();
                OnlineSettings.SendDeck(Mazzo);
                await OnlineSettings.WaitForDeck.Task;
            }
        }

        private void InizializzaPartitaPartiInComune()
        {
            CarteGiocate = 0;
            Playing = true; //di default l'utente vuole fare una partita
        }
        public override async void GameLoop()
        {

            //Con CartaSceltaDalPc si intende la carta arrivata con il Socket
            Carta CartaSceltaDalPc = new();
            SemeBriscolaInGioco = PescaBriscola();
            int turno = OnlineSettings!.PrincipalHost ? 0 : 1;
            controllerView.RicostruisciWindow();
            if (turno == 0)
            {
                Giocatore1.RiempiMano();
                Giocatore2.RiempiMano();
            }
            else
            {
                Giocatore2.RiempiMano();
                Giocatore1.RiempiMano();
            }
            controllerView.RicostruisciWindow();
            controllerView.DisabilitaBottoni();

            while (CarteGiocate < 40)
            {
                switch (turno)
                {
                    case 0:
                        controllerView.AbilitaBottoni();
                        await TaskCartaScelta.Task;
                        controllerView.DisabilitaBottoni();
                        OnlineSettings.SendCard(CartaScelta);
                        TaskCartaScelta = new();
                        CarteGiocate+=2;
                        await OnlineSettings.WaitForCard.Task;
                        CartaSceltaDalPc = OnlineSettings.ReceivedCard;
                        controllerView.Aggiorna(CartaSceltaDalPc);
                        break;
                    case 1:
                        await OnlineSettings.WaitForCard.Task;
                        CartaSceltaDalPc = OnlineSettings.ReceivedCard;
                        controllerView.Aggiorna(CartaSceltaDalPc);
                        controllerView.AbilitaBottoni();
                        await TaskCartaScelta.Task;
                        controllerView.DisabilitaBottoni();
                        OnlineSettings.SendCard(CartaScelta);
                        TaskCartaScelta = new();
                        CarteGiocate+=2;
                        break;
                }
                OnlineSettings.WaitForCard = new();
                Giocatore1.Mano.Remove(CartaScelta);
                Giocatore2.Mano.Remove(CartaSceltaDalPc);

                int vincitore = CalcolaVincitore(CartaScelta, CartaSceltaDalPc, turno);

                int punteggio = CalcolaPunteggio(CartaScelta, CartaSceltaDalPc);
                switch (vincitore)
                {
                    case 1:
                        turno = 0;
                        Giocatore1.Punti += punteggio;
                        await Task.Delay(TimeSpan.FromSeconds(2));
                        break;
                    case -1:
                        turno = 1;
                        Giocatore2.Punti += punteggio;
                        await Task.Delay(TimeSpan.FromSeconds(2));
                        break;
                }

                
                //inoltrare il turno, e in base a quello pescare la carta alla posizione 0 o 1

                if (Mazzo.ListaCarte.Count > 0)
                {
                    if (vincitore == 1)
                    {
                        Giocatore1.RiempiMano();
                        Giocatore2.RiempiMano();
                    }
                    else
                    {
                        Giocatore2.RiempiMano();
                        Giocatore1.RiempiMano();
                    }

                    if (Mazzo.ListaCarte.Count == 0)
                    {
                        controllerView.RimuoviCartaMazzo();
                    }
                }
                controllerView.Animazione(vincitore, Animazione);
                await Animazione.Task;
                Animazione = new();
                controllerView.PulisciView();
                controllerView.DisabilitaBottoni();
                controllerView.CambiaSfondoCarteRimanenti();
                CartaScelta = new();
                CartaSceltaDalPc = new();
            }

            TaskPartita.SetResult();
        }

        public async override void StartPlaying()
        {

            await CreaPartita();
            Giocatore1.Mazzo = Giocatore2.Mazzo = Mazzo;
            CarteGiocate = 0;
            GameLoop();

            await TaskPartita.Task;
            TaskPartita = new TaskCompletionSource();

            controllerView.VisulizzaVincitore(VisualizzaMessagioVincitore());
            await Task.Delay(TimeSpan.FromSeconds(2));

            controllerView.SwitchaFinestra();
        }
    }
}
