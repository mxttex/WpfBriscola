using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using WpfBriscola.Models;

namespace WpfBriscola
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public Partita Partita { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            Partita = new Partita("matteo", "pc");
            Partita.StartPlaying();


        }

        public MainWindow(Partita partita)
        {
            Partita = partita;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImmagini();
            //ThreadStart ts = new ThreadStart(() =>
            //{
                

            //});
            //Thread gameThread = new Thread(ts);
        }

        internal void LoadImmagini()
        {
            imgCartaMazzo1.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[0].Path, UriKind.Relative));
            imgCartaMazzo2.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[1].Path, UriKind.Relative));
            imgCartaMazzo3.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[2].Path, UriKind.Relative));
            imgBriscola.Source    = new BitmapImage(new Uri(Partita.BriscolaFinale.Path, UriKind.Relative));
            //imgCartaPc1.Source  = new BitmapImage(new Uri(Partita.Giocatore2.Mano[0].Path, UriKind.Relative));
            //imgCartaPc2.Source  = new BitmapImage(new Uri(Partita.Giocatore2.Mano[1].Path, UriKind.Relative));
            //imgCartaPc3.Source  = new BitmapImage(new Uri(Partita.Giocatore2.Mano[2].Path, UriKind.Relative));
            imgCartaPc1.Source = imgCartaPc2.Source = imgCartaPc3.Source = new BitmapImage(new Uri(@"..\carte\legend.png", UriKind.Relative));
            imgCartaTavolo1.Source = imgCartaTavolo2.Source = null;
            btnCartaMazzo1.IsEnabled = btnCartaMazzo2.IsEnabled = btnCartaMazzo3.IsEnabled = true;

        }
        private void imgCartaMazzo_Click(object sender, RoutedEventArgs e)
        {
            Carta CartaScelta = Partita.Giocatore1.Mano[(int.Parse(((Button)sender).Name.Substring(13, 1)) - 1)];
            imgCartaTavolo1.Source = new BitmapImage(new Uri(CartaScelta.Path, UriKind.Relative));
            (sender as Button).Content = null;
            btnCartaMazzo1.IsEnabled = btnCartaMazzo2.IsEnabled = btnCartaMazzo3.IsEnabled = false;

            

            Partita.RitornaCartaScelta(CartaScelta);
        }

        internal void CaricaCartaPC(Carta C)
        {
            imgCartaTavolo2.Source = new BitmapImage(new Uri(C.Path, UriKind.Relative));
        }

       
    }
}
