using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad3OpdrachtSamGhesquiere
{

    public class Agenda : IAgenda, INotifyPropertyChanged, IComparable
    {
        private int _id;
        private String _name;
        private IList<Appointment> _appointments;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("id"));
            }
        }
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public IList<Appointment> Appointments
        {
            get { return _appointments; }
            set
            {
                _appointments = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Appointments"));
            }
        }
        public Agenda(String name, IList<Appointment> appointments, int id)
        {
            Name = name;
            Appointments = appointments;
            Id = id;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Agenda agendaToCompare = obj as Agenda;

            if (agendaToCompare != null)
            {
                return Name.CompareTo(agendaToCompare.Name);
            }

            throw new ArgumentException("Obj niet van het type student.");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString() => $"{Name}";


    }
}



