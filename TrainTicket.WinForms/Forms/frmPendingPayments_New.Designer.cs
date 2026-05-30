namespace TrainTicket.WinForms.Forms
{
    partial class frmPendingPayments_New
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            _topPanel = new Guna.UI2.WinForms.Guna2Panel();
            _lblInfo = new System.Windows.Forms.Label();
            _btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            _btnPay = new Guna.UI2.WinForms.Guna2Button();
            _grid = new Guna.UI2.WinForms.Guna2DataGridView();
            _topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
            SuspendLayout();
            // 
            // _topPanel
            // 
            _topPanel.Controls.Add(_btnPay);
            _topPanel.Controls.Add(_btnRefresh);
            _topPanel.Controls.Add(_lblInfo);
            _topPanel.CustomizableEdges = customizableEdges1;
            _topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            _topPanel.Location = new System.Drawing.Point(0, 0);
            _topPanel.Name = "_topPanel";
            _topPanel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            _topPanel.ShadowDecoration.CustomizableEdges = customizableEdges2;
            _topPanel.ShadowDecoration.Depth = 4;
            _topPanel.ShadowDecoration.Enabled = true;
            _topPanel.Size = new System.Drawing.Size(900, 52);
            _topPanel.TabIndex = 0;
            // 
            // _lblInfo
            // 
            _lblInfo.AutoSize = true;
            _lblInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            _lblInfo.Location = new System.Drawing.Point(12, 16);
            _lblInfo.Name = "_lblInfo";
            _lblInfo.Size = new System.Drawing.Size(300, 17);
            _lblInfo.TabIndex = 0;
            _lblInfo.Text = "?ang t?i...";
            // 
            // _btnRefresh
            // 
            _btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnRefresh.BorderRadius = 8;
            _btnRefresh.CustomizableEdges = customizableEdges3;
            _btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            _btnRefresh.ForeColor = System.Drawing.Color.White;
            _btnRefresh.Location = new System.Drawing.Point(790, 9);
            _btnRefresh.Name = "_btnRefresh";
            _btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _btnRefresh.Size = new System.Drawing.Size(96, 32);
            _btnRefresh.TabIndex = 2;
            _btnRefresh.Text = "?? Làm m?i";
            _btnRefresh.Click += _btnRefresh_Click;
            // 
            // _btnPay
            // 
            _btnPay.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            _btnPay.BorderRadius = 8;
            _btnPay.CustomizableEdges = customizableEdges5;
            _btnPay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            _btnPay.ForeColor = System.Drawing.Color.White;
            _btnPay.Location = new System.Drawing.Point(660, 9);
            _btnPay.Name = "_btnPay";
            _btnPay.ShadowDecoration.CustomizableEdges = customizableEdges6;
            _btnPay.Size = new System.Drawing.Size(124, 32);
            _btnPay.TabIndex = 1;
            _btnPay.Text = "?? Thanh toán vé";
            _btnPay.Click += _btnPay_Click;
            // 
            // _grid
            // 
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            _grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            _grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            _grid.ColumnHeadersHeight = 36;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            _grid.DefaultCellStyle = dataGridViewCellStyle3;
            _grid.Dock = System.Windows.Forms.DockStyle.Fill;
            _grid.Location = new System.Drawing.Point(0, 52);
            _grid.Name = "_grid";
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.RowTemplate.Height = 36;
            _grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            _grid.Size = new System.Drawing.Size(900, 500);
            _grid.TabIndex = 1;
            // 
            // frmPendingPayments_New
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(900, 552);
            Controls.Add(_grid);
            Controls.Add(_topPanel);
            Name = "frmPendingPayments_New";
            Text = "Thanh toán vé";
            Load += frmPendingPayments_New_Load;
            _topPanel.ResumeLayout(false);
            _topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel _topPanel;
        private System.Windows.Forms.Label _lblInfo;
        private Guna.UI2.WinForms.Guna2Button _btnRefresh;
        private Guna.UI2.WinForms.Guna2Button _btnPay;
        private Guna.UI2.WinForms.Guna2DataGridView _grid;
    }
}
