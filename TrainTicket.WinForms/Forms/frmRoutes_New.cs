using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;
using Guna.UI2.WinForms;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmRoutes_New : Form, IThemeableForm
    {
        private readonly ICatalogService _catalogService;
        private RouteDto? _currentRoute;
        private LoadingOverlay? _loadingOverlay;

        public frmRoutes_New(ICatalogService catalogService)
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
            }
        }

        private async void frmRoutes_New_Load(object sender, EventArgs e)
        {
            await LoadStationsAsync();
            await LoadDataAsync();
        }

        private async Task LoadStationsAsync()
        {
            try
            {
                var stations = await _catalogService.GetAllStationsAsync();

                var listDep = stations.Select(s => new { s.StationId, s.StationName }).ToList();
                var listArr = stations.Select(s => new { s.StationId, s.StationName }).ToList();

                _cboDepartureStation.DataSource = new BindingSource(listDep.ToDictionary(s => s.StationId, s => s.StationName), null);
                _cboDepartureStation.DisplayMember = "Value";
                _cboDepartureStation.ValueMember = "Key";

                _cboArrivalStation.DataSource = new BindingSource(listArr.ToDictionary(s => s.StationId, s => s.StationName), null);
                _cboArrivalStation.DisplayMember = "Value";
                _cboArrivalStation.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi tải danh sách ga: {ex.Message}");
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _loadingOverlay?.Show("Đang tải dữ liệu...");
                var list = await _catalogService.GetAllRoutesAsync();

                var displayList = list.Select(r => new
                {
                    RouteId = r.RouteId,
                    TenTuyen = r.RouteName,
                    GaDi = r.DepartureStationName ?? "",
                    GaDen = r.ArrivalStationName ?? "",
                    KhoangCachKM = r.Distance
                }).ToList();

                _grid.DataSource = displayList;

                if (_grid.Columns["RouteId"] != null) _grid.Columns["RouteId"].Visible = false;

                if (_grid.Columns["TenTuyen"] != null) _grid.Columns["TenTuyen"].HeaderText = "Tên Tuyến";
                if (_grid.Columns["GaDi"] != null) _grid.Columns["GaDi"].HeaderText = "Ga Đi";
                if (_grid.Columns["GaDen"] != null) _grid.Columns["GaDen"].HeaderText = "Ga Đến";
                if (_grid.Columns["KhoangCachKM"] != null) _grid.Columns["KhoangCachKM"].HeaderText = "Khoảng cách (km)";

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
            _currentRoute = new RouteDto();
            _lblFormTitle.Text = "Thêm Tuyến mới";
            _txtRouteName.Clear();
            _cboDepartureStation.SelectedIndex = -1;
            _cboArrivalStation.SelectedIndex = -1;
            _txtDistance.Clear();
            _btnDelete.Visible = false;
            ShowRightPanel();
        }

        private async void _grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (!_grid.Columns.Contains("RouteId")) return;
            var idObj = _grid.Rows[e.RowIndex].Cells["RouteId"].Value;
            if (idObj == null) return;

            try
            {
                int id = Convert.ToInt32(idObj);
                var route = await _catalogService.GetRouteByIdAsync(id);
                if (route == null) return;

                _currentRoute = route;
                _lblFormTitle.Text = "Sửa Tuyến";
                _txtRouteName.Text = route.RouteName;
                _cboDepartureStation.SelectedValue = route.DepartureStation;
                _cboArrivalStation.SelectedValue = route.ArrivalStation;
                _txtDistance.Text = route.Distance?.ToString() ?? "";
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
            if (string.IsNullOrWhiteSpace(_txtRouteName.Text) ||
                _cboDepartureStation.SelectedValue == null ||
                _cboArrivalStation.SelectedValue == null)
            {
                UiNotifier.ErrorToast("Vui lòng nhập đủ Tên tuyến và chọn Ga đi/Ga đến.");
                return;
            }

            if (_cboDepartureStation.SelectedValue.Equals(_cboArrivalStation.SelectedValue))
            {
                UiNotifier.ErrorToast("Ga đi và Ga đến không được trùng nhau.");
                return;
            }

            _currentRoute ??= new RouteDto();
            _currentRoute.RouteName = _txtRouteName.Text.Trim();
            _currentRoute.DepartureStation = (int)_cboDepartureStation.SelectedValue!;
            _currentRoute.ArrivalStation = (int)_cboArrivalStation.SelectedValue!;

            if (decimal.TryParse(_txtDistance.Text.Trim(), out decimal distance))
                _currentRoute.Distance = distance;
            else
                _currentRoute.Distance = null;

            try
            {
                _loadingOverlay?.Show("Đang lưu...");
                await _catalogService.SaveRouteAsync(_currentRoute);
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
            if (_currentRoute == null || _currentRoute.RouteId == 0) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa tuyến {_currentRoute.RouteName} không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _loadingOverlay?.Show("Đang xóa...");
                await _catalogService.DeleteRouteAsync(_currentRoute.RouteId);
                UiNotifier.SuccessToast("Xóa thành công!");
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi xóa dữ liệu: {ex.Message}");
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
            _currentRoute = null;
        }
    }
}