using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad3OpdrachtSamGhesquiere
{
    public class Agendas : ObservableCollection<IAgenda>
    {
        public Agendas()
        {
            this.CollectionChanged += Agendas_CollectionChanged;
        }

        private void Agendas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (IAgenda agenda in e.NewItems)
                    if (agenda != null)
                        agenda.PropertyChanged += Agenda_PropertyChanged;
            if (e.OldItems != null)
                foreach (IAgenda agenda in e.OldItems)
                    if (agenda != null)
                        agenda.PropertyChanged -= Agenda_PropertyChanged;
        }

        private void Agenda_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
    }
}
