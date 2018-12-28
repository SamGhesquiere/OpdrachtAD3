using System;
using System.ComponentModel;

namespace Kalenders
{
    public interface IAfspraak : INotifyPropertyChanged
    {
        int AfsId { get; set; }
        string Begin { get; set; }
        DateTime Datum { get; set; }
        string Einde { get; set; }
        int Kalid { get; set; }
        string Titel { get; set; }
    }
}