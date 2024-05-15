using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfBriscola.GameValues;

namespace WpfBriscola.Models
{
    

    public class Carta : IComparable<Carta>, IEquatable<Carta>
    {
        
        internal int Numero { get; set; }
        internal string Seme { get; set; }
        internal Semi SemeNumerico { get; set; }
        internal string Path { get; set; }  
        internal int Punteggio { get; set; }
        internal bool IsBriscola { get; set; }
        internal double PesoConst { get; set; }
        
        public Carta(string path)
        {
            //da creare l'overload
            ConvertFromPathToValues(path);
        }

        public Carta()
        {
            Path = string.Empty;
        }
        internal Carta(int numero, int seme, Semi sBriscola)
        {
            Numero = numero;

            switch ((CarteSpeciali)Numero)
            {
                default:
                    Punteggio = 0; break;
                case CarteSpeciali.Asso:
                    Punteggio = 11; break;
                case CarteSpeciali.Tre:
                    Punteggio = 10; break;
                case CarteSpeciali.Re:
                    Punteggio = 4; break;
                case CarteSpeciali.Cavallo:
                    Punteggio = 3; break;
                case CarteSpeciali.Fante:
                    Punteggio = 2; break;
            }
            SemeNumerico = (Semi)seme;
            IsBriscola = (SemeNumerico == sBriscola) ? true : false;

        }
        public Carta(int numero, int seme)
        {
            Numero = numero;

            SemeNumerico = (Semi)seme;

            switch (SemeNumerico)
            {
                case Semi.Denara:
                    Seme = "denara";
                    break;
                case Semi.Bastoni:
                    Seme = "bastoni";
                    break;
                case Semi.Coppe:
                    Seme = "coppe";
                    break;
                case Semi.Spade:
                    Seme = "spade";
                    break;
                default:
                    Seme = string.Empty; break;
            }

            Path = $@"..\carte\_{Seme.Substring(0, 1)}.{Numero}.png";

            switch ((CarteSpeciali)Numero)
            {
                default:
                    Punteggio = 0; break;
                case CarteSpeciali.Asso:
                    Punteggio = 11; break;
                case CarteSpeciali.Tre:
                    Punteggio = 10; break;
                case CarteSpeciali.Re:
                    Punteggio = 4; break;
                case CarteSpeciali.Cavallo:
                    Punteggio = 3; break;
                case CarteSpeciali.Fante:
                    Punteggio = 2; break;
            }

            IsBriscola = false;
            PesoConst = CalcolaPesoConst();
        }

        public double CalcolaPesoConst()
        {
            return coeffP * Punteggio + pesoBrisc * (IsBriscola ? 0 : 1);
        }

        internal void SettaBriscola()
        {
            IsBriscola = true;
        }

        public override string ToString()
        {
            string ritorno = string.Empty;

            switch (Numero) 
            {
                case 1:
                    ritorno += "Asso "; break;
                case 2:
                    ritorno += "Due "; break;
                case 3:
                    ritorno += "Tre "; break;
                case 4:
                    ritorno += "Quattro "; break;
                case 5:
                    ritorno += "Cinque "; break;
                case 6:
                    ritorno += "Sei "; break;
                case 7:
                    ritorno += "Sette "; break;
                case 8:
                    ritorno += "Fante "; break;
                case 9:
                    ritorno += "Cavallo "; break;
                case 10:
                    ritorno += "Re "; break;
            }

            return ritorno + this.Seme;
        }
        private void ConvertFromPathToValues(string path)
        {
            char seme = path[10];
            string val;
            if(path.Length != 5)
            {
                 val = path[12].ToString();
            }
            else
            {
                val = path[12].ToString() + path[13];
            }

            switch (seme)
            {
                case 'b':
                    Seme = "bastoni";
                    SemeNumerico = Semi.Bastoni; 
                    break;
                case 'c':
                    Seme = "coppe";
                    SemeNumerico = Semi.Coppe;
                    break;
                case 'd':
                    Seme = "denara";
                    SemeNumerico = Semi.Denara;
                    break;
                case 's':
                    Seme = "spade";
                    SemeNumerico = Semi.Spade;
                    break;
            }
            this.Numero = int.Parse(val);

            switch ((CarteSpeciali)Numero)
            {
                default:
                    Punteggio = 0; break;
                case CarteSpeciali.Asso:
                    Punteggio = 11; break;
                case CarteSpeciali.Tre:
                    Punteggio = 10; break;
                case CarteSpeciali.Re:
                    Punteggio = 4; break;
                case CarteSpeciali.Cavallo:
                    Punteggio = 3; break;
                case CarteSpeciali.Fante:
                    Punteggio = 2; break;
            }

            IsBriscola = false;
            PesoConst = CalcolaPesoConst();
        }


        public int CompareTo(Carta? other)
        {
            if (this.IsBriscola && !(other.IsBriscola)) return 1;
            //if (other.Seme != semeInGioco) return -1;
            if (other.IsBriscola && !(this.IsBriscola)) return -1;
            if (this.Punteggio > other.Punteggio) return 1;
            if ((this.Punteggio == 0 && other.Punteggio == 0) && (this.Numero > other.Numero)) return 1;

            return -1;
        }

        public bool Equals(Carta? other)
        {
            if (this.ToString() == other.ToString()) return true;
            else return false;
        }

        //overload per poter confrontare qual è il punteggio associato a quella carta
        public static bool operator <(Carta a, int valore)
        {
            return a.Punteggio < valore;
        }
        public static bool operator >(Carta a, int valore)
        {
            return !(a.Punteggio < valore);
        }

        //overload per poter confrontare il numero della carta
        public static bool operator <(int valore, Carta a)
        {
            return a.Numero < valore;
        }
        public static bool operator >(int valore, Carta a)
        {
            return !(a.Numero < valore);
        }
    }
}
