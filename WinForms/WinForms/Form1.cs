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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = monthCalendar1.SelectionStart;

            // 오늘 일정 불러오기
            LoadSchedules(monthCalendar1.SelectionStart);

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            dateTimePicker1.Value = monthCalendar1.SelectionStart;
            LoadSchedules(monthCalendar1.SelectionStart);
        }

        private void btnAddSchedule_Click(object sender, EventArgs e)
        {
            DateTime date = dateTimePicker1.Value;
            string schedule = textBoxSchedule.Text.Trim();

            if (string.IsNullOrEmpty(schedule))
            {
                MessageBox.Show("일정을 입력하세요.");
                return;
            }

            // DB에 일정 추가
            AddScheduleToDB(date.ToString("yyyy-MM-dd"), schedule);

            // ListView 업데이트
            LoadSchedules(date);

            // 입력창 초기화
            textBoxSchedule.Clear();
        }

        private void LoadSchedules(DateTime date)
        {
            listViewSchedule.Items.Clear();

            string connStr = "Server=localhost;Database=money_calendar;Uid=root;Pwd=1q2w3e4r;Charset=utf8;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "SELECT `schedule` FROM `schedules` WHERE `date` = @date";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(date.ToString("yyyy-MM-dd"));
                    item.SubItems.Add(reader.GetString("schedule"));
                    listViewSchedule.Items.Add(item);
                }
            }
        }

        private void AddScheduleToDB(string date, string schedule)
        {
            string connStr = "Server=localhost;Database=money_calendar;Uid=root;Pwd=1q2w3e4r;Charset=utf8;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "INSERT INTO `schedules`(`date`, `schedule`) VALUES(@date, @schedule)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@schedule", schedule);

                cmd.ExecuteNonQuery();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        // 삭제 DB
        private void DeleteScheduleFromDB(string date, string schedule)
        {
            string connStr = "Server=localhost;Database=money_calendar;Uid=root;Pwd=1q2w3e4r;Charset=utf8;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "DELETE FROM schedules WHERE date = @date AND schedule = @schedule LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@schedule", schedule);

                cmd.ExecuteNonQuery();
            }
        }

        // 삭제 버튼 이벤트
        private void btnDeleteSchedule_Click(object sender, EventArgs e)
        {
            if (listViewSchedule.SelectedItems.Count == 0)
            {
                MessageBox.Show("삭제할 일정을 선택하세요.");
                return;
            }

            // 선택된 항목 가져오기
            ListViewItem item = listViewSchedule.SelectedItems[0];
            string date = item.SubItems[0].Text;
            string schedule = item.SubItems[1].Text;

            // DB에서 삭제
            DeleteScheduleFromDB(date, schedule);

            // ListView 새로고침
            LoadSchedules(DateTime.Parse(date));

            MessageBox.Show("일정이 삭제되었습니다.");
        }
        // DB 수정
        private void UpdateScheduleInDB(string date, string oldSchedule, string newSchedule)
        {
            string connStr = "Server=localhost;Database=money_calendar;Uid=root;Pwd=1q2w3e4r;Charset=utf8;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "UPDATE schedules SET schedule = @newSchedule WHERE date = @date AND schedule = @oldSchedule LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@newSchedule", newSchedule);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@oldSchedule", oldSchedule);

                cmd.ExecuteNonQuery();
            }
        }
        // 버튼 수정
        private void btnUpdateSchedule_Click(object sender, EventArgs e)
        {
            if (listViewSchedule.SelectedItems.Count == 0)
            {
                MessageBox.Show("수정할 일정을 선택하세요.");
                return;
            }

            string newSchedule = textBoxSchedule.Text.Trim();
            if (string.IsNullOrEmpty(newSchedule))
            {
                MessageBox.Show("새로운 일정 내용을 입력하세요.");
                return;
            }

            // 선택된 기존 일정
            ListViewItem item = listViewSchedule.SelectedItems[0];
            string date = item.SubItems[0].Text;
            string oldSchedule = item.SubItems[1].Text;

            // DB에서 UPDATE 실행
            UpdateScheduleInDB(date, oldSchedule, newSchedule);

            // 화면 새로고침
            LoadSchedules(DateTime.Parse(date));

            MessageBox.Show("일정이 수정되었습니다.");
            textBoxSchedule.Clear();
        }

        private void listViewSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSchedule.SelectedItems.Count > 0)
            {
                ListViewItem item = listViewSchedule.SelectedItems[0];
                dateTimePicker1.Value = DateTime.Parse(item.SubItems[0].Text);
                textBoxSchedule.Text = item.SubItems[1].Text;
            }
        }
    }
}
