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
            string pw = txtPwLogin.Text.Trim();

            if (email == "" || pw == "")
            {
                MessageBox.Show("이메일과 비밀번호를 입력하세요.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=1q2w3e4r;database=money_calendar;"))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "SELECT name FROM user WHERE email = @email AND password = @pw", conn);

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pw", pw);

                var nickname = cmd.ExecuteScalar();

                if (nickname == null)
                {
                    MessageBox.Show("로그인 실패. 이메일 또는 비밀번호가 틀렸습니다.");
                    return;
                }

                MessageBox.Show($"{nickname}님 환영합니다!");

                Form1 form1 = new Form1();
                form1.Show();
                this.Close();
            }
        }
    }
}
