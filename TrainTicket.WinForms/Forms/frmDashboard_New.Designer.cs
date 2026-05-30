namespace TrainTicket.WinForms.Forms
{
    partial class frmDashboard_New
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            _header = new Panel();
            _lblPageTitle = new Label();
            _lblBreadcrumb = new Label();
            _cardRow = new Panel();
            _pnlCardTickets = new Panel();
            _lblIconTickets = new Label();
            _lblTitleTickets = new Label();
            _lblTicketsVal = new Label();
            _lblSubTickets = new Label();
            _pnlCardUsers = new Panel();
            _lblIconUsers = new Label();
            _lblTitleUsers = new Label();
            _lblUsersVal = new Label();
            _lblSubUsers = new Label();
            _pnlCardSchedules = new Panel();
            _lblIconSchedules = new Label();
            _lblTitleSchedules = new Label();
            _lblSchedulesVal = new Label();
            _lblSubSchedules = new Label();
            _pnlCardRevenue = new Panel();
            _lblIconRevenue = new Label();
            _lblTitleRevenue = new Label();
            _lblRevenueVal = new Label();
            _lblSubRevenue = new Label();
            _body = new Panel();
            _gridPanel = new Panel();
            _gridRecent = new Guna2DataGridView();
            _lblGridTitle = new Label();
            _header.SuspendLayout();
            _cardRow.SuspendLayout();
            _pnlCardTickets.SuspendLayout();
            _pnlCardUsers.SuspendLayout();
            _pnlCardSchedules.SuspendLayout();
            _pnlCardRevenue.SuspendLayout();
            _body.SuspendLayout();
            _gridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_gridRecent).BeginInit();
            SuspendLayout();
            // 
            // _header
            // 
            _header.BackColor = Color.FromArgb(30, 41, 59);
            _header.Controls.Add(_lblPageTitle);
            _header.Controls.Add(_lblBreadcrumb);
            _header.Dock = DockStyle.Top;
            _header.Location = new Point(0, 0);
            _header.Name = "_header";
            _header.Size = new Size(1200, 56);
            _header.TabIndex = 0;
            // 
            // _lblPageTitle
            // 
            _lblPageTitle.AutoSize = true;
            _lblPageTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            _lblPageTitle.ForeColor = Color.White;
            _lblPageTitle.Location = new Point(20, 14);
            _lblPageTitle.Name = "_lblPageTitle";
            _lblPageTitle.Size = new Size(141, 35);
            _lblPageTitle.TabIndex = 0;
            _lblPageTitle.Text = "Dashboard";
            // 
            // _lblBreadcrumb
            // 
            _lblBreadcrumb.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblBreadcrumb.AutoSize = true;
            _lblBreadcrumb.Font = new Font("Segoe UI", 9F);
            _lblBreadcrumb.ForeColor = Color.FromArgb(148, 163, 184);
            _lblBreadcrumb.Location = new Point(1030, 18);
            _lblBreadcrumb.Name = "_lblBreadcrumb";
            _lblBreadcrumb.Size = new Size(137, 20);
            _lblBreadcrumb.TabIndex = 1;
            _lblBreadcrumb.Text = "Home / Dashboard";
            // 
            // _cardRow
            // 
            _cardRow.BackColor = Color.Transparent;
            _cardRow.Controls.Add(_pnlCardTickets);
            _cardRow.Controls.Add(_pnlCardUsers);
            _cardRow.Controls.Add(_pnlCardSchedules);
            _cardRow.Controls.Add(_pnlCardRevenue);
            _cardRow.Dock = DockStyle.Top;
            _cardRow.Location = new Point(0, 56);
            _cardRow.Name = "_cardRow";
            _cardRow.Padding = new Padding(16, 16, 16, 0);
            _cardRow.Size = new Size(1200, 130);
            _cardRow.TabIndex = 1;
            // 
            // _pnlCardTickets
            // 
            _pnlCardTickets.BackColor = Color.Transparent;
            _pnlCardTickets.Controls.Add(_lblIconTickets);
            _pnlCardTickets.Controls.Add(_lblTitleTickets);
            _pnlCardTickets.Controls.Add(_lblTicketsVal);
            _pnlCardTickets.Controls.Add(_lblSubTickets);
            _pnlCardTickets.Cursor = Cursors.Hand;
            _pnlCardTickets.Location = new Point(16, 16);
            _pnlCardTickets.Name = "_pnlCardTickets";
            _pnlCardTickets.Size = new Size(250, 98);
            _pnlCardTickets.TabIndex = 0;
            // 
            // _lblIconTickets
            // 
            _lblIconTickets.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblIconTickets.BackColor = Color.Transparent;
            _lblIconTickets.Font = new Font("Segoe UI Emoji", 34F);
            _lblIconTickets.ForeColor = SystemColors.Desktop;
            _lblIconTickets.Location = new Point(162, 9);
            _lblIconTickets.Name = "_lblIconTickets";
            _lblIconTickets.Size = new Size(80, 80);
            _lblIconTickets.TabIndex = 0;
            _lblIconTickets.Text = "🎫";
            _lblIconTickets.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _lblTitleTickets
            // 
            _lblTitleTickets.BackColor = Color.Transparent;
            _lblTitleTickets.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _lblTitleTickets.ForeColor = SystemColors.Desktop;
            _lblTitleTickets.Location = new Point(16, 14);
            _lblTitleTickets.Name = "_lblTitleTickets";
            _lblTitleTickets.Size = new Size(160, 20);
            _lblTitleTickets.TabIndex = 1;
            _lblTitleTickets.Text = "VÉ MỚI HÔM NAY";
            // 
            // _lblTicketsVal
            // 
            _lblTicketsVal.BackColor = Color.Transparent;
            _lblTicketsVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            _lblTicketsVal.ForeColor = SystemColors.Desktop;
            _lblTicketsVal.Location = new Point(16, 36);
            _lblTicketsVal.Name = "_lblTicketsVal";
            _lblTicketsVal.Size = new Size(160, 48);
            _lblTicketsVal.TabIndex = 2;
            _lblTicketsVal.Text = "0";
            // 
            // _lblSubTickets
            // 
            _lblSubTickets.BackColor = Color.Transparent;
            _lblSubTickets.Font = new Font("Segoe UI", 8F);
            _lblSubTickets.ForeColor = Color.FromArgb(210, 255, 255, 255);
            _lblSubTickets.Location = new Point(16, 72);
            _lblSubTickets.Name = "_lblSubTickets";
            _lblSubTickets.Size = new Size(160, 18);
            _lblSubTickets.TabIndex = 3;
            _lblSubTickets.Text = "Tổng số vé đã đặt";
            // 
            // _pnlCardUsers
            // 
            _pnlCardUsers.BackColor = Color.Transparent;
            _pnlCardUsers.Controls.Add(_lblIconUsers);
            _pnlCardUsers.Controls.Add(_lblTitleUsers);
            _pnlCardUsers.Controls.Add(_lblUsersVal);
            _pnlCardUsers.Controls.Add(_lblSubUsers);
            _pnlCardUsers.Cursor = Cursors.Hand;
            _pnlCardUsers.Location = new Point(282, 16);
            _pnlCardUsers.Name = "_pnlCardUsers";
            _pnlCardUsers.Size = new Size(250, 98);
            _pnlCardUsers.TabIndex = 1;
            // 
            // _lblIconUsers
            // 
            _lblIconUsers.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblIconUsers.BackColor = Color.Transparent;
            _lblIconUsers.Font = new Font("Segoe UI Emoji", 34F);
            _lblIconUsers.ForeColor = SystemColors.Desktop;
            _lblIconUsers.Location = new Point(162, 9);
            _lblIconUsers.Name = "_lblIconUsers";
            _lblIconUsers.Size = new Size(80, 80);
            _lblIconUsers.TabIndex = 0;
            _lblIconUsers.Text = "👥";
            _lblIconUsers.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _lblTitleUsers
            // 
            _lblTitleUsers.BackColor = Color.Transparent;
            _lblTitleUsers.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _lblTitleUsers.ForeColor = SystemColors.Desktop;
            _lblTitleUsers.Location = new Point(16, 14);
            _lblTitleUsers.Name = "_lblTitleUsers";
            _lblTitleUsers.Size = new Size(160, 20);
            _lblTitleUsers.TabIndex = 1;
            _lblTitleUsers.Text = "KHÁCH HÀNG";
            // 
            // _lblUsersVal
            // 
            _lblUsersVal.BackColor = Color.Transparent;
            _lblUsersVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            _lblUsersVal.ForeColor = SystemColors.Desktop;
            _lblUsersVal.Location = new Point(16, 36);
            _lblUsersVal.Name = "_lblUsersVal";
            _lblUsersVal.Size = new Size(160, 48);
            _lblUsersVal.TabIndex = 2;
            _lblUsersVal.Text = "0";
            // 
            // _lblSubUsers
            // 
            _lblSubUsers.BackColor = Color.Transparent;
            _lblSubUsers.Font = new Font("Segoe UI", 8F);
            _lblSubUsers.ForeColor = Color.FromArgb(210, 255, 255, 255);
            _lblSubUsers.Location = new Point(16, 72);
            _lblSubUsers.Name = "_lblSubUsers";
            _lblSubUsers.Size = new Size(160, 18);
            _lblSubUsers.TabIndex = 3;
            _lblSubUsers.Text = "Tài khoản đang hoạt động";
            // 
            // _pnlCardSchedules
            // 
            _pnlCardSchedules.BackColor = Color.Transparent;
            _pnlCardSchedules.Controls.Add(_lblIconSchedules);
            _pnlCardSchedules.Controls.Add(_lblTitleSchedules);
            _pnlCardSchedules.Controls.Add(_lblSchedulesVal);
            _pnlCardSchedules.Controls.Add(_lblSubSchedules);
            _pnlCardSchedules.Cursor = Cursors.Hand;
            _pnlCardSchedules.Location = new Point(548, 16);
            _pnlCardSchedules.Name = "_pnlCardSchedules";
            _pnlCardSchedules.Size = new Size(250, 98);
            _pnlCardSchedules.TabIndex = 2;
            // 
            // _lblIconSchedules
            // 
            _lblIconSchedules.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblIconSchedules.BackColor = Color.Transparent;
            _lblIconSchedules.Font = new Font("Segoe UI Emoji", 34F);
            _lblIconSchedules.ForeColor = SystemColors.Desktop;
            _lblIconSchedules.Location = new Point(162, 9);
            _lblIconSchedules.Name = "_lblIconSchedules";
            _lblIconSchedules.Size = new Size(80, 80);
            _lblIconSchedules.TabIndex = 0;
            _lblIconSchedules.Text = "🚆";
            _lblIconSchedules.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _lblTitleSchedules
            // 
            _lblTitleSchedules.BackColor = Color.Transparent;
            _lblTitleSchedules.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _lblTitleSchedules.ForeColor = SystemColors.Desktop;
            _lblTitleSchedules.Location = new Point(16, 14);
            _lblTitleSchedules.Name = "_lblTitleSchedules";
            _lblTitleSchedules.Size = new Size(160, 20);
            _lblTitleSchedules.TabIndex = 1;
            _lblTitleSchedules.Text = "CHUYẾN TÀU";
            // 
            // _lblSchedulesVal
            // 
            _lblSchedulesVal.BackColor = Color.Transparent;
            _lblSchedulesVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            _lblSchedulesVal.ForeColor = SystemColors.Desktop;
            _lblSchedulesVal.Location = new Point(16, 36);
            _lblSchedulesVal.Name = "_lblSchedulesVal";
            _lblSchedulesVal.Size = new Size(160, 48);
            _lblSchedulesVal.TabIndex = 2;
            _lblSchedulesVal.Text = "0";
            // 
            // _lblSubSchedules
            // 
            _lblSubSchedules.BackColor = Color.Transparent;
            _lblSubSchedules.Font = new Font("Segoe UI", 8F);
            _lblSubSchedules.ForeColor = Color.FromArgb(210, 255, 255, 255);
            _lblSubSchedules.Location = new Point(16, 72);
            _lblSubSchedules.Name = "_lblSubSchedules";
            _lblSubSchedules.Size = new Size(160, 18);
            _lblSubSchedules.TabIndex = 3;
            _lblSubSchedules.Text = "Lịch trình đang chạy";
            // 
            // _pnlCardRevenue
            // 
            _pnlCardRevenue.BackColor = Color.Transparent;
            _pnlCardRevenue.Controls.Add(_lblIconRevenue);
            _pnlCardRevenue.Controls.Add(_lblTitleRevenue);
            _pnlCardRevenue.Controls.Add(_lblRevenueVal);
            _pnlCardRevenue.Controls.Add(_lblSubRevenue);
            _pnlCardRevenue.Cursor = Cursors.Hand;
            _pnlCardRevenue.Location = new Point(814, 16);
            _pnlCardRevenue.Name = "_pnlCardRevenue";
            _pnlCardRevenue.Size = new Size(250, 98);
            _pnlCardRevenue.TabIndex = 3;
            // 
            // _lblIconRevenue
            // 
            _lblIconRevenue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _lblIconRevenue.BackColor = Color.Transparent;
            _lblIconRevenue.Font = new Font("Segoe UI Emoji", 34F);
            _lblIconRevenue.ForeColor = SystemColors.Desktop;
            _lblIconRevenue.Location = new Point(162, 9);
            _lblIconRevenue.Name = "_lblIconRevenue";
            _lblIconRevenue.Size = new Size(80, 80);
            _lblIconRevenue.TabIndex = 0;
            _lblIconRevenue.Text = "💰";
            _lblIconRevenue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // _lblTitleRevenue
            // 
            _lblTitleRevenue.BackColor = Color.Transparent;
            _lblTitleRevenue.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _lblTitleRevenue.ForeColor = SystemColors.Desktop;
            _lblTitleRevenue.Location = new Point(16, 14);
            _lblTitleRevenue.Name = "_lblTitleRevenue";
            _lblTitleRevenue.Size = new Size(160, 20);
            _lblTitleRevenue.TabIndex = 1;
            _lblTitleRevenue.Text = "DOANH THU";
            // 
            // _lblRevenueVal
            // 
            _lblRevenueVal.BackColor = Color.Transparent;
            _lblRevenueVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            _lblRevenueVal.ForeColor = SystemColors.Desktop;
            _lblRevenueVal.Location = new Point(16, 36);
            _lblRevenueVal.Name = "_lblRevenueVal";
            _lblRevenueVal.Size = new Size(160, 48);
            _lblRevenueVal.TabIndex = 2;
            _lblRevenueVal.Text = "0 ₫";
            // 
            // _lblSubRevenue
            // 
            _lblSubRevenue.BackColor = Color.Transparent;
            _lblSubRevenue.Font = new Font("Segoe UI", 8F);
            _lblSubRevenue.ForeColor = Color.FromArgb(210, 255, 255, 255);
            _lblSubRevenue.Location = new Point(16, 72);
            _lblSubRevenue.Name = "_lblSubRevenue";
            _lblSubRevenue.Size = new Size(160, 18);
            _lblSubRevenue.TabIndex = 3;
            _lblSubRevenue.Text = "Tổng doanh thu (₫)";
            // 
            // _body
            // 
            _body.Controls.Add(_gridPanel);
            _body.Dock = DockStyle.Fill;
            _body.Location = new Point(0, 186);
            _body.Name = "_body";
            _body.Padding = new Padding(16);
            _body.Size = new Size(1200, 514);
            _body.TabIndex = 2;
            // 
            // _gridPanel
            // 
            _gridPanel.BackColor = Color.White;
            _gridPanel.Controls.Add(_gridRecent);
            _gridPanel.Controls.Add(_lblGridTitle);
            _gridPanel.Dock = DockStyle.Fill;
            _gridPanel.Location = new Point(16, 16);
            _gridPanel.Name = "_gridPanel";
            _gridPanel.Padding = new Padding(16);
            _gridPanel.Size = new Size(1168, 482);
            _gridPanel.TabIndex = 0;
            // 
            // _gridRecent
            // 
            _gridRecent.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            _gridRecent.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            _gridRecent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            _gridRecent.ColumnHeadersHeight = 38;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(59, 130, 246);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            _gridRecent.DefaultCellStyle = dataGridViewCellStyle3;
            _gridRecent.Dock = DockStyle.Fill;
            _gridRecent.GridColor = Color.FromArgb(229, 231, 235);
            _gridRecent.Location = new Point(16, 52);
            _gridRecent.Name = "_gridRecent";
            _gridRecent.ReadOnly = true;
            _gridRecent.RowHeadersVisible = false;
            _gridRecent.RowHeadersWidth = 51;
            _gridRecent.RowTemplate.Height = 32;
            _gridRecent.Size = new Size(1136, 414);
            _gridRecent.TabIndex = 1;
            _gridRecent.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(248, 250, 252);
            _gridRecent.ThemeStyle.AlternatingRowsStyle.Font = null;
            _gridRecent.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            _gridRecent.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            _gridRecent.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            _gridRecent.ThemeStyle.BackColor = Color.White;
            _gridRecent.ThemeStyle.GridColor = Color.FromArgb(229, 231, 235);
            _gridRecent.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(30, 41, 59);
            _gridRecent.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            _gridRecent.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _gridRecent.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            _gridRecent.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _gridRecent.ThemeStyle.HeaderStyle.Height = 38;
            _gridRecent.ThemeStyle.ReadOnly = true;
            _gridRecent.ThemeStyle.RowsStyle.BackColor = Color.White;
            _gridRecent.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _gridRecent.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            _gridRecent.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(30, 41, 59);
            _gridRecent.ThemeStyle.RowsStyle.Height = 32;
            _gridRecent.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(59, 130, 246);
            _gridRecent.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;
            // 
            // _lblGridTitle
            // 
            _lblGridTitle.BackColor = Color.Transparent;
            _lblGridTitle.Dock = DockStyle.Top;
            _lblGridTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            _lblGridTitle.ForeColor = Color.FromArgb(30, 41, 59);
            _lblGridTitle.Location = new Point(16, 16);
            _lblGridTitle.Name = "_lblGridTitle";
            _lblGridTitle.Size = new Size(1136, 36);
            _lblGridTitle.TabIndex = 0;
            _lblGridTitle.Text = "📝  Vé đặt gần đây";
            // 
            // frmDashboard_New
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 251);
            ClientSize = new Size(1200, 700);
            Controls.Add(_body);
            Controls.Add(_cardRow);
            Controls.Add(_header);
            Name = "frmDashboard_New";
            Text = "Dashboard";
            Load += frmDashboard_New_Load;
            _header.ResumeLayout(false);
            _header.PerformLayout();
            _cardRow.ResumeLayout(false);
            _pnlCardTickets.ResumeLayout(false);
            _pnlCardUsers.ResumeLayout(false);
            _pnlCardSchedules.ResumeLayout(false);
            _pnlCardRevenue.ResumeLayout(false);
            _body.ResumeLayout(false);
            _gridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_gridRecent).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel _header;
        private System.Windows.Forms.Label _lblPageTitle;
        private System.Windows.Forms.Label _lblBreadcrumb;
        private System.Windows.Forms.Panel _cardRow;
        private System.Windows.Forms.Panel _pnlCardTickets;
        private System.Windows.Forms.Label _lblIconTickets;
        private System.Windows.Forms.Label _lblTitleTickets;
        private System.Windows.Forms.Label _lblTicketsVal;
        private System.Windows.Forms.Label _lblSubTickets;
        private System.Windows.Forms.Panel _pnlCardUsers;
        private System.Windows.Forms.Label _lblIconUsers;
        private System.Windows.Forms.Label _lblTitleUsers;
        private System.Windows.Forms.Label _lblUsersVal;
        private System.Windows.Forms.Label _lblSubUsers;
        private System.Windows.Forms.Panel _pnlCardSchedules;
        private System.Windows.Forms.Label _lblIconSchedules;
        private System.Windows.Forms.Label _lblTitleSchedules;
        private System.Windows.Forms.Label _lblSchedulesVal;
        private System.Windows.Forms.Label _lblSubSchedules;
        private System.Windows.Forms.Panel _pnlCardRevenue;
        private System.Windows.Forms.Label _lblIconRevenue;
        private System.Windows.Forms.Label _lblTitleRevenue;
        private System.Windows.Forms.Label _lblRevenueVal;
        private System.Windows.Forms.Label _lblSubRevenue;
        private System.Windows.Forms.Panel _body;
        private System.Windows.Forms.Panel _gridPanel;
        private System.Windows.Forms.Label _lblGridTitle;
        private Guna.UI2.WinForms.Guna2DataGridView _gridRecent;
    }
}