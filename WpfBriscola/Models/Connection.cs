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

        public Connection()
        {
            //creo il socket
            SenderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //creo l'endpoint e lo bindo nel socket del sender --> questo pc
            IPEndPoint senderEndpoint = new IPEndPoint(IPAddress.Any, Port);
            SenderSocket.Bind(senderEndpoint);

        }

        
        public void ListenForConnection(object sender, EventArgs e)
        {
            int nrBytes;

            if ((nrBytes = SenderSocket.Available) > 0)
            {
               byte[] buffer = new byte[nrBytes];

               EndPoint receiver = new IPEndPoint(IPAddress.Any, Port);
               SenderSocket.ReceiveFrom(buffer, ref receiver);

                OtherPlayerIp = (receiver as IPEndPoint).Address;

                if(Encoding.UTF8.GetString(buffer, 0, nrBytes) == StringaRichiestaDiConnessione)
                {
                    ConnectionRequest.Invoke;
                }
            }
        }

        public void ConnectionRequest(object sender, EventArgs e)
        {
           
        }
        
    }
}
