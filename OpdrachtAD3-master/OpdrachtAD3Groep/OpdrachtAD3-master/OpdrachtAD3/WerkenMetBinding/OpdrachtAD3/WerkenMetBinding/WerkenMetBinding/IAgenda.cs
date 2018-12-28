using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad3OpdrachtSamGhesquiere
{
    public interface IAgenda : INotifyPropertyChanged
    {
        String Name { get; set; }
    }
}
