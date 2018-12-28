using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalenders
{
    class KalendersCol : ObservableCollection<IKalender>
    {

        public KalendersCol()
        {
            this.CollectionChanged += Kalenders_CollectionChanged;
        }

        private void Kalenders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (IKalender kalender in e.NewItems)
                {
                    kalender.PropertyChanged += Kalender_PropertyChanged;
                }
        }

        private void Kalender_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset));
        }
    }
}
