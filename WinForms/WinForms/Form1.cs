using MySql.Data.MySqlClient;
using Mysqlx.Crud;
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
        private int? _userId;   // ⭐ nullable
        private string _userName;
        private bool _isGuest;

        private List<(DateTime date, string schedule, int cost)> guestSchedules
    = new List<(DateTime, string, int)>();

        // 하루 제한
        private int addCount = 0;
        private int deleteCount = 0;
        private int updateCount = 0;
        private DateTime lastActionDate = DateTime.Today;


        public Form1(int userId, string userName)
        {
            InitializeComponent();
            _userId = userId;
            _userName = userName;
            _isGuest = false;
            UserSession.UserId = userId;
            UserSession.UserName = userName;

        }

        // 비회원
        public Form1()
        {
            InitializeComponent();
            _userId = null;
            _userName = "비회원";
            _isGuest = true;

            UserSession.UserId = null;
            UserSession.UserName = "비회원";
        }

        private string connStr =
        "Server=localhost;Database=money_calendar;Uid=root;Pwd=1q2w3e4r;Charset=utf8;";




        private void ResetIfNewDay()
        {
            if (lastActionDate != DateTime.Today)
            {
                addCount = 0;
                deleteCount = 0;
                updateCount = 0;
                lastActionDate = DateTime.Today;
            }
        }

        private int GetDailyLimit()
        {
            return _isGuest ? 3 : 10;
        }

        private bool CheckLimit(ref int count)
        {
            ResetIfNewDay();

            int limit = GetDailyLimit();
            if (count >= limit)
            {
                MessageBox.Show($"하루에 {limit}번까지만 가능합니다.");
                return false;
            }

            count++;
            return true;
        }

        private void UpdateUI()
        {
            lblUserName.Text = $"{_userName} 로그인 중";

            if (_isGuest)
            {
                labelTotalCost.Visible = false;
            }
            else
            {
                labelTotalCost.Visible = true;
                LoadTotalCost();
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            dateTimePicker1.Value = monthCalendar1.SelectionStart;

            if (!_isGuest)
            {
                LoadSchedules(monthCalendar1.SelectionStart);
                LoadTotalCost();
            }

            UpdateUI();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

            dateTimePicker1.Value = monthCalendar1.SelectionStart;

            if (!_isGuest)
            {
                LoadSchedules(monthCalendar1.SelectionStart);
                LoadTotalCost();
            }
        }

        private void LoadGuestSchedules(DateTime date)
        {
            listViewSchedule.Items.Clear();

            foreach (var s in guestSchedules.Where(x => x.date == date.Date))
            {
                ListViewItem item = new ListViewItem(date.ToString("yyyy-MM-dd"));
                item.SubItems.Add(s.schedule);
                item.SubItems.Add(s.cost.ToString());

                listViewSchedule.Items.Add(item);
            }
        }

        private void btnAddSchedule_Click(object sender, EventArgs e)
        {
            if (!CheckLimit(ref addCount)) return;

            DateTime date = dateTimePicker1.Value;
            string schedule = textBoxSchedule.Text.Trim();

            if (string.IsNullOrEmpty(schedule))
            {
                MessageBox.Show("일정을 입력하세요.");
                return;
            }

            int cost = 0;
            int.TryParse(textBoxCost.Text, out cost);

            if (_isGuest)
            {
                // ✅ 비회원 → 로컬에만 저장
                guestSchedules.Add((date.Date, schedule, cost));
                LoadGuestSchedules(date.Date);
            }
            else
            {
                AddScheduleToDB(date.ToString("yyyy-MM-dd"), schedule);
                LoadSchedules(date);
                LoadTotalCost();
            }

            textBoxSchedule.Clear();
            textBoxCost.Clear();

        }

        private void LoadSchedules(DateTime date)
        {
            if (_isGuest) return;
            listViewSchedule.Items.Clear();

            string connStr = "Server=localhost;Database=money_calendar;Uid=root;Pwd=1q2w3e4r;Charset=utf8;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "SELECT id, schedule, cost\r\nFROM schedules\r\nWHERE date = @date AND user_id = @userId";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@date", date.Date);
                cmd.Parameters.AddWithValue("@userId", _userId);


                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(date.ToString("yyyy-MM-dd"));
                    item.SubItems.Add(reader.GetString("schedule"));
                    item.SubItems.Add(reader["cost"].ToString());

                    item.Tag = reader["id"];

                    listViewSchedule.Items.Add(item);

                }
            }
        }
        
        // 일정 추가
        private void AddScheduleToDB(string date, string schedule)
        {
            if (_isGuest) return;
            string connStr = "Server=localhost;Database=money_calendar;Uid=root;Pwd=1q2w3e4r;Charset=utf8;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = "INSERT INTO schedules(user_id ,date, schedule, cost) VALUES(@userId, @date, @schedule, @cost)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                // 인자로 들어온 date 사용
                cmd.Parameters.AddWithValue("@date", DateTime.Parse(date).Date);

                cmd.Parameters.AddWithValue("@schedule", schedule);

                int cost = 0;
                int.TryParse(textBoxCost.Text.Trim(), out cost);

                cmd.Parameters.AddWithValue("@cost", cost);
                cmd.Parameters.AddWithValue("@userId", _userId);

                cmd.ExecuteNonQuery();
            }
        }

        // 삭제 DB
        private void DeleteScheduleFromDB()
        {
            if (_isGuest) return;
            using (MySqlConnection conn = new MySqlConnection(connStr))
    {
                conn.Open();

                ListViewItem item = listViewSchedule.SelectedItems[0];
                int id = (int)item.Tag;

                string sql = "DELETE FROM schedules WHERE id = @id AND user_id = @userId";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@userId", _userId);

                cmd.ExecuteNonQuery();
            }
        }

        // 삭제 버튼 이벤트
        private void btnDeleteSchedule_Click(object sender, EventArgs e)
        {
            if (!CheckLimit(ref deleteCount)) return;

            if (listViewSchedule.SelectedItems.Count == 0)
            {
                MessageBox.Show("삭제할 일정을 선택하세요.");
                return;
            }

            if (_isGuest)
            {
                int index = listViewSchedule.SelectedItems[0].Index;
                guestSchedules.RemoveAt(index);
                LoadGuestSchedules(dateTimePicker1.Value);
            }
            else
            {
                DeleteScheduleFromDB();
                LoadSchedules(dateTimePicker1.Value);
                LoadTotalCost();

            }
        }
        // 수정 DB쪽 
        private void UpdateScheduleInDB(string newSchedule, int newCost)
        {
            if (_isGuest) return;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                ListViewItem item = listViewSchedule.SelectedItems[0];
                int id = (int)item.Tag;

                string sql = @"UPDATE schedules
                       SET schedule = @newSchedule, cost = @newCost
                       WHERE id = @id AND user_id = @userId";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@newSchedule", newSchedule);
                cmd.Parameters.AddWithValue("@newCost", newCost);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@userId", _userId);

                cmd.ExecuteNonQuery();
            }
        }
        // 버튼 수정
        private void btnUpdateSchedule_Click(object sender, EventArgs e)
        {
            if (!CheckLimit(ref updateCount)) return;

            if (listViewSchedule.SelectedItems.Count == 0)
            {
                MessageBox.Show("수정할 일정을 선택하세요.");
                return;
            }

            string newSchedule = textBoxSchedule.Text.Trim();
            int newCost = 0;
            int.TryParse(textBoxCost.Text, out newCost);

            if (_isGuest)
            {
                int index = listViewSchedule.SelectedItems[0].Index;
                guestSchedules[index] =
                    (dateTimePicker1.Value.Date, newSchedule, newCost);

                LoadGuestSchedules(dateTimePicker1.Value);
            }
            else
            {
                UpdateScheduleInDB(newSchedule, newCost);
                LoadSchedules(dateTimePicker1.Value);
                LoadTotalCost();
            }
        }

        private void listViewSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSchedule.SelectedItems.Count > 0)
            {
                ListViewItem item = listViewSchedule.SelectedItems[0];
                dateTimePicker1.Value = DateTime.Parse(item.SubItems[0].Text);
                textBoxSchedule.Text = item.SubItems[1].Text;
                textBoxCost.Text = item.SubItems[2].Text;
            }
        }

        private void labelTotal_Click(object sender, EventArgs e)
        {
            
        }

        private void LoadTotalCost()
        {
            if (UserSession.UserId == null)
                return;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                DateTime selected = dateTimePicker1.Value;

                string sql = @"
            SELECT SUM(cost)
            FROM schedules
            WHERE user_id = @userId
            AND YEAR(date) = @year
            AND MONTH(date) = @month";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@year", selected.Year);
                cmd.Parameters.AddWithValue("@month", selected.Month);
                cmd.Parameters.AddWithValue("@userId", _userId);

                object result = cmd.ExecuteScalar();
                int total = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                labelTotalCost.Text = $"{selected.Month}월 총 지출 금액: {total}원";
            }
        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadTotalCost();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("로그아웃 되었습니다.");

            this.Hide();
            new Home().Show();
        }
    }
}
