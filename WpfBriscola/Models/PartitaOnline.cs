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
            InizializzaPartita(nomeGiocatore1, nomeGiocatore2);
        }

        internal override async void InizializzaPartita(string nomeGiocatore1, string nomeGiocatore2)
        {
            if (!OnlineSettings.AlreadyConnected)
            {
                await OnlineSettings.WaitForDeck.Task;
                Mazzo = new();
                InizializzaPartitaPartiInComune(nomeGiocatore1, nomeGiocatore2);
                OnlineSettings.PrincipalHost = false;
            }
            else
            {
                OnlineSettings.PrincipalHost = true;
                OnlineSettings.GrandezzaMazzo = 40;
                Mazzo = new Mazzo();
                InizializzaPartitaPartiInComune(nomeGiocatore1, nomeGiocatore2);
                OnlineSettings.SendDeck(Mazzo);
                await OnlineSettings.WaitForDeck.Task;
            }
        }

        private void InizializzaPartitaPartiInComune(string nomeGiocatore1, string nomeGiocatore2)
        {
            SemeBriscolaInGioco = PescaBriscola();
            Giocatore1 = new Giocatore(1, nomeGiocatore1, Mazzo);
            CarteGiocate = 0;
            Playing = true; //di default l'utente vuole fare una partita
        }
        public override async void GameLoop()
        {
            int turno = OnlineSettings.PrincipalHost ? 0:1;
            Carta CartaSceltaDalPc = new();
            await OnlineSettings.WaitForDeck.Task;
            if(turno == 0)
            {
                Giocatore1.RiempiMano();
                Giocatore2.RiempiMano();
            }
            else
            {
                Giocatore2.RiempiMano();
                Giocatore1.RiempiMano();
            }
            while (CarteGiocate < 40)
            {
                switch (turno)
                {
                    case 0:
                        await TaskCartaScelta.Task;
                        OnlineSettings.SendCard(CartaScelta);
                        TaskCartaScelta = new();
                        CarteGiocate++;
                        //CartaSceltaDalPc = GiocataPc(CartaScelta, turno);
                        await OnlineSettings.WaitForCard.Task;
                        CartaSceltaDalPc = OnlineSettings.ReceivedCard;
                        break;
                    case 1:
                        //CartaSceltaDalPc = GiocataPc(null, turno);
                        await OnlineSettings.WaitForCard.Task;
                        CartaSceltaDalPc = OnlineSettings.ReceivedCard;
                        await TaskCartaScelta.Task;
                        OnlineSettings.SendCard(CartaScelta);
                        TaskCartaScelta = new();
                        CarteGiocate++;
                        break;
                }

                Giocatore1.Mano.Remove(CartaScelta);


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
                controllerView.CambiaSfondoCarteRimanenti();
            }

            TaskPartita.SetResult();
        }

        public async override void StartPlaying()
        {
            await OnlineSettings.TryToConnect(OtherPlayerIp);

            await OnlineSettings.WaitForDeck.Task;
            
            
            controllerView.CaricaBriscola();

            CarteGiocate = 0;
            GameLoop();

            await TaskPartita.Task;
            TaskPartita = new TaskCompletionSource();

            controllerView.VisulizzaVincitore(VisualizzaMessagioVincitore());
            if (MessageBox.Show("Vuoi Ricominciare la Partita?", "Ricomincia Partita", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                Playing = false;
            else InizializzaPartita(Giocatore1.Nome, Giocatore2.Nome); controllerView.RicostruisciWindow();

            controllerView.SwitchaFinestra();
        }
    }
}
