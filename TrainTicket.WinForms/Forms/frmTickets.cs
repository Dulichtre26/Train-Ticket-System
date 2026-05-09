using System.Data;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;
using Guna.UI2.WinForms;

namespace TrainTicket.WinForms.Forms
{
    // Form quản lý vé của nhân viên: hiển thị danh sách vé, cho phép hủy vé.
    // Chỉ nhân viên đặt vé hoặc Admin/Staff mới có quyền hủy.
    public class frmTickets : Form, IThemeableForm
    {
        private readonly ITicketService _ticketService;
        private readonly TrainTicketDbContext _dbContext;
        private readonly Guna2DataGridView _grid = new();
        private readonly Guna2Button _btnCancel = new();
        private readonly Guna2Panel _topPanel = new();
        private LoadingOverlay? _loadingOverlay;

        public frmTickets(ITicketService ticketService, TrainTicketDbContext dbContext)
        {
            _ticketService = ticketService;
            _dbContext = dbContext;

            InitializeUi();
            Load += FrmTickets_Load;
        }

        private void InitializeUi()
        {
            Text = "Qu\u1ea3n l\u00fd v\u00e9";
            Width = 1200;
            Height = 700;
            BackColor = UiTheme.Background;

            _topPanel.Dock = DockStyle.Top;
            _topPanel.Height = 60;
            _topPanel.FillColor = UiTheme.Surface;
            _topPanel.ShadowDecoration.Enabled = true;

            _btnCancel.Text = "H\u1ee7y v\u00e9 \u0111\u00e3 ch\u1ecdn";
            _btnCancel.Left = 20;
            _btnCancel.Top = 15;
            _btnCancel.Width = 150;
            _btnCancel.Height = 35;
            _btnCancel.BorderRadius = 8;
            _btnCancel.FillColor = Color.FromArgb(239, 68, 68);
            _btnCancel.HoverState.FillColor = Color.FromArgb(220, 38, 38);
            _btnCancel.Click += BtnCancel_Click;
            _topPanel.Controls.Add(_btnCancel);

            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(15, 23, 42);
            _grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            _grid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(191, 219, 254);
            _grid.ThemeStyle.RowsStyle.SelectionForeColor = Color.Black;
            _grid.CellDoubleClick += Grid_CellDoubleClick;

            Controls.Add(_grid);
            Controls.Add(_topPanel);

            _loadingOverlay = new LoadingOverlay(this);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _topPanel.FillColor = UiTheme.Surface;
            _btnCancel.FillColor = Color.FromArgb(239, 68, 68);
            _btnCancel.HoverState.FillColor = Color.FromArgb(220, 38, 38);
        }

        private async void FrmTickets_Load(object? sender, EventArgs e)
        {
            await LoadTicketsAsync();
        }

        private async Task LoadTicketsAsync()
        {
            try
            {
                _loadingOverlay?.Show("Đang tải danh sách vé...");

                // Nếu là Admin hoặc Staff, xem tất cả vé (truyền null)
                int? targetUserId = SessionManager.CurrentRole == "Admin" || SessionManager.CurrentRole == "Staff"
                    ? null
                    : SessionManager.CurrentUser?.UserID;

                var tickets = await _ticketService.GetTicketsAsync(targetUserId);

                // Lọc thêm theo tenantRegion nếu là Single Database (không phải SQL Server Replication)
                var currentRegion = SessionManager.CurrentRegion;
                if (!string.IsNullOrEmpty(currentRegion) && currentRegion != "HQ" && tickets != null)
                {
                    tickets = tickets.Where(x => string.IsNullOrEmpty(x.RegionCode) || x.RegionCode == currentRegion || x.RegionCode == "HQ").ToList();
                }

                _grid.DataSource = tickets;
                UiNotifier.InfoToast($"Đã tải {tickets?.Count ?? 0} vé.");
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Không thể tải danh sách vé: {ex.Message}");
            }
            finally
            {
                _loadingOverlay?.Hide();
            }
        }

        private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _grid.Rows[e.RowIndex].DataBoundItem == null)
            {
                return;
            }

            var ticketIdObj = _grid.Rows[e.RowIndex].Cells["TicketID"].Value;
            if (ticketIdObj == null || ticketIdObj == DBNull.Value)
            {
                return;
            }

            var ticketId = Convert.ToInt32(ticketIdObj);
            var status = _grid.Rows[e.RowIndex].Cells["Status"].Value?.ToString();

            // Mở form thanh toán nếu vé đang Pending
            if (status == "Pending")
            {
                using var paymentForm = new frmPayments(ticketId, _ticketService);
                paymentForm.ShowDialog(this);
            }
        }

        private async void BtnCancel_Click(object? sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count == 0)
            {
                UiNotifier.Info("Vui lòng chọn vé để hủy.");
                return;
            }

            var ticketIdObj = _grid.SelectedRows[0].Cells["TicketID"].Value;
            if (ticketIdObj == null || ticketIdObj == DBNull.Value)
            {
                return;
            }

            var ticketId = Convert.ToInt32(ticketIdObj);
            var result = MessageBox.Show("Bạn có chắc muốn hủy vé này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _loadingOverlay?.Show("Đang hủy vé...");
                var success = await _ticketService.CancelTicketAsync(ticketId, SessionManager.CurrentUser?.UserID ?? 0);
                if (success)
                {
                    UiNotifier.SuccessToast("Hủy vé thành công.");
                    await LoadTicketsAsync(); // Refresh list
                }
                else
                {
                    UiNotifier.ErrorToast("Hủy vé thất bại. Vé đã sử dụng hoặc quá thời hạn.");
                }
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi hủy vé: {ex.Message}");
            }
            finally
            {
                _loadingOverlay?.Hide();
            }
        }
    }
}