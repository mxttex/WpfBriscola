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
            if ((new Random()).Next(0, 100) < errP) return GiocaCartaCasuale();
            switch (turno)
            {
                case 0:
                    Carta vincente = MigliorCartaVincente();
                    if (vincente != null) return vincente;
                    return CartaValoreMinimo();
                case 1:
                    return CartaValoreMinimo();
                default: return null;
            }

            
        }

        private Carta MigliorCartaVincente()
        {
            throw new NotImplementedException();
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
            int numeroCarteSuCuiVince = 0;
            if (carta.IsBriscola)
            {
                for (int seme = 0; seme < 4; seme++)
                    for (int numero = 0; numero < 10; numero++)
                        if (ProvaAVincere(numero, seme, carta) == true)
                            numeroCarteSuCuiVince++;
            }
            else
            {
                for (int numero = 0; numero < 10; numero++)
                {
                    if (ProvaAVincere(numero, (int)carta.SemeNumerico, carta) == true)
                        numeroCarteSuCuiVince++;
                }
            }

            return (int)Math.Round(carta.CalcolaPesoConst(), 0) + numeroCarteSuCuiVince;
        }

        private bool ProvaAVincere(int numero, int seme, Carta carta)
        {
            if (this[numero, seme] == true) return false;
            int vince = carta.CompareTo(new(numero, seme, SemeBriscolaInGioco));

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
