namespace TrainTicket.WinForms.Forms
{
    partial class frmReports_New
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            _topPanel = new Guna2Panel();
            _btnExportExcel = new Guna2Button();
            _btnLoad = new Guna2Button();
            _cboMonth = new Guna2ComboBox();
            lblMonth = new Label();
            _numYear = new Guna2NumericUpDown();
            lblYear = new Label();
            _bodyPanel = new Guna2Panel();
            splitContainer = new SplitContainer();
            _grid = new Guna2DataGridView();
            _chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            _topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_numYear).BeginInit();
            _bodyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_chart).BeginInit();
            SuspendLayout();
            // 
            // _topPanel
            // 
            _topPanel.Controls.Add(_btnExportExcel);
            _topPanel.Controls.Add(_btnLoad);
            _topPanel.Controls.Add(_cboMonth);
            _topPanel.Controls.Add(lblMonth);
            _topPanel.Controls.Add(_numYear);
            _topPanel.Controls.Add(lblYear);
            _topPanel.CustomizableEdges = customizableEdges9;
            _topPanel.Dock = DockStyle.Top;
            _topPanel.Location = new Point(0, 0);
            _topPanel.Margin = new Padding(3, 2, 3, 2);
            _topPanel.Name = "_topPanel";
            _topPanel.ShadowDecoration.CustomizableEdges = customizableEdges10;
            _topPanel.ShadowDecoration.Enabled = true;
            _topPanel.Size = new Size(1050, 62);
            _topPanel.TabIndex = 0;
            // 
            // _btnExportExcel
            // 
            _btnExportExcel.BorderRadius = 10;
            _btnExportExcel.CustomizableEdges = customizableEdges1;
            _btnExportExcel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnExportExcel.ForeColor = Color.White;
            _btnExportExcel.Location = new Point(432, 10);
            _btnExportExcel.Margin = new Padding(3, 2, 3, 2);
            _btnExportExcel.Name = "_btnExportExcel";
            _btnExportExcel.ShadowDecoration.CustomizableEdges = customizableEdges2;
            _btnExportExcel.Size = new Size(114, 28);
            _btnExportExcel.TabIndex = 5;
            _btnExportExcel.Text = "Xuất Excel";
            _btnExportExcel.Click += _btnExportExcel_Click;
            // 
            // _btnLoad
            // 
            _btnLoad.BorderRadius = 10;
            _btnLoad.CustomizableEdges = customizableEdges3;
            _btnLoad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            _btnLoad.ForeColor = Color.White;
            _btnLoad.Location = new Point(298, 10);
            _btnLoad.Margin = new Padding(3, 2, 3, 2);
            _btnLoad.Name = "_btnLoad";
            _btnLoad.ShadowDecoration.CustomizableEdges = customizableEdges4;
            _btnLoad.Size = new Size(122, 28);
            _btnLoad.TabIndex = 4;
            _btnLoad.Text = "Xem báo cáo";
            _btnLoad.Click += _btnLoad_Click;
            // 
            // _cboMonth
            // 
            _cboMonth.BackColor = Color.Transparent;
            _cboMonth.BorderRadius = 8;
            _cboMonth.CustomizableEdges = customizableEdges5;
            _cboMonth.DrawMode = DrawMode.OwnerDrawFixed;
            _cboMonth.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboMonth.FocusedColor = Color.Empty;
            _cboMonth.Font = new Font("Segoe UI", 10F);
            _cboMonth.ForeColor = Color.FromArgb(68, 88, 112);
            _cboMonth.ItemHeight = 30;
            _cboMonth.Items.AddRange(new object[] { "Tất cả", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" });
            _cboMonth.Location = new Point(197, 10);
            _cboMonth.Margin = new Padding(3, 2, 3, 2);
            _cboMonth.Name = "_cboMonth";
            _cboMonth.ShadowDecoration.CustomizableEdges = customizableEdges6;
            _cboMonth.Size = new Size(88, 36);
            _cboMonth.TabIndex = 3;
            // 
            // lblMonth
            // 
            lblMonth.AutoSize = true;
            lblMonth.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMonth.Location = new Point(149, 14);
            lblMonth.Name = "lblMonth";
            lblMonth.Size = new Size(41, 15);
            lblMonth.TabIndex = 2;
            lblMonth.Text = "Tháng";
            // 
            // _numYear
            // 
            _numYear.BackColor = Color.Transparent;
            _numYear.BorderRadius = 8;
            _numYear.CustomizableEdges = customizableEdges7;
            _numYear.Font = new Font("Segoe UI", 9F);
            _numYear.Location = new Point(58, 10);
            _numYear.Margin = new Padding(3, 2, 3, 2);
            _numYear.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            _numYear.Minimum = new decimal(new int[] { 2000, 0, 0, 0 });
            _numYear.Name = "_numYear";
            _numYear.ShadowDecoration.CustomizableEdges = customizableEdges8;
            _numYear.Size = new Size(79, 27);
            _numYear.TabIndex = 1;
            _numYear.Value = new decimal(new int[] { 2024, 0, 0, 0 });
            // 
            // lblYear
            // 
            lblYear.AutoSize = true;
            lblYear.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblYear.Location = new Point(18, 14);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(32, 15);
            lblYear.TabIndex = 0;
            lblYear.Text = "N?m";
            // 
            // _bodyPanel
            // 
            _bodyPanel.Controls.Add(splitContainer);
            _bodyPanel.CustomizableEdges = customizableEdges11;
            _bodyPanel.Dock = DockStyle.Fill;
            _bodyPanel.Location = new Point(0, 62);
            _bodyPanel.Margin = new Padding(3, 2, 3, 2);
            _bodyPanel.Name = "_bodyPanel";
            _bodyPanel.Padding = new Padding(12, 10, 12, 10);
            _bodyPanel.ShadowDecoration.CustomizableEdges = customizableEdges12;
            _bodyPanel.Size = new Size(1050, 463);
            _bodyPanel.TabIndex = 1;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(12, 10);
            splitContainer.Margin = new Padding(3, 2, 3, 2);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(_grid);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(_chart);
            splitContainer.Size = new Size(1026, 443);
            splitContainer.SplitterDistance = 612;
            splitContainer.TabIndex = 0;
            // 
            // _grid
            // 
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            _grid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            _grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            _grid.DefaultCellStyle = dataGridViewCellStyle3;
            _grid.Dock = DockStyle.Fill;
            _grid.GridColor = Color.FromArgb(231, 229, 255);
            _grid.Location = new Point(0, 0);
            _grid.Margin = new Padding(3, 2, 3, 2);
            _grid.Name = "_grid";
            _grid.ReadOnly = true;
            _grid.RowHeadersVisible = false;
            _grid.Size = new Size(612, 443);
            _grid.TabIndex = 0;
            _grid.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            _grid.ThemeStyle.AlternatingRowsStyle.Font = null;
            _grid.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            _grid.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            _grid.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            _grid.ThemeStyle.BackColor = Color.White;
            _grid.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            _grid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            _grid.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            _grid.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            _grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            _grid.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            _grid.ThemeStyle.HeaderStyle.Height = 23;
            _grid.ThemeStyle.ReadOnly = true;
            _grid.ThemeStyle.RowsStyle.BackColor = Color.White;
            _grid.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            _grid.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            _grid.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            _grid.ThemeStyle.RowsStyle.Height = 25;
            _grid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            _grid.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // _chart
            // 
            var chartArea = new System.Windows.Forms.DataVisualization.Charting.ChartArea("Main");
            chartArea.AxisX.Title = "Tháng";
            chartArea.AxisY.Title = "Doanh thu (VNĐ)";
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(220, 220, 220);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(220, 220, 220);
            chartArea.BackColor = Color.White;
            _chart.ChartAreas.Add(chartArea);
            var legend = new System.Windows.Forms.DataVisualization.Charting.Legend("Legend");
            _chart.Legends.Add(legend);
            var series = new System.Windows.Forms.DataVisualization.Charting.Series("DoanhThu");
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            series.Color = Color.FromArgb(99, 102, 241);
            series.BorderWidth = 2;
            series.IsValueShownAsLabel = true;
            series.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            _chart.Series.Add(series);
            _chart.Dock = DockStyle.Fill;
            _chart.Name = "_chart";
            _chart.TabIndex = 1;
            _chart.Titles.Add(new System.Windows.Forms.DataVisualization.Charting.Title("Biểu đồ doanh thu theo tháng") { Font = new Font("Segoe UI", 11F, FontStyle.Bold) });
            // 
            // frmReports_New
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1050, 525);
            Controls.Add(_bodyPanel);
            Controls.Add(_topPanel);
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmReports_New";
            Text = "Báo cáo doanh thu";
            _topPanel.ResumeLayout(false);
            _topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_numYear).EndInit();
            _bodyPanel.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
            ((System.ComponentModel.ISupportInitialize)_chart).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel _topPanel;
        private System.Windows.Forms.Label lblYear;
        private Guna.UI2.WinForms.Guna2NumericUpDown _numYear;
        private System.Windows.Forms.Label lblMonth;
        private Guna.UI2.WinForms.Guna2ComboBox _cboMonth;
        private Guna.UI2.WinForms.Guna2Button _btnLoad;
        private Guna.UI2.WinForms.Guna2Button _btnExportExcel;
        private Guna.UI2.WinForms.Guna2Panel _bodyPanel;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Guna.UI2.WinForms.Guna2DataGridView _grid;
        private System.Windows.Forms.DataVisualization.Charting.Chart _chart;
    }
}