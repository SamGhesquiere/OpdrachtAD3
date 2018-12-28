using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kalenders
{
    public class Databank
    {

        public SqlConnection Con { get; set; }
        private string constring { get; set; }
        public SqlCommandBuilder builder { get; set; }
        public SqlParameter parameter { get; set; }

        public SqlConnection Connection()
        {
            constring = @"Data Source=(localdb)\mssqllocaldb;Initial Catalog=dbKalenders;Integrated Security=True";
            return Con = new SqlConnection(constring);
 
        }

        
        

    }
}
