namespace TrainTicket.WinForms.Forms
{
    partial class frmSeatMap_New
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            _toolbarPanel = new Guna2Panel();
            _lblSelected = new Label();
            _btnBook = new Guna2Button();
            _seatPanel = new FlowLayoutPanel();
            _toolbarPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _toolbarPanel
            // 
            _toolbarPanel.Controls.Add(_lblSelected);
            _toolbarPanel.Controls.Add(_btnBook);
            _toolbarPanel.CustomizableEdges = customizableEdges3;
            _toolbarPanel.Dock = DockStyle.Bottom;
            _toolbarPanel.Location = new Point(0, 411);
            _toolbarPanel.Margin = new Padding(3, 2, 3, 2);
            _toolbarPanel.Name = "_toolbarPanel";
            _toolbarPanel.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _toolbarPanel.ShadowDecoration.Enabled = true;
            _toolbarPanel.Size = new Size(858, 54);
            _toolbarPanel.TabIndex = 0;
            // 
            // _lblSelected
            // 
            _lblSelected.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _lblSelected.Location = new Point(18, 18);
            _lblSelected.Name = "_lblSelected";
            _lblSelected.Size = new Size(542, 17);
            _lblSelected.TabIndex = 0;
            _lblSelected.Text = "Chua chon ghe";
            // 
            // _btnBook
            // 
            _btnBook.BorderRadius = 10;
            _btnBook.CustomizableEdges = customizableEdges1;
            _btnBook.Font = new Font("Segoe UI", 9F);
            _btnBook.ForeColor = Color.White;
            _btnBook.HoverState.FillColor = Color.FromArgb(5, 150, 105);
            _btnBook.Location = new Point(665, 12);
            _btnBook.Margin = new Padding(3, 2, 3, 2);
            _btnBook.Name = "_btnBook";
            _btnBook.ShadowDecoration.CustomizableEdges = customizableEdges2;
            _btnBook.Size = new Size(158, 30);
            _btnBook.TabIndex = 1;
            _btnBook.Text = "??t v� gh? ?ang ch?n";
            _btnBook.Click += _btnBook_Click;
            // 
            // _seatPanel
            // 
            _seatPanel.AutoScroll = true;
            _seatPanel.Dock = DockStyle.Fill;
            _seatPanel.Location = new Point(0, 0);
            _seatPanel.Margin = new Padding(3, 2, 3, 2);
            _seatPanel.Name = "_seatPanel";
            _seatPanel.Padding = new Padding(18, 15, 18, 15);
            _seatPanel.Size = new Size(858, 411);
            _seatPanel.TabIndex = 1;
            // 
            // frmSeatMap_New
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(858, 465);
            Controls.Add(_seatPanel);
            Controls.Add(_toolbarPanel);
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmSeatMap_New";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Sơ đồ ghế";
            Load += frmSeatMap_New_Load;
            _toolbarPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel _toolbarPanel;
        private System.Windows.Forms.Label _lblSelected;
        private Guna.UI2.WinForms.Guna2Button _btnBook;
        private FlowLayoutPanel _seatPanel;
    }
}