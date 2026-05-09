using Guna.UI2.WinForms;
using TrainTicket.Business.Interfaces;
using TrainTicket.Business.DTOs;
using TrainTicket.Data.Entities;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public class frmRoutes : Form, IThemeableForm
    {
        private readonly ICatalogService _catalogService;
        private RouteDto? _currentRoute;

        private readonly Guna2Panel _topPanel = new();
        private readonly Guna2Panel _mainPanel = new();
        private readonly Guna2Panel _rightPanel = new();
        private readonly Guna2DataGridView _grid = new();

        private readonly Label _lblTitle = new();
        private readonly Guna2Button _btnAdd = new();

        // Form inputs
        private readonly Label _lblFormTitle = new();
        private readonly Guna2TextBox _txtRouteName = new();
        private readonly Guna2ComboBox _cboRouteType = new();
        private readonly Guna2ComboBox _cboDepartureStation = new();
        private readonly Guna2ComboBox _cboArrivalStation = new();
        private readonly Guna2TextBox _txtDistance = new();
        private readonly Guna2Button _btnSave = new();
        private readonly Guna2Button _btnDelete = new();
        private readonly Guna2Button _btnCancel = new();

        private LoadingOverlay? _loadingOverlay;

        public frmRoutes(ICatalogService catalogService)
        {
            _catalogService = catalogService;
            InitializeUi();
            Load += FrmRoutes_Load;
        }

        private void InitializeUi()
        {
            Text = "Qu?n lý tuy?n ???ng";
            Width = 1200;
            Height = 700;
            BackColor = UiTheme.Background;

            // TOP PANEL
            _topPanel.Dock = DockStyle.Top;
            _topPanel.Height = 70;
            _topPanel.FillColor = UiTheme.Surface;
            _topPanel.ShadowDecoration.Enabled = true;

            _lblTitle.Text = "Danh sách Tuy?n ???ng";
            _lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            _lblTitle.ForeColor = UiTheme.TextPrimary;
            _lblTitle.Location = new Point(20, 20);
            _lblTitle.AutoSize = true;

            _btnAdd.Text = "+ Thêm tuy?n ???ng";
            _btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _btnAdd.Size = new Size(180, 40);
            _btnAdd.Location = new Point(280, 15);
            _btnAdd.BorderRadius = 8;
            _btnAdd.FillColor = Color.FromArgb(34, 197, 94);
            _btnAdd.HoverState.FillColor = Color.FromArgb(22, 163, 74);
            _btnAdd.Click += (_, _) => PrepareAddMode();

            _topPanel.Controls.Add(_lblTitle);
            _topPanel.Controls.Add(_btnAdd);

            // RIGHT PANEL (FORM)
            _rightPanel.Dock = DockStyle.Right;
            _rightPanel.Width = 400;
            _rightPanel.FillColor = UiTheme.Surface;
            _rightPanel.Padding = new Padding(20);
            _rightPanel.Visible = false; // Hide by default

            _lblFormTitle.Text = "Thêm / S?a Tuy?n";
            _lblFormTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            _lblFormTitle.ForeColor = UiTheme.TextPrimary;
            _lblFormTitle.Location = new Point(20, 20);
            _lblFormTitle.AutoSize = true;

            // Define inputs
            int startY = 70;
            int gap = 70;

            CreateInputGroup("Tê?n tuyê?n", _txtRouteName, startY, "Vi? du?: Sa?i Gòn - Ha? Nô?i...");
            CreateComboGroup("Loa?i tuyê?n", _cboRouteType, startY + gap);
            CreateComboGroup("Ga ?i", _cboDepartureStation, startY + gap * 2);
            CreateComboGroup("Ga ?ê?n", _cboArrivalStation, startY + gap * 3);
            CreateInputGroup("Kho?ng cách (km)", _txtDistance, startY + gap * 4, "Vi? du?: 1728.5");

            // Initialize RouteType items
            _cboRouteType.Items.AddRange(new object[] { "Th??ng", "T?c hành", "B?c - Nam", "??a ph??ng", "Du l?ch" });
            _cboRouteType.SelectedIndex = 0;

            // Buttons
            _btnSave.Text = "L?u la?i";
            _btnSave.Size = new Size(100, 40);
            _btnSave.Location = new Point(20, startY + gap * 5 + 20);
            _btnSave.BorderRadius = 8;
            _btnSave.FillColor = UiTheme.Primary;
            _btnSave.Click += BtnSave_Click;

            _btnDelete.Text = "Xóa";
            _btnDelete.Size = new Size(80, 40);
            _btnDelete.Location = new Point(130, startY + gap * 5 + 20);
            _btnDelete.BorderRadius = 8;
            _btnDelete.FillColor = Color.FromArgb(239, 68, 68); // Red
            _btnDelete.Click += BtnDelete_Click;

            _btnCancel.Text = "Hu?y";
            _btnCancel.Size = new Size(80, 40);
            _btnCancel.Location = new Point(220, startY + gap * 5 + 20);
            _btnCancel.BorderRadius = 8;
            _btnCancel.FillColor = Color.Gray;
            _btnCancel.Click += (_, _) => HideRightPanel();

            _rightPanel.Controls.AddRange(new Control[] { _lblFormTitle, _btnSave, _btnDelete, _btnCancel });

            // MAIN PANEL (GRID)
            _mainPanel.Dock = DockStyle.Fill;
            _mainPanel.Padding = new Padding(20);
            _mainPanel.BackColor = Color.Transparent;

            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(15, 23, 42);
            _grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            _grid.CellDoubleClick += Grid_CellDoubleClick;

            _mainPanel.Controls.Add(_grid);

            Controls.Add(_mainPanel);
            Controls.Add(_rightPanel);
            Controls.Add(_topPanel);

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
            txt.Size = new Size(360, 36);
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
            cbo.Size = new Size(360, 36);
            cbo.BorderRadius = 6;
            cbo.DropDownStyle = ComboBoxStyle.DropDownList;
            _rightPanel.Controls.Add(lbl);
            _rightPanel.Controls.Add(cbo);
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
            }
        }

        private async void FrmRoutes_Load(object? sender, EventArgs e)
        {
            await LoadStationsAsync();
            await LoadDataAsync();
        }

        private async Task LoadStationsAsync()
        {
            try
            {
                var stations = await _catalogService.GetAllStationsAsync();

                var dictStationsDep = stations.ToDictionary(s => s.StationID, s => s.StationName);
                var dictStationsArr = stations.ToDictionary(s => s.StationID, s => s.StationName);

                _cboDepartureStation.DataSource = new BindingSource(dictStationsDep, null);
                _cboDepartureStation.DisplayMember = "Value";
                _cboDepartureStation.ValueMember = "Key";

                _cboArrivalStation.DataSource = new BindingSource(dictStationsArr, null);
                _cboArrivalStation.DisplayMember = "Value";
                _cboArrivalStation.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"L?i t?i danh sách ga: {ex.Message}");
            }
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _loadingOverlay?.Show("?ang t?i d? li?u...");
                var list = await _catalogService.GetAllRoutesAsync();

                var displayList = list.Select(r => new { 
                    r.RouteID, 
                    TenTuyen = r.RouteName,
                    LoaiTuyen = r.RouteType,
                    GaDi = r.DepartureStationName ?? "",
                    GaDen = r.ArrivalStationName ?? "",
                    KhoangCachKM = r.Distance 
                }).ToList();

                _grid.DataSource = displayList;
                if (_grid.Columns["RouteID"] != null)
                {
                    _grid.Columns["RouteID"].Visible = false;
                }
                HideRightPanel();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"L?i t?i d? li?u: {ex.Message}");
            }
            finally
            {
                _loadingOverlay?.Hide();
            }
        }

        private void PrepareAddMode()
        {
            _currentRoute = new RouteDto();
            _lblFormTitle.Text = "Thêm Tuy?n m?i";
            _txtRouteName.Clear();
            _cboRouteType.SelectedIndex = 0;
            _cboDepartureStation.SelectedIndex = -1;
            _cboArrivalStation.SelectedIndex = -1;
            _txtDistance.Clear();
            _btnDelete.Visible = false;

            ShowRightPanel();
        }

        private async void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var idObj = _grid.Rows[e.RowIndex].Cells["RouteID"].Value;
            if (idObj == null) return;

            try
            {
                int id = Convert.ToInt32(idObj);
                var route = await _catalogService.GetRouteByIdAsync(id);
                if (route == null) return;

                _currentRoute = route;
                _lblFormTitle.Text = "S?a Tuy?n";
                _txtRouteName.Text = route.RouteName;
                if (!string.IsNullOrEmpty(route.RouteType) && _cboRouteType.Items.Contains(route.RouteType))
                    _cboRouteType.SelectedItem = route.RouteType;
                else
                    _cboRouteType.SelectedIndex = 0;

                _cboDepartureStation.SelectedValue = route.DepartureStation;
                _cboArrivalStation.SelectedValue = route.ArrivalStation;
                _txtDistance.Text = route.Distance?.ToString() ?? "";
                _btnDelete.Visible = true;

                ShowRightPanel();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"L?i chi ti?t: {ex.Message}");
            }
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtRouteName.Text) || 
                _cboDepartureStation.SelectedValue == null ||
                _cboArrivalStation.SelectedValue == null)
            {
                UiNotifier.ErrorToast("Vui lòng nh?p ?? Tên tuy?n và ch?n Ga ?i/Ga ??n.");
                return;
            }

            if (_cboDepartureStation.SelectedValue.Equals(_cboArrivalStation.SelectedValue))
            {
                UiNotifier.ErrorToast("Ga ?i và Ga ??n không ???c trùng nhau.");
                return;
            }

            if (_currentRoute == null) _currentRoute = new RouteDto();
            _currentRoute.RouteName = _txtRouteName.Text.Trim();
            _currentRoute.RouteType = _cboRouteType.SelectedItem?.ToString() ?? "Th??ng";
            _currentRoute.DepartureStation = (int)_cboDepartureStation.SelectedValue!;
            _currentRoute.ArrivalStation = (int)_cboArrivalStation.SelectedValue!;

            if (decimal.TryParse(_txtDistance.Text.Trim(), out decimal distance))
                _currentRoute.Distance = distance;
            else
                _currentRoute.Distance = null;

            try
            {
                _loadingOverlay?.Show("?ang l?u...");
                await _catalogService.SaveRouteAsync(_currentRoute);
                UiNotifier.SuccessToast("L?u thành công!");
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"L?i: {ex.Message}");
                _loadingOverlay?.Hide();
            }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_currentRoute == null || _currentRoute.RouteID == 0) return;

            var confirm = MessageBox.Show(
                $"B?n có ch?c ch?n mu?n xóa tuy?n {_currentRoute.RouteName} không?", 
                "Xác nh?n xóa", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _loadingOverlay?.Show("?ang xóa...");
                    await _catalogService.DeleteRouteAsync(_currentRoute.RouteID);
                    UiNotifier.SuccessToast("Xóa thành công!");
                    await LoadDataAsync();
                }
                catch (Exception ex)
                {
                    UiNotifier.ErrorToast($"L?i xóa d? li?u: {ex.Message}");
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
            _currentRoute = null;
        }
    }
}
