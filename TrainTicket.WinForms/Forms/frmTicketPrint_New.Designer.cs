namespace TrainTicket.WinForms.Forms
{
    partial class frmTicketPrint_New
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
            _toolbar = new System.Windows.Forms.Panel();
            _btnPrint = new Guna.UI2.WinForms.Guna2Button();
            _btnClose = new Guna.UI2.WinForms.Guna2Button();
            _preview = new System.Windows.Forms.PrintPreviewControl();

            _header.SuspendLayout();
            _toolbar.SuspendLayout();
            SuspendLayout();

            // 
            // _header
            // 
            _header.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            _header.Controls.Add(_lblHeader);
            _header.Dock = System.Windows.Forms.DockStyle.Top;
            _header.Location = new System.Drawing.Point(0, 0);
            _header.Name = "_header";
            _header.Size = new System.Drawing.Size(640, 50);
            _header.TabIndex = 0;
            // 
            // _lblHeader
            // 
            _lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            _lblHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            _lblHeader.ForeColor = System.Drawing.Color.White;
            _lblHeader.Location = new System.Drawing.Point(0, 0);
            _lblHeader.Name = "_lblHeader";
            _lblHeader.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            _lblHeader.Size = new System.Drawing.Size(640, 50);
            _lblHeader.TabIndex = 0;
            _lblHeader.Text = "???  In vé / Xem tr??c — Vé #?";
            _lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _toolbar
            // 
            _toolbar.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            _toolbar.Controls.Add(_btnPrint);
            _toolbar.Controls.Add(_btnClose);
            _toolbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            _toolbar.Location = new System.Drawing.Point(0, 510);
            _toolbar.Name = "_toolbar";
            _toolbar.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            _toolbar.Size = new System.Drawing.Size(640, 50);
            _toolbar.TabIndex = 1;
            // 
            // _btnPrint
            // 
            _btnPrint.BorderRadius = 8;
            _btnPrint.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            _btnPrint.ForeColor = System.Drawing.Color.White;
            _btnPrint.Location = new System.Drawing.Point(10, 8);
            _btnPrint.Name = "_btnPrint";
            _btnPrint.Size = new System.Drawing.Size(130, 34);
            _btnPrint.TabIndex = 0;
            _btnPrint.Text = "???  In ngay";
            _btnPrint.Click += new System.EventHandler(_btnPrint_Click);
            // 
            // _btnClose
            // 
            _btnClose.BorderRadius = 8;
            _btnClose.FillColor = System.Drawing.Color.FromArgb(100, 116, 139);
            _btnClose.Font = new System.Drawing.Font("Segoe UI", 10F);
            _btnClose.ForeColor = System.Drawing.Color.White;
            _btnClose.Location = new System.Drawing.Point(150, 8);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new System.Drawing.Size(90, 34);
            _btnClose.TabIndex = 1;
            _btnClose.Text = "?óng";
            _btnClose.Click += new System.EventHandler(_btnClose_Click);
            // 
            // _preview
            // 
            _preview.BackColor = System.Drawing.Color.FromArgb(200, 200, 210);
            _preview.Dock = System.Windows.Forms.DockStyle.Fill;
            _preview.Location = new System.Drawing.Point(0, 50);
            _preview.Name = "_preview";
            _preview.Size = new System.Drawing.Size(640, 460);
            _preview.TabIndex = 2;
            // 
            // frmTicketPrint_New
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 247, 251);
            ClientSize = new System.Drawing.Size(640, 560);
            Controls.Add(_preview);
            Controls.Add(_toolbar);
            Controls.Add(_header);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            MaximizeBox = true;
            Name = "frmTicketPrint_New";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "In vé";
            Load += new System.EventHandler(frmTicketPrint_New_Load);

            _header.ResumeLayout(false);
            _toolbar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel _header;
        private System.Windows.Forms.Label _lblHeader;
        private System.Windows.Forms.Panel _toolbar;
        private Guna.UI2.WinForms.Guna2Button _btnPrint;
        private Guna.UI2.WinForms.Guna2Button _btnClose;
        private System.Windows.Forms.PrintPreviewControl _preview;
    }
}