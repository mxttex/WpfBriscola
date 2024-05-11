using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static WpfBriscola.GameValues;

namespace WpfBriscola.Models
{
    internal class GiocatoreOnline : Giocatore
    {
        internal Socket ReceiverSocket { get; set; }
        internal Socket SenderSocket { get; set; }

        public GiocatoreOnline(int nR, string nome, Mazzo mazzo, IPAddress receiverIp ):base(nR, nome, mazzo)
        {
            //creo i due socket
            SenderSocket = ReceiverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //creo l'endpoint e lo bindo nel socket del sender --> questo pc
            IPEndPoint senderEndpoint = new IPEndPoint(IPAddress.Any, Port);
            SenderSocket.Bind(senderEndpoint);
            
            //creo l'endopoint e lo bindo nel socket del receiver
            IPEndPoint receiverEndpoint = new IPEndPoint(receiverIp, Port);
            ReceiverSocket.Bind(senderEndpoint);
        }
    }
}
