namespace TrainTicket.WinForms.Forms
{
    partial class frmPaymentHistory_New
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            _toolbar = new Panel();
            _btnRefresh = new Guna2Button();
            _cboStatus = new Guna2ComboBox();
            _cboMethod = new Guna2ComboBox();
            _txtSearch = new Guna2TextBox();
            _totalBar = new Panel();
            _lblTotal = new Label();
            _grid = new Guna2DataGridView();
            _toolbar.SuspendLayout();
            _totalBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
            SuspendLayout();
            // 
            // _toolbar
            // 
            _toolbar.BackColor = Color.PeachPuff;
            _toolbar.Controls.Add(_btnRefresh);
            _toolbar.Controls.Add(_cboStatus);
            _toolbar.Controls.Add(_cboMethod);
            _toolbar.Controls.Add(_txtSearch);
            _toolbar.Dock = DockStyle.Top;
            _toolbar.Location = new Point(0, 0);
            _toolbar.Name = "_toolbar";
            _toolbar.Padding = new Padding(12, 10, 12, 0);
            _toolbar.Size = new Size(1200, 60);
            _toolbar.TabIndex = 0;
            // 
            // _btnRefresh
            // 
            _btnRefresh.BorderRadius = 8;
            _btnRefresh.CustomizableEdges = customizableEdges1;
            _btnRefresh.FillColor = Color.FromArgb(30, 41, 59);
            _btnRefresh.Font = new Font("Segoe UI", 9F);
            _btnRefresh.ForeColor = Color.White;
            _btnRefresh.Location = new Point(615, 10);
            _btnRefresh.Name = "_btnRefresh";
            _btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges2;
            _btnRefresh.Size = new Size(156, 36);
            _btnRefresh.TabIndex = 3;
            _btnRefresh.Text = "🔄 Làm mới";
            _btnRefresh.Click += _btnRefresh_Click;
            // 
            // _cboStatus
            // 
            _cboStatus.BackColor = Color.Transparent;
            _cboStatus.BorderRadius = 8;
            _cboStatus.CustomizableEdges = customizableEdges3;
            _cboStatus.DrawMode = DrawMode.OwnerDrawFixed;
            _cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboStatus.FocusedColor = Color.Empty;
            _cboStatus.Font = new Font("Segoe UI", 10F);
            _cboStatus.ForeColor = Color.FromArgb(68, 88, 112);
            _cboStatus.ItemHeight = 30;
            _cboStatus.Items.AddRange(new object[] { "Tất cả TT", "Completed", "Pending", "Refunded", "Failed" });
            _cboStatus.Location = new Point(465, 10);
            _cboStatus.Name = "_cboStatus";
            _cboStatus.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _cboStatus.Size = new Size(140, 36);
            _cboStatus.TabIndex = 2;
            _cboStatus.SelectedIndexChanged += _cboStatus_SelectedIndexChanged;
            // 
            // _cboMethod
            // 
            _cboMethod.BackColor = Color.Transparent;
            _cboMethod.BorderRadius = 8;
            _cboMethod.CustomizableEdges = customizableEdges5;
            _cboMethod.DrawMode = DrawMode.OwnerDrawFixed;
            _cboMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboMethod.FocusedColor = Color.Empty;
            _cboMethod.Font = new Font("Segoe UI", 10F);
            _cboMethod.ForeColor = Color.FromArgb(68, 88, 112);
            _cboMethod.ItemHeight = 30;
            _cboMethod.Items.AddRange(new object[] { "Tất cả phương thức", "Cash", "Card", "Transfer", "QR" });
            _cboMethod.Location = new Point(210, 10);
            _cboMethod.Name = "_cboMethod";
            _cboMethod.ShadowDecoration.CustomizableEdges = customizableEdges6;
            _cboMethod.Size = new Size(226, 36);
            _cboMethod.TabIndex = 1;
            _cboMethod.SelectedIndexChanged += _cboMethod_SelectedIndexChanged;
            // 
            // _txtSearch
            // 
            _txtSearch.BorderRadius = 8;
            _txtSearch.CustomizableEdges = customizableEdges7;
            _txtSearch.DefaultText = "";
            _txtSearch.FillColor = Color.FromArgb(248, 250, 252);
            _txtSearch.Font = new Font("Segoe UI", 9F);
            _txtSearch.Location = new Point(12, 10);
            _txtSearch.Margin = new Padding(3, 4, 3, 4);
            _txtSearch.Name = "_txtSearch";
            _txtSearch.PlaceholderText = "🔍 Tìm mã vé / giao dịch...";
            _txtSearch.SelectedText = "";
            _txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges8;
            _txtSearch.Size = new Size(192, 36);
            _txtSearch.TabIndex = 0;
            _txtSearch.KeyDown += _txtSearch_KeyDown;
            // 
            // _totalBar
            // 
            _totalBar.BackColor = Color.FromArgb(238, 242, 255);
            _totalBar.Controls.Add(_lblTotal);
            _totalBar.Dock = DockStyle.Top;
            _totalBar.Location = new Point(0, 60);
            _totalBar.Name = "_totalBar";
            _totalBar.Size = new Size(1200, 32);
            _totalBar.TabIndex = 1;
            // 
            // _lblTotal
            // 
            _lblTotal.Dock = DockStyle.Fill;
            _lblTotal.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _lblTotal.ForeColor = Color.FromArgb(99, 102, 241);
            _lblTotal.Location = new Point(0, 0);
            _lblTotal.Name = "_lblTotal";
            _lblTotal.Padding = new Padding(12, 0, 0, 0);
            _lblTotal.Size = new Size(1200, 32);
            _lblTotal.TabIndex = 0;
            _lblTotal.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _grid
            // 
            _grid.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            _grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            _grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            _grid.ColumnHeadersHeight = 38;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(59, 130, 246);
            dataGridViewCellStyle3.SelectionForeColor = Color.White;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            _grid.DefaultCellStyle = dataGridViewCellStyle3;
            _grid.Dock = DockStyle.Fill;
            _grid.GridColor = Color.FromArgb(229, 231, 235);
            _grid.Location = new Point(0, 92);
            _grid.Name = "_grid";
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.RowHeadersWidth = 51;
            _grid.RowTemplate.Height = 32;
            _grid.Size = new Size(1200, 608);
            _grid.TabIndex = 2;
            _grid.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(248, 250, 252);
            _grid.ThemeStyle.AlternatingRowsStyle.Font = null;
            _grid.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            _grid.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            _grid.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            _grid.ThemeStyle.BackColor = Color.White;
            _grid.ThemeStyle.GridColor = Color.FromArgb(229, 231, 235);
            _grid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(30, 41, 59);
            _grid.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            _grid.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            _grid.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _grid.ThemeStyle.HeaderStyle.Height = 38;
            _grid.ThemeStyle.ReadOnly = true;
            _grid.ThemeStyle.RowsStyle.BackColor = Color.White;
            _grid.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _grid.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            _grid.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(30, 41, 59);
            _grid.ThemeStyle.RowsStyle.Height = 32;
            _grid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(59, 130, 246);
            _grid.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;
            // 
            // frmPaymentHistory_New
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 251);
            ClientSize = new Size(1200, 700);
            Controls.Add(_grid);
            Controls.Add(_totalBar);
            Controls.Add(_toolbar);
            Name = "frmPaymentHistory_New";
            Text = "Lịch sử thanh toán";
            Load += frmPaymentHistory_New_Load;
            _toolbar.ResumeLayout(false);
            _totalBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel _toolbar;
        private Guna.UI2.WinForms.Guna2TextBox _txtSearch;
        private Guna.UI2.WinForms.Guna2ComboBox _cboMethod;
        private Guna.UI2.WinForms.Guna2ComboBox _cboStatus;
        private Guna.UI2.WinForms.Guna2Button _btnRefresh;
        private System.Windows.Forms.Panel _totalBar;
        private System.Windows.Forms.Label _lblTotal;
        private Guna.UI2.WinForms.Guna2DataGridView _grid;
    }
}