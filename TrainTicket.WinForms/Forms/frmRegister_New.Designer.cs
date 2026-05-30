namespace TrainTicket.WinForms.Forms
{
    partial class frmRegister_New
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
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            elipse = new Guna2Elipse(components);
            card = new Guna2ShadowPanel();
            _lblStatus = new Label();
            _btnRegister = new Guna2Button();
            _txtPhone = new Guna2TextBox();
            lblPhone = new Label();
            _txtPassword = new Guna2TextBox();
            lblPassword = new Label();
            _txtEmail = new Guna2TextBox();
            lblEmail = new Label();
            _txtFullName = new Guna2TextBox();
            lblFullName = new Label();
            lblSubTitle = new Label();
            lblTitle = new Label();
            btnClose = new Guna2Button();
            card.SuspendLayout();
            SuspendLayout();
            // 
            // elipse
            // 
            elipse.BorderRadius = 16;
            elipse.TargetControl = this;
            // 
            // card
            // 
            card.BackColor = Color.Transparent;
            card.Controls.Add(_lblStatus);
            card.Controls.Add(_btnRegister);
            card.Controls.Add(_txtPhone);
            card.Controls.Add(lblPhone);
            card.Controls.Add(_txtPassword);
            card.Controls.Add(lblPassword);
            card.Controls.Add(_txtEmail);
            card.Controls.Add(lblEmail);
            card.Controls.Add(_txtFullName);
            card.Controls.Add(lblFullName);
            card.Controls.Add(lblSubTitle);
            card.Controls.Add(lblTitle);
            card.FillColor = Color.White;
            card.Location = new Point(40, 30);
            card.Name = "card";
            card.Radius = 14;
            card.ShadowColor = Color.Black;
            card.ShadowDepth = 60;
            card.Size = new Size(420, 380);
            card.TabIndex = 0;
            // 
            // _lblStatus
            // 
            _lblStatus.BackColor = Color.Silver;
            _lblStatus.Font = new Font("Segoe UI", 9F);
            _lblStatus.ForeColor = Color.RosyBrown;
            _lblStatus.Location = new Point(28, 314);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(200, 30);
            _lblStatus.TabIndex = 11;
            // 
            // _btnRegister
            // 
            _btnRegister.BorderRadius = 10;
            _btnRegister.CustomizableEdges = customizableEdges3;
            _btnRegister.FillColor = Color.FromArgb(99, 102, 241);
            _btnRegister.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnRegister.ForeColor = Color.White;
            _btnRegister.HoverState.FillColor = Color.FromArgb(79, 70, 229);
            _btnRegister.Location = new Point(250, 310);
            _btnRegister.Name = "_btnRegister";
            _btnRegister.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _btnRegister.Size = new Size(138, 38);
            _btnRegister.TabIndex = 10;
            _btnRegister.Text = "Đăng ký";
            _btnRegister.Click += _btnRegister_Click;
            // 
            // _txtPhone
            // 
            _txtPhone.BorderRadius = 8;
            _txtPhone.CustomizableEdges = customizableEdges5;
            _txtPhone.DefaultText = "";
            _txtPhone.Font = new Font("Segoe UI", 9F);
            _txtPhone.Location = new Point(28, 262);
            _txtPhone.Margin = new Padding(3, 4, 3, 4);
            _txtPhone.Name = "_txtPhone";
            _txtPhone.PlaceholderText = "Nhập số điện thoại";
            _txtPhone.SelectedText = "";
            _txtPhone.ShadowDecoration.CustomizableEdges = customizableEdges6;
            _txtPhone.Size = new Size(360, 36);
            _txtPhone.TabIndex = 9;
            // 
            // lblPhone
            // 
            lblPhone.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPhone.ForeColor = Color.FromArgb(71, 85, 105);
            lblPhone.Location = new Point(28, 242);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(120, 20);
            lblPhone.TabIndex = 8;
            lblPhone.Text = "Số điện thoại";
            // 
            // _txtPassword
            // 
            _txtPassword.BorderRadius = 8;
            _txtPassword.CustomizableEdges = customizableEdges7;
            _txtPassword.DefaultText = "";
            _txtPassword.Font = new Font("Segoe UI", 9F);
            _txtPassword.Location = new Point(28, 206);
            _txtPassword.Margin = new Padding(3, 4, 3, 4);
            _txtPassword.Name = "_txtPassword";
            _txtPassword.PlaceholderText = "Nhập mật khẩu";
            _txtPassword.SelectedText = "";
            _txtPassword.ShadowDecoration.CustomizableEdges = customizableEdges8;
            _txtPassword.Size = new Size(360, 36);
            _txtPassword.TabIndex = 7;
            _txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            lblPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(71, 85, 105);
            lblPassword.Location = new Point(28, 186);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(100, 20);
            lblPassword.TabIndex = 6;
            lblPassword.Text = "Mật khẩu";
            // 
            // _txtEmail
            // 
            _txtEmail.BorderRadius = 8;
            _txtEmail.CustomizableEdges = customizableEdges9;
            _txtEmail.DefaultText = "";
            _txtEmail.Font = new Font("Segoe UI", 9F);
            _txtEmail.Location = new Point(28, 152);
            _txtEmail.Margin = new Padding(3, 4, 3, 4);
            _txtEmail.Name = "_txtEmail";
            _txtEmail.PlaceholderText = "Nhập email";
            _txtEmail.SelectedText = "";
            _txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges10;
            _txtEmail.Size = new Size(360, 36);
            _txtEmail.TabIndex = 5;
            // 
            // lblEmail
            // 
            lblEmail.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(71, 85, 105);
            lblEmail.Location = new Point(28, 132);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(100, 20);
            lblEmail.TabIndex = 4;
            lblEmail.Text = "Email";
            // 
            // _txtFullName
            // 
            _txtFullName.BorderRadius = 8;
            _txtFullName.CustomizableEdges = customizableEdges11;
            _txtFullName.DefaultText = "";
            _txtFullName.Font = new Font("Segoe UI", 9F);
            _txtFullName.Location = new Point(28, 98);
            _txtFullName.Margin = new Padding(3, 4, 3, 4);
            _txtFullName.Name = "_txtFullName";
            _txtFullName.PlaceholderText = "Nhập họ tên";
            _txtFullName.SelectedText = "";
            _txtFullName.ShadowDecoration.CustomizableEdges = customizableEdges12;
            _txtFullName.Size = new Size(360, 36);
            _txtFullName.TabIndex = 3;
            // 
            // lblFullName
            // 
            lblFullName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFullName.ForeColor = Color.FromArgb(71, 85, 105);
            lblFullName.Location = new Point(28, 78);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(100, 20);
            lblFullName.TabIndex = 2;
            lblFullName.Text = "Họ tên";
            // 
            // lblSubTitle
            // 
            lblSubTitle.Font = new Font("Segoe UI", 10F);
            lblSubTitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubTitle.Location = new Point(26, 52);
            lblSubTitle.Name = "lblSubTitle";
            lblSubTitle.Size = new Size(250, 23);
            lblSubTitle.TabIndex = 1;
            lblSubTitle.Text = "Đăng ký tài khoản mới";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(30, 41, 59);
            lblTitle.Location = new Point(24, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(219, 41);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "TRAIN TICKET";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BorderRadius = 16;
            btnClose.CustomizableEdges = customizableEdges1;
            btnClose.FillColor = Color.FromArgb(60, 255, 255, 255);
            btnClose.Font = new Font("Segoe UI", 10F);
            btnClose.ForeColor = Color.White;
            btnClose.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnClose.Location = new Point(454, 8);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnClose.Size = new Size(32, 32);
            btnClose.TabIndex = 1;
            btnClose.Text = "✕";
            btnClose.Click += btnClose_Click;
            // 
            // frmRegister_New
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 23, 42);
            ClientSize = new Size(500, 450);
            Controls.Add(btnClose);
            Controls.Add(card);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmRegister_New";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng ký";
            card.ResumeLayout(false);
            card.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2ShadowPanel card;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubTitle;
        private System.Windows.Forms.Label lblFullName;
        private Guna.UI2.WinForms.Guna2TextBox _txtFullName;
        private System.Windows.Forms.Label lblEmail;
        private Guna.UI2.WinForms.Guna2TextBox _txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private Guna.UI2.WinForms.Guna2TextBox _txtPassword;
        private System.Windows.Forms.Label lblPhone;
        private Guna.UI2.WinForms.Guna2TextBox _txtPhone;
        private Guna.UI2.WinForms.Guna2Button _btnRegister;
        private System.Windows.Forms.Label _lblStatus;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna2Elipse elipse;
    }
}