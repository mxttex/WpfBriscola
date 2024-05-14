using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static WpfBriscola.GameValues;

namespace WpfBriscola.Models
{
    internal class Connection
    {
        public Socket SenderSocket { get; set; }
        public EndPoint Receiver { get; set; }
        public bool alreadyConnected { get; set; }
        public TaskCompletionSource WaitForConnection { get; set; }

        public Connection()
        {
            //creo il socket
            SenderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //creo l'endpoint e lo bindo nel socket del sender --> questo pc
            IPEndPoint senderEndpoint = new IPEndPoint(IPAddress.Any, Port);
            SenderSocket.Bind(senderEndpoint);
            WaitForConnection = new();

        }

        
        public void ListenForConnection(object sender, EventArgs e)
        {
            int nrBytes;

            if ((nrBytes = SenderSocket.Available) > 0 && !alreadyConnected)
            {
               byte[] buffer = new byte[nrBytes];

               EndPoint receiver = new IPEndPoint(IPAddress.Any, Port);
               SenderSocket.ReceiveFrom(buffer, ref receiver);
               Receiver = receiver;
               OtherPlayerIp = (receiver as IPEndPoint).Address;

                if(Encoding.UTF8.GetString(buffer, 0, nrBytes) == StringaRichiestaDiConnessione)
                {
                    alreadyConnected = true;
                    WaitForConnection.SetResult();
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
            }catch(Exception e)
            {
                return false;
            }
        }
        public Task<Carta> ReceiveCard(object sender, EventArgs e)
        {
            int nrBytes;
            if ((nrBytes = SenderSocket.Available) > 0)
            {
                byte[] buffer = new byte[nrBytes];
                EndPoint receiver = new IPEndPoint(IPAddress.Any, Port); ;
                SenderSocket.ReceiveFrom(buffer, ref receiver);

                return new Carta(Encoding.UTF8.GetString(buffer, 0, nrBytes));
                //da risolvere
            }
        }

    }
}
