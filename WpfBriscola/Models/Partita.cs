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

    internal class Partita
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
        private async void GameLoop()
        {
            while (Playing)
            {

                await TaskCartaScelta.Task;
                TaskCartaScelta = new TaskCompletionSource();

                Dispatcher.Invoke(() =>
                {
                    //da mettere apposto
                });
                if (MessageBox.Show("Vuoi Ricominciare la Partita?", "Ricomincia Partita", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Playing = false;
            }
            
           
        }

        public void RitornaCartaScelta(Carta? C)
        {
            TaskCartaScelta.SetResult();
            CartaScelta = C;
        }

        public void StartPlaying()
        {
            //hread gameThread = new Thread(GameLoop);
            while (Playing)
            {
                //playerTurnEvent.WaitOne();

                ////qua serve il delegato


                //playerTurnEvent.Set();
            }
        }
        //public Carta SceltaCartaUtente(out Carta C)
        //{
        //}
    }
}
