namespace TrainTicket.WinForms.Forms
{
    partial class frmEditTicket_New
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            _header = new Panel();
            _lblHeader = new Label();
            _card = new Panel();
            _lblInfo = new Label();
            _txtName = new Guna2TextBox();
            _txtIdNum = new Guna2TextBox();
            _txtPhone = new Guna2TextBox();
            _btnSave = new Guna2Button();
            _btnCancel = new Guna2Button();
            _header.SuspendLayout();
            _card.SuspendLayout();
            SuspendLayout();
            // 
            // _header
            // 
            _header.BackColor = Color.FromArgb(30, 41, 59);
            _header.Controls.Add(_lblHeader);
            _header.Dock = DockStyle.Top;
            _header.Location = new Point(0, 0);
            _header.Name = "_header";
            _header.Size = new Size(460, 52);
            _header.TabIndex = 0;
            // 
            // _lblHeader
            // 
            _lblHeader.Dock = DockStyle.Fill;
            _lblHeader.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _lblHeader.ForeColor = Color.White;
            _lblHeader.Location = new Point(0, 0);
            _lblHeader.Name = "_lblHeader";
            _lblHeader.Padding = new Padding(16, 0, 0, 0);
            _lblHeader.Size = new Size(460, 52);
            _lblHeader.TabIndex = 0;
            _lblHeader.Text = "📝  Sửa thông tin hành khách — Vé #?";
            _lblHeader.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _card
            // 
            _card.BackColor = Color.White;
            _card.Controls.Add(_lblInfo);
            _card.Controls.Add(_txtName);
            _card.Controls.Add(_txtIdNum);
            _card.Controls.Add(_txtPhone);
            _card.Location = new Point(20, 68);
            _card.Name = "_card";
            _card.Size = new Size(400, 240);
            _card.TabIndex = 1;
            // 
            // _lblInfo
            // 
            _lblInfo.BackColor = Color.Transparent;
            _lblInfo.Font = new Font("Segoe UI", 9F);
            _lblInfo.ForeColor = Color.FromArgb(100, 116, 139);
            _lblInfo.Location = new Point(0, 0);
            _lblInfo.Name = "_lblInfo";
            _lblInfo.Size = new Size(400, 24);
            _lblInfo.TabIndex = 0;
            // 
            // _txtName
            // 
            _txtName.BorderRadius = 8;
            _txtName.CustomizableEdges = customizableEdges1;
            _txtName.DefaultText = "";
            _txtName.FillColor = Color.FromArgb(248, 250, 252);
            _txtName.FocusedState.BorderColor = Color.FromArgb(59, 130, 246);
            _txtName.Font = new Font("Segoe UI", 10F);
            _txtName.ForeColor = Color.FromArgb(30, 41, 59);
            _txtName.Location = new Point(0, 30);
            _txtName.Margin = new Padding(3, 4, 3, 4);
            _txtName.Name = "_txtName";
            _txtName.PlaceholderText = "Họ và tên hành khách";
            _txtName.SelectedText = "";
            _txtName.ShadowDecoration.CustomizableEdges = customizableEdges2;
            _txtName.Size = new Size(400, 40);
            _txtName.TabIndex = 1;
            // 
            // _txtIdNum
            // 
            _txtIdNum.BorderRadius = 8;
            _txtIdNum.CustomizableEdges = customizableEdges3;
            _txtIdNum.DefaultText = "";
            _txtIdNum.FillColor = Color.FromArgb(248, 250, 252);
            _txtIdNum.FocusedState.BorderColor = Color.FromArgb(59, 130, 246);
            _txtIdNum.Font = new Font("Segoe UI", 10F);
            _txtIdNum.ForeColor = Color.FromArgb(30, 41, 59);
            _txtIdNum.Location = new Point(0, 90);
            _txtIdNum.Margin = new Padding(3, 4, 3, 4);
            _txtIdNum.Name = "_txtIdNum";
            _txtIdNum.PlaceholderText = "Số CMND / CCCD";
            _txtIdNum.SelectedText = "";
            _txtIdNum.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _txtIdNum.Size = new Size(400, 40);
            _txtIdNum.TabIndex = 2;
            // 
            // _txtPhone
            // 
            _txtPhone.BorderRadius = 8;
            _txtPhone.CustomizableEdges = customizableEdges5;
            _txtPhone.DefaultText = "";
            _txtPhone.FillColor = Color.FromArgb(248, 250, 252);
            _txtPhone.FocusedState.BorderColor = Color.FromArgb(59, 130, 246);
            _txtPhone.Font = new Font("Segoe UI", 10F);
            _txtPhone.ForeColor = Color.FromArgb(30, 41, 59);
            _txtPhone.Location = new Point(0, 150);
            _txtPhone.Margin = new Padding(3, 4, 3, 4);
            _txtPhone.Name = "_txtPhone";
            _txtPhone.PlaceholderText = "Số điện thoại";
            _txtPhone.SelectedText = "";
            _txtPhone.ShadowDecoration.CustomizableEdges = customizableEdges6;
            _txtPhone.Size = new Size(400, 40);
            _txtPhone.TabIndex = 3;
            // 
            // _btnSave
            // 
            _btnSave.BorderRadius = 8;
            _btnSave.CustomizableEdges = customizableEdges7;
            _btnSave.FillColor = Color.FromArgb(59, 130, 246);
            _btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnSave.ForeColor = Color.White;
            _btnSave.Location = new Point(20, 318);
            _btnSave.Name = "_btnSave";
            _btnSave.ShadowDecoration.CustomizableEdges = customizableEdges8;
            _btnSave.Size = new Size(199, 38);
            _btnSave.TabIndex = 2;
            _btnSave.Text = "💾  Lưu thay đổi";
            _btnSave.Click += _btnSave_Click;
            // 
            // _btnCancel
            // 
            _btnCancel.BorderRadius = 8;
            _btnCancel.CustomizableEdges = customizableEdges9;
            _btnCancel.FillColor = Color.FromArgb(226, 232, 240);
            _btnCancel.Font = new Font("Segoe UI", 10F);
            _btnCancel.ForeColor = Color.FromArgb(71, 85, 105);
            _btnCancel.Location = new Point(240, 318);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges10;
            _btnCancel.Size = new Size(110, 38);
            _btnCancel.TabIndex = 3;
            _btnCancel.Text = "Hủy";
            _btnCancel.Click += _btnCancel_Click;
            // 
            // frmEditTicket_New
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 251);
            ClientSize = new Size(460, 380);
            Controls.Add(_btnCancel);
            Controls.Add(_btnSave);
            Controls.Add(_card);
            Controls.Add(_header);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimizeBox = false;
            Name = "frmEditTicket_New";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Sửa thông tin vé";
            Load += frmEditTicket_New_Load;
            _header.ResumeLayout(false);
            _card.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel _header;
        private System.Windows.Forms.Label _lblHeader;
        private System.Windows.Forms.Panel _card;
        private System.Windows.Forms.Label _lblInfo;
        private Guna.UI2.WinForms.Guna2TextBox _txtName;
        private Guna.UI2.WinForms.Guna2TextBox _txtIdNum;
        private Guna.UI2.WinForms.Guna2TextBox _txtPhone;
        private Guna.UI2.WinForms.Guna2Button _btnSave;
        private Guna.UI2.WinForms.Guna2Button _btnCancel;
    }
}