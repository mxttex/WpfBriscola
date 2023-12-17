using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfBriscola.Models;

namespace WpfBriscola
{
    /// <summary>
    /// Logica di interazione per CarteDebug.xaml
    /// </summary>
    public partial class CarteDebug : Window
    {
        private Giocatore g1, g2;
        internal CarteDebug(Giocatore g1, Giocatore g2)
        {
            InitializeComponent();
            this.g1 = g1;
            this.g2 = g2;
        }

        internal void AggiornaMano()
        {
            carteg1.Items.Clear();
            carteg2.Items.Clear();

            foreach (Carta c in g1.Mano)
            {
                carteg1.Items.Add(c);
            }
            foreach (Carta c in g2.Mano)
            {
                carteg2.Items.Add(c);
            }


        }
    }
}
