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
        private int nrBytes { get; set; }
        public StartingWindow()
        {
            GameValues.OnlineSettings = new();
            InitializeComponent();
            sliderDifficoltà.Value = 0;
            nrBytes = 0;
            Thread ListenForConnection = new Thread(new ThreadStart(() =>
            {
                while (!GameValues.OnlineSettings.AlreadyConnected)
                {
                    //MessageBox.Show(GameValues.OnlineSettings.SenderSocket.Available.ToString());

                    if ((nrBytes = GameValues.OnlineSettings.SenderSocket.Available) > 0)
                    {
                        //originale
                        byte[] buffer = new byte[nrBytes];
                        EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, GameValues.Port);

                        GameValues.OnlineSettings.SenderSocket.ReceiveFrom(buffer, ref remoteEndpoint);
                        GameValues.OtherPlayerIp = (remoteEndpoint as IPEndPoint)!.Address;
                        GameValues.OnlineSettings.Receiver = remoteEndpoint;

                        string response = Encoding.UTF8.GetString(buffer, 0, nrBytes);
                        if (response.Substring(0, GameValues.StringaRichiestaDiConnessione.Length) == GameValues.StringaRichiestaDiConnessione)
                        {
                            GameValues.OnlineSettings.AlreadyConnected = true;
                            GameValues.OnlineSettings.PrincipalHost = false;
                            GameValues.OnlineSettings.WaitForConnection.SetResult();
                            string opponentName = string.Empty;
                            Dispatcher.Invoke(() =>
                            {
                                MainWindow mw = new MainWindow(txtNomeGiocatore.Text, true, opponentName); mw.Show(); this.Close();
                            });
                        }
                    }
                }
            }));
            ListenForConnection.Start();


        }


        private void btnAvviaPartita_Click(object sender, RoutedEventArgs e)
        {
            WpfBriscola.GameValues.OnlineSettings!.PrincipalHost = true;
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
            if ((sender as TextBox)!.Text.Length > 0) btnAvviaPartita.IsEnabled = true;
            else btnAvviaPartita.IsEnabled = false;
        }

        private void sliderDifficoltà_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int valSlider = int.Parse(Math.Round((sender as Slider)!.Value, 0).ToString());
            GameValues.errP = (100 - valSlider);
            GameValues.memP = valSlider;
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            //attivo bottone per online  
            if (IPAddress.TryParse(txtIp.Text, out GameValues.OtherPlayerIp!) && txtNomeGiocatore.Text.Length > 0)
                btnAvviaPartita.IsEnabled = true;
            else
                btnAvviaPartita.IsEnabled = false;

            //attivo bottone per locale
            if ((sender as TextBox).Text.Length > 0) 
                btnAvviaPartita.IsEnabled = true;
            else 
                btnAvviaPartita.IsEnabled = false;
        }
    }
}
