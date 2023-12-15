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
        private Random Random { get; set; }
        public AIGiocatore(int numero, string nome, Mazzo mazzo) :base(numero, nome, mazzo)
        {
            CarteUscite = new bool[40];
        }

        public Carta Mossa(Carta? cartaAvversario, int turno)
        {
            Carta CartaScelta;
            //if (cartaAvversario is null) return CartaValoreMinimo();
            //if (ProvaAVincere(cartaAvversario, out CartaScelta)) return CartaScelta;
            //CartaScelta = CartaValoreMinimo();
            //return CartaScelta;
            switch (turno)
            {
                case 0:
                    break;
                case 1:
                    if ((new Random()).Next(0, 100) < errP) return GiocaCartaCasuale();
                    return CartaValoreMinimo();
            }

            
        }

        private Carta GiocaCartaCasuale()
        {
            return Mano[(new Random()).Next(0, 3)];
        }

        private Carta CartaValoreMinimo()
        {
            int pesoMinimo = int.MaxValue;
            Carta rit = new();
            foreach (Carta carta in Mano)
            {
                int pes = CalcolaPesoCarta(carta);
                if (pes < pesoMinimo) pesoMinimo = pes; rit = carta;
            }

            return rit;
        }

        private int CalcolaPesoCarta(Carta carta)
        {
            int numeroCarteSuCuiVince;
            if (carta.IsBriscola)
            {
                
            }
        }

        private bool ProvaAVincere(Carta cartaAvversario, out Carta CartaVincente)
        {
            
            int ris = 0;
            for (int i = 0; i < Mano.Count; i++)
            {
                if ((!Mano[i].IsBriscola) && (Mano[i].Seme != cartaAvversario.Seme))
                {
                    CartaVincente = Mano[i]; 
                    return false;
                }
                   

                //controllo se la carta non è una briscola
                ris = Mano[i].CompareTo(cartaAvversario);
                if (ris == 1)
                {
                    CartaVincente = Mano[i];
                    return true;
                }
            }
            CartaVincente = null;
            return false;
        }

        public bool this[int numero, int seme]
        {
            get => CarteUscite[seme * 10 + numero - 1];
            set => CarteUscite[seme * 10 + numero - 1] = value;
        }
}
}
