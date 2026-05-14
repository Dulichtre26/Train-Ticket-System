using Guna.UI2.WinForms;
using TrainTicket.Business.Interfaces;
using TrainTicket.Business.DTOs;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public class frmStations : Form, IThemeableForm
    {
        private readonly ICatalogService _catalogService;
        private StationDto? _currentStation;

        private readonly Guna2Panel _topPanel = new();
        private readonly Guna2Panel _mainPanel = new();
        private readonly Guna2Panel _rightPanel = new();
        private readonly Guna2DataGridView _grid = new();

        private readonly Label _lblTitle = new();
        private readonly Guna2Button _btnAdd = new();

        // Form inputs
        private readonly Label _lblFormTitle = new();
        private readonly Guna2TextBox _txtStationCode = new();
        private readonly Guna2TextBox _txtStationName = new();
        private readonly Guna2TextBox _txtCity = new();
        private readonly Guna2TextBox _txtAddress = new();
        private readonly Guna2Button _btnSave = new();
        private readonly Guna2Button _btnDelete = new();
        private readonly Guna2Button _btnCancel = new();

        private LoadingOverlay? _loadingOverlay;

        public frmStations(ICatalogService catalogService)
        {
            _catalogService = catalogService;
            InitializeUi();
            Load += FrmStations_Load;
        }

        private void InitializeUi()
        {
            Text = "Qu?n lý ga tàu";
            Width = 1200;
            Height = 700;
            BackColor = UiTheme.Background;

            // TOP PANEL
            _topPanel.Dock = DockStyle.Top;
            _topPanel.Height = 70;
            _topPanel.FillColor = UiTheme.Surface;
            _topPanel.ShadowDecoration.Enabled = true;

            _lblTitle.Text = "Danh sách Ga tàu";
            _lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            _lblTitle.ForeColor = UiTheme.TextPrimary;
            _lblTitle.Location = new Point(20, 20);
            _lblTitle.AutoSize = true;

            _btnAdd.Text = "+ Thêm ga m?i";
            _btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _btnAdd.Size = new Size(160, 40);
            _btnAdd.Location = new Point(250, 15);
            _btnAdd.BorderRadius = 8;
            _btnAdd.FillColor = Color.FromArgb(34, 197, 94); // Emerald 500
            _btnAdd.HoverState.FillColor = Color.FromArgb(22, 163, 74);
            _btnAdd.Click += (_, _) => PrepareAddMode();

            _topPanel.Controls.Add(_lblTitle);
            _topPanel.Controls.Add(_btnAdd);

            // GRID PANEL
            _mainPanel.Dock = DockStyle.Fill;
            _mainPanel.Padding = new Padding(20);
            _mainPanel.FillColor = Color.Transparent;

            _grid.Dock = DockStyle.Fill;
            _grid.AllowUserToAddRows = false;
            _grid.ReadOnly = true;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.CellDoubleClick += Grid_CellDoubleClick;

            _mainPanel.Controls.Add(_grid);

            // RIGHT PANEL (FORM)
            _rightPanel.Dock = DockStyle.Right;
            _rightPanel.Width = 380;
            _rightPanel.FillColor = UiTheme.Surface;
            _rightPanel.Padding = new Padding(20);
            _rightPanel.Visible = false; // Hide by default

            _lblFormTitle.Text = "Thêm/S?a Ga";
            _lblFormTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            _lblFormTitle.ForeColor = UiTheme.TextPrimary;
            _lblFormTitle.Location = new Point(20, 20);
            _lblFormTitle.AutoSize = true;

            // Define inputs
            int startY = 70;
            int gap = 70;

            CreateInputGroup("M\u00e3 ga", _txtStationCode, startY, "V\u00ED d\u1ee5: SGN, HAN...");
            CreateInputGroup("T\u00ean ga", _txtStationName, startY + gap, "V\u00ED d\u1ee5: Ga S\u00e0i G\u00f2n...");
            CreateInputGroup("Th\u00e0nh ph\u1ed1", _txtCity, startY + gap * 2, "V\u00ED d\u1ee5: H\u00f2 Ch\u00ed Minh...");
            CreateInputGroup("\u0110\u1ecba ch\u1ec9", _txtAddress, startY + gap * 3, "V\u00ED d\u1ee5: 01 Nguy\u1ec5n Th\u01b0\u1ee3ng Huy...");

            // Buttons
            _btnSave.Text = "L\u01b0u l\u1ea1i";
            _btnSave.Size = new Size(100, 40);
            _btnSave.Location = new Point(20, startY + gap * 4 + 20);
            _btnSave.BorderRadius = 8;
            _btnSave.FillColor = UiTheme.Primary;
            _btnSave.Click += BtnSave_Click;

            _btnDelete.Text = "X\u00f3a";
            _btnDelete.Size = new Size(80, 40);
            _btnDelete.Location = new Point(130, startY + gap * 4 + 20);
            _btnDelete.BorderRadius = 8;
            _btnDelete.FillColor = Color.FromArgb(239, 68, 68); // Red
            _btnDelete.Click += BtnDelete_Click;

            _btnCancel.Text = "H\u1ee7y";
            _btnCancel.Size = new Size(80, 40);
            _btnCancel.Location = new Point(220, startY + gap * 4 + 20);
            _btnCancel.BorderRadius = 8;
            _btnCancel.FillColor = Color.Gray;
            _btnCancel.Click += (_, _) => HideRightPanel();

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
            txt.Size = new Size(340, 36);
            txt.BorderRadius = 6;
            txt.PlaceholderText = placeholder;
            _rightPanel.Controls.Add(lbl);
            _rightPanel.Controls.Add(txt);
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _topPanel.FillColor = UiTheme.Surface;
            _rightPanel.FillColor = UiTheme.Surface;
            _lblTitle.ForeColor = UiTheme.TextPrimary;
            _lblFormTitle.ForeColor = UiTheme.TextPrimary;
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
                    lbl.ForeColor = UiTheme.TextSecondary;
                
                if (c is Guna2TextBox txt)
                {
                    txt.FillColor = UiTheme.Background;
                    txt.ForeColor = UiTheme.TextPrimary;
                    txt.BorderColor = UiTheme.Border;
                }

                if (c is Guna2Button btn && btn != _btnAdd)
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
                    else
                    {
                        btn.FillColor = UiTheme.SurfaceVariant;
                        btn.HoverState.FillColor = UiTheme.Border;
                    }
                }
            }
        }

        private async void FrmStations_Load(object? sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _loadingOverlay?.Show("?ang t?i d? li?u Ga...");
                var list = await _catalogService.GetAllStationsAsync();

                var displayList = list.Select(s => new { 
                    s.StationID, 
                    Ma_Ga = s.StationCode, 
                    Ten_Ga = s.StationName, 
                    Thanh_pho = s.City,
                    Dia_chi = s.Address 
                }).ToList();

                _grid.DataSource = displayList;
                if (_grid.Columns["StationID"] != null)
                {
                    _grid.Columns["StationID"].Visible = false;
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
            _currentStation = new StationDto();
            _lblFormTitle.Text = "Thêm Ga m?i";
            _txtStationCode.Clear();
            _txtStationName.Clear();
            _txtCity.Clear();
            _txtAddress.Clear();
            _btnDelete.Visible = false;

            ShowRightPanel();
        }

        private async void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
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
                _lblFormTitle.Text = "S?a Ga";
                _txtStationCode.Text = station.StationCode;
                _txtStationName.Text = station.StationName;
                _txtCity.Text = station.City;
                _txtAddress.Text = station.Address ?? "";
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
            if (string.IsNullOrWhiteSpace(_txtStationCode.Text) || 
                string.IsNullOrWhiteSpace(_txtStationName.Text) ||
                string.IsNullOrWhiteSpace(_txtCity.Text))
            {
                UiNotifier.ErrorToast("Vui lòng nh?p ?? Mã ga, Tên ga và Thành ph?.");
                return;
            }

            if (_currentStation == null) _currentStation = new StationDto();
            _currentStation.StationCode = _txtStationCode.Text.Trim();
            _currentStation.StationName = _txtStationName.Text.Trim();
            _currentStation.City = _txtCity.Text.Trim();
            _currentStation.Address = string.IsNullOrWhiteSpace(_txtAddress.Text) ? null : _txtAddress.Text.Trim();

            try
            {
                _loadingOverlay?.Show("\u0110ang l\u01b0u...");
                await _catalogService.SaveStationAsync(_currentStation);
                UiNotifier.SuccessToast("L\u01b0u th\u00e0nh c\u00f4ng!");
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
            if (_currentStation == null || _currentStation.StationID == 0) return;

            var confirm = MessageBox.Show(
                $"B\u1ea1n c\u00f3 ch\u1eafc ch\u1eacn mu\u1ed1n x\u00f3a ga {_currentStation.StationName} kh\u00f4ng?", 
                "X\u00e1c nh\u1eadn x\u00f3a", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _loadingOverlay?.Show("\u0110ang x\u00f3a...");
                    await _catalogService.DeleteStationAsync(_currentStation.StationID);
                    UiNotifier.SuccessToast("X\u00f3a th\u00e0nh c\u00f4ng!");
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
            _currentStation = null;
        }
    }
}
