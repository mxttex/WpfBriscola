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
using System.Windows.Shapes;
using WpfBriscola.Models;

namespace WpfBriscola
{
    /// <summary>
    /// Logica di interazione per StartingWindow.xaml
    /// </summary>
    public partial class StartingWindow : Window
    {
        public StartingWindow()
        {
            InitializeComponent();
        }

        private void btnAvviaPartita_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(txtNomeGiocatore.Text);
            mw.Show();
            this.Close();
        }

        private void btnStoricoPartite_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtNomeGiocatore.Text = string.Empty;
            btnAvviaPartita.IsEnabled = false;
        }

        private void txtNomeGiocatore_TextChanged(object sender, TextChangedEventArgs e)
        {
            if((sender as TextBox).Text.Length > 0) btnAvviaPartita.IsEnabled = true;
            else btnAvviaPartita.IsEnabled = false; 
        }
    }
}
