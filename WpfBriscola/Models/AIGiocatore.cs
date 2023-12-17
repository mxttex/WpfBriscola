using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfBriscola.GameValues;

namespace WpfBriscola.Models
{
    internal class AIGiocatore:Giocatore
    {
        internal bool[] CarteUscite { get; set; }
        public AIGiocatore(int numero, string nome, Mazzo mazzo) :base(numero, nome, mazzo)
        {
            CarteUscite = new bool[40];
        }

        public Carta? Mossa(Carta? cartaAvversario, int turno)
        {
            int r = (new Random()).Next(0, 100);
            if (r < errP) return GiocaCartaCasuale();
            switch (turno)
            {
                case 0:
                    Carta? vincente = MigliorCartaVincente(cartaAvversario);
                    if (vincente != null) return vincente;
                    return CartaValoreMinimo();
                case 1:
                    return CartaValoreMinimo();
                default: return null;
            }
        }

        private Carta? MigliorCartaVincente(Carta? carta)
        {
            int valCartaVincente = int.MinValue;
            Carta? cartaVincente = null;

            foreach (Carta possibileVincitrice in Mano)
            {
                bool vince = ProvaAVincere(possibileVincitrice, carta);
                if (vince)
                {
                    if (possibileVincitrice.Punteggio >= valCartaVincente)
                    {
                        valCartaVincente = possibileVincitrice.Punteggio;
                        cartaVincente = possibileVincitrice;
                    }
                }
            }

            return cartaVincente;
        }

        private Carta GiocaCartaCasuale()
        {
            return Mano[(new Random()).Next(0, 3)];
        }

        private Carta CartaValoreMinimo()
        {
            double pesoMinimo = double.MaxValue;
            Carta rit = new();
            foreach (Carta carta in Mano)
            {
                //controllo se la carta non è un tre o un due e che il pc nella mano non abbia solo 3 o assi
                bool tuttiTreoAssi = (Mano[0].Punteggio < SogliaValoreCarta && Mano[1].Punteggio < SogliaValoreCarta && Mano[2].Punteggio < SogliaValoreCarta);
                if (carta.Punteggio < SogliaValoreCarta || tuttiTreoAssi)
                {
                    double pes = CalcolaPesoCarta(carta);
                    if (pes < pesoMinimo) { pesoMinimo = pes; rit = carta; }
                }
                

            }

            return rit;
        }

        private double CalcolaPesoCarta(Carta carta)
        {
            int numeroCarteSuCuiVince = 0;
            if (carta.IsBriscola)
            {
                for (int seme = 0; seme < 4; seme++)
                    for (int numero = 1; numero <= 10; numero++)
                        if (ProvaAVincere(numero, seme, carta) == true)
                            numeroCarteSuCuiVince++;
            }
            else
            {
                for (int numero = 1; numero <= 10; numero++)
                {
                    if (ProvaAVincere(numero, (int)carta.SemeNumerico, carta) == true)
                        numeroCarteSuCuiVince++;
                }
            }

            return Math.Round(carta.CalcolaPesoConst(), 3) + numeroCarteSuCuiVince;
        }

        private bool ProvaAVincere(int numero, int seme, Carta carta)
        {
            if (this[numero, seme] == true) return false;
            int vince = carta.CompareTo(new(numero, seme, SemeBriscolaInGioco));

            if (vince == 1) return true;
            else return false;

        }

        private bool ProvaAVincere(Carta c1, Carta? c2)
        {
            int vince = c1.CompareTo(c2);

            if (vince == 1) return true;
            else return false;
        }

        public bool this[int numero, int seme]
        {
            get => CarteUscite[seme * 10 + numero - 1];
            set => CarteUscite[seme * 10 + numero - 1] = value;
        }
}
}
