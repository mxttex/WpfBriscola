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
            SemeBriscola = PescaBriscola();
            Giocatore1 = new Giocatore(1, nomeGiocatore1, Mazzo);
            Giocatore2 = new AIGiocatore(2, nomeGiocatore2, Mazzo);
            
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

        private Carta GiocataPc(Carta? carta)
        {
            Carta scelta = Giocatore2.Mossa(carta);
            CarteGiocate++;
            Giocatore2.Mano.Remove(scelta);
            ControllerView.Aggiorna(scelta);
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
                        Thread.Sleep(50);
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

                
                switch (vincitore)
                {
                    case 1:
                        turno = 0;
                        Giocatore1.Punti += CartaScelta.Punteggio;
                        MessageBox.Show("Ha preso l'utente");
                        break;
                    case -1:
                        turno = 1;
                        Giocatore2.Punti += CartaSceltaDalPc.Punteggio;
                        MessageBox.Show("Ha preso il PC");
                        break;
                }

                try
                {
                    Giocatore1.RiempiMano();
                    Giocatore2.RiempiMano();
                }
                catch (Exception) { }


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
            while (Playing)
            {
                CarteGiocate = 0;
                GameLoop();

                await TaskPartita.Task;
                TaskPartita = new TaskCompletionSource();
               
                if (MessageBox.Show("Vuoi Ricominciare la Partita?", "Ricomincia Partita", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    Playing = false;
            }
        }

        private int CalcolaVincitore(Carta utente, Carta pc, int turno)
        {
            
            switch (turno)
            {
                case 0:
                    if (utente.Seme != pc.Seme) return 1;
                    break;

                case 1:
                    if (pc.Seme != utente.Seme) return -1;
                    break;
            }
            return utente.CompareTo(pc);
            
        }

    }
}
