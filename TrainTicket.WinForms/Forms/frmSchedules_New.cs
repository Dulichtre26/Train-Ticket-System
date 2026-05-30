using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;
using Guna.UI2.WinForms;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmSchedules_New : Form, IThemeableForm
    {
        private readonly ICatalogService _catalogService;
        private ScheduleDto? _currentSchedule;
        private List<TrainDto> _trains = new();
        private List<RouteDto> _routes = new();
        private LoadingOverlay? _loadingOverlay;

        public frmSchedules_New(ICatalogService catalogService)
        {
            InitializeComponent();
            _catalogService = catalogService;
            _loadingOverlay = new LoadingOverlay(this);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _topPanel.FillColor = UiTheme.Surface;
            _rightPanel.FillColor = UiTheme.Surface;
            _lblTitle.ForeColor = UiTheme.TextPrimary;
            _lblTitle.BackColor = Color.Transparent;
            _lblFormTitle.ForeColor = UiTheme.TextPrimary;
            _lblFormTitle.BackColor = Color.Transparent;

            foreach (Control c in _rightPanel.Controls)
            {
                if (c is Label lbl && lbl != _lblFormTitle)
                {
                    lbl.ForeColor = UiTheme.TextSecondary;
                    lbl.BackColor = Color.Transparent;
                }

                if (c is Guna2TextBox txt)
                {
                    txt.FillColor = UiTheme.SurfaceVariant;
                    txt.ForeColor = UiTheme.TextPrimary;
                    txt.BorderColor = UiTheme.Border;
                }

                if (c is Guna2ComboBox cbo)
                {
                    cbo.FillColor = UiTheme.SurfaceVariant;
                    cbo.ForeColor = UiTheme.TextPrimary;
                    cbo.BorderColor = UiTheme.Border;
                }

                if (c is Guna2DateTimePicker dtp)
                {
                    dtp.FillColor = UiTheme.SurfaceVariant;
                    dtp.ForeColor = UiTheme.TextPrimary;
                    dtp.BorderColor = UiTheme.Border;
                    dtp.CustomFormat = "dd/MM/yyyy HH:mm";
                }
            }
        }

        private async void frmSchedules_New_Load(object sender, EventArgs e)
        {
            await LoadDropdownDataAsync();
            await LoadDataAsync();
        }

        private async Task LoadDropdownDataAsync()
        {
            try
            {
                _trains = await _catalogService.GetAllTrainsAsync();
                _routes = await _catalogService.GetAllRoutesAsync();

                _cboTrain.DataSource = _trains.ToList();
                _cboTrain.DisplayMember = "TrainName";
                _cboTrain.ValueMember = "TrainId";

                _cboRoute.DataSource = _routes.ToList();
                _cboRoute.DisplayMember = "RouteName";
                _cboRoute.ValueMember = "RouteId";
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi tải dữ liệu tham chiếu: {ex.Message}");
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _loadingOverlay?.Show("Đang tải dữ liệu...");
                var list = await _catalogService.GetAllSchedulesAsync();

                var displayList = list.Select(s => new
                {
                    ScheduleId = s.ScheduleId,
                    TenTau = s.TrainName ?? "",
                    Tuyen = s.RouteName ?? "",
                    GioKhoiHanh = s.DepartureTime,
                    GioDen = s.ArrivalTime,
                    TrangThai = s.Status
                }).ToList();

                _grid.DataSource = displayList;

                if (_grid.Columns["ScheduleId"] != null) _grid.Columns["ScheduleId"].Visible = false;
                if (_grid.Columns["TenTau"] != null) _grid.Columns["TenTau"].HeaderText = "Tên Tàu";
                if (_grid.Columns["Tuyen"] != null) _grid.Columns["Tuyen"].HeaderText = "Tuyến";
                if (_grid.Columns["GioKhoiHanh"] != null) _grid.Columns["GioKhoiHanh"].HeaderText = "Giờ Khởi Hành";
                if (_grid.Columns["GioDen"] != null) _grid.Columns["GioDen"].HeaderText = "Giờ Đến";
                if (_grid.Columns["TrangThai"] != null) _grid.Columns["TrangThai"].HeaderText = "Trạng Thái";

                HideRightPanel();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                _loadingOverlay?.Hide();
            }
        }

        private void _btnAdd_Click(object sender, EventArgs e)
        {
            PrepareAddMode();
        }

        private void PrepareAddMode()
        {
            _currentSchedule = new ScheduleDto();
            _lblFormTitle.Text = "Thêm Lịch trình mới";
            _cboTrain.SelectedIndex = -1;
            _cboRoute.SelectedIndex = -1;
            _dtpDepartureTime.Value = DateTime.Now.AddDays(1);
            _dtpArrivalTime.Value = DateTime.Now.AddDays(1).AddHours(2);
            _txtStatus.Text = "Scheduled";
            _btnDelete.Visible = false;
            ShowRightPanel();
        }

        private async void _grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (!_grid.Columns.Contains("ScheduleId")) return;
            var idObj = _grid.Rows[e.RowIndex].Cells["ScheduleId"].Value;
            if (idObj == null) return;

            try
            {
                int id = Convert.ToInt32(idObj);
                var schedule = await _catalogService.GetScheduleByIdAsync(id);
                if (schedule == null) return;

                _currentSchedule = schedule;
                _lblFormTitle.Text = "Sửa Lịch trình";
                _cboTrain.SelectedValue = schedule.TrainId;
                _cboRoute.SelectedValue = schedule.RouteId;
                _dtpDepartureTime.Value = schedule.DepartureTime;
                _dtpArrivalTime.Value = schedule.ArrivalTime;
                _txtStatus.Text = schedule.Status ?? "Scheduled";
                _btnDelete.Visible = true;

                ShowRightPanel();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi chi tiết: {ex.Message}");
            }
        }

        private async void _btnSave_Click(object sender, EventArgs e)
        {
            if (_cboTrain.SelectedValue == null || _cboRoute.SelectedValue == null)
            {
                UiNotifier.ErrorToast("Vui lòng chọn Tàu và Tuyến đường.");
                return;
            }

            if (_dtpDepartureTime.Value >= _dtpArrivalTime.Value)
            {
                UiNotifier.ErrorToast("Giờ đến phải sau giờ khởi hành.");
                return;
            }

            _currentSchedule ??= new ScheduleDto();
            _currentSchedule.TrainId = (int)_cboTrain.SelectedValue!;
            _currentSchedule.RouteId = (int)_cboRoute.SelectedValue!;
            _currentSchedule.DepartureTime = _dtpDepartureTime.Value;
            _currentSchedule.ArrivalTime = _dtpArrivalTime.Value;
            _currentSchedule.Status = string.IsNullOrWhiteSpace(_txtStatus.Text)
                ? "Scheduled"
                : _txtStatus.Text.Trim();

            try
            {
                _loadingOverlay?.Show("Đang lưu...");
                await _catalogService.SaveScheduleAsync(_currentSchedule);
                UiNotifier.SuccessToast("Lưu thành công!");
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi: {ex.Message}");
                _loadingOverlay?.Hide();
            }
        }

        private async void _btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentSchedule == null || _currentSchedule.ScheduleId == 0) return;

            var confirm = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa lịch trình này không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _loadingOverlay?.Show("Đang xóa...");
                await _catalogService.DeleteScheduleAsync(_currentSchedule.ScheduleId);
                UiNotifier.SuccessToast("Xóa thành công!");
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi: {ex.Message}");
                _loadingOverlay?.Hide();
            }
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            HideRightPanel();
        }

        private void ShowRightPanel() => _rightPanel.Visible = true;

        private void HideRightPanel()
        {
            _rightPanel.Visible = false;
            _currentSchedule = null;
        }
    }
}