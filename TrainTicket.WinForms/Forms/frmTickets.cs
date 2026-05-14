// ============================================================
// FILE: frmTickets.cs — NÂNG CẤP
// Cải tiến:
//   - Filter theo status, mã vé, ngày
//   - Hiển thị chính sách hoàn tiền khi hủy
//   - Thêm nút Check-in (MỚI)
//   - Export Excel
// ============================================================
using System.Data;
using Guna.UI2.WinForms;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public class frmTickets : Form, IThemeableForm
    {
        private readonly ITicketService     _ticketService;
        private readonly TrainTicketDbContext _dbContext;

        private readonly Guna2DataGridView _grid       = new();
        private readonly Guna2Button   _btnCancel      = new();
        private readonly Guna2Button   _btnCheckIn     = new();  // [MỚI]
        private readonly Guna2Button   _btnRefresh     = new();
        private readonly Guna2Button   _btnExport      = new();  // [MỚI]
        private readonly Guna2ComboBox _cboStatus      = new();
        private readonly Guna2TextBox  _txtSearch      = new();
        private readonly Guna2Panel    _topPanel       = new();
        private readonly Label         _lblTotal       = new();
        private LoadingOverlay? _loading;

        public frmTickets(ITicketService ticketService, TrainTicketDbContext db)
        {
            _ticketService = ticketService;
            _dbContext     = db;
            InitializeUi();
            Load += async (_, _) => await LoadTicketsAsync();
        }

        private void InitializeUi()
        {
            Text      = "Quản lý vé";
            Width     = 1200; Height = 700;
            BackColor = UiTheme.Background;

            _topPanel.Dock      = DockStyle.Top;
            _topPanel.Height    = 70;
            _topPanel.FillColor = UiTheme.Surface;
            _topPanel.ShadowDecoration.Enabled = true;
            _topPanel.ShadowDecoration.Depth   = 4;
            _topPanel.Padding   = new Padding(12, 14, 12, 0);

            // Search box
            _txtSearch.Left = 14; _txtSearch.Top = 16;
            _txtSearch.Width = 200; _txtSearch.BorderRadius = 8;
            _txtSearch.PlaceholderText = "🔍 Tìm mã vé...";
            _txtSearch.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) _ = LoadTicketsAsync(); };

            // Status filter
            _cboStatus.Left = 228; _cboStatus.Top = 16;
            _cboStatus.Width = 150; _cboStatus.BorderRadius = 8;
            _cboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboStatus.Items.AddRange(new object[] { "Tất cả", "Pending", "Confirmed", "Cancelled", "Used" });
            _cboStatus.SelectedIndex = 0;

            // Buttons
            var btnDefs = new (Guna2Button btn, string text, int left, Color color)[]
            {
                (_btnRefresh, "🔄 Làm mới",     394, Color.FromArgb(71,85,105)),
                (_btnCheckIn, "✅ Check-in",     492, Color.FromArgb(16,185,129)),
                (_btnCancel,  "❌ Hủy vé",       590, Color.FromArgb(239,68,68)),
                (_btnExport,  "📥 Xuất Excel",   688, Color.FromArgb(37,99,235)),
            };
            foreach (var (btn, text, left, color) in btnDefs)
            {
                btn.Text = text; btn.Left = left; btn.Top = 14;
                btn.Width = 90; btn.Height = 36; btn.BorderRadius = 9;
                btn.FillColor = color; btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 9);
                _topPanel.Controls.Add(btn);
            }

            _lblTotal.Left = 14; _lblTotal.Top = 50;
            _lblTotal.Width = 400; _lblTotal.Font = new Font("Segoe UI", 9);
            _lblTotal.ForeColor = UiTheme.TextSecondary;

            _topPanel.Controls.AddRange(new Control[] { _txtSearch, _cboStatus, _lblTotal });

            // Grid
            _grid.Dock               = DockStyle.Fill;
            _grid.ReadOnly           = true;
            _grid.AllowUserToAddRows = false;
            _grid.SelectionMode      = DataGridViewSelectionMode.FullRowSelect;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.ThemeStyle.HeaderStyle.BackColor = UiTheme.PrimaryDark;
            _grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;

            // Events
            _btnRefresh.Click += async (_, _) => await LoadTicketsAsync();
            _btnCancel.Click  += BtnCancel_Click;
            _btnCheckIn.Click += BtnCheckIn_Click;
            _btnExport.Click  += BtnExport_Click;
            _cboStatus.SelectedIndexChanged += async (_, _) => await LoadTicketsAsync();

            Controls.Add(_grid);
            Controls.Add(_topPanel);
            _loading = new LoadingOverlay(this);
            ApplyTheme();
        }

        private async Task LoadTicketsAsync()
        {
            _loading?.Show("Đang tải danh sách vé...");
            try
            {
                var status = _cboStatus.SelectedIndex > 0 ? _cboStatus.SelectedItem?.ToString() : null;
                var userId = SessionManager.CurrentUser?.IsStaff == true ? null : (int?)SessionManager.CurrentUser?.UserID;
                var table  = await _ticketService.GetTicketsAsync(userId, status, ticketCode: _txtSearch.Text.Trim());

                // Lọc thêm theo tenantRegion nếu không phải HQ
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
                _lblTotal.Text = $"Tổng: {table?.Rows.Count ?? 0} vé | " +
                    $"Tổng tiền: {(table?.Rows.Count > 0 ? table.AsEnumerable().Where(r => r["Status"].ToString() == "Confirmed").Sum(r => r.IsNull("FinalPrice") ? 0 : Convert.ToDecimal(r["FinalPrice"])) : 0):N0} VNĐ";
            }
            finally { _loading?.Hide(); }
        }

        private void FormatColumns(DataTable table)
        {
            foreach (DataGridViewColumn col in _grid.Columns)
            {
                col.HeaderText = col.Name switch
                {
                    "TicketID"      => "ID",
                    "TicketCode"    => "Mã vé",
                    "Status"        => "Trạng thái",
                    "PassengerName" => "Hành khách",
                    "FinalPrice"    => "Giá (VNĐ)",
                    "GioDi"         => "Giờ đi",
                    "MaTau"         => "Tàu",
                    "GaDi"          => "Từ",
                    "GaDen"         => "Đến",
                    "MaToa"         => "Toa",
                    "SoGhe"         => "Ghế",
                    "PaymentMethod" => "Thanh toán",
                    "BookedAt"      => "Ngày đặt",
                    _               => col.HeaderText
                };
                if (col.Name is "FinalPrice") col.DefaultCellStyle.Format = "N0";
                if (col.Name is "GioDi" or "BookedAt") col.DefaultCellStyle.Format = "HH:mm dd/MM/yy";
            }

            // Tô màu theo trạng thái
            _grid.CellFormatting -= OnCellFormatting;
            _grid.CellFormatting += OnCellFormatting;
        }

        private void OnCellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (_grid.Columns[e.ColumnIndex].Name != "Status") return;
            var status = e.Value?.ToString();
            if (status == null) return;
            e.CellStyle.ForeColor = status switch
            {
                "Confirmed" => Color.FromArgb(16, 185, 129),
                "Pending"   => Color.FromArgb(245, 158, 11),
                "Cancelled" => Color.FromArgb(239, 68, 68),
                "Used"      => Color.FromArgb(99, 102, 241),
                _           => UiTheme.TextPrimary
            };
            e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        private async void BtnCancel_Click(object? sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count == 0) return;
            var row      = _grid.SelectedRows[0];
            var ticketId = Convert.ToInt32(row.Cells["TicketID"].Value);
            var code     = row.Cells["TicketCode"].Value?.ToString();
            var status   = row.Cells["Status"].Value?.ToString();

            if (status == "Cancelled") { MessageBox.Show("Vé đã bị hủy rồi."); return; }
            if (status == "Used")      { MessageBox.Show("Không thể hủy vé đã sử dụng."); return; }

            var confirm = MessageBox.Show(
                $"Hủy vé {code}?\n⚠ Chính sách hoàn tiền phụ thuộc vào thời gian tàu chạy.",
                "Xác nhận hủy vé", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            _loading?.Show("Đang hủy vé...");
            var result = await _ticketService.CancelTicketAsync(
                ticketId, SessionManager.CurrentUser?.UserID ?? 0,
                "Hủy bởi người dùng");
            _loading?.Hide();

            MessageBox.Show(result.Success
                ? $"✅ Đã hủy vé!\n{result.Message}"
                : $"❌ {result.Message}",
                result.Success ? "Thành công" : "Lỗi",
                MessageBoxButtons.OK,
                result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (result.Success) await LoadTicketsAsync();
        }

        private async void BtnCheckIn_Click(object? sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count == 0) return;
            var code = _grid.SelectedRows[0].Cells["TicketCode"].Value?.ToString() ?? "";
            _loading?.Show("Đang check-in...");
            var ok = await _ticketService.CheckInAsync(code);
            _loading?.Hide();

            MessageBox.Show(ok ? $"✅ Check-in thành công!\nVé: {code}" : "❌ Không thể check-in vé này.",
                ok ? "Check-in thành công" : "Lỗi", MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (ok) await LoadTicketsAsync();
        }

        private void BtnExport_Click(object? sender, EventArgs e)
        {
            if (_grid.DataSource is not System.Data.DataTable dt) return;
            using var dlg = new SaveFileDialog { Filter = "CSV|*.csv", FileName = $"Tickets_{DateTime.Now:yyyyMMdd_HHmm}.csv" };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            var lines = new List<string>
            {
                string.Join(",", dt.Columns.Cast<DataColumn>().Select(c => $"\"{c.ColumnName}\""))
            };
            foreach (DataRow row in dt.Rows)
                lines.Add(string.Join(",", row.ItemArray.Select(v => $"\"{v}\"")));

            File.WriteAllLines(dlg.FileName, lines, System.Text.Encoding.UTF8);
            MessageBox.Show($"Đã xuất {dt.Rows.Count} dòng.", "Xuất thành công");
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _topPanel.FillColor = UiTheme.Surface;
            _lblTotal.ForeColor = UiTheme.TextSecondary;
        }
    }
}