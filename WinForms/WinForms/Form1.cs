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
        private int _monthlyBudget = 0;


        private List<(DateTime date, string schedule, int cost)> guestSchedules
         = new List<(DateTime, string, int)>();

        // 하루 제한
        private int addCount = 0;
        private int deleteCount = 0;
        private int updateCount = 0;
        private DateTime lastActionDate = DateTime.Today;

        // 비회원
        public Form1()
        {
            InitializeComponent();
            textBoxCost.KeyPress += textBoxCost_KeyPress;
            textBoxCost.TextChanged += textBoxCost_TextChanged;
            _userId = null;
            _userName = "비회원";
            _isGuest = true;

            UserSession.UserId = null;
            UserSession.UserName = "비회원";
        }

        public Form1(int userId, string userName)
        {
            InitializeComponent();
            textBoxCost.KeyPress += textBoxCost_KeyPress;
            textBoxCost.TextChanged += textBoxCost_TextChanged;
            _userId = userId;
            _userName = userName;
            _isGuest = false;

            UserSession.UserId = userId;
            UserSession.UserName = userName;

            // ⭐ UserSession에 저장된 비회원 일정 사용
            guestSchedules = UserSession.GuestSchedules ??
                new List<(DateTime, string, int)>();

            MoveGuestSchedulesToMember();

            LoadMonthlyBudget(); // 로그인 시 기존 예산 불러오기
            CheckBudgetWarning(); // 예산 상태 표시
        }


        private string connStr =
        "Server=localhost;Database=money_calendar;Uid=root;Pwd=1q2w3e4r;Charset=utf8;";


        private void MoveGuestSchedulesToMember()
        {
            if (guestSchedules == null || guestSchedules.Count == 0)
                return;

            DialogResult result = MessageBox.Show(
                "비회원 일정이 있습니다.\n회원 계정으로 저장할까요?",
                "일정 이전",
                MessageBoxButtons.YesNo
            );

            if (result == DialogResult.Yes)
            {
                foreach (var s in guestSchedules)
                {
                    AddScheduleToDB(
                        s.date.ToString("yyyy-MM-dd"),
                        s.schedule, s.cost
                    );
                }
                guestSchedules.Clear();
            }
        }


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
                labelBudgetStatus.Visible = false;
                btnLogout.Visible = false;
                btnLogin.Visible = true;
            }
            else
            {
                labelTotalCost.Visible = true;
                labelBudgetStatus.Visible = true; // 반드시 표시
                LoadTotalCost();
                CheckBudgetWarning();

                btnLogout.Visible = true;
                btnLogin.Visible = false;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            listViewSchedule.Columns[0].TextAlign = HorizontalAlignment.Center;
            listViewSchedule.Columns[1].TextAlign = HorizontalAlignment.Center;
            listViewSchedule.Columns[2].TextAlign = HorizontalAlignment.Center;

            listViewSchedule.CheckBoxes = true;
            listViewSchedule.FullRowSelect = true;
            listViewSchedule.MultiSelect = true;
            listViewSchedule.View = View.Details;

            dateTimePicker1.Value = monthCalendar1.SelectionStart;

            if (_isGuest)
            {
                LoadGuestSchedules(monthCalendar1.SelectionStart);
            }
            else
            {
                LoadSchedules(monthCalendar1.SelectionStart);
                LoadTotalCost();
                LoadMonthlyBudget();   // 해당 달 예산 불러오기
                CheckBudgetWarning();  // 예산 상태 표시
            }



            UpdateUI();
        }

        private void LoadMonthlyBudget()
        {
            if (_isGuest) return;

            DateTime selected = dateTimePicker1.Value; // 현재 달 기준

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
                            SELECT budget 
                            FROM monthly_budget 
                            WHERE user_id = @userId 
                              AND year = @year 
                              AND month = @month";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", _userId);
                cmd.Parameters.AddWithValue("@year", selected.Year);
                cmd.Parameters.AddWithValue("@month", selected.Month);

                object result = cmd.ExecuteScalar();
                _monthlyBudget = (result != null) ? Convert.ToInt32(result) : 0;

                textBoxBudget.Text = _monthlyBudget.ToString(); // 기존 예산 표시
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

            dateTimePicker1.Value = monthCalendar1.SelectionStart;

            if (_isGuest)
            {
                LoadGuestSchedules(monthCalendar1.SelectionStart);
            }
            else
            {
                LoadSchedules(monthCalendar1.SelectionStart);
                LoadMonthlyBudget();   // 해당 달 예산 불러오기
                LoadTotalCost();
                CheckBudgetWarning();
            }
        }

        private void LoadGuestSchedules(DateTime date)
        {
            listViewSchedule.Items.Clear();

            foreach (var s in guestSchedules.Where(x => x.date == date.Date))
            {
                ListViewItem item = new ListViewItem(date.ToString("yyyy-MM-dd"));
                item.SubItems.Add(s.schedule);
                item.SubItems.Add("₩ " + s.cost.ToString("#,0"));

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

            int cost = GetCostValue();

            if (_isGuest)
            {
                //  비회원 → 로컬에만 저장
                guestSchedules.Add((date.Date, schedule, cost));
                LoadGuestSchedules(date.Date);
            }
            else
            {
                AddScheduleToDB(date.ToString("yyyy-MM-dd"), schedule, cost);
                LoadSchedules(date);
                LoadTotalCost();
                CheckBudgetWarning();
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
                    item.SubItems.Add("₩ " + Convert.ToInt32(reader["cost"]).ToString("#,0"));

                    item.Tag = reader["id"];

                    listViewSchedule.Items.Add(item);

                }
            }
        }
        
        // 일정 추가
        private void AddScheduleToDB(string date, string schedule, int cost)
        {
            if (_isGuest) return;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = @"INSERT INTO schedules(user_id, date, schedule, cost)
                       VALUES(@userId, @date, @schedule, @cost)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", _userId);
                cmd.Parameters.AddWithValue("@date", DateTime.Parse(date).Date);
                cmd.Parameters.AddWithValue("@schedule", schedule);
                cmd.Parameters.AddWithValue("@cost", cost);

                cmd.ExecuteNonQuery();
            }
        }

        private void DeleteGuestSchedules()
        {
            var checkedIndexes = listViewSchedule.CheckedItems
                  .Cast<ListViewItem>()
                  .Select(item => item.Index)
                  .OrderByDescending(i => i)
                  .ToList();


            foreach (int index in checkedIndexes)
            {
                guestSchedules.RemoveAt(index);
            }

            LoadGuestSchedules(dateTimePicker1.Value);
        }

        // 삭제 DB
        private void DeleteScheduleFromDB(List<int> ids)
        {
            if (_isGuest || ids.Count == 0) return;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                // IN (@id1, @id2, ...)
                string paramNames = string.Join(",",
                    ids.Select((id, index) => $"@id{index}"));

                string sql = $@"
                            DELETE FROM schedules
                            WHERE user_id = @userId
                            AND id IN ({paramNames})";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", _userId);

                for (int i = 0; i < ids.Count; i++)
                {
                    cmd.Parameters.AddWithValue($"@id{i}", ids[i]);
                }

                cmd.ExecuteNonQuery();
            }
        }

        // 삭제 버튼 이벤트
        private void btnDeleteSchedule_Click(object sender, EventArgs e)
        {
            if (!CheckLimit(ref deleteCount)) return;

            if (listViewSchedule.CheckedItems.Count == 0)
            {
                MessageBox.Show("삭제할 일정을 선택하세요.");
                return;
            }

            DialogResult result = MessageBox.Show(
                $"{listViewSchedule.CheckedItems.Count}개 일정을 삭제하시겠습니까?",
                "삭제 확인",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result != DialogResult.Yes) return;

            if (_isGuest)
            {
                DeleteGuestSchedules();
            }
            else
            {
                List<int> ids = listViewSchedule.CheckedItems
                .Cast<ListViewItem>()
                .Select(item => (int)item.Tag)
                .ToList();


                DeleteScheduleFromDB(ids);
                LoadSchedules(dateTimePicker1.Value);
                LoadTotalCost();
                CheckBudgetWarning();
            }

            ClearInput();

        }
        // 수정 DB쪽 
        private void UpdateScheduleInDB(string newSchedule, int newCost)
        {
            if (_isGuest) return;

            DateTime selected = dateTimePicker1.Value;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string sql = @"
            INSERT INTO monthly_budget (user_id, year, month, budget)
            VALUES (@userId, @year, @month, @budget)
            ON DUPLICATE KEY UPDATE budget = @budget";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", _userId);
                cmd.Parameters.AddWithValue("@year", selected.Year);
                cmd.Parameters.AddWithValue("@month", selected.Month);
                cmd.Parameters.AddWithValue("@budget", _monthlyBudget);
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateBudgetInDB()
        {
            if (_isGuest) return; // 비회원이면 저장 안 함

            DateTime selected = dateTimePicker1.Value;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
            INSERT INTO monthly_budget (user_id, year, month, budget)
            VALUES (@userId, @year, @month, @budget)
            ON DUPLICATE KEY UPDATE budget = @budget";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", _userId);
                cmd.Parameters.AddWithValue("@year", selected.Year);
                cmd.Parameters.AddWithValue("@month", selected.Month);
                cmd.Parameters.AddWithValue("@budget", _monthlyBudget);

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
            int newCost = GetCostValue();

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
                CheckBudgetWarning();
            }
            ClearInput();

        }

        private void listViewSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewSchedule.SelectedItems.Count > 0)
            {
                ListViewItem item = listViewSchedule.SelectedItems[0];
                dateTimePicker1.Value = DateTime.Parse(item.SubItems[0].Text);
                textBoxSchedule.Text = item.SubItems[1].Text;
                textBoxCost.Text = item.SubItems[2].Text
                .Replace("₩", "")
                .Replace(",", "")
                .Trim();

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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // 비회원 일정 UserSession에 저장
            if (_isGuest)
            {
                UserSession.GuestSchedules = guestSchedules;
            }

            this.Hide();
            new Login().Show();
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
                this.Hide();              
                new Home().Show();        
            }
        }

        private void textBoxCost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxCost_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxCost.Text))
                return;

            // ₩, 콤마 제거
            string numericText = textBoxCost.Text
                .Replace("₩", "")
                .Replace(",", "")
                .Trim();

            if (!int.TryParse(numericText, out int value))
                return;

            // ₩ + 천 단위 콤마
            textBoxCost.Text = "₩ " + value.ToString("#,0");

            // 커서 맨 뒤로
            textBoxCost.SelectionStart = textBoxCost.Text.Length;
        }


        private int GetCostValue()
        {
            if (string.IsNullOrWhiteSpace(textBoxCost.Text))
                return 0;

            string text = textBoxCost.Text
                .Replace("₩", "")
                .Replace(",", "")
                .Trim();

            int.TryParse(text, out int cost);
            return cost;
        }
        private void ClearInput()
        {
            textBoxSchedule.Clear();
            textBoxCost.Clear();

            foreach (ListViewItem item in listViewSchedule.Items)
                item.Checked = false;

            listViewSchedule.SelectedItems.Clear();
        }

        private void CheckBudgetWarning()
        {
            if (_isGuest)
            {
                labelBudgetStatus.Visible = false;
                return;
            }

            labelBudgetStatus.Visible = true; // 항상 표시

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
                cmd.Parameters.AddWithValue("@userId", _userId);
                cmd.Parameters.AddWithValue("@year", selected.Year);
                cmd.Parameters.AddWithValue("@month", selected.Month);

                object result = cmd.ExecuteScalar();
                int total = (result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                if (_monthlyBudget <= 0)
                {
                    labelBudgetStatus.Text = "예산이 설정되지 않았습니다";
                    labelBudgetStatus.ForeColor = Color.Black;
                }
                else if (total > _monthlyBudget)
                {
                    labelBudgetStatus.Text = "⚠ 예산을 초과했습니다!";
                    labelBudgetStatus.ForeColor = Color.Red;
                }
                else
                {
                    labelBudgetStatus.Text = "✔ 예산 내에서 사용 중";
                    labelBudgetStatus.ForeColor = Color.Green;
                }
            }
        }

        private void textBoxBudget_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxBudget.Text, out int budget))
            {
                _monthlyBudget = budget;
                CheckBudgetWarning();
            }
        }

        private void btnSaveBudget_Click(object sender, EventArgs e)
        {

            if (_isGuest) return;

            DateTime selected = dateTimePicker1.Value;
            string input = textBoxBudget.Text.Trim();
            int newBudget = _monthlyBudget;

            if (!int.TryParse(input, out newBudget) || newBudget < 0)
            {
                MessageBox.Show("올바른 예산 금액을 입력하세요.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string sql = @"
            INSERT INTO monthly_budget (user_id, year, month, budget)
            VALUES (@userId, @year, @month, @budget)
            ON DUPLICATE KEY UPDATE budget = @budget"; // 이미 있으면 업데이트

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userId", _userId);
                cmd.Parameters.AddWithValue("@year", selected.Year);
                cmd.Parameters.AddWithValue("@month", selected.Month);
                cmd.Parameters.AddWithValue("@budget", newBudget);

                cmd.ExecuteNonQuery();
            }

            _monthlyBudget = newBudget;
            MessageBox.Show("월 예산이 저장되었습니다.");
            CheckBudgetWarning();
        }

        private void btnIncreaseBudget_Click(object sender, EventArgs e)
        {
            int amount = (int)numericBudgetStep.Value;
            _monthlyBudget += amount;

            textBoxBudget.Text = _monthlyBudget.ToString();
            CheckBudgetWarning();
            UpdateBudgetInDB(); // DB 반영
        }

      

        private void numericBudgetStep_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnDecreaseBudget_Click_1(object sender, EventArgs e)
        {
            int amount = (int)numericBudgetStep.Value;
            _monthlyBudget -= amount;
            if (_monthlyBudget < 0) _monthlyBudget = 0;

            textBoxBudget.Text = _monthlyBudget.ToString();
            CheckBudgetWarning();
            UpdateBudgetInDB(); // DB 반영
        }
    }
}


