namespace TrainTicket.WinForms.Forms
{
    partial class frmCustomerDashboard_New
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            _header = new Panel();
            _lblPageTitle = new Label();
            lblSub = new Label();
            _cardRow = new Panel();
            _pnlCardTickets = new Panel();
            lblIconTickets = new Label();
            lblTitleTickets = new Label();
            _lblTicketsVal = new Label();
            lblSubTickets = new Label();
            _pnlCardPending = new Panel();
            lblIconPending = new Label();
            lblTitlePending = new Label();
            _lblPendingVal = new Label();
            lblSubPending = new Label();
            _pnlCardSpent = new Panel();
            lblIconSpent = new Label();
            lblTitleSpent = new Label();
            _lblSpentVal = new Label();
            lblSubSpent = new Label();
            _body = new Panel();
            _gridPanel = new Panel();
            _gridTickets = new Guna2DataGridView();
            _toolbar = new Panel();
            _btnCancel = new Guna2Button();
            _lblGridTitle = new Label();
            gap = new Panel();
            _banner = new Panel();
            lblBanner = new Label();
            guna2Button1 = new Guna2Button();
            _header.SuspendLayout();
            _cardRow.SuspendLayout();
            _pnlCardTickets.SuspendLayout();
            _pnlCardPending.SuspendLayout();
            _pnlCardSpent.SuspendLayout();
            _body.SuspendLayout();
            _gridPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_gridTickets).BeginInit();
            _toolbar.SuspendLayout();
            _banner.SuspendLayout();
            SuspendLayout();
            // 
            // _header
            // 
            _header.BackColor = Color.FromArgb(30, 41, 59);
            _header.Controls.Add(_lblPageTitle);
            _header.Controls.Add(lblSub);
            _header.Dock = DockStyle.Top;
            _header.Location = new Point(0, 0);
            _header.Margin = new Padding(3, 2, 3, 2);
            _header.Name = "_header";
            _header.Size = new Size(1050, 42);
            _header.TabIndex = 0;
            // 
            // _lblPageTitle
            // 
            _lblPageTitle.AutoSize = true;
            _lblPageTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            _lblPageTitle.ForeColor = Color.White;
            _lblPageTitle.Location = new Point(18, 10);
            _lblPageTitle.Name = "_lblPageTitle";
            _lblPageTitle.Size = new Size(98, 25);
            _lblPageTitle.TabIndex = 0;
            _lblPageTitle.Text = "Xin chào, ";
            // 
            // lblSub
            // 
            lblSub.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSub.AutoSize = true;
            lblSub.Font = new Font("Segoe UI", 9F);
            lblSub.ForeColor = Color.FromArgb(148, 163, 184);
            lblSub.Location = new Point(886, 14);
            lblSub.Name = "lblSub";
            lblSub.Size = new Size(140, 15);
            lblSub.TabIndex = 1;
            lblSub.Text = "Trang tổng quan cá nhân";
            // 
            // _cardRow
            // 
            _cardRow.BackColor = Color.Transparent;
            _cardRow.Controls.Add(_pnlCardTickets);
            _cardRow.Controls.Add(_pnlCardPending);
            _cardRow.Controls.Add(_pnlCardSpent);
            _cardRow.Dock = DockStyle.Top;
            _cardRow.Location = new Point(0, 42);
            _cardRow.Margin = new Padding(3, 2, 3, 2);
            _cardRow.Name = "_cardRow";
            _cardRow.Padding = new Padding(14, 12, 14, 0);
            _cardRow.Size = new Size(1050, 98);
            _cardRow.TabIndex = 1;
            // 
            // _pnlCardTickets
            // 
            _pnlCardTickets.BackColor = Color.Transparent;
            _pnlCardTickets.Controls.Add(lblIconTickets);
            _pnlCardTickets.Controls.Add(lblTitleTickets);
            _pnlCardTickets.Controls.Add(_lblTicketsVal);
            _pnlCardTickets.Controls.Add(lblSubTickets);
            _pnlCardTickets.Cursor = Cursors.Hand;
            _pnlCardTickets.Location = new Point(14, 12);
            _pnlCardTickets.Margin = new Padding(3, 2, 3, 2);
            _pnlCardTickets.Name = "_pnlCardTickets";
            _pnlCardTickets.Size = new Size(324, 84);
            _pnlCardTickets.TabIndex = 0;
            // 
            // lblIconTickets
            // 
            lblIconTickets.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblIconTickets.BackColor = Color.Transparent;
            lblIconTickets.Font = new Font("Segoe UI Emoji", 34F);
            lblIconTickets.ForeColor = SystemColors.ActiveCaptionText;
            lblIconTickets.Location = new Point(247, 7);
            lblIconTickets.Name = "lblIconTickets";
            lblIconTickets.Size = new Size(70, 60);
            lblIconTickets.TabIndex = 0;
            lblIconTickets.Text = "🎫";
            lblIconTickets.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitleTickets
            // 
            lblTitleTickets.BackColor = Color.Transparent;
            lblTitleTickets.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblTitleTickets.ForeColor = SystemColors.ActiveCaptionText;
            lblTitleTickets.Location = new Point(14, 10);
            lblTitleTickets.Name = "lblTitleTickets";
            lblTitleTickets.Size = new Size(140, 15);
            lblTitleTickets.TabIndex = 1;
            lblTitleTickets.Text = "TỔNG VÉ ĐÃ ĐẶT";
            // 
            // _lblTicketsVal
            // 
            _lblTicketsVal.BackColor = Color.Transparent;
            _lblTicketsVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            _lblTicketsVal.ForeColor = SystemColors.ActiveCaptionText;
            _lblTicketsVal.Location = new Point(14, 27);
            _lblTicketsVal.Name = "_lblTicketsVal";
            _lblTicketsVal.Size = new Size(140, 40);
            _lblTicketsVal.TabIndex = 2;
            _lblTicketsVal.Text = "0";
            // 
            // lblSubTickets
            // 
            lblSubTickets.BackColor = Color.Transparent;
            lblSubTickets.Font = new Font("Segoe UI", 8F);
            lblSubTickets.ForeColor = Color.FromArgb(210, 255, 255, 255);
            lblSubTickets.Location = new Point(14, 54);
            lblSubTickets.Name = "lblSubTickets";
            lblSubTickets.Size = new Size(140, 14);
            lblSubTickets.TabIndex = 3;
            lblSubTickets.Text = "Vé của tôi";
            // 
            // _pnlCardPending
            // 
            _pnlCardPending.BackColor = Color.Transparent;
            _pnlCardPending.Controls.Add(lblIconPending);
            _pnlCardPending.Controls.Add(lblTitlePending);
            _pnlCardPending.Controls.Add(_lblPendingVal);
            _pnlCardPending.Controls.Add(lblSubPending);
            _pnlCardPending.Cursor = Cursors.Hand;
            _pnlCardPending.Location = new Point(352, 12);
            _pnlCardPending.Margin = new Padding(3, 2, 3, 2);
            _pnlCardPending.Name = "_pnlCardPending";
            _pnlCardPending.Size = new Size(324, 84);
            _pnlCardPending.TabIndex = 1;
            // 
            // lblIconPending
            // 
            lblIconPending.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblIconPending.BackColor = Color.Transparent;
            lblIconPending.Font = new Font("Segoe UI Emoji", 34F);
            lblIconPending.ForeColor = SystemColors.ActiveCaptionText;
            lblIconPending.Location = new Point(247, 7);
            lblIconPending.Name = "lblIconPending";
            lblIconPending.Size = new Size(70, 60);
            lblIconPending.TabIndex = 0;
            lblIconPending.Text = "⏳";
            lblIconPending.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitlePending
            // 
            lblTitlePending.BackColor = Color.Transparent;
            lblTitlePending.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblTitlePending.ForeColor = SystemColors.ActiveCaptionText;
            lblTitlePending.Location = new Point(14, 9);
            lblTitlePending.Name = "lblTitlePending";
            lblTitlePending.Size = new Size(140, 15);
            lblTitlePending.TabIndex = 1;
            lblTitlePending.Text = "VÉ ĐANG CHỜ";
            // 
            // _lblPendingVal
            // 
            _lblPendingVal.BackColor = Color.Transparent;
            _lblPendingVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            _lblPendingVal.ForeColor = SystemColors.ActiveCaptionText;
            _lblPendingVal.Location = new Point(14, 25);
            _lblPendingVal.Name = "_lblPendingVal";
            _lblPendingVal.Size = new Size(140, 32);
            _lblPendingVal.TabIndex = 2;
            _lblPendingVal.Text = "0";
            // 
            // lblSubPending
            // 
            lblSubPending.BackColor = Color.Transparent;
            lblSubPending.Font = new Font("Segoe UI", 8F);
            lblSubPending.ForeColor = SystemColors.ActiveCaptionText;
            lblSubPending.Location = new Point(14, 56);
            lblSubPending.Name = "lblSubPending";
            lblSubPending.Size = new Size(140, 14);
            lblSubPending.TabIndex = 3;
            lblSubPending.Text = "Chưa thanh toán";
            // 
            // _pnlCardSpent
            // 
            _pnlCardSpent.BackColor = Color.Transparent;
            _pnlCardSpent.Controls.Add(lblIconSpent);
            _pnlCardSpent.Controls.Add(lblTitleSpent);
            _pnlCardSpent.Controls.Add(_lblSpentVal);
            _pnlCardSpent.Controls.Add(lblSubSpent);
            _pnlCardSpent.Cursor = Cursors.Hand;
            _pnlCardSpent.Location = new Point(690, 12);
            _pnlCardSpent.Margin = new Padding(3, 2, 3, 2);
            _pnlCardSpent.Name = "_pnlCardSpent";
            _pnlCardSpent.Size = new Size(324, 84);
            _pnlCardSpent.TabIndex = 2;
            // 
            // lblIconSpent
            // 
            lblIconSpent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblIconSpent.BackColor = Color.Transparent;
            lblIconSpent.Font = new Font("Segoe UI Emoji", 34F);
            lblIconSpent.ForeColor = SystemColors.ActiveCaptionText;
            lblIconSpent.Location = new Point(247, 7);
            lblIconSpent.Name = "lblIconSpent";
            lblIconSpent.Size = new Size(70, 60);
            lblIconSpent.TabIndex = 0;
            lblIconSpent.Text = "💸";
            lblIconSpent.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTitleSpent
            // 
            lblTitleSpent.BackColor = Color.Transparent;
            lblTitleSpent.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblTitleSpent.ForeColor = SystemColors.ActiveCaptionText;
            lblTitleSpent.Location = new Point(14, 8);
            lblTitleSpent.Name = "lblTitleSpent";
            lblTitleSpent.Size = new Size(140, 15);
            lblTitleSpent.TabIndex = 1;
            lblTitleSpent.Text = "ĐÃ CHI TIÊU";
            // 
            // _lblSpentVal
            // 
            _lblSpentVal.BackColor = Color.Transparent;
            _lblSpentVal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            _lblSpentVal.ForeColor = SystemColors.ActiveCaptionText;
            _lblSpentVal.Location = new Point(14, 22);
            _lblSpentVal.Name = "_lblSpentVal";
            _lblSpentVal.Size = new Size(140, 35);
            _lblSpentVal.TabIndex = 2;
            _lblSpentVal.Text = "0 ₫";
            _lblSpentVal.Click += _lblSpentVal_Click;
            // 
            // lblSubSpent
            // 
            lblSubSpent.BackColor = Color.Transparent;
            lblSubSpent.Font = new Font("Segoe UI", 8F);
            lblSubSpent.ForeColor = SystemColors.ActiveCaptionText;
            lblSubSpent.Location = new Point(14, 58);
            lblSubSpent.Name = "lblSubSpent";
            lblSubSpent.Size = new Size(140, 14);
            lblSubSpent.TabIndex = 3;
            lblSubSpent.Text = "Tổng tiền vé (₫)";
            // 
            // _body
            // 
            _body.Controls.Add(_gridPanel);
            _body.Controls.Add(gap);
            _body.Controls.Add(_banner);
            _body.Dock = DockStyle.Fill;
            _body.Location = new Point(0, 140);
            _body.Margin = new Padding(3, 2, 3, 2);
            _body.Name = "_body";
            _body.Padding = new Padding(14, 12, 14, 12);
            _body.Size = new Size(1050, 385);
            _body.TabIndex = 2;
            // 
            // _gridPanel
            // 
            _gridPanel.BackColor = Color.White;
            _gridPanel.Controls.Add(_gridTickets);
            _gridPanel.Controls.Add(_toolbar);
            _gridPanel.Controls.Add(_lblGridTitle);
            _gridPanel.Dock = DockStyle.Fill;
            _gridPanel.Location = new Point(14, 75);
            _gridPanel.Margin = new Padding(3, 2, 3, 2);
            _gridPanel.Name = "_gridPanel";
            _gridPanel.Padding = new Padding(14, 12, 14, 12);
            _gridPanel.Size = new Size(1022, 298);
            _gridPanel.TabIndex = 2;
            // 
            // _gridTickets
            // 
            _gridTickets.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            _gridTickets.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            _gridTickets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            _gridTickets.ColumnHeadersHeight = 38;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(59, 130, 246);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            _gridTickets.DefaultCellStyle = dataGridViewCellStyle3;
            _gridTickets.Dock = DockStyle.Fill;
            _gridTickets.GridColor = Color.FromArgb(229, 231, 235);
            _gridTickets.Location = new Point(14, 72);
            _gridTickets.Margin = new Padding(3, 2, 3, 2);
            _gridTickets.Name = "_gridTickets";
            _gridTickets.ReadOnly = true;
            _gridTickets.RowHeadersVisible = false;
            _gridTickets.RowHeadersWidth = 51;
            _gridTickets.RowTemplate.Height = 32;
            _gridTickets.Size = new Size(994, 214);
            _gridTickets.TabIndex = 2;
            _gridTickets.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(248, 250, 252);
            _gridTickets.ThemeStyle.AlternatingRowsStyle.Font = null;
            _gridTickets.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            _gridTickets.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            _gridTickets.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            _gridTickets.ThemeStyle.BackColor = Color.White;
            _gridTickets.ThemeStyle.GridColor = Color.FromArgb(229, 231, 235);
            _gridTickets.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(30, 41, 59);
            _gridTickets.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            _gridTickets.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _gridTickets.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            _gridTickets.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _gridTickets.ThemeStyle.HeaderStyle.Height = 38;
            _gridTickets.ThemeStyle.ReadOnly = true;
            _gridTickets.ThemeStyle.RowsStyle.BackColor = Color.White;
            _gridTickets.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _gridTickets.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            _gridTickets.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(30, 41, 59);
            _gridTickets.ThemeStyle.RowsStyle.Height = 32;
            _gridTickets.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(59, 130, 246);
            _gridTickets.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;
            // 
            // _toolbar
            // 
            _toolbar.BackColor = Color.Transparent;
            _toolbar.Controls.Add(guna2Button1);
            _toolbar.Controls.Add(_btnCancel);
            _toolbar.Dock = DockStyle.Top;
            _toolbar.Location = new Point(14, 39);
            _toolbar.Margin = new Padding(3, 2, 3, 2);
            _toolbar.Name = "_toolbar";
            _toolbar.Size = new Size(994, 33);
            _toolbar.TabIndex = 1;
            // 
            // _btnCancel
            // 
            _btnCancel.BorderRadius = 8;
            _btnCancel.Cursor = Cursors.Hand;
            _btnCancel.CustomizableEdges = customizableEdges3;
            _btnCancel.FillColor = Color.FromArgb(239, 68, 68);
            _btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnCancel.ForeColor = Color.White;
            _btnCancel.Location = new Point(0, 4);
            _btnCancel.Margin = new Padding(3, 2, 3, 2);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _btnCancel.Size = new Size(173, 26);
            _btnCancel.TabIndex = 0;
            _btnCancel.Text = "❌ Hủy vé đã chọn";
            _btnCancel.Click += _btnCancel_Click;
            // 
            // _lblGridTitle
            // 
            _lblGridTitle.BackColor = Color.Transparent;
            _lblGridTitle.Dock = DockStyle.Top;
            _lblGridTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            _lblGridTitle.ForeColor = Color.FromArgb(30, 41, 59);
            _lblGridTitle.Location = new Point(14, 12);
            _lblGridTitle.Name = "_lblGridTitle";
            _lblGridTitle.Size = new Size(994, 27);
            _lblGridTitle.TabIndex = 0;
            _lblGridTitle.Text = "🕒 Lịch sử đặt vé của bạn";
            // 
            // gap
            // 
            gap.BackColor = Color.Transparent;
            gap.Dock = DockStyle.Top;
            gap.Location = new Point(14, 66);
            gap.Margin = new Padding(3, 2, 3, 2);
            gap.Name = "gap";
            gap.Size = new Size(1022, 9);
            gap.TabIndex = 1;
            // 
            // _banner
            // 
            _banner.BackColor = Color.FromArgb(238, 242, 255);
            _banner.Controls.Add(lblBanner);
            _banner.Dock = DockStyle.Top;
            _banner.Location = new Point(14, 12);
            _banner.Margin = new Padding(3, 2, 3, 2);
            _banner.Name = "_banner";
            _banner.Size = new Size(1022, 54);
            _banner.TabIndex = 0;
            // 
            // lblBanner
            // 
            lblBanner.BackColor = Color.Transparent;
            lblBanner.Dock = DockStyle.Fill;
            lblBanner.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblBanner.ForeColor = SystemColors.ActiveCaptionText;
            lblBanner.Location = new Point(0, 0);
            lblBanner.Name = "lblBanner";
            lblBanner.Padding = new Padding(14, 0, 0, 0);
            lblBanner.Size = new Size(1022, 54);
            lblBanner.TabIndex = 0;
            lblBanner.Text = "🚀 Khám phá các chuyến tàu hấp dẫn! Đặt vé sớm để có chỗ ngồi tốt nhất.";
            lblBanner.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderRadius = 8;
            guna2Button1.Cursor = Cursors.Hand;
            guna2Button1.CustomizableEdges = customizableEdges1;
            guna2Button1.FillColor = Color.FromArgb(239, 68, 68);
            guna2Button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Location = new Point(188, 4);
            guna2Button1.Margin = new Padding(3, 2, 3, 2);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2Button1.Size = new Size(173, 26);
            guna2Button1.TabIndex = 1;
            guna2Button1.Text = "❌ Hủy vé đã chọn";
            // 
            // frmCustomerDashboard_New
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 251);
            ClientSize = new Size(1050, 525);
            Controls.Add(_body);
            Controls.Add(_cardRow);
            Controls.Add(_header);
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmCustomerDashboard_New";
            Text = "Tổng quan";
            Load += frmCustomerDashboard_New_Load;
            _header.ResumeLayout(false);
            _header.PerformLayout();
            _cardRow.ResumeLayout(false);
            _pnlCardTickets.ResumeLayout(false);
            _pnlCardPending.ResumeLayout(false);
            _pnlCardSpent.ResumeLayout(false);
            _body.ResumeLayout(false);
            _gridPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_gridTickets).EndInit();
            _toolbar.ResumeLayout(false);
            _banner.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel _header;
        private System.Windows.Forms.Label _lblPageTitle;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.Panel _cardRow;
        private System.Windows.Forms.Panel _pnlCardTickets;
        private System.Windows.Forms.Label lblIconTickets;
        private System.Windows.Forms.Label lblTitleTickets;
        private System.Windows.Forms.Label _lblTicketsVal;
        private System.Windows.Forms.Label lblSubTickets;
        private System.Windows.Forms.Panel _pnlCardPending;
        private System.Windows.Forms.Label lblIconPending;
        private System.Windows.Forms.Label lblTitlePending;
        private System.Windows.Forms.Label _lblPendingVal;
        private System.Windows.Forms.Label lblSubPending;
        private System.Windows.Forms.Panel _pnlCardSpent;
        private System.Windows.Forms.Label lblIconSpent;
        private System.Windows.Forms.Label lblTitleSpent;
        private System.Windows.Forms.Label _lblSpentVal;
        private System.Windows.Forms.Label lblSubSpent;
        private System.Windows.Forms.Panel _body;
        private System.Windows.Forms.Panel _banner;
        private System.Windows.Forms.Label lblBanner;
        private System.Windows.Forms.Panel gap;
        private System.Windows.Forms.Panel _gridPanel;
        private System.Windows.Forms.Label _lblGridTitle;
        private System.Windows.Forms.Panel _toolbar;
        private Guna.UI2.WinForms.Guna2Button _btnCancel;
        private Guna.UI2.WinForms.Guna2DataGridView _gridTickets;
        private Guna2Button guna2Button1;
    }
}