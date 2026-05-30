using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmStations_New : Form, IThemeableForm
    {
        private readonly ICatalogService _catalogService;
        private StationDto? _currentStation;
        private LoadingOverlay? _loadingOverlay;

        public frmStations_New(ICatalogService catalogService)
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

            _btnAdd.FillColor = UiTheme.Secondary;
            _btnAdd.HoverState.FillColor = UiTheme.SecondaryLight;

            _grid.ThemeStyle.AlternatingRowsStyle.BackColor = UiTheme.Surface;
            _grid.ThemeStyle.AlternatingRowsStyle.ForeColor = UiTheme.TextPrimary;
            _grid.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = UiTheme.PrimaryLight;
            _grid.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = UiTheme.TextPrimary;
            _grid.ThemeStyle.RowsStyle.BackColor = UiTheme.Background;
            _grid.ThemeStyle.RowsStyle.ForeColor = UiTheme.TextPrimary;
            _grid.ThemeStyle.RowsStyle.SelectionBackColor = UiTheme.PrimaryLight;
            _grid.ThemeStyle.RowsStyle.SelectionForeColor = UiTheme.TextPrimary;

            foreach (Control c in _rightPanel.Controls)
            {
                if (c is Label lbl && lbl != _lblFormTitle)
                {
                    lbl.ForeColor = UiTheme.TextSecondary;
                    lbl.BackColor = Color.Transparent;
                }

                if (c is Guna.UI2.WinForms.Guna2TextBox txt)
                {
                    txt.FillColor = UiTheme.Background;
                    txt.ForeColor = UiTheme.TextPrimary;
                    txt.BorderColor = UiTheme.Border;
                }

                if (c is Guna.UI2.WinForms.Guna2Button btn)
                {
                    if (btn == _btnSave)
                    {
                        btn.FillColor = UiTheme.Primary;
                        btn.HoverState.FillColor = UiTheme.PrimaryHover;
                    }
                    else if (btn == _btnDelete)
                    {
                        btn.FillColor = UiTheme.Error;
                        btn.HoverState.FillColor = Color.FromArgb(220, 53, 69);
                    }
                    else if (btn == _btnCancel)
                    {
                        btn.FillColor = UiTheme.SurfaceVariant;
                        btn.HoverState.FillColor = UiTheme.Border;
                    }
                }
            }
        }

        private async void frmStations_New_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _loadingOverlay?.Show("Đang tải dữ liệu Ga...");
                var list = await _catalogService.GetAllStationsAsync();

                var displayList = list.Select(s => new { 
                    s.StationId, 
                    MaGa = s.StationCode, 
                    TenGa = s.StationName, 
                    ThanhPho = s.City,
                    DiaChi = s.Address 
                }).ToList();

                _grid.DataSource = displayList;
                if (_grid.Columns["StationId"] != null) _grid.Columns["StationId"].Visible = false;
                if (_grid.Columns["MaGa"] != null) _grid.Columns["MaGa"].HeaderText = "Mã Ga";
                if (_grid.Columns["TenGa"] != null) _grid.Columns["TenGa"].HeaderText = "Tên Ga";
                if (_grid.Columns["ThanhPho"] != null) _grid.Columns["ThanhPho"].HeaderText = "Thành phố";
                if (_grid.Columns["DiaChi"] != null) _grid.Columns["DiaChi"].HeaderText = "Địa chỉ";

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
            _currentStation = new StationDto();
            _lblFormTitle.Text = "Thêm Ga mới";
            _txtStationCode.Clear();
            _txtStationName.Clear();
            _txtCity.Clear();
            _txtAddress.Clear();
            _btnDelete.Visible = false;

            ShowRightPanel();
        }

        private async void _grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var idObj = _grid.Rows[e.RowIndex].Cells["StationID"].Value;
            if (idObj == null) return;

            try
            {
                int id = Convert.ToInt32(idObj);
                var station = await _catalogService.GetStationByIdAsync(id);
                if (station == null) return;

                _currentStation = station;
                _lblFormTitle.Text = "Sửa Ga";
                _txtStationCode.Text = station.StationCode;
                _txtStationName.Text = station.StationName;
                _txtCity.Text = station.City;
                _txtAddress.Text = station.Address ?? "";
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
            if (string.IsNullOrWhiteSpace(_txtStationCode.Text) || 
                string.IsNullOrWhiteSpace(_txtStationName.Text) ||
                string.IsNullOrWhiteSpace(_txtCity.Text))
            {
                UiNotifier.ErrorToast("Vui lòng nhập đủ Mã ga, Tên ga và Thành phố.");
                return;
            }

            if (_currentStation == null) _currentStation = new StationDto();
            _currentStation.StationCode = _txtStationCode.Text.Trim();
            _currentStation.StationName = _txtStationName.Text.Trim();
            _currentStation.City = _txtCity.Text.Trim();
            _currentStation.Address = string.IsNullOrWhiteSpace(_txtAddress.Text) ? null : _txtAddress.Text.Trim();

            try
            {
                _loadingOverlay?.Show("Đang lưu...");
                await _catalogService.SaveStationAsync(_currentStation);
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
            if (_currentStation == null || _currentStation.StationId == 0) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa ga {_currentStation.StationName} không?", 
                "Xác nhận xóa", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _loadingOverlay?.Show("Đang xóa...");
                    await _catalogService.DeleteStationAsync(_currentStation.StationId);
                    UiNotifier.SuccessToast("Xóa thành công!");
                    await LoadDataAsync();
                }
                catch (Exception ex)
                {
                    UiNotifier.ErrorToast($"Lỗi: {ex.Message}");
                    _loadingOverlay?.Hide();
                }
            }
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            HideRightPanel();
        }

        private void ShowRightPanel()
        {
            _rightPanel.Visible = true;
        }

        private void HideRightPanel()
        {
            _rightPanel.Visible = false;
            _currentStation = null;
        }
    }
}
