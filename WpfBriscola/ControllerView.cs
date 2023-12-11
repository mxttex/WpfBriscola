using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBriscola.Models;
using System.Windows;
using System.Windows.Threading;

namespace WpfBriscola
{
    internal class ControllerView
    {
        private static MainWindow m = (Application.Current.MainWindow as MainWindow);
        public static void Aggiorna(Carta C)
        {
            m.CaricaCartaPC(C);    
        }

        public static void PulisciView()
        {
            m.LoadImmagini();
            m.PulisciTavolo();
            m.AttivaBottoni();
        }

        public static void RimuoviCartaMazzo()
        {
            m.RimuoviBriscola();
        }
    }
}
