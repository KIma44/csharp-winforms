namespace WinForms
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.textBoxSchedule = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnAddSchedule = new System.Windows.Forms.Button();
            this.listViewSchedule = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelTotalCost = new System.Windows.Forms.Label();
            this.btnDeleteSchedule = new System.Windows.Forms.Button();
            this.btnUpdateSchedule = new System.Windows.Forms.Button();
            this.textBoxCost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblUserName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBoxBudget = new System.Windows.Forms.TextBox();
            this.btnSaveBudget = new System.Windows.Forms.Button();
            this.labelBudgetStatus = new System.Windows.Forms.Label();
            this.btnIncreaseBudget = new System.Windows.Forms.Button();
            this.btnDecreaseBudget = new System.Windows.Forms.Button();
            this.numericBudgetStep = new System.Windows.Forms.NumericUpDown();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.날짜 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.일정 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.금액 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericBudgetStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(151, 223);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
            // 
            // textBoxSchedule
            // 
            this.textBoxSchedule.Location = new System.Drawing.Point(237, 425);
            this.textBoxSchedule.Name = "textBoxSchedule";
            this.textBoxSchedule.Size = new System.Drawing.Size(216, 21);
            this.textBoxSchedule.TabIndex = 1;
            this.textBoxSchedule.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(151, 175);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(220, 21);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // btnAddSchedule
            // 
            this.btnAddSchedule.Location = new System.Drawing.Point(92, 599);
            this.btnAddSchedule.Name = "btnAddSchedule";
            this.btnAddSchedule.Size = new System.Drawing.Size(75, 23);
            this.btnAddSchedule.TabIndex = 3;
            this.btnAddSchedule.Text = "일정 추가";
            this.btnAddSchedule.UseVisualStyleBackColor = true;
            this.btnAddSchedule.Click += new System.EventHandler(this.btnAddSchedule_Click);
            // 
            // listViewSchedule
            // 
            this.listViewSchedule.CheckBoxes = true;
            this.listViewSchedule.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewSchedule.FullRowSelect = true;
            this.listViewSchedule.GridLines = true;
            this.listViewSchedule.HideSelection = false;
            this.listViewSchedule.Location = new System.Drawing.Point(546, 197);
            this.listViewSchedule.Name = "listViewSchedule";
            this.listViewSchedule.Size = new System.Drawing.Size(444, 473);
            this.listViewSchedule.TabIndex = 4;
            this.listViewSchedule.UseCompatibleStateImageBehavior = false;
            this.listViewSchedule.View = System.Windows.Forms.View.Details;
            this.listViewSchedule.SelectedIndexChanged += new System.EventHandler(this.listViewSchedule_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "날짜";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "일정";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "지출 금액";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 120;
            // 
            // labelTotalCost
            // 
            this.labelTotalCost.AutoSize = true;
            this.labelTotalCost.Location = new System.Drawing.Point(42, 108);
            this.labelTotalCost.Name = "labelTotalCost";
            this.labelTotalCost.Size = new System.Drawing.Size(59, 12);
            this.labelTotalCost.TabIndex = 5;
            this.labelTotalCost.Text = "총합 : 0원";
            this.labelTotalCost.Click += new System.EventHandler(this.labelTotal_Click);
            // 
            // btnDeleteSchedule
            // 
            this.btnDeleteSchedule.Location = new System.Drawing.Point(358, 599);
            this.btnDeleteSchedule.Name = "btnDeleteSchedule";
            this.btnDeleteSchedule.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteSchedule.TabIndex = 6;
            this.btnDeleteSchedule.Text = "삭제";
            this.btnDeleteSchedule.UseVisualStyleBackColor = true;
            this.btnDeleteSchedule.Click += new System.EventHandler(this.btnDeleteSchedule_Click);
            // 
            // btnUpdateSchedule
            // 
            this.btnUpdateSchedule.Location = new System.Drawing.Point(222, 599);
            this.btnUpdateSchedule.Name = "btnUpdateSchedule";
            this.btnUpdateSchedule.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateSchedule.TabIndex = 7;
            this.btnUpdateSchedule.Text = "수정";
            this.btnUpdateSchedule.UseVisualStyleBackColor = true;
            this.btnUpdateSchedule.Click += new System.EventHandler(this.btnUpdateSchedule_Click);
            // 
            // textBoxCost
            // 
            this.textBoxCost.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBoxCost.Location = new System.Drawing.Point(237, 514);
            this.textBoxCost.Name = "textBoxCost";
            this.textBoxCost.Size = new System.Drawing.Size(216, 21);
            this.textBoxCost.TabIndex = 8;
            this.textBoxCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxCost.TextChanged += new System.EventHandler(this.textBoxCost_TextChanged);
            this.textBoxCost.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxCost_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(90, 434);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "일정을 입력하세요 : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 523);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "지출금액 :";
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(1104, 12);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLogout.TabIndex = 13;
            this.btnLogout.Text = "로그아웃";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(1104, 12);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 14;
            this.btnLogin.Text = "로그인";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(22, 12);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(0, 12);
            this.lblUserName.TabIndex = 15;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1132, 672);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // textBoxBudget
            // 
            this.textBoxBudget.Location = new System.Drawing.Point(678, 131);
            this.textBoxBudget.Name = "textBoxBudget";
            this.textBoxBudget.Size = new System.Drawing.Size(140, 21);
            this.textBoxBudget.TabIndex = 17;
            // 
            // btnSaveBudget
            // 
            this.btnSaveBudget.Location = new System.Drawing.Point(915, 82);
            this.btnSaveBudget.Name = "btnSaveBudget";
            this.btnSaveBudget.Size = new System.Drawing.Size(75, 23);
            this.btnSaveBudget.TabIndex = 18;
            this.btnSaveBudget.Text = "저장";
            this.btnSaveBudget.UseVisualStyleBackColor = true;
            this.btnSaveBudget.Click += new System.EventHandler(this.btnSaveBudget_Click);
            // 
            // labelBudgetStatus
            // 
            this.labelBudgetStatus.AutoSize = true;
            this.labelBudgetStatus.Location = new System.Drawing.Point(501, 134);
            this.labelBudgetStatus.Name = "labelBudgetStatus";
            this.labelBudgetStatus.Size = new System.Drawing.Size(29, 12);
            this.labelBudgetStatus.TabIndex = 19;
            this.labelBudgetStatus.Text = "      ";
            // 
            // btnIncreaseBudget
            // 
            this.btnIncreaseBudget.Location = new System.Drawing.Point(688, 82);
            this.btnIncreaseBudget.Name = "btnIncreaseBudget";
            this.btnIncreaseBudget.Size = new System.Drawing.Size(75, 23);
            this.btnIncreaseBudget.TabIndex = 20;
            this.btnIncreaseBudget.Text = "+";
            this.btnIncreaseBudget.UseVisualStyleBackColor = true;
            this.btnIncreaseBudget.Click += new System.EventHandler(this.btnIncreaseBudget_Click);
            // 
            // btnDecreaseBudget
            // 
            this.btnDecreaseBudget.Location = new System.Drawing.Point(797, 82);
            this.btnDecreaseBudget.Name = "btnDecreaseBudget";
            this.btnDecreaseBudget.Size = new System.Drawing.Size(75, 23);
            this.btnDecreaseBudget.TabIndex = 21;
            this.btnDecreaseBudget.Text = "-";
            this.btnDecreaseBudget.UseVisualStyleBackColor = true;
            this.btnDecreaseBudget.Click += new System.EventHandler(this.btnDecreaseBudget_Click_1);
            // 
            // numericBudgetStep
            // 
            this.numericBudgetStep.Location = new System.Drawing.Point(870, 132);
            this.numericBudgetStep.Name = "numericBudgetStep";
            this.numericBudgetStep.Size = new System.Drawing.Size(120, 21);
            this.numericBudgetStep.TabIndex = 22;
            this.numericBudgetStep.ValueChanged += new System.EventHandler(this.numericBudgetStep_ValueChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.날짜,
            this.일정,
            this.금액});
            this.dataGridView1.Location = new System.Drawing.Point(599, 197);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(339, 473);
            this.dataGridView1.TabIndex = 23;
            // 
            // 날짜
            // 
            this.날짜.HeaderText = "날짜";
            this.날짜.Name = "날짜";
            // 
            // 일정
            // 
            this.일정.HeaderText = "일정";
            this.일정.Name = "일정";
            // 
            // 금액
            // 
            this.금액.HeaderText = "금액";
            this.금액.Name = "금액";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1191, 723);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.numericBudgetStep);
            this.Controls.Add(this.btnDecreaseBudget);
            this.Controls.Add(this.btnIncreaseBudget);
            this.Controls.Add(this.labelBudgetStatus);
            this.Controls.Add(this.btnSaveBudget);
            this.Controls.Add(this.textBoxBudget);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxCost);
            this.Controls.Add(this.btnUpdateSchedule);
            this.Controls.Add(this.btnDeleteSchedule);
            this.Controls.Add(this.labelTotalCost);
            this.Controls.Add(this.listViewSchedule);
            this.Controls.Add(this.btnAddSchedule);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.textBoxSchedule);
            this.Controls.Add(this.monthCalendar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericBudgetStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.TextBox textBoxSchedule;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnAddSchedule;
        private System.Windows.Forms.ListView listViewSchedule;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnDeleteSchedule;
        private System.Windows.Forms.Button btnUpdateSchedule;
        private System.Windows.Forms.TextBox textBoxCost;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label labelTotalCost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBoxBudget;
        private System.Windows.Forms.Button btnSaveBudget;
        private System.Windows.Forms.Label labelBudgetStatus;
        private System.Windows.Forms.Button btnIncreaseBudget;
        private System.Windows.Forms.Button btnDecreaseBudget;
        private System.Windows.Forms.NumericUpDown numericBudgetStep;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 날짜;
        private System.Windows.Forms.DataGridViewTextBoxColumn 일정;
        private System.Windows.Forms.DataGridViewTextBoxColumn 금액;
    }
}

