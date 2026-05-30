namespace TrainTicket.WinForms.Forms
{
    partial class frmCustomerProfile_New
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            _lblTitle = new Label();
            _cardInfo = new Guna2Panel();
            _btnSaveInfo = new Guna2Button();
            _txtPhone = new Guna2TextBox();
            _txtEmail = new Guna2TextBox();
            _txtFullName = new Guna2TextBox();
            _lblInfoTitle = new Label();
            gap = new Panel();
            _cardPwd = new Guna2Panel();
            _btnChangePwd = new Guna2Button();
            _txtNewPwd2 = new Guna2TextBox();
            _txtNewPwd = new Guna2TextBox();
            _txtOldPwd = new Guna2TextBox();
            _lblPwdTitle = new Label();
            _cardInfo.SuspendLayout();
            _cardPwd.SuspendLayout();
            SuspendLayout();
            // 
            // _lblTitle
            // 
            _lblTitle.AutoSize = true;
            _lblTitle.BackColor = Color.Transparent;
            _lblTitle.Dock = DockStyle.Top;
            _lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            _lblTitle.Location = new Point(21, 18);
            _lblTitle.Name = "_lblTitle";
            _lblTitle.Size = new Size(217, 32);
            _lblTitle.TabIndex = 0;
            _lblTitle.Text = "👤 Hồ sơ cá nhân";
            // 
            // _cardInfo
            // 
            _cardInfo.BackColor = Color.Transparent;
            _cardInfo.BorderRadius = 12;
            _cardInfo.Controls.Add(_btnSaveInfo);
            _cardInfo.Controls.Add(_txtPhone);
            _cardInfo.Controls.Add(_txtEmail);
            _cardInfo.Controls.Add(_txtFullName);
            _cardInfo.Controls.Add(_lblInfoTitle);
            _cardInfo.CustomizableEdges = customizableEdges9;
            _cardInfo.Dock = DockStyle.Top;
            _cardInfo.Location = new Point(21, 50);
            _cardInfo.Margin = new Padding(3, 2, 3, 2);
            _cardInfo.Name = "_cardInfo";
            _cardInfo.Padding = new Padding(18, 15, 18, 15);
            _cardInfo.ShadowDecoration.CustomizableEdges = customizableEdges10;
            _cardInfo.ShadowDecoration.Depth = 8;
            _cardInfo.ShadowDecoration.Enabled = true;
            _cardInfo.Size = new Size(658, 195);
            _cardInfo.TabIndex = 1;
            // 
            // _btnSaveInfo
            // 
            _btnSaveInfo.BorderRadius = 8;
            _btnSaveInfo.CustomizableEdges = customizableEdges1;
            _btnSaveInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnSaveInfo.ForeColor = Color.White;
            _btnSaveInfo.Location = new Point(18, 150);
            _btnSaveInfo.Margin = new Padding(3, 2, 3, 2);
            _btnSaveInfo.Name = "_btnSaveInfo";
            _btnSaveInfo.ShadowDecoration.CustomizableEdges = customizableEdges2;
            _btnSaveInfo.Size = new Size(158, 28);
            _btnSaveInfo.TabIndex = 4;
            _btnSaveInfo.Text = "💾 Lưu thông tin";
            _btnSaveInfo.Click += _btnSaveInfo_Click;
            // 
            // _txtPhone
            // 
            _txtPhone.BorderRadius = 8;
            _txtPhone.CustomizableEdges = customizableEdges3;
            _txtPhone.DefaultText = "";
            _txtPhone.Font = new Font("Segoe UI", 10F);
            _txtPhone.Location = new Point(18, 112);
            _txtPhone.Name = "_txtPhone";
            _txtPhone.PlaceholderText = "Số điện thoại";
            _txtPhone.SelectedText = "";
            _txtPhone.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _txtPhone.Size = new Size(350, 30);
            _txtPhone.TabIndex = 3;
            // 
            // _txtEmail
            // 
            _txtEmail.BorderRadius = 8;
            _txtEmail.CustomizableEdges = customizableEdges5;
            _txtEmail.DefaultText = "";
            _txtEmail.Font = new Font("Segoe UI", 10F);
            _txtEmail.Location = new Point(18, 75);
            _txtEmail.Name = "_txtEmail";
            _txtEmail.PlaceholderText = "Email";
            _txtEmail.ReadOnly = true;
            _txtEmail.SelectedText = "";
            _txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges6;
            _txtEmail.Size = new Size(350, 30);
            _txtEmail.TabIndex = 2;
            // 
            // _txtFullName
            // 
            _txtFullName.BorderRadius = 8;
            _txtFullName.CustomizableEdges = customizableEdges7;
            _txtFullName.DefaultText = "";
            _txtFullName.Font = new Font("Segoe UI", 10F);
            _txtFullName.Location = new Point(18, 38);
            _txtFullName.Name = "_txtFullName";
            _txtFullName.PlaceholderText = "Họ và tên";
            _txtFullName.SelectedText = "";
            _txtFullName.ShadowDecoration.CustomizableEdges = customizableEdges8;
            _txtFullName.Size = new Size(350, 30);
            _txtFullName.TabIndex = 1;
            // 
            // _lblInfoTitle
            // 
            _lblInfoTitle.AutoSize = true;
            _lblInfoTitle.BackColor = Color.Transparent;
            _lblInfoTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _lblInfoTitle.Location = new Point(18, 12);
            _lblInfoTitle.Name = "_lblInfoTitle";
            _lblInfoTitle.Size = new Size(127, 20);
            _lblInfoTitle.TabIndex = 0;
            _lblInfoTitle.Text = "Thông tin cơ bản";
            // 
            // gap
            // 
            gap.Dock = DockStyle.Top;
            gap.Location = new Point(21, 245);
            gap.Margin = new Padding(3, 2, 3, 2);
            gap.Name = "gap";
            gap.Size = new Size(658, 12);
            gap.TabIndex = 2;
            // 
            // _cardPwd
            // 
            _cardPwd.BackColor = Color.Transparent;
            _cardPwd.BorderRadius = 12;
            _cardPwd.Controls.Add(_btnChangePwd);
            _cardPwd.Controls.Add(_txtNewPwd2);
            _cardPwd.Controls.Add(_txtNewPwd);
            _cardPwd.Controls.Add(_txtOldPwd);
            _cardPwd.Controls.Add(_lblPwdTitle);
            _cardPwd.CustomizableEdges = customizableEdges19;
            _cardPwd.Dock = DockStyle.Top;
            _cardPwd.Location = new Point(21, 257);
            _cardPwd.Margin = new Padding(3, 2, 3, 2);
            _cardPwd.Name = "_cardPwd";
            _cardPwd.Padding = new Padding(18, 15, 18, 15);
            _cardPwd.ShadowDecoration.CustomizableEdges = customizableEdges20;
            _cardPwd.ShadowDecoration.Depth = 8;
            _cardPwd.ShadowDecoration.Enabled = true;
            _cardPwd.Size = new Size(658, 195);
            _cardPwd.TabIndex = 3;
            // 
            // _btnChangePwd
            // 
            _btnChangePwd.BorderRadius = 8;
            _btnChangePwd.CustomizableEdges = customizableEdges11;
            _btnChangePwd.FillColor = Color.FromArgb(16, 185, 129);
            _btnChangePwd.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnChangePwd.ForeColor = Color.White;
            _btnChangePwd.Location = new Point(18, 150);
            _btnChangePwd.Margin = new Padding(3, 2, 3, 2);
            _btnChangePwd.Name = "_btnChangePwd";
            _btnChangePwd.ShadowDecoration.CustomizableEdges = customizableEdges12;
            _btnChangePwd.Size = new Size(158, 28);
            _btnChangePwd.TabIndex = 4;
            _btnChangePwd.Text = "🔑 Đổi mật khẩu";
            _btnChangePwd.Click += _btnChangePwd_Click;
            // 
            // _txtNewPwd2
            // 
            _txtNewPwd2.BorderRadius = 8;
            _txtNewPwd2.CustomizableEdges = customizableEdges13;
            _txtNewPwd2.DefaultText = "";
            _txtNewPwd2.Font = new Font("Segoe UI", 10F);
            _txtNewPwd2.Location = new Point(18, 112);
            _txtNewPwd2.Name = "_txtNewPwd2";
            _txtNewPwd2.PlaceholderText = "Nhập lại mật khẩu";
            _txtNewPwd2.SelectedText = "";
            _txtNewPwd2.ShadowDecoration.CustomizableEdges = customizableEdges14;
            _txtNewPwd2.Size = new Size(350, 30);
            _txtNewPwd2.TabIndex = 3;
            _txtNewPwd2.UseSystemPasswordChar = true;
            // 
            // _txtNewPwd
            // 
            _txtNewPwd.BorderRadius = 8;
            _txtNewPwd.CustomizableEdges = customizableEdges15;
            _txtNewPwd.DefaultText = "";
            _txtNewPwd.Font = new Font("Segoe UI", 10F);
            _txtNewPwd.Location = new Point(18, 75);
            _txtNewPwd.Name = "_txtNewPwd";
            _txtNewPwd.PlaceholderText = "Mật khẩu mới";
            _txtNewPwd.SelectedText = "";
            _txtNewPwd.ShadowDecoration.CustomizableEdges = customizableEdges16;
            _txtNewPwd.Size = new Size(350, 30);
            _txtNewPwd.TabIndex = 2;
            _txtNewPwd.UseSystemPasswordChar = true;
            // 
            // _txtOldPwd
            // 
            _txtOldPwd.BorderRadius = 8;
            _txtOldPwd.CustomizableEdges = customizableEdges17;
            _txtOldPwd.DefaultText = "";
            _txtOldPwd.Font = new Font("Segoe UI", 10F);
            _txtOldPwd.Location = new Point(18, 38);
            _txtOldPwd.Name = "_txtOldPwd";
            _txtOldPwd.PlaceholderText = "Mật khẩu hiện tại";
            _txtOldPwd.SelectedText = "";
            _txtOldPwd.ShadowDecoration.CustomizableEdges = customizableEdges18;
            _txtOldPwd.Size = new Size(350, 30);
            _txtOldPwd.TabIndex = 1;
            _txtOldPwd.UseSystemPasswordChar = true;
            // 
            // _lblPwdTitle
            // 
            _lblPwdTitle.AutoSize = true;
            _lblPwdTitle.BackColor = Color.Transparent;
            _lblPwdTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _lblPwdTitle.Location = new Point(18, 12);
            _lblPwdTitle.Name = "_lblPwdTitle";
            _lblPwdTitle.Size = new Size(103, 20);
            _lblPwdTitle.TabIndex = 0;
            _lblPwdTitle.Text = "Đổi mật khẩu";
            // 
            // frmCustomerProfile_New
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(700, 488);
            Controls.Add(_cardPwd);
            Controls.Add(gap);
            Controls.Add(_cardInfo);
            Controls.Add(_lblTitle);
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmCustomerProfile_New";
            Padding = new Padding(21, 18, 21, 18);
            Text = "Hồ sơ cá nhân";
            Load += frmCustomerProfile_New_Load;
            _cardInfo.ResumeLayout(false);
            _cardInfo.PerformLayout();
            _cardPwd.ResumeLayout(false);
            _cardPwd.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblTitle;
        private Guna.UI2.WinForms.Guna2Panel _cardInfo;
        private System.Windows.Forms.Label _lblInfoTitle;
        private Guna.UI2.WinForms.Guna2TextBox _txtFullName;
        private Guna.UI2.WinForms.Guna2TextBox _txtEmail;
        private Guna.UI2.WinForms.Guna2TextBox _txtPhone;
        private Guna.UI2.WinForms.Guna2Button _btnSaveInfo;
        private System.Windows.Forms.Panel gap;
        private Guna.UI2.WinForms.Guna2Panel _cardPwd;
        private System.Windows.Forms.Label _lblPwdTitle;
        private Guna.UI2.WinForms.Guna2TextBox _txtOldPwd;
        private Guna.UI2.WinForms.Guna2TextBox _txtNewPwd;
        private Guna.UI2.WinForms.Guna2TextBox _txtNewPwd2;
        private Guna.UI2.WinForms.Guna2Button _btnChangePwd;
    }
}