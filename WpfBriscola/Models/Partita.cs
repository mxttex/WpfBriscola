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

        private ManualResetEvent playerTurnEvent = new ManualResetEvent(false);
        private bool Playing { get; set; }
        internal Models.Mazzo Mazzo { get; set; }
        internal Giocatore Giocatore1 { get; set; }
        internal AIGiocatore Giocatore2 { get; set; }
        internal string SemeBriscola { get; set; }
        internal Carta BriscolaFinale { get; set; }
        internal List<Carta> CarteGiocate { get; set; }
        internal Carta CartaScelta { get; set; }
        public Partita(string nomeGiocatore1, string nomeGiocatore2)
        {
            Mazzo = new Mazzo();
            Giocatore1 = new Giocatore(1, nomeGiocatore1, Mazzo);
            Giocatore2 = new AIGiocatore(2, nomeGiocatore2, Mazzo);
            SemeBriscola = PescaBriscola();
            CarteGiocate = new List<Carta>();
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
            while (CarteGiocate.Count < 40)
            {
                await TaskCartaScelta.Task;
                TaskCartaScelta = new TaskCompletionSource();
                //l'utente ha già scelto la sua carta

                Thread.Sleep(1000);
                
                Giocatore1.Mano.Remove(CartaScelta);

                Carta CartaSceltaDalPc = Giocatore2.Mossa(CartaScelta);
                Giocatore1.Mano.Remove(CartaSceltaDalPc);

                ControllerView.Aggiorna(CartaSceltaDalPc);

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

                ControllerView.PulisciView();
                
            }
            
           
        }

        public void RitornaCartaScelta(Carta? C)
        {
            CartaScelta = C;
            TaskCartaScelta.SetResult();
        }

        public void StartPlaying()
        {
            //hread gameThread = new Thread(GameLoop)
            while (Playing)
            {
                //playerTurnEvent.WaitOne();
                GameLoop();
                ////qua serve il delegato

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
