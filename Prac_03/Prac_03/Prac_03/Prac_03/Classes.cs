using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prac_03
{
    internal class User
    {
        public string Login;
        public string Pass;
        public string Name;
        public string Surname;
        public bool Disabled;
        public List<User> GetUsers(SqlConnection connection)
        {
            connection.Open();
            List<User> users = new List<User>();
            string command = "USE Prac_03_DB SELECT * FROM MainTable";
            SqlCommand cmd = new SqlCommand(command, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                User user = new User
                {
                    Login = reader.GetString(0),
                    Pass = reader.GetString(1),
                    Name = reader.GetString(2),
                    Surname = reader.GetString(3),
                    Disabled = reader.GetBoolean(4)
                };
                users.Add(user);
            }
            connection.Close();
            return users;
        }
    }
}
