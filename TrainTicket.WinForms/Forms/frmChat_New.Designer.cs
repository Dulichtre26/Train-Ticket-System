namespace TrainTicket.WinForms.Forms
{
    partial class frmChat_New
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            _leftPanel = new Guna2Panel();
            _lstUsers = new ListBox();
            lblContacts = new Label();
            _rightPanel = new Guna2Panel();
            _chatArea = new Panel();
            _bottomBar = new Panel();
            _txtMessage = new Guna2TextBox();
            _btnSend = new Guna2Button();
            _lblChatWith = new Label();
            _pollTimer = new System.Windows.Forms.Timer(components);
            _leftPanel.SuspendLayout();
            _rightPanel.SuspendLayout();
            _bottomBar.SuspendLayout();
            SuspendLayout();
            // 
            // _leftPanel
            // 
            _leftPanel.Controls.Add(_lstUsers);
            _leftPanel.Controls.Add(lblContacts);
            _leftPanel.CustomizableEdges = customizableEdges1;
            _leftPanel.Dock = DockStyle.Left;
            _leftPanel.FillColor = Color.White;
            _leftPanel.Location = new Point(0, 0);
            _leftPanel.Name = "_leftPanel";
            _leftPanel.ShadowDecoration.CustomizableEdges = customizableEdges2;
            _leftPanel.ShadowDecoration.Depth = 6;
            _leftPanel.ShadowDecoration.Enabled = true;
            _leftPanel.Size = new Size(220, 600);
            _leftPanel.TabIndex = 0;
            // 
            // _lstUsers
            // 
            _lstUsers.BackColor = Color.Turquoise;
            _lstUsers.BorderStyle = BorderStyle.None;
            _lstUsers.Dock = DockStyle.Fill;
            _lstUsers.Font = new Font("Segoe UI", 10F);
            _lstUsers.FormattingEnabled = true;
            _lstUsers.ItemHeight = 23;
            _lstUsers.Location = new Point(0, 44);
            _lstUsers.Name = "_lstUsers";
            _lstUsers.Size = new Size(220, 556);
            _lstUsers.TabIndex = 1;
            // 
            // lblContacts
            // 
            lblContacts.BackColor = Color.SkyBlue;
            lblContacts.Dock = DockStyle.Top;
            lblContacts.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblContacts.Location = new Point(0, 0);
            lblContacts.Name = "lblContacts";
            lblContacts.Padding = new Padding(12, 12, 0, 0);
            lblContacts.Size = new Size(220, 44);
            lblContacts.TabIndex = 0;
            lblContacts.Text = "👤 Liên hệ";
            // 
            // _rightPanel
            // 
            _rightPanel.BackColor = Color.Beige;
            _rightPanel.Controls.Add(_chatArea);
            _rightPanel.Controls.Add(_bottomBar);
            _rightPanel.Controls.Add(_lblChatWith);
            _rightPanel.CustomizableEdges = customizableEdges7;
            _rightPanel.Dock = DockStyle.Fill;
            _rightPanel.Location = new Point(220, 0);
            _rightPanel.Name = "_rightPanel";
            _rightPanel.Padding = new Padding(12);
            _rightPanel.ShadowDecoration.CustomizableEdges = customizableEdges8;
            _rightPanel.Size = new Size(680, 600);
            _rightPanel.TabIndex = 1;
            // 
            // _chatArea
            // 
            _chatArea.AutoScroll = true;
            _chatArea.Dock = DockStyle.Fill;
            _chatArea.Location = new Point(12, 52);
            _chatArea.Name = "_chatArea";
            _chatArea.Padding = new Padding(4);
            _chatArea.Size = new Size(656, 480);
            _chatArea.TabIndex = 2;
            // 
            // _bottomBar
            // 
            _bottomBar.Controls.Add(_txtMessage);
            _bottomBar.Controls.Add(_btnSend);
            _bottomBar.Dock = DockStyle.Bottom;
            _bottomBar.Location = new Point(12, 532);
            _bottomBar.Name = "_bottomBar";
            _bottomBar.Size = new Size(656, 56);
            _bottomBar.TabIndex = 1;
            // 
            // _txtMessage
            // 
            _txtMessage.BorderRadius = 20;
            _txtMessage.CustomizableEdges = customizableEdges3;
            _txtMessage.DefaultText = "";
            _txtMessage.Dock = DockStyle.Fill;
            _txtMessage.Font = new Font("Segoe UI", 10F);
            _txtMessage.Location = new Point(0, 0);
            _txtMessage.Margin = new Padding(8, 8, 4, 8);
            _txtMessage.Name = "_txtMessage";
            _txtMessage.PlaceholderText = "Nhập tin nhắn...";
            _txtMessage.SelectedText = "";
            _txtMessage.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _txtMessage.Size = new Size(592, 56);
            _txtMessage.TabIndex = 0;
            // 
            // _btnSend
            // 
            _btnSend.BorderRadius = 20;
            _btnSend.Cursor = Cursors.Hand;
            _btnSend.CustomizableEdges = customizableEdges5;
            _btnSend.Dock = DockStyle.Right;
            _btnSend.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            _btnSend.ForeColor = Color.White;
            _btnSend.Location = new Point(592, 0);
            _btnSend.Margin = new Padding(4, 8, 8, 8);
            _btnSend.Name = "_btnSend";
            _btnSend.ShadowDecoration.CustomizableEdges = customizableEdges6;
            _btnSend.Size = new Size(64, 56);
            _btnSend.TabIndex = 1;
            _btnSend.Text = "➤";
            _btnSend.Click += _btnSend_Click;
            // 
            // _lblChatWith
            // 
            _lblChatWith.BackColor = Color.Transparent;
            _lblChatWith.Dock = DockStyle.Top;
            _lblChatWith.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            _lblChatWith.Location = new Point(12, 12);
            _lblChatWith.Name = "_lblChatWith";
            _lblChatWith.Size = new Size(656, 40);
            _lblChatWith.TabIndex = 0;
            _lblChatWith.Text = "Chọn người cần liên hệ từ danh sách bên trái";
            // 
            // _pollTimer
            // 
            _pollTimer.Interval = 5000;
            _pollTimer.Tick += _pollTimer_Tick;
            // 
            // frmChat_New
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 600);
            Controls.Add(_rightPanel);
            Controls.Add(_leftPanel);
            Name = "frmChat_New";
            Text = "Cửa sổ Chat";
            FormClosed += frmChat_New_FormClosed;
            Load += frmChat_New_Load;
            _leftPanel.ResumeLayout(false);
            _rightPanel.ResumeLayout(false);
            _bottomBar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel _leftPanel;
        private System.Windows.Forms.Label lblContacts;
        private System.Windows.Forms.ListBox _lstUsers;
        private Guna.UI2.WinForms.Guna2Panel _rightPanel;
        private System.Windows.Forms.Label _lblChatWith;
        private System.Windows.Forms.Panel _bottomBar;
        private Guna.UI2.WinForms.Guna2TextBox _txtMessage;
        private Guna.UI2.WinForms.Guna2Button _btnSend;
        private System.Windows.Forms.Panel _chatArea;
        private System.Windows.Forms.Timer _pollTimer;
    }
}