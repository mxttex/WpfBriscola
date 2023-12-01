using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBriscola.Models
{
    internal class AIGiocatore:Giocatore
    {
        public AIGiocatore(int numero, string nome, Mazzo mazzo) :base(numero, nome, mazzo) { }

        public Carta Mossa(Carta cartaAvversario)
        {
            Carta? CartaScelta;
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
                if ((!Mano[i].IsBriscola) && (Mano[i].Seme == cartaAvversario.Seme))  //controllo se la carta non è una briscola
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
