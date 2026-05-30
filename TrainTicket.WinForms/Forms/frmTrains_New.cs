using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;
using Guna.UI2.WinForms;
using System.Drawing.Drawing2D;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmTrains_New : Form, IThemeableForm
    {
        private readonly ICatalogService _catalogService;
        private TrainDto? _currentTrain;
        private LoadingOverlay? _loadingOverlay;

        public frmTrains_New(ICatalogService catalogService)
        {
            InitializeComponent();
            _catalogService = catalogService;
            _loadingOverlay = new LoadingOverlay(this);

            // Set up right header paint event for gradient setup (like in code-based version)
            rightHeader.Paint += RightHeader_Paint;

            ApplyTheme();
        }

        private void RightHeader_Paint(object sender, PaintEventArgs e)
        {
            using var br = new LinearGradientBrush(
                rightHeader.ClientRectangle,
                UiTheme.HeaderGradientStart,
                UiTheme.HeaderGradientEnd,
                LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(br, rightHeader.ClientRectangle);
        }

        public void ApplyTheme()
        {
            BackColor                       = UiTheme.Background;
            _topPanel.FillColor             = UiTheme.Surface;
            _mainPanel.BackColor            = UiTheme.Background;
            _grid.BackgroundColor           = UiTheme.Background;
            _grid.DefaultCellStyle.BackColor= UiTheme.Surface;
            _grid.DefaultCellStyle.ForeColor= UiTheme.TextPrimary;
            _grid.ThemeStyle.HeaderStyle.BackColor = UiTheme.PrimaryDark;

            _grid.ThemeStyle.RowsStyle.BackColor          = UiTheme.Surface;
            _grid.ThemeStyle.RowsStyle.ForeColor          = UiTheme.TextPrimary;

            _grid.ThemeStyle.AlternatingRowsStyle.BackColor = UiTheme.SurfaceVariant;

            _cardTotal.FillColor       = UiTheme.Surface;
            _cardActive.FillColor      = UiTheme.Surface;
            _cardMaintenance.FillColor = UiTheme.Surface;

            _cardTotal.CustomBorderColor       = UiTheme.Primary;
            _cardActive.CustomBorderColor      = UiTheme.Success;
            _cardMaintenance.CustomBorderColor = UiTheme.Warning;

            _txtSearch.FillColor  = UiTheme.SurfaceVariant;
            _txtSearch.ForeColor  = UiTheme.TextPrimary;
            _cboStatus.FillColor  = UiTheme.SurfaceVariant;
            _cboStatus.ForeColor  = UiTheme.TextPrimary;

            _lblTitle.ForeColor    = UiTheme.TextPrimary;
            _lblTitle.BackColor    = Color.Transparent;

            _rightPanel.FillColor  = UiTheme.Surface;

            foreach (Control c in _rightPanel.Controls)
            {
                if (c is Label lbl && lbl != _lblFormTitle)
                {
                    lbl.ForeColor = UiTheme.TextSecondary;
                    lbl.BackColor = Color.Transparent;
                }
                if (c is Guna2TextBox txt)
                {
                    txt.FillColor   = UiTheme.SurfaceVariant;
                    txt.ForeColor   = UiTheme.TextPrimary;
                    txt.BorderColor = UiTheme.Border;
                }
            }

            foreach (Control c in _cardTotal.Controls) { if (c is Label lbl && lbl != _lblTotalCount) { lbl.ForeColor = UiTheme.TextSecondary; } }
            foreach (Control c in _cardActive.Controls) { if (c is Label lbl && lbl != _lblActiveCount) { lbl.ForeColor = UiTheme.TextSecondary; } }
            foreach (Control c in _cardMaintenance.Controls) { if (c is Label lbl && lbl != _lblMaintenanceCount) { lbl.ForeColor = UiTheme.TextSecondary; } }

            _lblTotalCount.ForeColor = UiTheme.TextPrimary;
            _lblActiveCount.ForeColor = UiTheme.TextPrimary;
            _lblMaintenanceCount.ForeColor = UiTheme.TextPrimary;
        }

        private async void frmTrains_New_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _loadingOverlay?.Show("Đang tải dữ liệu tàu...");
                var list = await _catalogService.GetAllTrainsAsync();

                _lblTotalCount.Text       = list.Count().ToString();
                _lblActiveCount.Text      = list.Count(t => t.TrainType != "Bảo trì").ToString();
                _lblMaintenanceCount.Text = list.Count(t => t.TrainType == "Bảo trì").ToString();

                var displayList = list.Select(t => new
                {
                    TrainId   = t.TrainId,
                    MaTau   = t.TrainCode,
                    TenTau  = t.TrainName,
                    LoaiTau = t.TrainType
                }).ToList();

                _grid.DataSource = displayList;
                if (_grid.Columns["TrainId"] != null) _grid.Columns["TrainId"].Visible = false;

                if (_grid.Columns["MaTau"] != null) _grid.Columns["MaTau"].HeaderText = "Mã Tàu";
                if (_grid.Columns["TenTau"] != null) _grid.Columns["TenTau"].HeaderText = "Tên Tàu";
                if (_grid.Columns["LoaiTau"] != null) _grid.Columns["LoaiTau"].HeaderText = "Loại Tàu";

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
            _currentTrain = new TrainDto();
            _lblFormTitle.Text = "Thêm tàu mới";
            _txtTrainCode.Clear();
            _txtTrainName.Clear();
            _txtTrainType.Clear();
            _btnDelete.Visible = false;
            ShowRightPanel();
        }

        private async void _grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var idObj = _grid.Rows[e.RowIndex].Cells["TrainID"].Value;
            if (idObj == null) return;

            try
            {
                var train = await _catalogService.GetTrainByIdAsync(Convert.ToInt32(idObj));
                if (train == null) return;

                _currentTrain      = train;
                _lblFormTitle.Text = "Sửa thông tin tàu";
                _txtTrainCode.Text = train.TrainCode;
                _txtTrainName.Text = train.TrainName;
                _txtTrainType.Text = train.TrainType;
                _btnDelete.Visible = true;
                ShowRightPanel();
            }
            catch (Exception ex) 
            { 
                UiNotifier.ErrorToast($"Lỗi: {ex.Message}"); 
            }
        }

        private async void _btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtTrainCode.Text) || string.IsNullOrWhiteSpace(_txtTrainName.Text))
            {
                UiNotifier.ErrorToast("Vui lòng nhập đủ Mã và Tên Tàu.");
                return;
            }

            if (_currentTrain == null) _currentTrain = new TrainDto();
            _currentTrain.TrainCode  = _txtTrainCode.Text.Trim();
            _currentTrain.TrainName  = _txtTrainName.Text.Trim();
            _currentTrain.TrainType  = _txtTrainType.Text.Trim();

            try
            {
                _loadingOverlay?.Show("Đang lưu...");
                await _catalogService.SaveTrainAsync(_currentTrain);
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
            if (_currentTrain == null || _currentTrain.TrainId == 0) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa tàu {_currentTrain.TrainCode}?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _loadingOverlay?.Show("Đang xóa...");
                    await _catalogService.DeleteTrainAsync(_currentTrain.TrainId);
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

        private void ShowRightPanel() => _rightPanel.Visible = true;

        private void HideRightPanel() 
        { 
            _rightPanel.Visible = false; 
            _currentTrain = null; 
        }
    }
}
