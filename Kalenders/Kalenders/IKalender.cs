using System.Collections.Generic;
using System.ComponentModel;

namespace Kalenders
{
    public interface IKalender : INotifyPropertyChanged
    {
        IList<IAfspraak> Afspraken { get; set; }
        int KalId { get; set; }
        string Naam { get; set; }
    }
}