using Guna.UI2.WinForms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmTickets_New : Form, IThemeableForm
    {
        private readonly ITicketService _ticketService;
        private readonly TrainTicketDbContext _dbContext;
        private LoadingOverlay? _loading;

        public frmTickets_New(ITicketService ticketService, TrainTicketDbContext db)
        {
            InitializeComponent();
            _ticketService = ticketService;
            _dbContext = db;
            _loading = new LoadingOverlay(this);

            // Đăng ký sự kiện click cho nút Thanh toán hàng được chọn
            _btnPaySelected.Click += _btnPaySelected_Click;

            ApplyTheme();
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _topPanel.FillColor = UiTheme.Surface;
            _lblTotal.ForeColor = UiTheme.TextSecondary;
            _lblTotal.BackColor = Color.Transparent;

            _grid.ThemeStyle.HeaderStyle.BackColor = UiTheme.PrimaryDark;
            _grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
        }

        private async void frmTickets_New_Load(object sender, EventArgs e)
        {
            await LoadTicketsAsync();
        }

        private async Task LoadTicketsAsync()
        {
            _loading?.Show("Đang tải danh sách vé...");
            try
            {
                var status = _cboStatus.SelectedIndex > 0 ? _cboStatus.SelectedItem?.ToString() : null;
                var userId = SessionManager.CurrentUser?.IsStaff == true ? null : (int?)SessionManager.CurrentUser?.UserId;
                var table = await _ticketService.GetTicketsAsync(userId, status, ticketCode: _txtSearch.Text.Trim());

                var currentRegion = SessionManager.CurrentRegion;
                if (!string.IsNullOrEmpty(currentRegion) && currentRegion != "HQ" && table != null && table.Rows.Count > 0)
                {
                    if (table.Columns.Contains("RegionCode"))
                    {
                        var filteredRows = table.AsEnumerable()
                            .Where(x => x.IsNull("RegionCode") || x.Field<string>("RegionCode") == currentRegion || x.Field<string>("RegionCode") == "HQ");
                        if (filteredRows.Any())
                        {
                            table = filteredRows.CopyToDataTable();
                        }
                        else
                        {
                            table.Rows.Clear();
                        }
                    }
                }

                _grid.DataSource = table;
                FormatColumns(table!);

                // Tính tổng tiền dựa trên trạng thái hợp lệ của dự án (Confirmed / Paid)
                _lblTotal.Text = $"Tổng: {table?.Rows.Count ?? 0} vé | " +
                    $"Tổng tiền: {(table?.Rows.Count > 0 ? table.AsEnumerable().Where(r => r["Status"].ToString() == "Confirmed" || r["Status"].ToString() == "Paid").Sum(r => r.IsNull("FinalPrice") ? 0 : Convert.ToDecimal(r["FinalPrice"])) : 0):N0} VNĐ";
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi tải danh sách: {ex.Message}");
            }
            finally { _loading?.Hide(); }
        }

        private void FormatColumns(DataTable table)
        {
            foreach (DataGridViewColumn col in _grid.Columns)
            {
                col.HeaderText = col.Name switch
                {
                    "TicketID" => "ID",
                    "TicketCode" => "Mã vé",
                    "Status" => "Trạng thái",
                    "PassengerName" => "Hành khách",
                    "FinalPrice" => "Giá (VNĐ)",
                    "GioDi" => "Giờ đi",
                    "MaTau" => "Tàu",
                    "GaDi" => "Từ",
                    "GaDen" => "Đến",
                    "MaToa" => "Toa",
                    "SoGhe" => "Ghế",
                    "PaymentMethod" => "Thanh toán",
                    "BookedAt" => "Ngày đặt",
                    _ => col.HeaderText
                };
                if (col.Name is "FinalPrice") col.DefaultCellStyle.Format = "N0";
                if (col.Name is "GioDi" or "BookedAt") col.DefaultCellStyle.Format = "HH:mm dd/MM/yy";
            }

            _grid.CellFormatting -= OnCellFormatting;
            _grid.CellFormatting += OnCellFormatting;
        }

        private void OnCellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (_grid.Columns[e.ColumnIndex].Name != "Status") return;

            var status = e.Value?.ToString();
            if (status == null) return;

            e.CellStyle.ForeColor = status switch
            {
                "Confirmed" or "Paid" => Color.FromArgb(16, 185, 129), // Xanh lá
                "Pending" => Color.FromArgb(245, 158, 11),  // Vàng cam
                "Cancelled" => Color.FromArgb(239, 68, 68),   // Đỏ
                "Used" => Color.FromArgb(99, 102, 241),  // Tím xanh
                _ => UiTheme.TextPrimary
            };
            e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        private async void _txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                await LoadTicketsAsync();
            }
        }

        private async void _cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadTicketsAsync();
        }

        private async void _btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadTicketsAsync();
        }

        private void _btnDetail_Click(object sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count == 0) return;
            var ticketIdObj = _grid.SelectedRows[0].Cells["TicketID"].Value;
            if (ticketIdObj == null || ticketIdObj == DBNull.Value) return;

            var ticketId = Convert.ToInt32(ticketIdObj);
            new frmTicketDetail_New(ticketId, _dbContext).ShowDialog(this);
        }

        private void _btnPrint_Click(object sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count == 0) return;
            var ticketIdObj = _grid.SelectedRows[0].Cells["TicketID"].Value;
            if (ticketIdObj == null || ticketIdObj == DBNull.Value) return;

            var ticketId = Convert.ToInt32(ticketIdObj);
            new frmTicketPrint_New(ticketId, _dbContext).ShowDialog(this);
        }

        private void _btnEdit_Click(object sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count == 0) return;
            var row = _grid.SelectedRows[0];
            var ticketIdObj = row.Cells["TicketID"].Value;
            if (ticketIdObj == null || ticketIdObj == DBNull.Value) return;

            var ticketId = Convert.ToInt32(ticketIdObj);
            var status = row.Cells["Status"].Value?.ToString();

            if (status is "Cancelled" or "Used")
            {
                MessageBox.Show("Không thể sửa vé đã hủy hoặc đã sử dụng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var editForm = new frmEditTicket_New(ticketId, _dbContext);
            editForm.ShowDialog(this);
            if (editForm.Saved) _ = LoadTicketsAsync();
        }

        private async void _btnCheckIn_Click(object sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count == 0) return;
            var code = _grid.SelectedRows[0].Cells["TicketCode"].Value?.ToString() ?? "";

            if (string.IsNullOrEmpty(code)) return;

            _loading?.Show("Đang check-in...");
            var ok = await _ticketService.CheckInAsync(code);
            _loading?.Hide();

            MessageBox.Show(ok ? $"✔️ Check-in thành công!\nVé: {code}" : "❌ Không thể check-in vé này.",
                ok ? "Check-in thành công" : "Lỗi", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (ok) await LoadTicketsAsync();
        }

        private async void _btnCancel_Click(object sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count == 0) return;
            var row = _grid.SelectedRows[0];
            var ticketIdObj = row.Cells["TicketID"].Value;
            if (ticketIdObj == null || ticketIdObj == DBNull.Value) return;

            var ticketId = Convert.ToInt32(ticketIdObj);
            var code = row.Cells["TicketCode"].Value?.ToString();
            var status = row.Cells["Status"].Value?.ToString();

            if (status == "Cancelled") { MessageBox.Show("Vé đã bị hủy rồi."); return; }
            if (status == "Used") { MessageBox.Show("Không thể hủy vé đã sử dụng."); return; }

            var confirm = MessageBox.Show(
                $"Hủy vé {code}?\n⚠️ Chính sách hoàn tiền phụ thuộc vào thời gian tàu chạy.",
                "Xác nhận hủy vé", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            _loading?.Show("Đang hủy vé...");
            try
            {
                var result = await _ticketService.CancelTicketAsync(
                    ticketId, SessionManager.CurrentUser?.UserId ?? 0,
                    "Hủy bới người dùng");
                _loading?.Hide();

                MessageBox.Show(result.Success
                    ? $"✅ Đã hủy vé!\n{result.Message}"
                    : $"❌ {result.Message}",
                    result.Success ? "Thành công" : "Lỗi",
                    MessageBoxButtons.OK,
                    result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                if (result.Success) await LoadTicketsAsync();
            }
            catch (Exception ex)
            {
                _loading?.Hide();
                MessageBox.Show($"❌ Lỗi khi hủy vé: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _btnExport_Click(object sender, EventArgs e)
        {
            if (_grid.DataSource is not DataTable dt) return;
            using var dlg = new SaveFileDialog { Filter = "CSV|*.csv", FileName = $"Tickets_{DateTime.Now:yyyyMMdd_HHmm}.csv" };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            var lines = new List<string>
            {
                string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => $"\"{c.ColumnName}\""))
            };
            foreach (DataRow row in dt.Rows)
                lines.Add(string.Join(",", row.ItemArray.Select(v => $"\"{v}\"")));

            File.WriteAllLines(dlg.FileName, lines, System.Text.Encoding.UTF8);
            MessageBox.Show($"Đã xuất {dt.Rows.Count} dòng.", "Xuất thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Logic xử lý thanh toán trực tiếp hàng vé đang chọn trên bảng (Không dùng checkbox)
        /// </summary>
        private async void _btnPaySelected_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra hàng được chọn
            if (_grid.CurrentRow == null || _grid.CurrentRow.Index < 0)
            {
                UiNotifier.ErrorToast("Vui lòng click chọn một hàng vé trên bảng để thanh toán!");
                return;
            }

            var currentRow = _grid.CurrentRow;

            // 2. Trích xuất thông tin định danh hàng từ các ô
            if (currentRow.Cells["TicketID"].Value == null || currentRow.Cells["TicketID"].Value == DBNull.Value) return;

            int ticketId = Convert.ToInt32(currentRow.Cells["TicketID"].Value);
            string code = currentRow.Cells["TicketCode"].Value?.ToString() ?? $"#{ticketId}";
            string status = currentRow.Cells["Status"].Value?.ToString() ?? "";

            // 3. Kiểm tra điều kiện trạng thái của vé
            if (status != "Pending")
            {
                if (status == "Confirmed" || status == "Paid" || status == "Used")
                {
                    UiNotifier.Info($"Vé {code} này đã được thanh toán hoàn tất.");
                }
                else if (status == "Cancelled")
                {
                    UiNotifier.ErrorToast($"Vé {code} đã bị hủy từ trước, không thể thanh toán.");
                }
                return;
            }

            // 4. Hộp thoại xác nhận nhanh tại quầy
            var confirm = MessageBox.Show(
                $"Xác nhận thanh toán TIỀN MẶT tại quầy cho vé {code}?",
                "Xác nhận giao dịch trực tiếp",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            // 5. Thực thi gọi Service cập nhật trạng thái
            _btnPaySelected.Enabled = false;
            _loading?.Show("Đang ghi nhận thanh toán...");
            try
            {
                using var scope = Program.ServiceProvider.CreateScope();
                var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();

                // Lưu vết tên nhân viên thực hiện thu tiền tại quầy
                string staffName = SessionManager.CurrentUser?.FullName?.Split(' ').Last().ToUpper() ?? "STAFF";
                string transactionNote = $"COUNTER_{staffName}";

                var success = await ticketService.ConfirmPaymentAsync(ticketId, transactionNote);

                if (success)
                {
                    UiNotifier.SuccessToast($"🎉 Vé {code} đã chuyển sang trạng thái ĐÃ THANH TOÁN!");

                    // 6. Tải lại danh sách để cập nhật giao diện
                    await LoadTicketsAsync();
                }
                else
                {
                    UiNotifier.ErrorToast("Thanh toán thất bại. Vui lòng kiểm tra lại cấu hình DB.");
                }
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi hệ thống: {ex.Message}");
            }
            finally
            {
                _loading?.Hide();
                _btnPaySelected.Enabled = true;
            }
        }
    }
}