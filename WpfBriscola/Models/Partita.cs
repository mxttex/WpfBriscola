using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfBriscola.Models
{

    public class Partita
    {
        TaskCompletionSource TaskCartaScelta = new TaskCompletionSource();
        TaskCompletionSource TaskPartita = new TaskCompletionSource();

        private bool Playing { get; set; }
        internal Models.Mazzo Mazzo { get; set; }
        internal Giocatore Giocatore1 { get; set; }
        internal AIGiocatore Giocatore2 { get; set; }
        internal string SemeBriscola { get; set; }
        internal Carta BriscolaFinale { get; set; }
        internal int CarteGiocate { get; set; }
        internal Carta CartaScelta { get; set; }
        public Partita(string nomeGiocatore1, string nomeGiocatore2)
        {
            Mazzo = new Mazzo();
            Giocatore1 = new Giocatore(1, nomeGiocatore1, Mazzo);
            Giocatore2 = new AIGiocatore(2, nomeGiocatore2, Mazzo);
            SemeBriscola = PescaBriscola();
            CarteGiocate =0;
            Playing = true; //di default l'utente vuole fare una partita

        }

        private string PescaBriscola()
        {
            Carta c = Mazzo.PrimaCarta();
            Mazzo.ListaCarte.Add(c);
            BriscolaFinale = c;
            foreach (Carta carta in Mazzo.ListaCarte) 
                if (carta.Seme == c.Seme) 
                    carta.SettaBriscola();
            return c.Seme;
        }
        public async void GameLoop()
        {
            while (CarteGiocate < 40)
            {
                await TaskCartaScelta.Task;
                TaskCartaScelta = new TaskCompletionSource();
                //l'utente ha già scelto la sua carta

                Thread.Sleep(50);
                
                Giocatore1.Mano.Remove(CartaScelta);

                Carta CartaSceltaDalPc = Giocatore2.Mossa(CartaScelta);
                Giocatore1.Mano.Remove(CartaSceltaDalPc);

                ControllerView.Aggiorna(CartaSceltaDalPc);

                CarteGiocate++;
                CarteGiocate++;

                switch (CartaScelta.CompareTo(CartaSceltaDalPc))
                {
                    case 1:
                        Giocatore1.Punti += CartaScelta.Punteggio; 
                        MessageBox.Show("Ha preso l'utente");
                        break;
                    case -1:
                        Giocatore2.Punti += CartaSceltaDalPc.Punteggio;
                        MessageBox.Show("Ha preso il PC");
                        break;
                }
                Giocatore1.RiempiMano();
                Giocatore2.RiempiMano();


                //Thread.Sleep(2000);
                ControllerView.PulisciView();

               
                
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
            //hread gameThread = new Thread(GameLoop)
            while (Playing)
            {
                //playerTurnEvent.WaitOne();
                CarteGiocate = 0;
                GameLoop();
                ////qua serve il delegato
                ///
                await TaskPartita.Task;
                TaskPartita = new TaskCompletionSource();
               
                if (MessageBox.Show("Vuoi Ricominciare la Partita?", "Ricomincia Partita", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Playing = false;

               
                //playerTurnEvent.Set();
            }
        }
        //public Carta SceltaCartaUtente(out Carta C)
        //{
        //}
    }
}
