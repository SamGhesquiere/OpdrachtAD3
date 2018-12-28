using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ad3OpdrachtSamGhesquiere
{
    public class Database
    {
        #region Fields

        /// <summary>
        /// Hier staan alle velden gedeclareerd. 
        /// </summary>
        /// 


        private static String _executableLocation = Assembly.GetExecutingAssembly().Location;
        private static String _dbLocation = Path.GetFullPath(Path.Combine(_executableLocation, @"..\..\.."));
        private static String _connectionString = $@"Data Source = (localdb)\mssqllocaldb; AttachDbFilename='{_dbLocation}\bin\db\DbDataAD3.mdf'; Initial Catalog = DbDataAD3; Integrated Security = True";


        /*  private static String _connectionString = @"
          Data Source = (localdb)\mssqllocaldb; 
          Initial Catalog = dbTest; 
          Integrated Security = True";
        */

        private static SqlConnection _connection = new SqlConnection(_connectionString);

        private SqlDataAdapter _adapterAgenda;
        private SqlDataAdapter _adapterAppointment;
        private DataSet _dataset;
        private String _name;


        /// <summary>
        /// Deze constructor voor de database klasse krijgt
        /// enkel een naam mee. Hier opent hij de verbinding
        /// met de mssql-databank.
        /// </summary>
        public Database(String name)
        {
            _name = name;
            try
            {
               _connection.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Connection failed", MessageBoxButton.OK);
            }
           
            NewAdapterAgenda();
            NewAdapterAppointment();
            NewDataSet();
            NewRelation();
        }
        #endregion

        #region Inserts

        /// <summary>
        /// Deze functie plaatst een meegegeven Agenda
        /// in de databank.
        /// </summary>
        public void InsertAgenda(Agenda agenda)
        {
            DataRow row = _dataset.Tables["tblAgendas"].NewRow();

            row["agendaName"] = agenda.Name;

            _dataset.Tables["tblAgendas"].Rows.Add(row);

            _adapterAgenda.Update(_dataset, "tblAgendas");
        }

        /// <summary>
        /// Deze functie plaatst een meegegeven appointment 
        /// in een reeds bestaande Agenda in de databank.
        /// </summary>
        public void InsertAppointment(Agenda agenda, Appointment appointment)
        {
            DataRow row;
            foreach (DataRow rowX in _dataset.Tables["tblAgendas"].Rows)
            {
                if (Convert.ToString(rowX["agendaId"]) == Convert.ToString(agenda.Id))
                {
                    row = _dataset.Tables["tblAppointments"].NewRow();
                                   
                    row["appointmentName"] = appointment.Name;
                    row["dateStart"] = appointment.DateStart;
                    row["dateEnd"] = appointment.DateEnd;
                    row["appointmentDescription"] = appointment.Description;
                    row["priorityLevel"] = appointment.PriorityLevel;

                    

                    row["idFromAgenda"] = RequestAgendaId(Convert.ToString(agenda));


                    _dataset.Tables["tblAppointments"].Rows.Add(row);
                }        
            }
            _adapterAppointment.Update(_dataset, "tblAppointments");
        }

        #endregion

        #region Deletes

        /// <summary>
        /// Deze functie verwijderd de meegegeven Agenda
        /// uit de databank.
        /// </summary>
        public void DeleteAgenda(Agenda agenda)
        {
            int RequestedAgendaId = (RequestAgendaId(Convert.ToString(agenda)));
            foreach (Appointment appointment in agenda.Appointments)
            {
                DeleteAppointment(agenda, appointment);
            }
            
            using (SqlCommand command = new SqlCommand("DELETE FROM " + "tblAgendas" + " WHERE " + "agendaId" + " = '" + RequestedAgendaId + "'", _connection))
            {
                command.ExecuteNonQuery();
            }
            _adapterAgenda.Update(_dataset, "tblAgendas");

        }

        /// <summary>
        /// Deze functie verwijderd een meegegeven appointment 
        /// uit een bestaande agenda uit de databank.
        /// </summary>
        public void DeleteAppointment(Agenda agenda, Appointment appointment)
        {
            //_connection.Open();
            using (SqlCommand command = new SqlCommand($"DELETE FROM tblAppointments WHERE idFromAgenda = '{agenda.Id}' AND appointmentName = '{appointment.Name}' AND dateStart = '{appointment.DateStart.ToString("yyyy-MM-dd HH:mm:ss")}'", _connection))
            {
                command.ExecuteNonQuery();
            }
            _adapterAgenda.Update(_dataset, "tblAppointments");
            //_connection.Close();
        }
        #endregion

        #region DataSet

        /// <summary>
        /// Deze functie maakt een nieuwe dataseten vult ze met de correcte
        /// tabellen uit de databank.
        /// </summary>
        private void NewDataSet()
        {
            _dataset = new DataSet();
            _adapterAppointment.Fill(_dataset, "tblAppointments");
            _adapterAgenda.Fill(_dataset, "tblAgendas");

            _dataset.Tables["tblAppointments"].Columns["appointmentId"].AutoIncrement = true;
            _dataset.Tables["tblAppointments"].Columns["appointmentId"].AutoIncrementSeed = -100;
            _dataset.Tables["tblAppointments"].Columns["appointmentId"].AutoIncrementStep = -1;

            _dataset.Tables["tblAgendas"].Columns["agendaId"].AutoIncrement = true;
            _dataset.Tables["tblAgendas"].Columns["agendaId"].AutoIncrementSeed = -100;
            _dataset.Tables["tblAgendas"].Columns["agendaId"].AutoIncrementStep = -1;

            _dataset.EnforceConstraints = false;
        }


        /// <summary>
        /// Deze functie legt de relatie tussen de tabellen.
        /// </summary>
        private void NewRelation()
        {
            DataRelation _AgendaAppointment;

            _AgendaAppointment = new DataRelation(
                "AgendaAppointment",
                _dataset.Tables["tblAgendas"].Columns["agendaId"],
                _dataset.Tables["tblAppointments"].Columns["idFromAgenda"]);

            _dataset.Relations.Add(_AgendaAppointment);
            _AgendaAppointment.ChildKeyConstraint.UpdateRule = Rule.Cascade;
        }
        #endregion

        #region Adapters

        /// <summary>
        /// Deze functie maakt een adapter.
        /// </summary>
        private void NewAdapterAgenda()
        {
            SqlParameter parameter;
            SqlCommandBuilder builder;

            _adapterAgenda = new SqlDataAdapter("select * from tblAgendas", _connection);

            builder = new SqlCommandBuilder(_adapterAgenda);
            _adapterAgenda.InsertCommand = builder.GetInsertCommand().Clone();
            _adapterAgenda.DeleteCommand = builder.GetDeleteCommand().Clone();
            _adapterAgenda.UpdateCommand = builder.GetUpdateCommand().Clone();

            builder.Dispose();

            _adapterAgenda.InsertCommand.CommandText += " set @newid = scope_identity()";

            parameter = new SqlParameter();
            parameter.ParameterName = "@newid";
            parameter.SqlDbType = SqlDbType.Int;
            parameter.Size = 4;
            parameter.Direction = ParameterDirection.Output;
            _adapterAgenda.InsertCommand.Parameters.Add(parameter);
            _adapterAgenda.RowUpdated += AdapterAgenda_RowUpdated;
        }

        /// <summary>
        /// Deze functie maakt een adapter
        /// </summary>
        private void NewAdapterAppointment()
        {
            SqlParameter parameter;
            SqlCommandBuilder builder;

            _adapterAppointment = new SqlDataAdapter("select * from tblAppointments", _connection);

            builder = new SqlCommandBuilder(_adapterAppointment);
            _adapterAppointment.InsertCommand = builder.GetInsertCommand().Clone();
            _adapterAppointment.DeleteCommand = builder.GetDeleteCommand().Clone();
            _adapterAppointment.UpdateCommand = builder.GetUpdateCommand().Clone();
            

            builder.Dispose();

            _adapterAppointment.InsertCommand.CommandText += " set @newid = scope_identity()";

            parameter = new SqlParameter();
            parameter.ParameterName = "@newid";
            parameter.SqlDbType = SqlDbType.Int;
            parameter.Size = 4;
            parameter.Direction = ParameterDirection.Output;
            _adapterAppointment.InsertCommand.Parameters.Add(parameter);
            _adapterAppointment.RowUpdated += AdapterAppointment_RowUpdated;
        }
        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        private void AdapterAgenda_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.StatementType == StatementType.Insert)
            {
                e.Row["agendaId"] = e.Command.Parameters["@newid"].Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AdapterAppointment_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.StatementType == StatementType.Insert)
            {
                e.Row["appointmentId"] = e.Command.Parameters["@newid"].Value;
            }
        }
        #endregion

        #region Requests

        /// <summary>
        /// Deze functie haalt de Agendas uit de databank op
        /// en geeft ze mee.
        /// </summary>
        public IList<String> RequestAgendas()
        {
            IList<String> requestListAgenda = new List<String>();
            foreach (DataRow rowAgenda in _dataset.Tables["tblAgendas"].Rows)
            {
                requestListAgenda.Add((String)rowAgenda["agendaName"]);
            }
            return requestListAgenda;
        }

        /// <summary>
        /// Deze functie haalt de appointments op voor een gegeven Agenda en geeft de appointments terug.
        /// </summary>
        public IList<Appointment> RequestAppointments(int id)
        {
            IList<Appointment> requestListAppointments = new List<Appointment>();
            foreach (DataRow rowAppointment in _dataset.Tables["tblAppointments"].Rows)
            {
                if ((int)rowAppointment["idFromAgenda"] == id)
                {
                    String name = (String)rowAppointment["appointmentName"];
                    DateTime dateStart = Convert.ToDateTime(rowAppointment["dateStart"]);
                    DateTime dateEnd = Convert.ToDateTime(rowAppointment["dateEnd"]);
                    String description = (String)rowAppointment["appointmentDescription"];
                    int idFromAgenda = (int)rowAppointment["idFromAgenda"];
                    Boolean priorityLevel = (Boolean)rowAppointment["priorityLevel"];
                    Appointment appointment = new Appointment(name, dateStart, dateEnd, description, idFromAgenda, priorityLevel);
                    requestListAppointments.Add(appointment);
                }
            }
            return requestListAppointments;
        }

        /// <summary>
        /// Deze haalt de Id op van een meegegeven agenda.
        /// </summary>
        public int RequestAgendaId(String agenda)
        {
            int agendaId= 1;
            foreach (DataRow rowX in _dataset.Tables["tblAgendas"].Rows)
            {
                if (rowX["agendaName"].ToString() == agenda)
                {
                    agendaId = Convert.ToInt32(rowX["agendaId"]);
                }
            }
            return agendaId;
        }
        #endregion
    }
}
