using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MVCClaseSeis.Models
{
    public static class Cnx
    {
        private static string cnx = "Server=localhost; Database=Payments; UserId=root; Password=;";
        public static MySqlConnection getCnx()
        {
            return new MySqlConnection(cnx);
        }
    }
}