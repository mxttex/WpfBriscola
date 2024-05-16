using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Threading;
using System.Windows.Threading;

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
            sliderDifficoltà.Value = 0;
            ListenForConnection();
        }

        private void ListenForConnection()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(WpfBriscola.GameValues.OnlineSettings.ListenForConnection);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            timer.Start();
        }

        private void btnAvviaPartita_Click(object sender, RoutedEventArgs e)
        {
            WpfBriscola.GameValues.OnlineSettings.PrincipalHost = true;
            bool mode = ((Button)sender).Name.Substring(15) == "Online" ? true : false;
            MainWindow mw = new MainWindow(txtNomeGiocatore.Text, mode);
            mw.Show();
            this.Close();
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

        private void sliderDifficoltà_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int valSlider = int.Parse(Math.Round((sender as Slider).Value, 0).ToString());
            GameValues.errP = (100 - valSlider);
            GameValues.memP = valSlider;
        }

        private void txtIp_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnAvviaPartitaOnline.IsEnabled = IPAddress.TryParse(txtIp.Text, out GameValues.OtherPlayerIp) ? true : false;     
        }
    }
}
