using Guna.UI2.WinForms;
using TrainTicket.Business.Interfaces;
using TrainTicket.Business.DTOs;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public class frmSchedules : Form, IThemeableForm
    {
        private readonly ICatalogService _catalogService;
        private ScheduleDto? _currentSchedule;
        private List<TrainDto> _trains = new();
        private List<RouteDto> _routes = new();

        private readonly Guna2Panel _topPanel = new();
        private readonly Guna2Panel _mainPanel = new();
        private readonly Guna2Panel _rightPanel = new();
        private readonly Guna2DataGridView _grid = new();

        private readonly Label _lblTitle = new();
        private readonly Guna2Button _btnAdd = new();

        // Form inputs
        private readonly Label _lblFormTitle = new();
        private readonly Guna2ComboBox _cboTrain = new();
        private readonly Guna2ComboBox _cboRoute = new();
        private readonly Guna2DateTimePicker _dtpDepartureTime = new();
        private readonly Guna2DateTimePicker _dtpArrivalTime = new();
        private readonly Guna2TextBox _txtStatus = new();
        private readonly Guna2Button _btnSave = new();
        private readonly Guna2Button _btnDelete = new();
        private readonly Guna2Button _btnCancel = new();

        private LoadingOverlay? _loadingOverlay;

        public frmSchedules(ICatalogService catalogService)
        {
            _catalogService = catalogService;
            InitializeUi();
            Load += FrmSchedules_Load;
        }

        private void InitializeUi()
        {
            Text = "Qu\u1ea3n l\u00fd l\u1ecbch tr\u00ecnh";
            Width = 1400;
            Height = 700;
            BackColor = UiTheme.Background;

            // TOP PANEL
            _topPanel.Dock = DockStyle.Top;
            _topPanel.Height = 70;
            _topPanel.FillColor = UiTheme.Surface;
            _topPanel.ShadowDecoration.Enabled = true;

            _lblTitle.Text = "Danh s\u00e1ch L\u1ecbch tr\u00ecnh";
            _lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            _lblTitle.ForeColor = UiTheme.TextPrimary;
            _lblTitle.Location = new Point(20, 20);
            _lblTitle.AutoSize = true;

            _btnAdd.Text = "+ Th\u00eam l\u1ecbch tr\u00ecnh";
            _btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _btnAdd.Size = new Size(160, 40);
            _btnAdd.Location = new Point(280, 15);
            _btnAdd.BorderRadius = 8;
            _btnAdd.FillColor = Color.FromArgb(34, 197, 94); // Emerald 500
            _btnAdd.HoverState.FillColor = Color.FromArgb(22, 163, 74);
            _btnAdd.Click += (_, _) => PrepareAddMode();

            _topPanel.Controls.Add(_lblTitle);
            _topPanel.Controls.Add(_btnAdd);

            // RIGHT PANEL (FORM)
            _rightPanel.Dock = DockStyle.Right;
            _rightPanel.Width = 420;
            _rightPanel.FillColor = UiTheme.Surface;
            _rightPanel.Padding = new Padding(20);
            _rightPanel.Visible = false; // Hide by default

            _lblFormTitle.Text = "Th\u00eam/S\u1eeda L\u1ecbch tr\u00ecnh";
            _lblFormTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            _lblFormTitle.ForeColor = UiTheme.TextPrimary;
            _lblFormTitle.Location = new Point(20, 20);
            _lblFormTitle.AutoSize = true;

            // Define inputs
            int startY = 70;
            int gap = 70;

            CreateComboGroup("T\u00e0u", _cboTrain, startY);
            CreateComboGroup("Tuy\u1ebfn \u0111\u01b0\u1eddng", _cboRoute, startY + gap);
            CreateDateTimeGroup("Gi\u1edd kh\u1edfi h\u00e0nh", _dtpDepartureTime, startY + gap * 2);
            CreateDateTimeGroup("Gi\u1edd \u0111\u1ebfn", _dtpArrivalTime, startY + gap * 3);
            CreateInputGroup("Tr\u1ea1ng th\u00e1i", _txtStatus, startY + gap * 4, "Scheduled/Cancelled/Delayed");

            // Buttons
            _btnSave.Text = "L\u01b0u l\u1ea1i";
            _btnSave.Size = new Size(100, 40);
            _btnSave.Location = new Point(20, startY + gap * 5 + 20);
            _btnSave.BorderRadius = 8;
            _btnSave.FillColor = UiTheme.Primary;
            _btnSave.Click += BtnSave_Click;

            _btnDelete.Text = "X\u00f3a";
            _btnDelete.Size = new Size(80, 40);
            _btnDelete.Location = new Point(130, startY + gap * 5 + 20);
            _btnDelete.BorderRadius = 8;
            _btnDelete.FillColor = Color.FromArgb(239, 68, 68); // Red
            _btnDelete.Click += BtnDelete_Click;

            _btnCancel.Text = "H\u1ee7y";
            _btnCancel.Size = new Size(80, 40);
            _btnCancel.Location = new Point(220, startY + gap * 5 + 20);
            _btnCancel.BorderRadius = 8;
            _btnCancel.FillColor = Color.Gray;
            _btnCancel.Click += (_, _) => HideRightPanel();

            _loadingOverlay = new LoadingOverlay(this);
            ApplyTheme();
        }

        private void CreateInputGroup(string labelText, Guna2TextBox txt, int top, string placeholder)
        {
            var lbl = new Label
            {
                Text = labelText,
                Location = new Point(20, top),
                AutoSize = true,
                ForeColor = UiTheme.TextSecondary,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            txt.Location = new Point(20, top + 25);
            txt.Size = new Size(380, 36);
            txt.BorderRadius = 6;
            txt.PlaceholderText = placeholder;
            _rightPanel.Controls.Add(lbl);
            _rightPanel.Controls.Add(txt);
        }

        private void CreateComboGroup(string labelText, Guna2ComboBox cbo, int top)
        {
            var lbl = new Label
            {
                Text = labelText,
                Location = new Point(20, top),
                AutoSize = true,
                ForeColor = UiTheme.TextSecondary,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            cbo.Location = new Point(20, top + 25);
            cbo.Size = new Size(380, 36);
            cbo.BorderRadius = 6;
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
            _rightPanel.Controls.Add(lbl);
            _rightPanel.Controls.Add(cbo);
        }

        private void CreateDateTimeGroup(string labelText, Guna2DateTimePicker dtp, int top)
        {
            var lbl = new Label
            {
                Text = labelText,
                Location = new Point(20, top),
                AutoSize = true,
                ForeColor = UiTheme.TextSecondary,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            dtp.Location = new Point(20, top + 25);
            dtp.Size = new Size(380, 36);
            dtp.BorderRadius = 6;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.CustomFormat = "dd/MM/yyyy HH:mm";
            _rightPanel.Controls.Add(lbl);
            _rightPanel.Controls.Add(dtp);
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _topPanel.FillColor = UiTheme.Surface;
            _rightPanel.FillColor = UiTheme.Surface;
            _lblTitle.ForeColor = UiTheme.TextPrimary;
            _lblFormTitle.ForeColor = UiTheme.TextPrimary;

            foreach (Control c in _rightPanel.Controls)
            {
                if (c is Label lbl && lbl != _lblFormTitle)
                    lbl.ForeColor = UiTheme.TextSecondary;

                if (c is Guna2TextBox txt)
                {
                    txt.FillColor = UiTheme.Background;
                    txt.ForeColor = UiTheme.TextPrimary;
                }

                if (c is Guna2ComboBox cbo)
                {
                    cbo.FillColor = UiTheme.Background;
                    cbo.ForeColor = UiTheme.TextPrimary;
                }

                if (c is Guna2DateTimePicker dtp)
                {
                    dtp.FillColor = UiTheme.Background;
                    dtp.ForeColor = UiTheme.TextPrimary;
                }
            }
        }

        private async void FrmSchedules_Load(object? sender, EventArgs e)
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
                _cboTrain.ValueMember = "TrainID";

                _cboRoute.DataSource = _routes.ToList();
                _cboRoute.DisplayMember = "RouteName";
                _cboRoute.ValueMember = "RouteID";
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"L\u1ed7i t\u1ea3i d\u1eef li\u1ec7u tham chi\u1ebfu: {ex.Message}");
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _loadingOverlay?.Show("?ang t?i d? li?u...");
                var list = await _catalogService.GetAllSchedulesAsync();

                var displayList = list.Select(s => new 
                { 
                    s.ScheduleID, 
                    TenTau = s.TrainName ?? "",
                    Tuyen = s.RouteName ?? "",
                    GioKhoiHanh = s.DepartureTime,
                    GioDen = s.ArrivalTime,
                    TrangThai = s.Status 
                }).ToList();

                _grid.DataSource = displayList;
                if (_grid.Columns["ScheduleID"] != null)
                {
                    _grid.Columns["ScheduleID"].Visible = false;
                }
                HideRightPanel();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"L\u1ed7i t\u1ea3i d\u1eef li\u1ec7u: {ex.Message}");
            }
            finally
            {
                _loadingOverlay?.Hide();
            }
        }

        private void PrepareAddMode()
        {
            _currentSchedule = new ScheduleDto();
            _lblFormTitle.Text = "Thêm L?ch trình m?i";
            _cboTrain.SelectedIndex = -1;
            _cboRoute.SelectedIndex = -1;
            _dtpDepartureTime.Value = DateTime.Now.AddDays(1);
            _dtpArrivalTime.Value = DateTime.Now.AddDays(1).AddHours(2);
            _txtStatus.Text = "Scheduled";
            _btnDelete.Visible = false;

            ShowRightPanel();
        }

        private async void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var idObj = _grid.Rows[e.RowIndex].Cells["ScheduleID"].Value;
            if (idObj == null) return;

            try
            {
                int id = Convert.ToInt32(idObj);
                var schedule = await _catalogService.GetScheduleByIdAsync(id);
                if (schedule == null) return;

                _currentSchedule = schedule;
                _lblFormTitle.Text = "S\u1eeda L\u1ecbch tr\u00ecnh";
                _cboTrain.SelectedValue = schedule.TrainID;
                _cboRoute.SelectedValue = schedule.RouteID;
                _dtpDepartureTime.Value = schedule.DepartureTime;
                _dtpArrivalTime.Value = schedule.ArrivalTime;
                _txtStatus.Text = schedule.Status ?? "Scheduled";
                _btnDelete.Visible = true;

                ShowRightPanel();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"L\u1ed7i chi ti\u1ebft: {ex.Message}");
            }
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            if (_cboTrain.SelectedValue == null || _cboRoute.SelectedValue == null)
            {
                UiNotifier.ErrorToast("Vui l\u00f2ng ch\u1ecdn T\u00e0u v\u00e0 Tuy\u1ebfn \u0111\u01b0\u1eddng.");
                return;
            }

            if (_dtpDepartureTime.Value >= _dtpArrivalTime.Value)
            {
                UiNotifier.ErrorToast("Gi? ??n ph?i sau gi? kh?i hành.");
                return;
            }

            if (_currentSchedule == null) _currentSchedule = new ScheduleDto();
            _currentSchedule.TrainID = (int)_cboTrain.SelectedValue!;
            _currentSchedule.RouteID = (int)_cboRoute.SelectedValue!;
            _currentSchedule.DepartureTime = _dtpDepartureTime.Value;
            _currentSchedule.ArrivalTime = _dtpArrivalTime.Value;
            _currentSchedule.Status = string.IsNullOrWhiteSpace(_txtStatus.Text) ? "Scheduled" : _txtStatus.Text.Trim();

            try
            {
                _loadingOverlay?.Show("?ang l?u...");
                await _catalogService.SaveScheduleAsync(_currentSchedule);
                UiNotifier.SuccessToast("L?u thành công!");
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"L\u1ed7i: {ex.Message}");
                _loadingOverlay?.Hide();
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_currentSchedule == null || _currentSchedule.ScheduleID == 0) return;

            var confirm = MessageBox.Show(
                $"B\u1ea1n c\u00f3 ch\u1eafc ch\u1eacf mu\u1ed1n x\u00f3a l\u1ecbch tr\u00ecnh n\u00e0y kh\u00f4ng?", 
                "X\u00e1c nh\u1eadn x\u00f3a", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _loadingOverlay?.Show("\u0110ang x\u00f3a...");
                    await _catalogService.DeleteScheduleAsync(_currentSchedule.ScheduleID);
                    UiNotifier.SuccessToast("Xóa thành công!");
                    await LoadDataAsync();
                }
                catch (Exception ex)
                {
                    UiNotifier.ErrorToast($"L\u1ed7i: {ex.Message}");
                    _loadingOverlay?.Hide();
                }
            }
        }

        private void ShowRightPanel()
        {
            _rightPanel.Visible = true;
        }

        private void HideRightPanel()
        {
            _rightPanel.Visible = false;
            _currentSchedule = null;
        }
    }
}
