using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ClosedXML.Excel;
using Guna.UI2.WinForms;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmReports_New : Form, IThemeableForm
    {
        private readonly IReportService _reportService;
        private LoadingOverlay? _loadingOverlay;

        public frmReports_New(IReportService reportService)
        {
            InitializeComponent();
            _reportService = reportService;

            _loadingOverlay = new LoadingOverlay(this);
            ApplyTheme();
            
            // Thay vì dùng Load, dùng Shown để đảm bảo toàn bộ UI control đã render xong hoàn toàn
            this.Shown += frmReports_New_Shown;
        }

        private async void frmReports_New_Shown(object? sender, EventArgs e)
        {
            _numYear.Value = DateTime.Now.Year;
            _cboMonth.SelectedIndex = 0; // Tất cả các tháng
            
            // Gọi hàm nạp dữ liệu trực tiếp thay vì giả lập click chuột
            await LoadReportDataAsync();
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

            lblYear.ForeColor = UiTheme.TextSecondary;
            lblYear.BackColor = Color.Transparent;
            lblMonth.ForeColor = UiTheme.TextSecondary;
            lblMonth.BackColor = Color.Transparent;

            _grid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(15, 23, 42);
            _grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            _grid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(191, 219, 254);
            _grid.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
        }

        private async void _btnLoad_Click(object sender, EventArgs e)
        {
            await LoadReportDataAsync();
        }

        // Tách biệt logic tải dữ liệu để tái sử dụng an toàn
        private async System.Threading.Tasks.Task LoadReportDataAsync()
        {
            try
            {
                _loadingOverlay?.Show("Đang tải báo cáo...");

                int? month = null;
                if (_cboMonth.SelectedIndex > 0)
                    month = int.Parse(_cboMonth.SelectedItem!.ToString()!);

                var filter = new ReportFilterDto
                {
                    Year = (int)_numYear.Value,
                    Month = month,
                    RouteID = null
                };

                var table = await _reportService.GetRevenueReportAsync(filter);
                _grid.DataSource = table;
                
                // Vẽ biểu đồ
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

        private void _btnExportExcel_Click(object sender, EventArgs e)
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

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                _loadingOverlay?.Show("Đang xuất file Excel...");
                using var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Doanh Thu");

                // Tiêu đề
                ws.Cell(1, 1).Value = "BÁO CÁO DOANH THU BÁN VÉ TÀU";
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 1).Style.Font.FontSize = 16;
                ws.Range(1, 1, 1, _grid.Columns.Count)
                  .Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Thông tin filter
                ws.Cell(2, 1).Value = $"Năm: {_numYear.Value}";
                ws.Cell(3, 1).Value = $"Tháng: {(_cboMonth.SelectedIndex > 0 ? _cboMonth.SelectedItem : "Tất cả")}";
                ws.Cell(4, 1).Value = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm}";

                // Header cột
                int colIndex = 1;
                foreach (DataGridViewColumn col in _grid.Columns)
                {
                    var cell = ws.Cell(6, colIndex);
                    cell.Value = col.HeaderText;
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    colIndex++;
                }

                // Dữ liệu
                int rowIndex = 7;
                foreach (DataGridViewRow row in _grid.Rows)
                {
                    colIndex = 1;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        var xlCell = ws.Cell(rowIndex, colIndex);
                        xlCell.Value = cell.Value?.ToString();
                        xlCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
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

        private void BindChart(DataTable table)
        {
            if (_chart == null) return;

            if (_chart.Series.IndexOf("DoanhThu") == -1)
            {
                var newSeries = _chart.Series.Add("DoanhThu");
                newSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                newSeries.Color = Color.FromArgb(99, 102, 241);
                newSeries.IsValueShownAsLabel = true;
                newSeries.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            }

            Series series = _chart.Series["DoanhThu"];
            series.Points.Clear();

            if (table == null || table.Rows.Count == 0) return;

            // Tìm tên cột doanh thu thực tế: ưu tiên "DoanhThu", fallback "TongDoanhThu"
            string? revenueCol = null;
            foreach (string candidate in new[] { "DoanhThu", "TongDoanhThu", "Revenue", "TotalRevenue" })
            {
                if (table.Columns.Contains(candidate)) { revenueCol = candidate; break; }
            }
            if (revenueCol == null || !table.Columns.Contains("Thang")) return;

            // Gộp (group) nhiều hàng cùng tháng → cộng doanh thu lại
            var grouped = table.AsEnumerable()
                .GroupBy(r => r.IsNull("Thang") ? 0 : Convert.ToInt32(r["Thang"]))
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Thang = g.Key,
                    TongTien = g.Sum(r => r.IsNull(revenueCol) ? 0d : Convert.ToDouble(r[revenueCol]))
                });

            foreach (var item in grouped)
            {
                var pt = series.Points.Add(item.TongTien);
                pt.AxisLabel = $"T{item.Thang}";
                pt.Label = item.TongTien >= 1_000_000
                    ? $"{item.TongTien / 1_000_000:F1}M"
                    : $"{item.TongTien:N0}";
            }
        }
    }
}