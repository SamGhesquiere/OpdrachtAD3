using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ad3OpdrachtSamGhesquiere
{
    public interface IAppointment : INotifyPropertyChanged
    {   
        String Name { get; set; }
        DateTime DateStart { get; set; }
        DateTime DateEnd { get; set; }
        String Description { get; set; }
        Boolean PriorityLevel { get; set; }
    }
}

