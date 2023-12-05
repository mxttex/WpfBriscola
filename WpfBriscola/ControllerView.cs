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
        public static void Aggiorna(Carta C)
        {
            (Application.Current.MainWindow as MainWindow).CaricaCartaPC(C);    
        }

        public static void PulisciView()
        {
            MainWindow m = (Application.Current.MainWindow as MainWindow);

            m.LoadImmagini();

        }
    }
}
