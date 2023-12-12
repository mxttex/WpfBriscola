using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBriscola.Models
{
    internal class Giocatore
    {
        internal int Numero { get; set; }
        internal string Nome { get; set; }
        internal int Punti { get; set; }
        internal List<Carta> Mano { get; set; }
        internal Models.Mazzo Mazzo { get; set; }

        public Giocatore(int numero, string nome, Mazzo mazzo)
        {
            Numero = numero;
            Nome = nome;
            Mazzo = mazzo;
            Mano = new List<Carta>();
            RiempiMano();
        }

        public void RiempiMano()
        {
            int ripetizioni = 3 - Mano.Count;
            for (int i = 0; i < ripetizioni; i++)
                Mano.Add(Mazzo.PrimaCarta()); 
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
