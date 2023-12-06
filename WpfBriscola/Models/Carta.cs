using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBriscola.Models
{
    public class Carta : IComparable<Carta>, IEquatable<Carta>
    {
        internal int Numero { get; set; }
        internal string Seme { get; set; }
        internal string Path { get; set; }  
        internal int Punteggio { get; set; }
        internal bool IsBriscola { get; set; }

        public Carta()
        {
            Path = string.Empty;
        }
        public Carta(int numero, int seme)
        {
            Numero = numero;

            switch (seme)
            {
                case 0:
                    Seme = "denara";
                    break;
                case 1:
                    Seme = "bastoni";
                    break;
                case 2:
                    Seme = "coppe";
                    break;
                case 3:
                    Seme = "spade";
                    break;
                default:
                    Seme = string.Empty; break;
            }

            Path = $@"..\carte\_{Seme.Substring(0, 1)}.{Numero}.png";

            switch (Numero)
            {
                default:
                    Punteggio = 0; break;
                case 1:
                    Punteggio = 11; break;
                case 3:
                    Punteggio = 10; break;
                case 10:
                    Punteggio = 4; break;
                case 9:
                    Punteggio = 3; break;
                case 8:
                    Punteggio = 2; break;
            }

            IsBriscola = false;
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
