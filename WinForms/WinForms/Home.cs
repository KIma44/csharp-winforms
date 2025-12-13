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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Login sign = new Login();
            sign.Show();
            this.Hide();  // 홈 화면 숨김
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SignUp login = new SignUp();
            login.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnGuestLogin_Click(object sender, EventArgs e)
        {
            Program.IsLoggedIn = false;
            Program.IsGuest = true;
            Program.UserId = "GUEST";

            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}
