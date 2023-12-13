using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBriscola
{
    internal static class GameValues
    {
        public static int errP { get; set; }
        public static int memP { get; set; }
        
        public const int minP = 2; 
        public const double coeffP = 1.9;
        public const double peroBrisc = 0.2;

    }
}
