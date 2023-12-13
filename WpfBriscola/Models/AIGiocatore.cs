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
        public AIGiocatore(int numero, string nome, Mazzo mazzo) :base(numero, nome, mazzo) { }

        public Carta Mossa(Carta? cartaAvversario)
        {
            Carta? CartaScelta;
            CarteUscite = new bool[40];
            if (cartaAvversario is null) return CartaValoreMinimo();
            if (ProvaAVincere(cartaAvversario, out CartaScelta)) return CartaScelta;
            CartaScelta = CartaValoreMinimo();
            return CartaScelta;
        }



        private Carta CartaValoreMinimo()
        {
            int valoreMinimo = int.MaxValue;
            int idx = 0;

            for(int i =0; i<Mano.Count; i++)
            {
                if (Mano[i] < valoreMinimo) valoreMinimo = Mano[i].Punteggio; idx = i;
            }

            if (valoreMinimo != 0) return Mano[idx];

            valoreMinimo = int.MaxValue;
            idx = 0;

            for (int i = 0; i < Mano.Count; i++)
            {
                if (valoreMinimo < Mano[i]) valoreMinimo = Mano[i].Punteggio; idx = i;
            }

            return Mano[idx];
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

       

    }
}
