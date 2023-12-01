﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfBriscola.Models;

namespace WpfBriscola
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Partita Partita { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //ScegliCarta += ScegliCartaDaWindow(Carta C);
            Partita = new Partita("matteo", "pc");
            LoadImmagini();
        }

        private void LoadImmagini()
        {
            imgCartaMazzo1.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[0].Path, UriKind.Relative));
            imgCartaMazzo2.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[1].Path, UriKind.Relative));
            imgCartaMazzo3.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[2].Path, UriKind.Relative));
            imgBriscola.Source    = new BitmapImage(new Uri(Partita.BriscolaFinale.Path, UriKind.Relative));
            //imgCartaPc1.Source  = new BitmapImage(new Uri(Partita.Giocatore2.Mano[0].Path, UriKind.Relative));
            //imgCartaPc2.Source  = new BitmapImage(new Uri(Partita.Giocatore2.Mano[1].Path, UriKind.Relative));
            //imgCartaPc3.Source  = new BitmapImage(new Uri(Partita.Giocatore2.Mano[2].Path, UriKind.Relative));
            imgCartaPc1.Source = imgCartaPc2.Source = imgCartaPc3.Source = new BitmapImage(new Uri(@"..\carte\legend.png", UriKind.Relative));
        }

        //public async void Playing()
        //{
        //    Partita.Partita
        //}

        internal delegate Carta ScegliCarta(Carta C);
        private void imgCartaMazzo_Click(object sender, RoutedEventArgs e)
        {
            imgCartaTavolo1.Source = new BitmapImage(new Uri(Partita.Giocatore1.Mano[(int.Parse(((Button)sender).Name.Substring(13, 1)) - 1) ].Path, UriKind.Relative));
            (sender as Button).Content = null;
            btnCartaMazzo1.IsEnabled = btnCartaMazzo2.IsEnabled = btnCartaMazzo3.IsEnabled = false;
            MessageBox.Show("Finisci il metodo coglione");
        }

        private Carta ScegliCartaDaWindow(Carta c)
        {
            return c;
        }
        //public async Carta ScegliCarta()
        //{
        //    Task<Carta> ScegliCarta = imgCartaMazzo_Click();
        //} 
    }
}
