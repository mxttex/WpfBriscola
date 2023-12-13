using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBriscola
{
    internal static class GameValues
    {
        internal enum Semi
        {
            Denara = 0,
            Bastoni = 1,
            Coppe = 2,
            Spade = 3
        }
        internal enum CarteSpeciali
        {
            Fante = 8,
            Cavallo = 9,
            Re = 10,
            Asso = 1,
            Tre = 3
        }

        public static int errP { get; set; }
        public static int memP { get; set; }
        
        public const int minP = 2; 
        public const double coeffP = 1.9;
        public const double peroBrisc = 0.2;

    }
}
