using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalenders
{
    public class Kalender : IKalender
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static int currentId;

        private int _kalid;
        private string _naam;
        private IList<IAfspraak> _afspraken;

        public int KalId
        {
            get { return _kalid; }
            set {
                if (_kalid > 0)
                    new ArgumentException("id moet groter zijn dan nul");
                _kalid = value;

            }
        }

        public string Naam
        {
            get { return _naam; }
            set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Mag niet leeg zijn");
                _naam = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Naam"));
            }

        }

        public IList<IAfspraak> Afspraken
        {
            get { return _afspraken; }
            set {
                _afspraken = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Afspraken"));
            }
        }

        public Kalender(string naam, IList<IAfspraak> afspraken)
        {
            KalId = GetNextId();
            Naam = naam;
            Afspraken = afspraken;
        }

        public override string ToString()
        {
            return Naam;
        }

        protected int GetNextId()
        {
            return currentId++;
        }

        protected virtual void OnpropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
