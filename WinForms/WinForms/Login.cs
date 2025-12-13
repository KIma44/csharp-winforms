using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmailLogin.Text.Trim();
            string password = txtPwLogin.Text.Trim();

            string connStr = "Server=localhost;Database=money_calendar;Uid=root;Pwd=1q2w3e4r;Charset=utf8;";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
            SELECT id, name
            FROM user
            WHERE email = @email AND password = @password";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int userId = reader.GetInt32("id");
                    string userName = reader.GetString("name");

                    MessageBox.Show("로그인 성공");

                    Form1 form = new Form1(userId, userName);
                    form.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("이메일 또는 비밀번호가 틀렸습니다.");
                }
            }
        }
    }
}
