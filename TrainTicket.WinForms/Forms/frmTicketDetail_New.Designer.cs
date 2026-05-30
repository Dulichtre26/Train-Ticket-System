namespace TrainTicket.WinForms.Forms
{
    partial class frmTicketDetail_New
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
            _header = new System.Windows.Forms.Panel();
            _lblHeader = new System.Windows.Forms.Label();
            _body = new System.Windows.Forms.Panel();
            _btnClose = new Guna.UI2.WinForms.Guna2Button();

            _header.SuspendLayout();
            SuspendLayout();

            // 
            // _header
            // 
            _header.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            _header.Controls.Add(_lblHeader);
            _header.Dock = System.Windows.Forms.DockStyle.Top;
            _header.Location = new System.Drawing.Point(0, 0);
            _header.Name = "_header";
            _header.Size = new System.Drawing.Size(520, 56);
            _header.TabIndex = 0;
            // 
            // _lblHeader
            // 
            _lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            _lblHeader.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            _lblHeader.ForeColor = System.Drawing.Color.White;
            _lblHeader.Location = new System.Drawing.Point(0, 0);
            _lblHeader.Name = "_lblHeader";
            _lblHeader.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            _lblHeader.Size = new System.Drawing.Size(520, 56);
            _lblHeader.TabIndex = 0;
            _lblHeader.Text = "??? Chi ti?t vé #?";
            _lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _body
            // 
            _body.AutoScroll = true;
            _body.BackColor = System.Drawing.Color.FromArgb(245, 247, 251);
            _body.Dock = System.Windows.Forms.DockStyle.Fill;
            _body.Location = new System.Drawing.Point(0, 56);
            _body.Name = "_body";
            _body.Padding = new System.Windows.Forms.Padding(20);
            _body.Size = new System.Drawing.Size(520, 482);
            _body.TabIndex = 1;
            // 
            // _btnClose
            // 
            _btnClose.BorderRadius = 0;
            _btnClose.Dock = System.Windows.Forms.DockStyle.Bottom;
            _btnClose.FillColor = System.Drawing.Color.FromArgb(30, 41, 59);
            _btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _btnClose.ForeColor = System.Drawing.Color.White;
            _btnClose.Location = new System.Drawing.Point(0, 538);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new System.Drawing.Size(520, 42);
            _btnClose.TabIndex = 2;
            _btnClose.Text = "?óng";
            _btnClose.Click += new System.EventHandler(_btnClose_Click);
            // 
            // frmTicketDetail_New
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 251);
            ClientSize = new System.Drawing.Size(520, 580);
            Controls.Add(_body);
            Controls.Add(_btnClose);
            Controls.Add(_header);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            MaximizeBox = true;
            Name = "frmTicketDetail_New";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Chi ti?t vé";
            Load += new System.EventHandler(frmTicketDetail_New_Load);

            _header.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel _header;
        private System.Windows.Forms.Label _lblHeader;
        private System.Windows.Forms.Panel _body;
        private Guna.UI2.WinForms.Guna2Button _btnClose;
    }
}