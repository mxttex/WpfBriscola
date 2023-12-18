using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBriscola.Models
{
    internal class Mazzo
    {
        internal List<Carta> ListaCarte;
        
        public Mazzo()
        {
            ListaCarte = InizializzaMazzo();
        }

        private List<Carta> CreaMazzo()
        {
            List<Carta> lista = new List<Carta>();
            for(int seme = 0; seme < 4;  seme++)
            {
                for(int valore = 1; valore <= 10; valore ++)
                {
                    lista.Add(new Carta(valore, seme));
                }
            }
            return lista;
        }

        private List<Carta> MischiaCarte(List<Carta> carte)
        {
            Carta c;
            Random rd = new Random();
            int n;

            for(int i = 0; i< 100; i ++) 
            {
                n = rd.Next(0, 40);
                c = carte[n];
                carte.RemoveAt(n);
                carte.Add(c);
            }

            return carte;
        }

        internal List<Carta> InizializzaMazzo()
        {
            List<Carta> carte = CreaMazzo();
            return MischiaCarte(carte); 
        }
        
        internal Carta PrimaCarta()
        {
            Carta c = ListaCarte[0];
            ListaCarte.RemoveAt(0);
            return c;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (Carta carta in ListaCarte)
                stringBuilder.AppendLine(carta.ToString());

            return stringBuilder.ToString();

        }
    }
}
