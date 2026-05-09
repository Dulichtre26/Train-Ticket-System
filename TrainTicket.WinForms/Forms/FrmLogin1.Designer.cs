namespace TrainTicket.WinForms.Forms
{
    partial class FrmLogin1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(components);
            guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            cardPanel = new Guna.UI2.WinForms.Guna2ShadowPanel();
            _lbStatus = new Label();
            _btnLogin = new Guna.UI2.WinForms.Guna2Button();
            _txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            _txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            label2 = new Label();
            label1 = new Label();
            cardPanel.SuspendLayout();
            SuspendLayout();
            // 
            // guna2Elipse1
            // 
            guna2Elipse1.BorderRadius = 16;
            guna2Elipse1.TargetControl = this;
            // 
            // guna2ControlBox1
            // 
            guna2ControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            guna2ControlBox1.CustomizableEdges = customizableEdges7;
            guna2ControlBox1.FillColor = Color.Transparent;
            guna2ControlBox1.ForeColor = SystemColors.Desktop;
            guna2ControlBox1.IconColor = Color.White;
            guna2ControlBox1.Location = new Point(656, 372);
            guna2ControlBox1.Margin = new Padding(3, 2, 3, 2);
            guna2ControlBox1.Name = "guna2ControlBox1";
            guna2ControlBox1.ShadowDecoration.CustomizableEdges = customizableEdges8;
            guna2ControlBox1.Size = new Size(114, 28);
            guna2ControlBox1.TabIndex = 0;
            // 
            // cardPanel
            // 
            cardPanel.BackColor = Color.Transparent;
            cardPanel.Controls.Add(_lbStatus);
            cardPanel.Controls.Add(_btnLogin);
            cardPanel.Controls.Add(_txtPassword);
            cardPanel.Controls.Add(_txtEmail);
            cardPanel.Controls.Add(label2);
            cardPanel.Controls.Add(label1);
            cardPanel.FillColor = Color.White;
            cardPanel.Location = new Point(141, 34);
            cardPanel.Margin = new Padding(3, 2, 3, 2);
            cardPanel.Name = "cardPanel";
            cardPanel.Radius = 14;
            cardPanel.ShadowColor = Color.Black;
            cardPanel.Size = new Size(518, 311);
            cardPanel.TabIndex = 1;
            // 
            // _lbStatus
            // 
            _lbStatus.AutoSize = true;
            _lbStatus.ForeColor = Color.Red;
            _lbStatus.Location = new Point(49, 256);
            _lbStatus.Name = "_lbStatus";
            _lbStatus.Size = new Size(21, 15);
            _lbStatus.TabIndex = 4;
            _lbStatus.Text = "dđ";
            // 
            // _btnLogin
            // 
            _btnLogin.CustomizableEdges = customizableEdges1;
            _btnLogin.DisabledState.BorderColor = Color.DarkGray;
            _btnLogin.DisabledState.CustomBorderColor = Color.DarkGray;
            _btnLogin.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            _btnLogin.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            _btnLogin.FillColor = Color.FromArgb(128, 128, 255);
            _btnLogin.Font = new Font("Segoe UI", 9F);
            _btnLogin.ForeColor = Color.White;
            _btnLogin.Location = new Point(313, 256);
            _btnLogin.Margin = new Padding(3, 2, 3, 2);
            _btnLogin.Name = "_btnLogin";
            _btnLogin.ShadowDecoration.CustomizableEdges = customizableEdges2;
            _btnLogin.Size = new Size(169, 34);
            _btnLogin.TabIndex = 2;
            _btnLogin.Text = "Đăng nhập";
            // 
            // _txtPassword
            // 
            _txtPassword.CustomizableEdges = customizableEdges3;
            _txtPassword.DefaultText = "Nhập mật khẩu:";
            _txtPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            _txtPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            _txtPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            _txtPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            _txtPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            _txtPassword.Font = new Font("Segoe UI", 9F);
            _txtPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            _txtPassword.Location = new Point(49, 196);
            _txtPassword.Name = "_txtPassword";
            _txtPassword.PlaceholderText = "Nhập email:";
            _txtPassword.SelectedText = "";
            _txtPassword.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _txtPassword.Size = new Size(371, 46);
            _txtPassword.TabIndex = 3;
            // 
            // _txtEmail
            // 
            _txtEmail.CustomizableEdges = customizableEdges5;
            _txtEmail.DefaultText = "Nhập email:";
            _txtEmail.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            _txtEmail.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            _txtEmail.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            _txtEmail.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            _txtEmail.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            _txtEmail.Font = new Font("Segoe UI", 9F);
            _txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            _txtEmail.Location = new Point(49, 114);
            _txtEmail.Name = "_txtEmail";
            _txtEmail.PlaceholderText = "Nhập email";
            _txtEmail.SelectedText = "";
            _txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges6;
            _txtEmail.Size = new Size(371, 46);
            _txtEmail.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(49, 62);
            label2.Name = "label2";
            label2.Size = new Size(124, 15);
            label2.TabIndex = 1;
            label2.Text = "Đăng nhập để tiếp tục";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(39, 22);
            label1.Name = "label1";
            label1.Size = new Size(175, 32);
            label1.TabIndex = 0;
            label1.Text = "TRAIN TICKET";
            // 
            // FrmLogin1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 0, 64);
            ClientSize = new Size(790, 384);
            Controls.Add(cardPanel);
            Controls.Add(guna2ControlBox1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmLogin1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập";
            cardPanel.ResumeLayout(false);
            cardPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2ShadowPanel cardPanel;
        private Label label1;
        private Guna.UI2.WinForms.Guna2TextBox _txtPassword;
        private Guna.UI2.WinForms.Guna2TextBox _txtEmail;
        private Label label2;
        private Label _lbStatus;
        private Guna.UI2.WinForms.Guna2Button _btnLogin;
    }
}