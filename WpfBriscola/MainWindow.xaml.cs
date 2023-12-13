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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace WpfBriscola
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
         
        public Partita Partita { get; set; }
        private ControllerView controllerView { get; set; }
        
        public MainWindow(string namePlayerOne)
        {
            InitializeComponent();
            controllerView = new(this);
            Partita = new Partita(namePlayerOne, "pc", controllerView);
            
        }

       

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PulisciTavolo();
            LoadImmagini();
            Partita.StartPlaying();
        }

        internal void LoadImmagini()
        {
            //Creo le immagini delle varie carte
            try
            {
                Image im1 = new Image();
                im1.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[0].Path, UriKind.Relative));
                btnCartaMazzo1.Content = im1;

            }
            catch (Exception)
            {
                btnCartaMazzo1.Visibility = Visibility.Collapsed;
            }

            try
            {
                Image im2 = new Image();
                im2.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[1].Path, UriKind.Relative));
                btnCartaMazzo2.Content = im2;

            }
            catch (Exception)
            {
                btnCartaMazzo2.Visibility = Visibility.Collapsed;
            }

            try
            {
                Image im3 = new Image();
                im3.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[2].Path, UriKind.Relative));
                btnCartaMazzo3.Content = im3;

            }
            catch (Exception)
            {
                btnCartaMazzo3.Visibility = Visibility.Collapsed;
            }

            //Assegno il contenuto dei bottoni a quelle immagini

            imgCartaPc1.Source = imgCartaPc2.Source = imgCartaPc3.Source = new BitmapImage(new Uri(@"..\carte\legend.png", UriKind.Relative));

        }

        public void CaricaBriscola()
        {
            imgBriscola.Source = new BitmapImage(new Uri(Partita.BriscolaFinale.Path, UriKind.Relative));
        }

        public void PulisciTavolo()
        {
            imgCartaTavolo1.Source = imgCartaTavolo2.Source = null;
            
        }

        public void AttivaBottoni()
        {
            btnCartaMazzo1.IsEnabled = btnCartaMazzo2.IsEnabled = btnCartaMazzo3.IsEnabled = true;

        }
        public void RiattivaBottoni()
        {
            btnCartaMazzo1.Visibility = btnCartaMazzo2.Visibility = btnCartaMazzo3.Visibility = Visibility.Visible;
        }
        private void imgCartaMazzo_Click(object sender, RoutedEventArgs e)
        {
            Carta CartaScelta = Partita.Giocatore1.Mano[(int.Parse(((Button)sender).Name.Substring(13, 1)) - 1)];
            imgCartaTavolo1.Source = new BitmapImage(new Uri(CartaScelta.Path, UriKind.Relative));
            (sender as Button).Content = null;
            btnCartaMazzo1.IsEnabled = btnCartaMazzo2.IsEnabled = btnCartaMazzo3.IsEnabled = false;

            Partita.RitornaCartaScelta(CartaScelta);
        }

        public void Aggiornatavolo(Carta Utente, Carta Pc)
        {
            imgCartaTavolo1.Source = new BitmapImage(new Uri(Utente.Path, UriKind.Relative));
            imgCartaTavolo2.Source = new BitmapImage(new Uri(Pc.Path, UriKind.Relative));

        }

        internal void CaricaCartaPC(Carta C)
        {
            //imgCartaTavolo2 = new Image();
            imgCartaTavolo2.Source = new BitmapImage(new Uri(C.Path, UriKind.Relative));
        }

        internal void RimuoviBriscola()
        {
            imgBriscola.Visibility = imgCartaTopMazzo.Visibility = Visibility.Hidden;
        }
        internal void RicaricaBriscola()
        {
            imgBriscola.Visibility = imgCartaTopMazzo.Visibility = Visibility.Visible;
        }
        internal void SegnalaFineMazzo()
        {
            //if (Partita.Mazzo.ListaCarte.Count == 4)
            //{
            //    BackGround.Background = Brushes.Green;
            //}
            //if (Partita.Mazzo.ListaCarte.Count == 2)
            //{
            //    BackGround.Background = Brushes.Red;
            //}
            //else BackGround.Background = Brushes.White;
        }
    }
}
