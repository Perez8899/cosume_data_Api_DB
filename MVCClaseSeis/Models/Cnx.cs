using MySql.Data.MySqlClient;


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