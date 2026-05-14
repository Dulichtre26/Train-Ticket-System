using Guna.UI2.WinForms;
using Guna.UI2.WinForms;
using TrainTicket.Business.Interfaces;
using TrainTicket.Business.DTOs;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public class frmTrains : Form, IThemeableForm
    {
        private readonly ICatalogService _catalogService;
        private TrainDto? _currentTrain;

        private readonly Guna2Panel _topPanel   = new();
        private readonly Guna2Panel _mainPanel  = new();
        private readonly Guna2Panel _rightPanel = new();
        private readonly Guna2DataGridView _grid = new();

        private readonly Label _lblTitle = new();
        private readonly Guna2Button _btnAdd = new();

        // Stats cards
        private readonly FlowLayoutPanel _statsPanel       = new();
        private readonly Guna2Panel      _cardTotal        = new();
        private readonly Label           _lblTotalCount    = new();
        private readonly Guna2Panel      _cardActive       = new();
        private readonly Label           _lblActiveCount   = new();
        private readonly Guna2Panel      _cardMaintenance  = new();
        private readonly Label           _lblMaintenanceCount = new();

        // Search & filter
        private readonly Guna2Panel   _filterPanel = new();
        private readonly Guna2TextBox _txtSearch   = new();
        private readonly Guna2ComboBox _cboStatus  = new();

        // Form inputs (right panel)
        private readonly Label        _lblFormTitle  = new();
        private readonly Guna2TextBox _txtTrainCode  = new();
        private readonly Guna2TextBox _txtTrainName  = new();
        private readonly Guna2TextBox _txtTrainType  = new();
        private readonly Guna2Button  _btnSave       = new();
        private readonly Guna2Button  _btnDelete     = new();
        private readonly Guna2Button  _btnCancel     = new();

        private LoadingOverlay? _loadingOverlay;

        public frmTrains(ICatalogService catalogService)
        {
            _catalogService = catalogService;
            InitializeUi();
            Load += FrmTrains_Load;
        }

        private void InitializeUi()
        {
            Text      = "Qu?n lý tàu";
            Width     = 1100;
            Height    = 700;
            BackColor = UiTheme.Background;

            // ?? TOP PANEL ????????????????????????????????????????????
            _topPanel.Dock                   = DockStyle.Top;
            _topPanel.Height                 = 70;
            _topPanel.FillColor              = UiTheme.Surface;
            _topPanel.ShadowDecoration.Enabled = true;
            _topPanel.ShadowDecoration.Color = Color.FromArgb(20, 0, 0, 0);
            _topPanel.ShadowDecoration.Depth = 4;

            // Icon + Title
            var lblIcon = new Label
            {
                Text      = "??",
                Font      = new Font("Segoe UI Emoji", 18),
                Location  = new Point(20, 18),
                AutoSize  = true,
                BackColor = Color.Transparent
            };

            _lblTitle.Text      = "Danh sách Tàu h?a";
            _lblTitle.Font      = new Font("Segoe UI", 14, FontStyle.Bold);
            _lblTitle.ForeColor = UiTheme.TextPrimary;
            _lblTitle.Location  = new Point(52, 22);
            _lblTitle.AutoSize  = true;
            _lblTitle.BackColor = Color.Transparent;

            _btnAdd.Text                 = "+ Thêm tàu m?i";
            _btnAdd.Font                 = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnAdd.Size                 = new Size(160, 40);
            _btnAdd.Location             = new Point(280, 15);
            _btnAdd.BorderRadius         = 8;
            _btnAdd.FillColor            = UiTheme.Success;
            _btnAdd.HoverState.FillColor = Color.FromArgb(22, 163, 74); // green-600
            _btnAdd.ForeColor            = Color.White;
            _btnAdd.Click               += (_, _) => PrepareAddMode();

            _topPanel.Controls.Add(lblIcon);
            _topPanel.Controls.Add(_lblTitle);
            _topPanel.Controls.Add(_btnAdd);

            // ?? STATS + FILTER ROW ???????????????????????????????????
            var middleContainer = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 120,
                BackColor = UiTheme.Background,
                Padding   = new Padding(20, 12, 20, 8)
            };

            _statsPanel.Dock      = DockStyle.Top;
            _statsPanel.Height    = 68;
            _statsPanel.BackColor = Color.Transparent;

            CreateStatCard(_cardTotal,       "??  T?ng s? tàu",   _lblTotalCount,       0, UiTheme.Primary);
            CreateStatCard(_cardActive,      "?  ?ang ho?t ??ng", _lblActiveCount,      0, UiTheme.Success);
            CreateStatCard(_cardMaintenance, "??  ?ang b?o trì",  _lblMaintenanceCount, 0, UiTheme.Warning);

            _statsPanel.Controls.Add(_cardTotal);
            _statsPanel.Controls.Add(_cardActive);
            _statsPanel.Controls.Add(_cardMaintenance);

            // Filter row
            _filterPanel.Dock      = DockStyle.Top;
            _filterPanel.Height    = 44;
            _filterPanel.BackColor = Color.Transparent;

            _txtSearch.PlaceholderText = "??  Tìm theo mã tàu, tên tàu...";
            _txtSearch.Size            = new Size(260, 38);
            _txtSearch.Location        = new Point(0, 3);
            _txtSearch.BorderRadius    = 19;
            _txtSearch.BorderColor     = UiTheme.Border;
            _txtSearch.BorderThickness = 1;
            _txtSearch.FillColor       = UiTheme.SurfaceVariant;
            _txtSearch.ForeColor       = UiTheme.TextPrimary;

            _cboStatus.Items.AddRange(new object[] { "T?t c? tr?ng thái", "?ang ch?y", "B?o trì" });
            _cboStatus.SelectedIndex   = 0;
            _cboStatus.Size            = new Size(180, 38);
            _cboStatus.Location        = new Point(272, 3);
            _cboStatus.BorderRadius    = 19;
            _cboStatus.BorderColor     = UiTheme.Border;
            _cboStatus.BorderThickness = 1;
            _cboStatus.FillColor       = UiTheme.SurfaceVariant;
            _cboStatus.ForeColor       = UiTheme.TextPrimary;

            _filterPanel.Controls.Add(_txtSearch);
            _filterPanel.Controls.Add(_cboStatus);

            middleContainer.Controls.Add(_filterPanel);
            middleContainer.Controls.Add(_statsPanel);

            // ?? RIGHT PANEL (form thêm/s?a) ?????????????????????????
            _rightPanel.Dock        = DockStyle.Right;
            _rightPanel.Width       = 340;
            _rightPanel.FillColor   = UiTheme.Surface;
            _rightPanel.Padding     = new Padding(24);
            _rightPanel.Visible     = false;
            _rightPanel.ShadowDecoration.Enabled = true;
            _rightPanel.ShadowDecoration.Depth   = 6;

            // Header c?a right panel
            var rightHeader = new Panel
            {
                Left      = 0,
                Top       = 0,
                Width     = 340,
                Height    = 56,
                BackColor = UiTheme.PrimaryDark
            };
            rightHeader.Paint += (s, e) =>
            {
                using var br = new System.Drawing.Drawing2D.LinearGradientBrush(
                    rightHeader.ClientRectangle,
                    UiTheme.HeaderGradientStart,
                    UiTheme.HeaderGradientEnd,
                    System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(br, rightHeader.ClientRectangle);
            };

            _lblFormTitle.Text      = "Thêm / S?a Tàu";
            _lblFormTitle.Font      = new Font("Segoe UI", 12, FontStyle.Bold);
            _lblFormTitle.ForeColor = Color.White;
            _lblFormTitle.Location  = new Point(16, 16);
            _lblFormTitle.AutoSize  = true;
            _lblFormTitle.BackColor = Color.Transparent;
            rightHeader.Controls.Add(_lblFormTitle);

            _rightPanel.Controls.Add(rightHeader);

            // Input groups
            int startY = 72, gap = 68;
            CreateInputGroup("Mã tàu",  _txtTrainCode, startY,         "Ví d?: SE1, SE2...");
            CreateInputGroup("Tên tàu", _txtTrainName, startY + gap,   "Ví d?: Tàu Th?ng Nh?t...");
            CreateInputGroup("Lo?i tàu",_txtTrainType, startY + gap*2, "Ví d?: Tàu khách, Tàu hàng...");

            int btnY = startY + gap * 3 + 10;

            _btnSave.Text                 = "??  L?u l?i";
            _btnSave.Size                 = new Size(110, 42);
            _btnSave.Location             = new Point(24, btnY);
            _btnSave.BorderRadius         = 8;
            _btnSave.FillColor            = UiTheme.Primary;
            _btnSave.HoverState.FillColor = UiTheme.PrimaryHover;
            _btnSave.ForeColor            = Color.White;
            _btnSave.Font                 = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnSave.Click               += BtnSave_Click;

            _btnDelete.Text                 = "??  Xóa";
            _btnDelete.Size                 = new Size(90, 42);
            _btnDelete.Location             = new Point(144, btnY);
            _btnDelete.BorderRadius         = 8;
            _btnDelete.FillColor            = UiTheme.Error;
            _btnDelete.HoverState.FillColor = Color.FromArgb(220, 38, 38); // red-600
            _btnDelete.ForeColor            = Color.White;
            _btnDelete.Font                 = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnDelete.Click               += BtnDelete_Click;

            _btnCancel.Text                 = "?  H?y";
            _btnCancel.Size                 = new Size(76, 42);
            _btnCancel.Location             = new Point(244, btnY);
            _btnCancel.BorderRadius         = 8;
            _btnCancel.FillColor            = UiTheme.SurfaceVariant;
            _btnCancel.HoverState.FillColor = UiTheme.Border;
            _btnCancel.ForeColor            = UiTheme.TextSecondary;
            _btnCancel.Font                 = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnCancel.Click               += (_, _) => HideRightPanel();

            _rightPanel.Controls.Add(_btnSave);
            _rightPanel.Controls.Add(_btnDelete);
            _rightPanel.Controls.Add(_btnCancel);

            // ?? MAIN PANEL (Grid) ???????????????????????????????????????
            _mainPanel.Dock      = DockStyle.Fill;
            _mainPanel.Padding   = new Padding(20);
            _mainPanel.BackColor = Color.Transparent;

            _grid.Dock                     = DockStyle.Fill;
            _grid.ReadOnly                 = true;
            _grid.AllowUserToAddRows       = false;
            _grid.SelectionMode            = DataGridViewSelectionMode.FullRowSelect;
            _grid.AutoSizeColumnsMode      = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.CellDoubleClick         += Grid_CellDoubleClick;

            _mainPanel.Controls.Add(_grid);

            Controls.Add(_mainPanel);
            Controls.Add(_rightPanel);
            Controls.Add(middleContainer);
            Controls.Add(_topPanel);

            _loadingOverlay = new LoadingOverlay(this);
            ApplyTheme();
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

            _txtSearch.FillColor  = UiTheme.SurfaceVariant;
            _txtSearch.ForeColor  = UiTheme.TextPrimary;
            _cboStatus.FillColor  = UiTheme.SurfaceVariant;
            _cboStatus.ForeColor  = UiTheme.TextPrimary;

            _lblTitle.ForeColor    = UiTheme.TextPrimary;

            _rightPanel.FillColor  = UiTheme.Surface;

            foreach (Control c in _rightPanel.Controls)
            {
                if (c is Label lbl && lbl != _lblFormTitle)
                    lbl.ForeColor = UiTheme.TextSecondary;
                if (c is Guna2TextBox txt)
                {
                    txt.FillColor   = UiTheme.SurfaceVariant;
                    txt.ForeColor   = UiTheme.TextPrimary;
                    txt.BorderColor = UiTheme.Border;
                }
            }
        }

        private async void FrmTrains_Load(object? sender, EventArgs e) => await LoadDataAsync();

        private async Task LoadDataAsync()
        {
            try
            {
                _loadingOverlay?.Show("?ang t?i d? li?u tàu...");
                var list = await _catalogService.GetAllTrainsAsync();

                _lblTotalCount.Text       = list.Count().ToString();
                _lblActiveCount.Text      = list.Count(t => t.TrainType != "B?o trì").ToString();
                _lblMaintenanceCount.Text = list.Count(t => t.TrainType == "B?o trì").ToString();

                var displayList = list.Select(t => new
                {
                    TrainID   = t.TrainID,
                    Ma_Tau   = t.TrainCode,
                    Ten_Tau  = t.TrainName,
                    Loai_Tau = t.TrainType
                }).ToList();

                _grid.DataSource = displayList;
                if (_grid.Columns["TrainID"] != null)
                    _grid.Columns["TrainID"].Visible = false;

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
            _currentTrain = new TrainDto();
            _lblFormTitle.Text = "Thêm tàu m?i";
            _txtTrainCode.Clear();
            _txtTrainName.Clear();
            _txtTrainType.Clear();
            _btnDelete.Visible = false;
            ShowRightPanel();
        }

        private async void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var idObj = _grid.Rows[e.RowIndex].Cells["TrainID"].Value;
            if (idObj == null) return;

            try
            {
                var train = await _catalogService.GetTrainByIdAsync(Convert.ToInt32(idObj));
                if (train == null) return;

                _currentTrain      = train;
                _lblFormTitle.Text = "S?a thông tin tàu";
                _txtTrainCode.Text = train.TrainCode;
                _txtTrainName.Text = train.TrainName;
                _txtTrainType.Text = train.TrainType;
                _btnDelete.Visible = true;
                ShowRightPanel();
            }
            catch (Exception ex) { UiNotifier.ErrorToast($"L?i: {ex.Message}"); }
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtTrainCode.Text) || string.IsNullOrWhiteSpace(_txtTrainName.Text))
            {
                UiNotifier.ErrorToast("Vui lòng nh?p ?? Mã và Tên Tàu.");
                return;
            }

            _currentTrain            ??= new TrainDto();
            _currentTrain.TrainCode  = _txtTrainCode.Text.Trim();
            _currentTrain.TrainName  = _txtTrainName.Text.Trim();
            _currentTrain.TrainType  = _txtTrainType.Text.Trim();

            try
            {
                _loadingOverlay?.Show("?ang l?u...");
                await _catalogService.SaveTrainAsync(_currentTrain);
                UiNotifier.SuccessToast("L?u thành công!");
                await LoadDataAsync();
            }
            catch (Exception ex) { UiNotifier.ErrorToast($"L?i: {ex.Message}"); _loadingOverlay?.Hide(); }
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_currentTrain == null || _currentTrain.TrainID == 0) return;

            var confirm = MessageBox.Show(
                $"B?n có ch?c mu?n xóa tàu {_currentTrain.TrainCode}?",
                "Xác nh?n xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _loadingOverlay?.Show("?ang xóa...");
                    await _catalogService.DeleteTrainAsync(_currentTrain.TrainID);
                    UiNotifier.SuccessToast("Xóa thành công!");
                    await LoadDataAsync();
                }
                catch (Exception ex) { UiNotifier.ErrorToast($"L?i: {ex.Message}"); _loadingOverlay?.Hide(); }
            }
        }

        private void ShowRightPanel() => _rightPanel.Visible = true;
        private void HideRightPanel() { _rightPanel.Visible = false; _currentTrain = null; }

        private void CreateStatCard(Guna2Panel card, string title, Label numLabel, int x, Color color)
        {
            card.Size = new Size(200, 68);
            card.Margin = new Padding(0, 0, 16, 0);
            card.BorderRadius = 12;
            card.FillColor = UiTheme.Surface;
            card.CustomBorderColor = color;
            card.CustomBorderThickness = new Padding(0, 4, 0, 0);

            var lblCardTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = UiTheme.TextSecondary,
                Location = new Point(16, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            numLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            numLabel.ForeColor = UiTheme.TextPrimary;
            numLabel.Location = new Point(14, 30);
            numLabel.AutoSize = true;
            numLabel.BackColor = Color.Transparent;

            card.Controls.Add(lblCardTitle);
            card.Controls.Add(numLabel);
        }

        private void CreateInputGroup(string labelText, Guna2TextBox tb, int y, string placeholder)
        {
            var lbl = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = UiTheme.TextSecondary,
                Location = new Point(24, y),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            tb.PlaceholderText = placeholder;
            tb.Size = new Size(292, 40);
            tb.Location = new Point(24, y + 24);
            tb.BorderRadius = 8;
            tb.BorderColor = UiTheme.Border;
            tb.BorderThickness = 1;
            tb.FillColor = UiTheme.SurfaceVariant;
            tb.ForeColor = UiTheme.TextPrimary;
            tb.BackColor = Color.Transparent;

            _rightPanel.Controls.Add(lbl);
            _rightPanel.Controls.Add(tb);
        }
    }
}
