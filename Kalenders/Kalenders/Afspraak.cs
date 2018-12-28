using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalenders
{
    public class Afspraak : IAfspraak
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static int currentId;

        private static int afsid;
        private static string titel;
        private static DateTime datum;
        private static string begin;
        private static string einde;
        private static int kalid;

        public int AfsId
        {
            get { return afsid; }
            set
            {
                if (afsid < 0)
                    new ArgumentException("id moet groter zijn dan nul");
                afsid = value;
            }
        }

        public string Titel
        {
            get { return titel; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mag niet leeg zijn");
                titel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Titel"));
            }
        }

        public DateTime Datum
        {
            get { return datum; }
            set
            {
                
                datum = value;

            }
        }

        public string Begin
        {
            get { return begin; }
            set
            {
                
                begin = value;

            }
        }

        public string Einde
        {
            get { return einde; }
            set
            {
                einde = value;

            }
        }

        public int Kalid
        {
            get { return kalid; }
            set
            {
                kalid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kalid"));
            }
        }

        public Afspraak(string titel, DateTime datum, string begin, string einde, int kalid)
        {
            AfsId = Nextid();
            Titel = titel;
            Datum = datum;
            Begin = begin;
            Einde = einde;
        }

        protected int Nextid()
        {
            return currentId++;
        }

        public override string ToString()
        {
            return titel + " op "+ datum.ToShortDateString() + " van " + begin + " tot " + einde;
        }

        protected virtual void OnpropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
