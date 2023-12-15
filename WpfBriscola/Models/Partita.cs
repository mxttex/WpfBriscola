using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static WpfBriscola.GameValues;

namespace WpfBriscola.Models
{

    public class Partita
    {
        private TaskCompletionSource TaskCartaScelta = new TaskCompletionSource();
        private TaskCompletionSource TaskPartita = new TaskCompletionSource();

        internal ControllerView controllerView { private get; set; }
        private bool Playing { get; set; }
        internal Models.Mazzo Mazzo { get; set; }
        internal Giocatore Giocatore1 { get; set; }
        internal AIGiocatore Giocatore2 { get; set; }
        internal Carta BriscolaFinale { get; set; }
        internal int CarteGiocate { get; set; }
        internal Carta CartaScelta { get; set; }
        internal Partita(string nomeGiocatore1, string nomeGiocatore2, ControllerView controller)
        {
            controllerView = controller;
            InizializzaPartita(nomeGiocatore1, nomeGiocatore2);

        }
        internal void InizializzaPartita(string nomeGiocatore1, string nomeGiocatore2)
        {
            Mazzo = new Mazzo();
            SemeBriscolaInGioco = PescaBriscola();
            Giocatore1 = new Giocatore(1, nomeGiocatore1, Mazzo);
            Giocatore2 = new AIGiocatore(2, nomeGiocatore2, Mazzo);
            CarteGiocate = 0;
            Playing = true; //di default l'utente vuole fare una partita
        }

        private Semi PescaBriscola()
        {
            Carta c = Mazzo.PrimaCarta();
            Mazzo.ListaCarte.Add(c);
            BriscolaFinale = c;
            foreach (Carta carta in Mazzo.ListaCarte) 
                if (carta.Seme == c.Seme)
                {
                    carta.SettaBriscola();
                    carta.CalcolaPesoConst();
                }
            return c.SemeNumerico;
        }

        private Carta GiocataPc(Carta? carta, int turno)
        {
            Carta scelta = Giocatore2.Mossa(carta, turno);
            CarteGiocate++;
            Giocatore2.Mano.Remove(scelta);
            controllerView.Aggiorna(scelta);
            return scelta;
        }
        public async void GameLoop()
        {
            int turno = new Random().Next(2);
            Carta CartaSceltaDalPc = new();
            while (CarteGiocate < 40)
            {
                switch (turno)
                {
                    case 0:
                        await TaskCartaScelta.Task;
                        TaskCartaScelta = new TaskCompletionSource();
                        CarteGiocate++;
                       
                        CartaSceltaDalPc = GiocataPc(CartaScelta);
                        break;
                    case 1:
                        CartaSceltaDalPc = GiocataPc(null);
                        await TaskCartaScelta.Task;
                        TaskCartaScelta = new TaskCompletionSource();
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

                if(Mazzo.ListaCarte.Count > 0)
                {
                    Giocatore1.RiempiMano();
                    Giocatore2.RiempiMano();
                    if (Mazzo.ListaCarte.Count == 0)
                        controllerView.RimuoviCartaMazzo();
                }
               
                controllerView.PulisciView();
                controllerView.CambiaSfondoCarteRimanenti();
            }

            TaskPartita.SetResult();
        }
        

        public void RitornaCartaScelta(Carta? C)
        {
            CartaScelta = C;
            TaskCartaScelta.SetResult();
        }

        public async void StartPlaying()
        {
            controllerView.CaricaBriscola();
            while (Playing)
            {
                CarteGiocate = 0;
                GameLoop();

                await TaskPartita.Task;
                TaskPartita = new TaskCompletionSource();

                VisualizzaMessageBoxVincitore();
                if (MessageBox.Show("Vuoi Ricominciare la Partita?", "Ricomincia Partita", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Playing = false;
                else InizializzaPartita(Giocatore1.Nome, Giocatore2.Nome); controllerView.RicostruisciWindow();

            }
            controllerView.SwitchaFinestra();
        }

        private int CalcolaPunteggio(Carta c1, Carta c2)
        {
            return c1.Punteggio + c2.Punteggio;
        }

        private int CalcolaVincitore(Carta utente, Carta pc, int turno)
        {
            
            switch (turno)
            {
                case 0:
                    if ((utente.Seme != pc.Seme) && !(pc.IsBriscola)) return 1;
                    break;

                case 1:
                    if (pc.Seme != utente.Seme && !(utente.IsBriscola)) return -1;
                    break;
            }
            return utente.CompareTo(pc);
            
        }

        private void VisualizzaMessageBoxVincitore()
        {
            StringBuilder sb = new();
            int differenzaPunti = Giocatore1.Punti - Giocatore2.Punti;

            if (differenzaPunti > 0)
                sb.AppendLine("Ha vinto " + Giocatore1.Nome);
            else if (differenzaPunti == 0)
                sb.AppendLine("La partita è finita in parità");
            else
                sb.AppendLine("Ha vinto " + Giocatore2.Nome);

            sb.AppendLine($"Punti fatti da {Giocatore1} = {Giocatore1.Punti}");
            sb.AppendLine($"Punti fatti da {Giocatore2} = {Giocatore2.Punti}");

            MessageBox.Show(sb.ToString(), "Risultato", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

    }
}
