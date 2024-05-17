using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfBriscola.Models;
using System.Windows;
using System.Windows.Threading;

namespace WpfBriscola
{
    internal class ControllerView
    {
        private MainWindow Window { get; set; }
        public ControllerView(MainWindow mw)
        {
            Window = mw;
        }
        public void Aggiorna(Carta C)
        {
            Window.CaricaCartaPC(C);
        }

        public void RicostruisciWindow()
        {
            Window.ResettaTextBlock();
            PulisciView();
            RicaricaBottoni();
            Window.RicaricaBriscola();
            CaricaBriscola();
            Window.NascondiMazziGiocatori();
        }

        public void PulisciView()
        {
            Window.LoadImmagini();
            Window.PulisciTavolo();
            Window.AttivaBottoni();

        }

        public void RimuoviCartaMazzo()
        {
            Window.RimuoviBriscola();
        }

        private void RicaricaBottoni()
        {
            Window.RiattivaBottoni();
        }

        public void CaricaBriscola()
        {
            Window.CaricaBriscola();
        }

        public void SwitchaFinestra()
        {
            StartingWindow sw = new();
            sw.Show();
            Window.Close();
        }

        public void CambiaSfondoCarteRimanenti()
        {
            Window.SegnalaFineMazzo();
        }

        public void Animazione(int vincitore, TaskCompletionSource task)
        {
            Window.AnimazioneCarte(vincitore, task);
        }

        internal void VisulizzaVincitore(string message)
        {
            Window.ScriviVincitore(message);
        }
        internal void AbilitaBottoni()
        {
            Window.AttivaBottoni();
        }
        internal void DisabilitaBottoni() { Window.DisabilitaBottoni(); }
    }
}
