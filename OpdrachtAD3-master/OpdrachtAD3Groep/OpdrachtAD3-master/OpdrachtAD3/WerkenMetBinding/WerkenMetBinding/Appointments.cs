using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad3OpdrachtSamGhesquiere
{
    public class Appointments : ObservableCollection<Appointment>
    {
        public Appointments()
        {
            CollectionChanged += Appointments_CollectionChanged;
        }

        private void Appointments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (IAppointment Appointment in e.NewItems)
                    if (Appointment != null)
                        Appointment.PropertyChanged += Appointment_PropertyChanged;
            if (e.OldItems != null)
                foreach (IAppointment Appointment in e.OldItems)
                    if (Appointment != null)
                        Appointment.PropertyChanged -= Appointment_PropertyChanged;
        }

        private void Appointment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
    }
}
