using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static WpfBriscola.GameValues;
using System.Threading;
using System.Threading.Tasks;

namespace WpfBriscola.Models
{
    internal class GiocatoreOnline : Giocatore
    {
        internal Socket SenderSocket { get; set; }
        internal IPEndPoint ReceiverEndpoint { get; set; }
        private TaskCompletionSource Connessione { get; set; }
        internal bool Connected { get; set; }

        public GiocatoreOnline(int nR, string nome, Mazzo mazzo, IPAddress receiverIp ):base(nR, nome, mazzo)
        {
            
            Connessione = new();
            TryToConnect();
            
        }

        private async void TryToConnect()
        {
            byte[] messaggio = Encoding.UTF8.GetBytes(StringaRichiestaDiConnessione);
            SenderSocket.SendTo(messaggio, ReceiverEndpoint);

            var connectedOrTimeout = new List<Task> { Connessione.Task, Task.Delay(TimeSpan.FromSeconds(30)) };
            Task task = Task.WhenAny(connectedOrTimeout);

            if(task == Connessione.Task)
            {
                Connected = true;
            }
            else
            {
                throw new Exception("Richiesta Di Connessione Scaduta");
            }
        }

        
    }
}
