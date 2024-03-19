using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTP5125_Cumulative1.Models
{
    public class SchoolDbContext
    {

        private static string User { get { return "root"; } }
        private static string Password { get { return "root"; } }
        private static string Database { get { return "school"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

        protected static string ConnectionString
        {
            get
            {
                //convert zero datetime is a db connection setting which returns NULL if the date is 0000-00-00
                //this can allow C# to have an easier interpretation of the date (no date instead of 0 BCE)

                return "server = " + Server
                    + "; user = " + User
                    + "; database = " + Database
                    + "; port = " + Port
                    + "; password = " + Password
                    + "; convert zero datetime = True";
            }
        }
        //Instantiate the MySqlConnection class to connect to the required school database
        public MySqlConnection AccessDatabase()
        {
            //We are instantiating the MySqlConnection Class to create an object
            return new MySqlConnection(ConnectionString);
        }
    }
}