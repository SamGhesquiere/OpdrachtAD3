using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalenders
{
    class Afspraken : ObservableCollection<IAfspraak>
    {
        public Afspraken()
        {
            this.CollectionChanged += Afspraken_CollectionChanged;
        }

        private void Afspraken_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (IAfspraak afspraak in e.NewItems)
                    afspraak.PropertyChanged += Afspraak_PropertyChanged;
            if (e.OldItems != null)
                foreach (IAfspraak afspraak in e.OldItems)
                    afspraak.PropertyChanged -= Afspraak_PropertyChanged;
        }

        private void Afspraak_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
    }
}
