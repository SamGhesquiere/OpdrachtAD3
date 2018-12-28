using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Ad3OpdrachtSamGhesquiere
{

    public class Appointment : IAppointment, INotifyPropertyChanged
    {
        private String _name;
        private DateTime _dateStart;
        private DateTime _dateEnd;
        private String _desc;
        private int _idFromAgenda;

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
        public DateTime DateStart
        {
            get { return _dateStart; }
            set
            {
                _dateStart = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DateStart"));
            }
        }

        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set
            {
                _dateEnd = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DateEnd"));
            }
        }

        public String Description
        {
            get { return _desc; }
            set
            {
                _desc = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Description"));
            }
        }

        public int IdFromAgenda
        {
            get { return _idFromAgenda; }
            set
            {
                _idFromAgenda = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("idFromAgenda"));
            }
        }
        public Appointment(String name, DateTime dateStart, DateTime dateEnd, String description, int idFromAgenda)
        {
            Name = name;
            DateStart = dateStart;
            DateEnd = dateEnd;
            Description = description;
            IdFromAgenda = idFromAgenda;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string DatbaseDateFormat => Convert.ToString(DateStart);
        public override string ToString() => $"{DateStart.Day}/{DateStart.Month} {DateStart.Hour}:{DateStart.Minute} - {DateEnd.Hour}:{DateEnd.Minute}, {Name}";
        
    }
}

