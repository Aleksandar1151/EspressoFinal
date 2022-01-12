using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EspressoFinal.Data
{
    class Database
    {
        public static MySqlConnection dbConn;
        public static void InitializeDB()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["myDatabaseConnection"].ConnectionString;
            dbConn = new MySqlConnection(connectionString);

        }
    }
}
