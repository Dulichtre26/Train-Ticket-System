using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using Guna.UI2.WinForms;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;
using System.Data.SqlClient; 
using ClosedXML.Excel;
using System.Data;

namespace TrainTicket.WinForms.Forms
{
    // Form báo cáo doanh thu hiện đại với Guna + Chart.
    public class frmReports : Form, IThemeableForm
    {
        private readonly IReportService _reportService;

        private readonly Guna2NumericUpDown _numYear = new();
        private readonly Guna2ComboBox _cboMonth = new();
        private readonly Guna2Button _btnLoad = new();
        private readonly Guna2Button _btnExportExcel = new();
        private readonly Guna2DataGridView _grid = new();
        private readonly Chart _chart = new();
        private readonly Guna2Panel _topPanel = new();
        private readonly Guna2Panel _bodyPanel = new();
        private LoadingOverlay? _loadingOverlay;

        public frmReports(IReportService reportService)
        {
            _reportService = reportService;

            InitializeUi();
        }

        private void InitializeUi()
        {
            Text = "Báo cáo doanh thu";
            BackColor = UiTheme.Background;

            _topPanel.Dock = DockStyle.Top;
            _topPanel.Height = 82;
            _topPanel.ShadowDecoration.Enabled = true;

            _bodyPanel.Dock = DockStyle.Fill;
            _bodyPanel.Padding = new Padding(14);

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 700,
                IsSplitterFixed = false
            };

            _topPanel.Controls.Add(new Label { Text = "Năm", Left = 20, Top = 18, Width = 40 });
            _numYear.Left = 60;
            _numYear.Top = 14;
            _numYear.Width = 90;
            _numYear.Minimum = 2000;
            _numYear.Maximum = 2100;
            _numYear.BorderRadius = 8;
            _numYear.Value = DateTime.Now.Year;
            _topPanel.Controls.Add(_numYear);

            _topPanel.Controls.Add(new Label { Text = "Tháng", Left = 170, Top = 18, Width = 45 });
            _cboMonth.Left = 220;
            _cboMonth.Top = 14;
            _cboMonth.Width = 100;
            _cboMonth.BorderRadius = 8;
            _cboMonth.DrawMode = DrawMode.OwnerDrawFixed;
            _cboMonth.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboMonth.Items.Add("Tất cả");  
            for (var i = 1; i <= 12; i++) _cboMonth.Items.Add(i.ToString());
            _cboMonth.SelectedIndex = 0;
            _topPanel.Controls.Add(_cboMonth);

            _btnLoad.Text = "Xem b\u00e1o c\u00e1o";
            _btnLoad.Left = 340;
            _btnLoad.Top = 13;
            _btnLoad.Width = 122;
            _btnLoad.Height = 38;
            _btnLoad.BorderRadius = 10;
            _btnLoad.FillColor = Color.FromArgb(37, 99, 235);
            _btnLoad.Click += BtnLoad_Click;
            _topPanel.Controls.Add(_btnLoad);

            _btnExportExcel.Text = "Xuất Excel";
            _btnExportExcel.Left = 475;
            _btnExportExcel.Top = 13;
            _btnExportExcel.Width = 122;
            _btnExportExcel.Height = 38;
            _btnExportExcel.BorderRadius = 10;
            _btnExportExcel.FillColor = Color.FromArgb(16, 185, 129); // Emerald 500
            _btnExportExcel.Click += BtnExportExcel_Click;
            _topPanel.Controls.Add(_btnExportExcel);

            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(15, 23, 42);
            _grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            _grid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(191, 219, 254);
            _grid.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;

            _chart.Dock = DockStyle.Fill;
            _chart.Palette = ChartColorPalette.Berry;
            _chart.ChartAreas.Add(new ChartArea("Default"));
            _chart.Series.Add(new Series("DoanhThu")
            {
                ChartType = SeriesChartType.Column,
                XValueType = ChartValueType.String,
                YValueType = ChartValueType.Double
            });
            split.Panel1.Controls.Add(_grid);
            split.Panel2.Controls.Add(_chart);

            _bodyPanel.Controls.Add(split);

            Controls.Add(_bodyPanel);
            Controls.Add(_topPanel);

