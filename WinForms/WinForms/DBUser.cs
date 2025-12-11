using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinForms
{
    internal class DBUser
    {
        private const string ConnString =
      "Server=localhost;Database=money_calendar;Uid=root;Pwd=1q2w3e4r;Charset=utf8;";
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnString);
        }
    }
}
