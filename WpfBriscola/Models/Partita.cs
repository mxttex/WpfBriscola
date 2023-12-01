using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBriscola.Models
{
    internal class Partita
    {
        internal Models.Mazzo Mazzo { get; set; }
        internal Giocatore Giocatore1 { get; set; }
        internal AIGiocatore Giocatore2 { get; set; }
        internal string SemeBriscola { get; set; }
        internal Carta BriscolaFinale { get; set; }
        internal List<Carta> CarteGiocate { get; set; }

        public Partita(string nomeGiocatore1, string nomeGiocatore2)
        {
            Mazzo = new Mazzo();
            Giocatore1 = new Giocatore(1, nomeGiocatore1, Mazzo);
            Giocatore2 = new AIGiocatore(2, nomeGiocatore2, Mazzo);
            SemeBriscola = PescaBriscola();
            CarteGiocate = new List<Carta>();
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
        public async void Playing()
        {
            bool giochiamo;
            Random rd = new Random();
           
        }

        //public Carta SceltaCartaUtente(out Carta C)
        //{
        //}
    }
}
