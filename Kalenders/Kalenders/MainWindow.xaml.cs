using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kalenders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IList<IKalender> Kalenders { get; } = new KalendersCol();
        public static IList<IAfspraak> Afspraken { get; } = new Afspraken();
        public static Databank dat = new Databank();
        SqlConnection con = dat.Connection();
        

        public MainWindow()
        {
            InitializeComponent();

            lstKalenders.ItemsSource = Kalenders;           

            txtAfsBegin.MaxLength = 5;
            txtAfsEind.MaxLength = 5;
            Kalenders.Add(new Kalender("Jef", Afspraken));
            Kalenders.Add(new Kalender("Jef", Afspraken));

            
        }

        private void btnKalToevoegen_Click(object sender, RoutedEventArgs e)
        {
            Kalenders.Add(new Kalender(txtKalNaam.Text, Afspraken));
        }

        private void btnKalVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            Kalenders.Remove((IKalender)lstKalenders.SelectedItem);
        }

        private void btnAfsToevoegen_Click(object sender, RoutedEventArgs e)
        {           
            Kalender kal = lstKalenders.SelectedItem as Kalender;
            
            kal.Afspraken.Add(new Afspraak(txtAfsNaam.Text, Convert.ToDateTime(dtpDatum.SelectedDate) , txtAfsBegin.Text, txtAfsEind.Text, kal.KalId ));

        }


        private void btnKalWijzigen_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Convert.ToString(DateTime.Now));
        }

        private void lstKalenders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstAfspraken.Items.Refresh();

            Kalender kal = lstKalenders.SelectedItem as Kalender;

            lstAfspraken.ItemsSource = kal.Afspraken;
        }

        private void btnAfsVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            Afspraken.Remove((IAfspraak)lstAfspraken.SelectedItem);
        }
    }
}
