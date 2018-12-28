using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
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


namespace Ad3OpdrachtSamGhesquiere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public IList<IAgenda> Agendas { get; } = new Agendas();
        public Database database = new Database("Database");

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            UpdateAgendas();

        }

        private void btnAddAgenda_Click(object sender, RoutedEventArgs e)
        {
            if (CheckAgendaName() && CheckAgendaNameDuplicate())
            {
                IList<Appointment> newAppointments = new Appointments();
                Agenda agenda = new Agenda(txtName.Text, newAppointments, 1);
                Agendas.Add(agenda);
                database.InsertAgenda(agenda);
            }
            else
            {
                return;

            }

        }

        private bool CheckAgendaName()
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("U moet agenda een naam geven");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckAgendaNameDuplicate()
        {
            foreach (Agenda agendaNameCheck in Agendas)
            {
                if (agendaNameCheck.Name == txtName.Text)
                {
                    MessageBox.Show("Er bestaat al een agenda met deze naam, u kan deze gebruiken");
                    return false;
                }
            }
            return true;
        }

        private void btnDeleteAgenda_Click(object sender, RoutedEventArgs e)
        {
            Agenda modifyAgenda = lstAgendas.SelectedItem as Agenda;

            foreach (Appointment appointment in modifyAgenda.Appointments)
            {
                database.DeleteAppointment(modifyAgenda, appointment);
            }
            modifyAgenda.Appointments.Clear();
            database.DeleteAgenda(modifyAgenda);
            Agendas.Remove(modifyAgenda);
            lstAppointments.ItemsSource = null;
            lstAppointments.ItemsSource = modifyAgenda.Appointments;
        }

        private void btnChangeNameAgenda_Click(object sender, RoutedEventArgs e)
        {

            if (CheckAgendaName() && CheckAgendaNameDuplicate())
            {
                Agenda modifyAgenda = lstAgendas.SelectedItem as Agenda;
                IList<Appointment> oldAppointments = modifyAgenda.Appointments;

                database.DeleteAgenda(modifyAgenda);
                Agendas.Remove(modifyAgenda);
                Agenda newAgenda = new Agenda(txtName.Text, oldAppointments, 1);
                database.InsertAgenda(newAgenda);
                Agendas.Add(newAgenda);
            }
            else
            {
                return;
            }


        }

        private void btnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (NameFilledTest() && AgendaSelectedTest() && CalendarSelectedTest() && DateStartSelectedTest() && DateEndSelectedTest() /*&& DateStartDuplicatetest()*/)
            {
                Agenda modifyAgenda = lstAgendas.SelectedItem as Agenda;
                int uurStart = Convert.ToInt32(ComboStartUur.Text);
                int minutenStart = Convert.ToInt32(ComboStartMinuten.Text);
                int uurEind = Convert.ToInt32(ComboEindUur.Text);
                int minutenEind = Convert.ToInt32(ComboEindMinuten.Text);

                DateTime StartDatum = ComboToDate(Calendar.SelectedDate.Value, uurStart, minutenStart);
                DateTime EindDatum = ComboToDate(Calendar.SelectedDate.Value, uurEind, minutenEind);
                Boolean priorityLevel = checkPriority.IsChecked.Value;
                Appointment newAppointment = new Appointment(txtName.Text, StartDatum, EindDatum, txtDescription.Text, 1, priorityLevel);
                modifyAgenda.Appointments.Add(newAppointment);             
                database.InsertAppointment(modifyAgenda, newAppointment);                
            }
            else
            {
                return;
            }
        }

        private bool DateStartDuplicatetest()
        {
            Agenda modifyAgenda = (lstAgendas.SelectedItem as Agenda);
            foreach (Appointment appointment in modifyAgenda.Appointments)
            {
                if (appointment.Name == txtName.Name && (appointment.DateStart.ToString("HH") == ComboStartUur.SelectedItem.ToString() && (appointment.DateStart.ToString("mm") == ComboStartMinuten.SelectedItem.ToString())))
                {
                    MessageBox.Show("Er bestaat al een appointment met deze naam en starttijd");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        private bool DateEndSelectedTest()
        {
            if (ComboEindUur.SelectedItem != null && ComboEindMinuten.SelectedItem != null)
            {
                return true;
            }
            else
            {
                MessageBox.Show("U moet een eind tijdsstip aanduiden");
                return false;
            }
        }

        private bool DateStartSelectedTest()
        {
            if (ComboStartUur.SelectedItem != null && ComboStartMinuten.SelectedItem != null)
            {
                return true;
            }
            else
            {
                MessageBox.Show("U moet een start tijdsstip aanduiden");
                return false;
            }
        }

        private bool AgendaSelectedTest()
        {
            if (lstAgendas.SelectedItem != null)
            {
                return true;
            }
            else
            {
                MessageBox.Show("U moet een agenda aanduiden");
                return false;
            }
        }

        public DateTime ComboToDate(DateTime date, int hours, int minutes)
        {
            int day = Convert.ToInt32(date.Day);
            int month = Convert.ToInt32(date.Month);
            int year = Convert.ToInt32(date.Year);

            DateTime result = new DateTime(year, month, day, hours, minutes, 00);
            return result;
        }

        private void btnDeleteAppointment_Click(object sender, RoutedEventArgs e)
        {
            int selectedAgendaIndex = lstAgendas.SelectedIndex;
            Agenda modifyAgenda = (lstAgendas.SelectedItem as Agenda);
            Appointment modifyAppointment = (lstAppointments.SelectedItem as Appointment);

            if (modifyAgenda != null)
            {
                database.DeleteAppointment(modifyAgenda, modifyAppointment);
                modifyAgenda.Appointments.Remove(modifyAppointment);
                lstAppointments.ItemsSource = null;
                lstAppointments.ItemsSource = modifyAgenda.Appointments;
                lstAppointments.SelectedIndex = selectedAgendaIndex;
            }
            else
            {
                MessageBox.Show("U moet een appointment aanduiden");
            }
        }

        private void btnChangeNameAppointment_Click(object sender, RoutedEventArgs e)
        {

            if (NameFilledTest() && AgendaSelectedTest() && AppointmentSelectedtest())
            {

                Agenda modifyAgenda = lstAgendas.SelectedItem as Agenda;
                Appointment modifyAppointment = lstAppointments.SelectedItem as Appointment;
                int uurStart = Convert.ToInt32(ComboStartUur.Text);
                int minutenStart = Convert.ToInt32(ComboStartMinuten.Text);
                int uurEind = Convert.ToInt32(ComboEindUur.Text);
                int minutenEind = Convert.ToInt32(ComboEindMinuten.Text);
                int RememberSelectedIndex = lstAppointments.SelectedIndex;
                DateTime StartDatum = ComboToDate(Calendar.SelectedDate.Value, uurStart, minutenStart);
                DateTime EindDatum = ComboToDate(Calendar.SelectedDate.Value, uurEind, minutenEind);
                Boolean priorityLevel = checkPriority.IsChecked.Value;
                Appointment newAppointment = new Appointment(txtName.Text, StartDatum, EindDatum, txtDescription.Text, modifyAgenda.Id, priorityLevel);

                modifyAgenda.Appointments.Remove(lstAppointments.SelectedItem as Appointment);
                database.DeleteAppointment(modifyAgenda, modifyAppointment);
                modifyAgenda.Appointments.Add(newAppointment);
                database.InsertAppointment(modifyAgenda, newAppointment);
                lstAppointments.SelectedIndex = RememberSelectedIndex;
                

            }
            else
            {
                return;
            }

        }

        private Boolean NameFilledTest()
        {
            if (txtName.Text.Length != 0)
            {
                return true;

            }
            else
            {
                MessageBox.Show("Naam kan niet leeg zijn");
                return false;
            }
        }
        private Boolean CalendarSelectedTest()
        {
            if (Calendar.SelectedDate != null)
            {
                return true;
            }
            else
            {
                MessageBox.Show("U moet een datum meegeven door deze aan te duiden in de Kalender");
                return false;
            }
        }

        private Boolean AppointmentSelectedtest()
        {
            Appointment SelectedAppointment = lstAppointments.SelectedItem as Appointment;
            if (SelectedAppointment != null)
            {
                return true;
            }
            else
            {
                MessageBox.Show("U moet een appointment aanduiden");
                return false;
            }
        }
        private void lstAppointmentsUpdate(object sender, SelectionChangedEventArgs e)
        {
            Agenda modifyAgenda = lstAgendas.SelectedItem as Agenda;
            if (modifyAgenda != null)
            {
                modifyAgenda.Appointments.OrderBy(date => date.DateStart);
                lstAppointments.ItemsSource = null;

                lstAppointments.ItemsSource = modifyAgenda.Appointments;
            }

        }

        private void AppointmentContentUpdate(object sender, SelectionChangedEventArgs e)
        {
            Appointment SelectedAppointment = lstAppointments.SelectedItem as Appointment;
            if (SelectedAppointment != null)
            {
                if (SelectedAppointment.Description != null)
                {
                    txtDescription.Text = SelectedAppointment.Description;
                }
                if (SelectedAppointment.Name != null)
                {
                    txtName.Text = SelectedAppointment.Name;
                }
                if (SelectedAppointment.DateStart != null)
                {
                    Calendar.DisplayDate = SelectedAppointment.DateStart.Date;
                    Calendar.SelectedDate = SelectedAppointment.DateStart.Date;
                    ComboStartUur.Text = Convert.ToString(SelectedAppointment.DateStart.Hour);
                    ComboStartMinuten.Text = Convert.ToString(SelectedAppointment.DateStart.Minute);
                    ComboEindUur.Text = Convert.ToString(SelectedAppointment.DateEnd.Hour);
                    ComboEindMinuten.Text = Convert.ToString(SelectedAppointment.DateEnd.Minute);
                }


            }
        }
        public void UpdateAgendas()
        {
            Agendas.Clear();
            foreach (String agenda in database.RequestAgendas())
            {
                IList<Appointment> applicationList = new Appointments();
                database.RequestAgendaId(agenda);
                int id = database.RequestAgendaId(agenda);
                Agenda newAgenda = new Agenda(agenda, applicationList, id);
                Agendas.Add(newAgenda);
                UpdateAppointments(newAgenda);
            }



        }
        public void UpdateAppointments(Agenda agenda)
        {
            agenda.Appointments.Clear();
            foreach (Appointment appointment in database.RequestAppointments(agenda.Id))
            {
                if (appointment != null)
                    agenda.Appointments.Add(appointment);
            }
        }

        private void ReapeatAppointment_Click(object sender, RoutedEventArgs e)
        {


            if (AppointmentSelectedtest() && CheckWeekCount())
            {
                int weekCount = Convert.ToInt32(cmbWeekCount.Text);

                Agenda modifyAgenda = lstAgendas.SelectedItem as Agenda;
                int uurStart = Convert.ToInt32(ComboStartUur.Text);
                int minutenStart = Convert.ToInt32(ComboStartMinuten.Text);
                int uurEind = Convert.ToInt32(ComboEindUur.Text);
                int minutenEind = Convert.ToInt32(ComboEindMinuten.Text);
                Boolean priority = checkPriority.IsChecked.Value;

                for (int i = 1; i <= weekCount; i++)
                {
                    DateTime StartDatum = ComboToDate(Calendar.SelectedDate.Value.AddDays(7 * i), uurStart, minutenStart);
                    DateTime EindDatum = ComboToDate(Calendar.SelectedDate.Value, uurEind, minutenEind);
                    Appointment newAppointment = new Appointment(txtName.Text, StartDatum, EindDatum, txtDescription.Text, 1, priority);
                    modifyAgenda.Appointments.Add(newAppointment);
                    database.InsertAppointment(modifyAgenda, newAppointment);
                }

            }
            else
            {
                return;
            }
        }

        private bool CheckWeekCount()
        {
            if (cmbWeekCount.Text == "")
            {
                MessageBox.Show("Selecteer het aantal weken om te herhalen");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnExportToExel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!");
                return;
            }


            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = lstAgendas.SelectedItem.ToString();
            xlWorkBook.Title = lstAgendas.SelectedItem.ToString();

            xlWorkSheet.Cells[1, 1] = lstAgendas.SelectedItem.ToString();

            for (int j = 2; j <= lstAppointments.Items.Count; j++)
            {
                xlWorkSheet.Cells[1, j] = lstAppointments.Items[j - 2].ToString();

            }

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
        }
        //private void btnExportToPDF_Click(object sender, RoutedEventArgs e)
        //{

        //    DataTable dt = new DataTable();
        //    dt.Clear();
        //    dt.Columns.Add("Afspraak");

        //    foreach (var item in lstAppointments.Items)
        //    {
        //        DataRow row1 = dt.NewRow();
        //        row1[""] = item.ToString();
        //        dt.Rows.Add(row1);

        //    }

        //    Document document = new Document();
        //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(@"c:\Afspraken.pdf", FileMode.Create));
        //    document.Open();
        //    iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 5);

        //    PdfPTable table = new PdfPTable(dt.Columns.Count);
        //    PdfPRow row = null;
        //    float[] widths = new float[] { 4f, 4f, 4f, 4f };

        //    table.SetWidths(widths);

        //    table.WidthPercentage = 100;
        //    int iCol = 0;
        //    string colname = "";
        //    PdfPCell cell = new PdfPCell(new Phrase("Afspraken"));

        //    cell.Colspan = dt.Columns.Count;

        //    foreach (DataColumn c in dt.Columns)
        //    {

        //        table.AddCell(new Phrase(c.ColumnName, font5));
        //    }

        //    foreach (DataRow r in dt.Rows)
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            table.AddCell(new Phrase(r[0].ToString(), font5));
        //            table.AddCell(new Phrase(r[1].ToString(), font5));
        //            table.AddCell(new Phrase(r[2].ToString(), font5));
        //            table.AddCell(new Phrase(r[3].ToString(), font5));
        //        }
        //    }
        //    document.Add(table);
        //    document.Close();
        //}




        private void btnExportToTXT_Click(object sender, RoutedEventArgs e)
        {
            const string sPath = "..\\Afspraken.txt";

            System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(sPath);
            foreach (var item in lstAppointments.Items)
            {
                SaveFile.WriteLine(item.ToString());
            }
            SaveFile.Close();

            MessageBox.Show("Bestand is opgeslagen");
        }
    }

}