            _loadingOverlay = new LoadingOverlay(this);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _topPanel.FillColor = UiTheme.Surface;
            _bodyPanel.FillColor = UiTheme.Background;
            _btnLoad.FillColor = UiTheme.Primary;
            _btnLoad.HoverState.FillColor = UiTheme.PrimaryHover;
            _btnExportExcel.FillColor = Color.FromArgb(16, 185, 129);
            _btnExportExcel.HoverState.FillColor = Color.FromArgb(5, 150, 105);
        }

        // Gọi service báo cáo và bind vào lưới + biểu đồ.
        private async void BtnLoad_Click(object? sender, EventArgs e)
        {
            try
            {
                _loadingOverlay?.Show("Đang tải báo cáo...");
                int? month = null;
                if (_cboMonth.SelectedIndex > 0)
                {
                    month = int.Parse(_cboMonth.SelectedItem!.ToString()!);
                }

                var filter = new ReportFilterDto
                {
                    Year = (int)_numYear.Value,
                    Month = month,
                    RouteID = null
                };

                var table = await _reportService.GetRevenueReportAsync(filter);
                _grid.DataSource = table;
                BindChart(table);
                UiNotifier.InfoToast($"Đã tải {table.Rows.Count} dòng báo cáo.");
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Không thể tải báo cáo: {ex.Message}");
            }
            finally
            {
                _loadingOverlay?.Hide();
            }
        }

        private void BtnExportExcel_Click(object? sender, EventArgs e)
        {
            if (_grid.Rows.Count == 0)
            {
                UiNotifier.ErrorToast("Không có dữ liệu để xuất Excel.");
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Excel Workbook|*.xlsx",
                Title = "Lưu báo cáo doanh thu",
                FileName = $"BaoCaoDoanhThu_{_numYear.Value}_{(_cboMonth.SelectedIndex > 0 ? _cboMonth.SelectedItem : "TatCa")}.xlsx"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _loadingOverlay?.Show("Đang xuất file Excel...");
                    using var wb = new XLWorkbook();
                    var ws = wb.Worksheets.Add("Doanh Thu");

                    // Title
                    ws.Cell(1, 1).Value = "BÁO CÁO DOANH THU BÁN VÉ TÀU";
                    ws.Cell(1, 1).Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Font.FontSize = 16;
                    ws.Range(1, 1, 1, _grid.Columns.Count).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Info
                    ws.Cell(2, 1).Value = $"Năm: {_numYear.Value}";
                    ws.Cell(3, 1).Value = $"Tháng: {(_cboMonth.SelectedIndex > 0 ? _cboMonth.SelectedItem : "Tất cả")}";
                    ws.Cell(4, 1).Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";

                    // Headers
                    int colIndex = 1;
                    foreach (DataGridViewColumn col in _grid.Columns)
                    {
                        ws.Cell(6, colIndex).Value = col.HeaderText;
                        ws.Cell(6, colIndex).Style.Font.Bold = true;
                        ws.Cell(6, colIndex).Style.Fill.BackgroundColor = XLColor.LightGray;
                        ws.Cell(6, colIndex).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        colIndex++;
                    }

                    // Data
                    int rowIndex = 7;
                    foreach (DataGridViewRow row in _grid.Rows)
                    {
                        colIndex = 1;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            ws.Cell(rowIndex, colIndex).Value = cell.Value?.ToString();
                            ws.Cell(rowIndex, colIndex).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            colIndex++;
                        }
                        rowIndex++;
                    }

                    ws.Columns().AdjustToContents();
                    wb.SaveAs(sfd.FileName);
                    UiNotifier.SuccessToast("Xuất Excel thành công!");
                }
                catch (Exception ex)
                {
                    UiNotifier.ErrorToast($"Lỗi xuất Excel: {ex.Message}");
                }
                finally
                {
                    _loadingOverlay?.Hide();
                }
            }
        }

        // Vẽ chart doanh thu theo tháng từ DataTable kết quả.
        private void BindChart(DataTable table)
        {
            var series = _chart.Series["DoanhThu"];
            series.Points.Clear();

            if (table.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in table.Rows)
            {
                var month = row.Table.Columns.Contains("Thang") ? Convert.ToString(row["Thang"]) : "?";
                var revenue = row.Table.Columns.Contains("TongDoanhThu")
                    ? Convert.ToDouble(row["TongDoanhThu"] == DBNull.Value ? 0 : row["TongDoanhThu"])
                    : 0d;

                series.Points.AddXY($"T{month}", revenue);
            }
        }
    }
}
