namespace TrainTicket.WinForms.Forms
{
    partial class frmSearch_new
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
            _filterPanel = new Guna.UI2.WinForms.Guna2Panel();
            _btnSearch = new Guna.UI2.WinForms.Guna2Button();
            _dtpNgayDi = new Guna.UI2.WinForms.Guna2DateTimePicker();
            lblNgay = new System.Windows.Forms.Label();
            _cboGaDen = new Guna.UI2.WinForms.Guna2ComboBox();
            lblGaDen = new System.Windows.Forms.Label();
            _cboGaDi = new Guna.UI2.WinForms.Guna2ComboBox();
            lblGaDi = new System.Windows.Forms.Label();
            _contentPanel = new Guna.UI2.WinForms.Guna2Panel();
            _grid = new Guna.UI2.WinForms.Guna2DataGridView();
            _filterPanel.SuspendLayout();
            _contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(_grid)).BeginInit();
            SuspendLayout();
            // 
            // _filterPanel
            // 
            _filterPanel.Controls.Add(_btnSearch);
            _filterPanel.Controls.Add(_dtpNgayDi);
            _filterPanel.Controls.Add(lblNgay);
            _filterPanel.Controls.Add(_cboGaDen);
            _filterPanel.Controls.Add(lblGaDen);
            _filterPanel.Controls.Add(_cboGaDi);
            _filterPanel.Controls.Add(lblGaDi);
            _filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            _filterPanel.Location = new System.Drawing.Point(0, 0);
            _filterPanel.Name = "_filterPanel";
            _filterPanel.ShadowDecoration.Enabled = true;
            _filterPanel.Size = new System.Drawing.Size(984, 88);
            _filterPanel.TabIndex = 0;
            // 
            // _btnSearch
            // 
            _btnSearch.BorderRadius = 8;
            _btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            _btnSearch.ForeColor = System.Drawing.Color.White;
            _btnSearch.Location = new System.Drawing.Point(860, 14);
            _btnSearch.Name = "_btnSearch";
            _btnSearch.Size = new System.Drawing.Size(100, 36);
            _btnSearch.TabIndex = 6;
            _btnSearch.Text = "🔍 Tìm";
            _btnSearch.Click += new System.EventHandler(_btnSearch_Click);
            // 
            // _dtpNgayDi
            // 
            _dtpNgayDi.BorderRadius = 8;
            _dtpNgayDi.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            _dtpNgayDi.Location = new System.Drawing.Point(690, 15);
            _dtpNgayDi.Name = "_dtpNgayDi";
            _dtpNgayDi.Size = new System.Drawing.Size(150, 36);
            _dtpNgayDi.TabIndex = 5;
            _dtpNgayDi.Value = new System.DateTime(2023, 10, 27, 0, 0, 0, 0);
            // 
            // lblNgay
            // 
            lblNgay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblNgay.Location = new System.Drawing.Point(625, 20);
            lblNgay.Name = "lblNgay";
            lblNgay.Size = new System.Drawing.Size(60, 23);
            lblNgay.TabIndex = 4;
            lblNgay.Text = "Ngày đi";
            // 
            // _cboGaDen
            // 
            _cboGaDen.BackColor = System.Drawing.Color.Transparent;
            _cboGaDen.BorderRadius = 8;
            _cboGaDen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _cboGaDen.Font = new System.Drawing.Font("Segoe UI", 10F);
            _cboGaDen.Location = new System.Drawing.Point(385, 15);
            _cboGaDen.Name = "_cboGaDen";
            _cboGaDen.Size = new System.Drawing.Size(220, 36);
            _cboGaDen.TabIndex = 3;
            // 
            // lblGaDen
            // 
            lblGaDen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblGaDen.Location = new System.Drawing.Point(320, 20);
            lblGaDen.Name = "lblGaDen";
            lblGaDen.Size = new System.Drawing.Size(60, 23);
            lblGaDen.TabIndex = 2;
            lblGaDen.Text = "Ga đến";
            // 
            // _cboGaDi
            // 
            _cboGaDi.BackColor = System.Drawing.Color.Transparent;
            _cboGaDi.BorderRadius = 8;
            _cboGaDi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            _cboGaDi.Font = new System.Drawing.Font("Segoe UI", 10F);
            _cboGaDi.Location = new System.Drawing.Point(80, 15);
            _cboGaDi.Name = "_cboGaDi";
            _cboGaDi.Size = new System.Drawing.Size(220, 36);
            _cboGaDi.TabIndex = 1;
            // 
            // lblGaDi
            // 
            lblGaDi.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblGaDi.Location = new System.Drawing.Point(20, 20);
            lblGaDi.Name = "lblGaDi";
            lblGaDi.Size = new System.Drawing.Size(60, 23);
            lblGaDi.TabIndex = 0;
            lblGaDi.Text = "Ga đi";
            // 
            // _contentPanel
            // 
            _contentPanel.Controls.Add(_grid);
            _contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _contentPanel.Location = new System.Drawing.Point(0, 88);
            _contentPanel.Name = "_contentPanel";
            _contentPanel.Padding = new System.Windows.Forms.Padding(16);
            _contentPanel.Size = new System.Drawing.Size(984, 473);
            _contentPanel.TabIndex = 1;
            // 
            // _grid
            // 
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            _grid.Dock = System.Windows.Forms.DockStyle.Fill;
            _grid.Location = new System.Drawing.Point(16, 16);
            _grid.Name = "_grid";
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            _grid.Size = new System.Drawing.Size(952, 441);
            _grid.TabIndex = 0;
            _grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(_grid_CellDoubleClick);
            // 
            // frmSearch_new
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(984, 561);
            Controls.Add(_contentPanel);
            Controls.Add(_filterPanel);
            Name = "frmSearch_new";
            Text = "Tìm chuyến tàu";
            Load += new System.EventHandler(frmSearch_new_Load);
            _filterPanel.ResumeLayout(false);
            _contentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(_grid)).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel _filterPanel;
        private Guna.UI2.WinForms.Guna2Panel _contentPanel;
        private Guna.UI2.WinForms.Guna2ComboBox _cboGaDi;
        private System.Windows.Forms.Label lblGaDi;
        private Guna.UI2.WinForms.Guna2ComboBox _cboGaDen;
        private System.Windows.Forms.Label lblGaDen;
        private Guna.UI2.WinForms.Guna2DateTimePicker _dtpNgayDi;
        private System.Windows.Forms.Label lblNgay;
        private Guna.UI2.WinForms.Guna2Button _btnSearch;
        private Guna.UI2.WinForms.Guna2DataGridView _grid;
    }
}