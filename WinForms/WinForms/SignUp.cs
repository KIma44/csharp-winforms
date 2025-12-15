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
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string name = txtName.Text.Trim();
            string pw = txtPw.Text.Trim();
            string pw2 = txtPw2.Text.Trim();

            // 1) 빈값 체크
            if (email == "" || name == "" || pw == "" || pw2 == "")
            {
                MessageBox.Show("모든 값을 입력해주세요.");
                return;
            }

            // 2) 이메일 형식 체크 (간단한 방식)
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("올바른 이메일 형식이 아닙니다.");
                return;
            }

            // 3) 비밀번호 일치 여부
            if (pw != pw2)
            {
                MessageBox.Show("비밀번호가 일치하지 않습니다.");
                return;
            }

            // 4) DB 저장
            using (MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=1q2w3e4r;database=money_calendar;"))
            {
                conn.Open();

                // 이메일 중복 체크
                MySqlCommand checkEmail = new MySqlCommand("SELECT COUNT(*) FROM user WHERE email = @email", conn);
                checkEmail.Parameters.AddWithValue("@email", email);

                int count = Convert.ToInt32(checkEmail.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("이미 존재하는 이메일입니다.");
                    return;
                }

                // 회원가입 INSERT
                MySqlCommand cmd = new MySqlCommand(
                    "INSERT INTO user (email, name, password) VALUES (@email, @name, @pw)", conn);

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@pw", pw);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("회원가입 완료!");
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void SignUp_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Home으로 이동하시겠습니까?",
                "확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
    );

            if (result == DialogResult.Yes)
            {
                this.Hide();              // 현재 폼 숨기기
                new Home().Show();        // Home 폼 열기
            }
        }
    }
}
