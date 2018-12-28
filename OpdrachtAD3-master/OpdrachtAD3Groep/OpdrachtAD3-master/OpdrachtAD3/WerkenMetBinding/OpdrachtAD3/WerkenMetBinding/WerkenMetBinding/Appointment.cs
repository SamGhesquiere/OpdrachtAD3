using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Ad3OpdrachtSamGhesquiere
{

    public class Appointment : IAppointment, INotifyPropertyChanged, IComparable
    {
        private String _name;
        private DateTime _dateStart;
        private DateTime _dateEnd;
        private String _desc;
        private int _idFromAgenda;
        private Boolean _priorityLevel;
        private String Priority;

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
        public Boolean PriorityLevel
        {
            get { return _priorityLevel; }
            set
            {
                _priorityLevel = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("priorityLevel"));
                if(_priorityLevel == true)
                {
                    Priority = "!!!!";
                }
                else
                {
                    Priority = "";
                }
            }
        }
        public Appointment(String name, DateTime dateStart, DateTime dateEnd, String description, int idFromAgenda, Boolean priorityLevel)
        {
            Name = name;
            DateStart = dateStart;
            DateEnd = dateEnd;
            Description = description;
            IdFromAgenda = idFromAgenda;
            PriorityLevel = priorityLevel;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Appointment appointmentToCompare = obj as Appointment;

            if (appointmentToCompare != null)
            {
                if (appointmentToCompare.DateStart < DateStart)
                {
                    return 1;
                }
                if (appointmentToCompare.DateStart < DateStart)
                {
                    return -1;
                }
                return 0;
            }

            throw new ArgumentException("Obj niet van het type Appointment");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string DatbaseDateFormat => Convert.ToString(DateStart);

       
        
        public override string ToString() => $"{Priority} {DateStart.ToShortDateString()} | {Name} | {DateStart.ToShortTimeString()} - {DateEnd.ToShortTimeString()}";

    }
}

