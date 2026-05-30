namespace TrainTicket.WinForms.Forms
{
    partial class frmCustomers_New
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            _leftPanel = new Guna2Panel();
            pnlRegister = new Panel();
            _btnRegister = new Guna2Button();
            pnlPhone = new Panel();
            _txtPhone = new Guna2TextBox();
            pnlPassword = new Panel();
            _txtPassword = new Guna2TextBox();
            pnlEmail = new Panel();
            _txtEmail = new Guna2TextBox();
            pnlFullName = new Panel();
            _txtFullName = new Guna2TextBox();
            lblTitle = new Label();
            _rightPanel = new Guna2Panel();
            _grid = new Guna2DataGridView();
            _leftPanel.SuspendLayout();
            pnlRegister.SuspendLayout();
            pnlPhone.SuspendLayout();
            pnlPassword.SuspendLayout();
            pnlEmail.SuspendLayout();
            pnlFullName.SuspendLayout();
            _rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
            SuspendLayout();
            // 
            // _leftPanel
            // 
            _leftPanel.Controls.Add(pnlRegister);
            _leftPanel.Controls.Add(pnlPhone);
            _leftPanel.Controls.Add(pnlPassword);
            _leftPanel.Controls.Add(pnlEmail);
            _leftPanel.Controls.Add(pnlFullName);
            _leftPanel.Controls.Add(lblTitle);
            _leftPanel.CustomizableEdges = customizableEdges11;
            _leftPanel.Dock = DockStyle.Left;
            _leftPanel.Location = new Point(0, 0);
            _leftPanel.Margin = new Padding(3, 2, 3, 2);
            _leftPanel.Name = "_leftPanel";
            _leftPanel.Padding = new Padding(14, 12, 14, 12);
            _leftPanel.ShadowDecoration.CustomizableEdges = customizableEdges12;
            _leftPanel.Size = new Size(306, 525);
            _leftPanel.TabIndex = 0;
            // 
            // pnlRegister
            // 
            pnlRegister.Controls.Add(_btnRegister);
            pnlRegister.Dock = DockStyle.Top;
            pnlRegister.Location = new Point(14, 194);
            pnlRegister.Margin = new Padding(3, 2, 3, 2);
            pnlRegister.Name = "pnlRegister";
            pnlRegister.Size = new Size(278, 34);
            pnlRegister.TabIndex = 5;
            // 
            // _btnRegister
            // 
            _btnRegister.BorderRadius = 8;
            _btnRegister.CustomizableEdges = customizableEdges1;
            _btnRegister.Dock = DockStyle.Fill;
            _btnRegister.Font = new Font("Segoe UI", 9F);
            _btnRegister.ForeColor = Color.White;
            _btnRegister.Location = new Point(0, 0);
            _btnRegister.Margin = new Padding(3, 2, 3, 2);
            _btnRegister.Name = "_btnRegister";
            _btnRegister.ShadowDecoration.CustomizableEdges = customizableEdges2;
            _btnRegister.Size = new Size(278, 34);
            _btnRegister.TabIndex = 0;
            _btnRegister.Text = "Đăng ký mới";
            _btnRegister.Click += _btnRegister_Click;
            // 
            // pnlPhone
            // 
            pnlPhone.Controls.Add(_txtPhone);
            pnlPhone.Dock = DockStyle.Top;
            pnlPhone.Location = new Point(14, 156);
            pnlPhone.Margin = new Padding(3, 2, 3, 2);
            pnlPhone.Name = "pnlPhone";
            pnlPhone.Padding = new Padding(0, 0, 0, 8);
            pnlPhone.Size = new Size(278, 38);
            pnlPhone.TabIndex = 4;
            // 
            // _txtPhone
            // 
            _txtPhone.BorderRadius = 8;
            _txtPhone.CustomizableEdges = customizableEdges3;
            _txtPhone.DefaultText = "";
            _txtPhone.Dock = DockStyle.Fill;
            _txtPhone.Font = new Font("Segoe UI", 9F);
            _txtPhone.Location = new Point(0, 0);
            _txtPhone.Margin = new Padding(3, 2, 3, 2);
            _txtPhone.Name = "_txtPhone";
            _txtPhone.PlaceholderText = "Số điện thoại";
            _txtPhone.SelectedText = "";
            _txtPhone.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _txtPhone.Size = new Size(278, 30);
            _txtPhone.TabIndex = 0;
            // 
            // pnlPassword
            // 
            pnlPassword.Controls.Add(_txtPassword);
            pnlPassword.Dock = DockStyle.Top;
            pnlPassword.Location = new Point(14, 118);
            pnlPassword.Margin = new Padding(3, 2, 3, 2);
            pnlPassword.Name = "pnlPassword";
            pnlPassword.Padding = new Padding(0, 0, 0, 8);
            pnlPassword.Size = new Size(278, 38);
            pnlPassword.TabIndex = 3;
            // 
            // _txtPassword
            // 
            _txtPassword.BorderRadius = 8;
            _txtPassword.CustomizableEdges = customizableEdges5;
            _txtPassword.DefaultText = "";
            _txtPassword.Dock = DockStyle.Fill;
            _txtPassword.Font = new Font("Segoe UI", 9F);
            _txtPassword.Location = new Point(0, 0);
            _txtPassword.Margin = new Padding(3, 2, 3, 2);
            _txtPassword.Name = "_txtPassword";
            _txtPassword.PlaceholderText = "Mật khẩu (Tối thiểu 6 ký tự)";
            _txtPassword.SelectedText = "";
            _txtPassword.ShadowDecoration.CustomizableEdges = customizableEdges6;
            _txtPassword.Size = new Size(278, 30);
            _txtPassword.TabIndex = 0;
            _txtPassword.UseSystemPasswordChar = true;
            // 
            // pnlEmail
            // 
            pnlEmail.Controls.Add(_txtEmail);
            pnlEmail.Dock = DockStyle.Top;
            pnlEmail.Location = new Point(14, 80);
            pnlEmail.Margin = new Padding(3, 2, 3, 2);
            pnlEmail.Name = "pnlEmail";
            pnlEmail.Padding = new Padding(0, 0, 0, 8);
            pnlEmail.Size = new Size(278, 38);
            pnlEmail.TabIndex = 2;
            // 
            // _txtEmail
            // 
            _txtEmail.BorderRadius = 8;
            _txtEmail.CustomizableEdges = customizableEdges7;
            _txtEmail.DefaultText = "";
            _txtEmail.Dock = DockStyle.Fill;
            _txtEmail.Font = new Font("Segoe UI", 9F);
            _txtEmail.Location = new Point(0, 0);
            _txtEmail.Margin = new Padding(3, 2, 3, 2);
            _txtEmail.Name = "_txtEmail";
            _txtEmail.PlaceholderText = "Email";
            _txtEmail.SelectedText = "";
            _txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges8;
            _txtEmail.Size = new Size(278, 30);
            _txtEmail.TabIndex = 0;
            // 
            // pnlFullName
            // 
            pnlFullName.Controls.Add(_txtFullName);
            pnlFullName.Dock = DockStyle.Top;
            pnlFullName.Location = new Point(14, 42);
            pnlFullName.Margin = new Padding(3, 2, 3, 2);
            pnlFullName.Name = "pnlFullName";
            pnlFullName.Padding = new Padding(0, 0, 0, 8);
            pnlFullName.Size = new Size(278, 38);
            pnlFullName.TabIndex = 1;
            // 
            // _txtFullName
            // 
            _txtFullName.BorderRadius = 8;
            _txtFullName.CustomizableEdges = customizableEdges9;
            _txtFullName.DefaultText = "";
            _txtFullName.Dock = DockStyle.Fill;
            _txtFullName.Font = new Font("Segoe UI", 9F);
            _txtFullName.Location = new Point(0, 0);
            _txtFullName.Margin = new Padding(3, 2, 3, 2);
            _txtFullName.Name = "_txtFullName";
            _txtFullName.PlaceholderText = "Họ và tên";
            _txtFullName.SelectedText = "";
            _txtFullName.ShadowDecoration.CustomizableEdges = customizableEdges10;
            _txtFullName.Size = new Size(278, 30);
            _txtFullName.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(14, 12);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(278, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Đăng ký Khách hàng";
            // 
            // _rightPanel
            // 
            _rightPanel.Controls.Add(_grid);
            _rightPanel.CustomizableEdges = customizableEdges13;
            _rightPanel.Dock = DockStyle.Fill;
            _rightPanel.Location = new Point(306, 0);
            _rightPanel.Margin = new Padding(3, 2, 3, 2);
            _rightPanel.Name = "_rightPanel";
            _rightPanel.Padding = new Padding(14, 12, 14, 12);
            _rightPanel.ShadowDecoration.CustomizableEdges = customizableEdges14;
            _rightPanel.Size = new Size(744, 525);
            _rightPanel.TabIndex = 1;
            // 
            // _grid
            // 
            _grid.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            _grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            _grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            _grid.DefaultCellStyle = dataGridViewCellStyle3;
            _grid.Dock = DockStyle.Fill;
            _grid.GridColor = Color.FromArgb(231, 229, 255);
            _grid.Location = new Point(14, 12);
            _grid.Margin = new Padding(3, 2, 3, 2);
            _grid.Name = "_grid";
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.Size = new Size(716, 501);
            _grid.TabIndex = 0;
            _grid.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            _grid.ThemeStyle.AlternatingRowsStyle.Font = null;
            _grid.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            _grid.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            _grid.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            _grid.ThemeStyle.BackColor = Color.White;
            _grid.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            _grid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            _grid.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            _grid.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            _grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            _grid.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _grid.ThemeStyle.HeaderStyle.Height = 23;
            _grid.ThemeStyle.ReadOnly = true;
            _grid.ThemeStyle.RowsStyle.BackColor = Color.White;
            _grid.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _grid.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            _grid.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            _grid.ThemeStyle.RowsStyle.Height = 25;
            _grid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            _grid.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // frmCustomers_New
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1050, 525);
            Controls.Add(_rightPanel);
            Controls.Add(_leftPanel);
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmCustomers_New";
            Text = "Qu?n lý khách hàng";
            Load += frmCustomers_New_Load;
            _leftPanel.ResumeLayout(false);
            pnlRegister.ResumeLayout(false);
            pnlPhone.ResumeLayout(false);
            pnlPassword.ResumeLayout(false);
            pnlEmail.ResumeLayout(false);
            pnlFullName.ResumeLayout(false);
            _rightPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel _leftPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFullName;
        private Guna.UI2.WinForms.Guna2TextBox _txtFullName;
        private System.Windows.Forms.Panel pnlEmail;
        private Guna.UI2.WinForms.Guna2TextBox _txtEmail;
        private System.Windows.Forms.Panel pnlPassword;
        private Guna.UI2.WinForms.Guna2TextBox _txtPassword;
        private System.Windows.Forms.Panel pnlPhone;
        private Guna.UI2.WinForms.Guna2TextBox _txtPhone;
        private System.Windows.Forms.Panel pnlRegister;
        private Guna.UI2.WinForms.Guna2Button _btnRegister;
        private Guna.UI2.WinForms.Guna2Panel _rightPanel;
        private Guna.UI2.WinForms.Guna2DataGridView _grid;
    }
}