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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using WpfBriscola.Models;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;
using static WpfBriscola.GameValues;

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
        public int PreseG1 { get; set; }
        public int PreseG2 {get; set; }
        private int CarteGiocate { get; set; }
        private bool IsOnline { get; set; }
        private string NamePlayerOne { get; set; }

        public MainWindow(string namePlayerOne, bool mode)
        {
            InitializeComponent();
            controllerView = new(this);
            IsOnline = mode;
            NamePlayerOne = namePlayerOne;    
        }

       

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PulisciTavolo();
            
            if (IsOnline && OnlineSettings.PrincipalHost)
            {
                await OnlineSettings.TryToConnect(OtherPlayerIp);
                Partita = new PartitaOnline(NamePlayerOne, "remote", controllerView);
            }
            else if(IsOnline && !OnlineSettings.PrincipalHost)
            {
                await OnlineSettings.WaitForDeck.Task;
                Partita = new PartitaOnline(NamePlayerOne, "remote", controllerView);
            }
            else
            {
                Partita = new Partita(NamePlayerOne, "pc", controllerView);
            }

            Partita.StartPlaying();
            LoadImmagini();
        }

        internal void LoadImmagini()
        {
            //Creo le immagini delle varie carte
            try
            {
                Image im1 = new Image();
                im1.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[0].Path, UriKind.Relative));
                btnCartaMazzo1.Content = im1;
                imgCartaPc1.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {
                btnCartaMazzo1.Visibility= imgCartaPc1.Visibility = Visibility.Collapsed;
            }

            try
            {
                Image im2 = new Image();
                im2.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[1].Path, UriKind.Relative));
                btnCartaMazzo2.Content = im2;
                imgCartaPc2.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {
                btnCartaMazzo2.Visibility = imgCartaPc2.Visibility = Visibility.Collapsed;
            }

            try
            {
                Image im3 = new Image();
                im3.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[2].Path, UriKind.Relative));
                btnCartaMazzo3.Content = im3;
                imgCartaPc3.Visibility = Visibility.Visible;
                
            }
            catch (Exception)
            {
                btnCartaMazzo3.Visibility = imgCartaPc3.Visibility =Visibility.Collapsed;
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
            imgCartaTavolo1.Visibility = imgCartaTavolo2.Visibility = Visibility.Visible;

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
            imgCartaTavolo2.Source = new BitmapImage(new Uri(C.Path, UriKind.Relative));
            if (Partita.CarteGiocate == 40) imgCartaPc1.Visibility = Visibility.Collapsed;
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
            if (Partita.Mazzo.ListaCarte.Count == 2){ tbkAggiornamenti.Text = "Ultime due carte nel mazzo"; return; }
            if (Partita.Mazzo.ListaCarte.Count == 4) tbkAggiornamenti.Text = "Ultime quattro carte nel mazzo";
            else ResettaTextBlock();
            
        }

        internal void ScriviVincitore(string msg)
        {
            tbkAggiornamenti.Text = msg;
        }

        internal void ResettaTextBlock()
        {
            tbkAggiornamenti.Text = string.Empty;
        }
        internal void NascondiMazziGiocatori()
        {
            PreseG1 = PreseG2 = 0;
            imgMazzoG1.Visibility = imgMazzoG2.Visibility = Visibility.Hidden;
        }

        internal void AnimazioneCarte(int vincitore, TaskCompletionSource task)
        {
            double margineX = (vincitore != 1 ? DX : SX);
            double margineY = (vincitore != 1 ? DY : SY);
            if (vincitore == 1) PreseG1++;
            else PreseG2++;
            

            Thread ts = new Thread(new ThreadStart(() =>
            {
                Thickness marginiOriginali;
                Dispatcher.Invoke(new Action(() =>
                {
                    imgCartaTavolo1.Visibility = imgCartaTavolo2.Visibility = Visibility.Hidden;
                    CartaDaMuovere.Visibility = Visibility.Visible;
                    marginiOriginali = CartaDaMuovere.Margin;
                }));
                for (int i = 0; i < 21; i++) 
                {
                    Thread.Sleep(35);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        Thickness margini = CartaDaMuovere.Margin;
                        margini.Left += margineX;
                        margini.Top += margineY;
                        CartaDaMuovere.Margin = margini;
                    }));
                   
                }
                Dispatcher.Invoke(new Action(() =>
                {
                    Thread.Sleep(500);
                    CartaDaMuovere.Visibility = Visibility.Collapsed;
                    CartaDaMuovere.Margin = marginiOriginali;
                    if(PreseG1 > 0) imgMazzoG1.Visibility = Visibility.Visible;
                    if (PreseG2 > 0) imgMazzoG2.Visibility = Visibility.Visible;
                }));
                task.SetResult();

            }));
            ts.Start();
        }
    }
}
