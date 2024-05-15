using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static WpfBriscola.GameValues;
using System.Windows;

namespace WpfBriscola.Models
{
    internal class Connection
    {
        public Socket SenderSocket { get; set; }
        public EndPoint Receiver { get; set; }
        public bool AlreadyConnected { get; set; }
        public TaskCompletionSource WaitForConnection { get; set; }
        public TaskCompletionSource WaitForDeck { get; set; } 
        public TaskCompletionSource WaitForCard { get; set; }
        public int GrandezzaMazzo { get; set; }
        public List<Carta> Mazzo { get; set; }
        public bool PrincipalHost { get; set; }
        public Carta ReceivedCard { get; private set; }

        public Connection()
        {
            //creo il socket
            SenderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //creo l'endpoint e lo bindo nel socket del sender --> questo pc
            IPEndPoint senderEndpoint = new IPEndPoint(IPAddress.Any, Port);
            SenderSocket.Bind(senderEndpoint);
            WaitForConnection = new();
            GrandezzaMazzo = 0;
            WaitForDeck = new();
        }

        
        public void ListenForConnection(object sender, EventArgs e)
        {
            int nrBytes;

            if ((nrBytes = SenderSocket.Available) > 0 && !AlreadyConnected)
            {
               byte[] buffer = new byte[nrBytes];

               EndPoint receiver = new IPEndPoint(IPAddress.Any, Port);
               SenderSocket.ReceiveFrom(buffer, ref receiver);
               Receiver = receiver;
               OtherPlayerIp = (receiver as IPEndPoint).Address;

                if(Encoding.UTF8.GetString(buffer, 0, nrBytes) == StringaRichiestaDiConnessione)
                {
                    AlreadyConnected = true;
                    WaitForConnection.SetResult();
                    MainWindow mw = new MainWindow("giocatore", true); Application.Current.Shutdown(); mw.Show();
                   
                }
            }

           
        }

        public bool SendCard(Carta c)
        {
            byte[] bufferCarta = Encoding.UTF8.GetBytes(c.Path);

            try
            {
                SenderSocket.SendTo(bufferCarta, Receiver);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public async void ReceiveCard(object sender, EventArgs e)
        {
            int nrBytes;
            if ((nrBytes = SenderSocket.Available) > 0)
            {
                byte[] buffer = new byte[nrBytes];
                EndPoint receiver = new IPEndPoint(IPAddress.Any, Port); ;
                SenderSocket.ReceiveFrom(buffer, ref receiver);

                Carta c = new Carta(Encoding.UTF8.GetString(buffer, 0, nrBytes));
                if(GrandezzaMazzo < 40)
                {
                    Mazzo.Add(c);
                    GrandezzaMazzo++;
                    if (GrandezzaMazzo == 40)
                    {
                        WaitForDeck.SetResult();
                    }
                }

                ReceivedCard = c;
                WaitForCard.SetResult();
            }
        }

        public async void TryToConnect(IPAddress ip)
        {
            byte[] messaggio = Encoding.UTF8.GetBytes(StringaRichiestaDiConnessione);
            Receiver = new IPEndPoint(ip, Port);
            SenderSocket.SendTo(messaggio, Receiver);

            var connectedOrTimeout = new List<Task> { WaitForConnection.Task, Task.Delay(TimeSpan.FromSeconds(30)) };
            Task task = await Task.WhenAny(connectedOrTimeout);

            if (task == WaitForConnection.Task)
            {
                AlreadyConnected = true;
            }
            else
            {
                throw new Exception("Richiesta Di Connessione Scaduta");
            }
        }

        public void SendDeck(Mazzo mazzo)
        {
            foreach(Carta c in mazzo.ListaCarte)
            {
                SendCard(c);
            }
            WaitForDeck.SetResult();
        }
    }
}
