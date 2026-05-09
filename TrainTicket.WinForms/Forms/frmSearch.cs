using Microsoft.EntityFrameworkCore;
using Guna.UI2.WinForms;
using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    // Form tìm chuyến tàu với bộ lọc và bảng kết quả phong cách Guna.
    public class frmSearch : Form, IThemeableForm
    {
        private readonly IScheduleService _scheduleService;
        private readonly ITicketService _ticketService;
        private readonly TrainTicketDbContext _dbContext;

        private readonly Guna2ComboBox _cboGaDi = new();
        private readonly Guna2ComboBox _cboGaDen = new();
        private readonly Guna2DateTimePicker _dtpNgayDi = new();
        private readonly Guna2Button _btnSearch = new();
        private readonly Guna2DataGridView _grid = new();
        private readonly Guna2Panel _filterPanel = new();
        private readonly Guna2Panel _contentPanel = new();
        private LoadingOverlay? _loadingOverlay;

        public frmSearch(
            IScheduleService scheduleService,
            ITicketService ticketService,
            TrainTicketDbContext dbContext)
        {
            _scheduleService = scheduleService;
            _ticketService = ticketService;
            _dbContext = dbContext;

            InitializeUi();
            Load += FrmSearch_Load;
        }

        private void InitializeUi()
        {
            Text = "T\u00ecm chuy\u1ebfn t\u00e0u";
            BackColor = UiTheme.Background;

            _filterPanel.Dock = DockStyle.Top;
            _filterPanel.Height = 88;
            _filterPanel.ShadowDecoration.Enabled = true;

            _contentPanel.Dock = DockStyle.Fill;
            _contentPanel.Padding = new Padding(16);

            var lblGaDi = new Label { Text = "Ga \u0111i", Left = 20, Top = 20, Width = 60 };
            _cboGaDi.Left = 80;
            _cboGaDi.Top = 15;
            _cboGaDi.Width = 220;
            _cboGaDi.BorderRadius = 8;
            _cboGaDi.DrawMode = DrawMode.OwnerDrawFixed;
            _cboGaDi.DropDownStyle = ComboBoxStyle.DropDownList;

            var lblGaDen = new Label { Text = "Ga \u0111\u1ebfn", Left = 320, Top = 20, Width = 60 };
            _cboGaDen.Left = 385;
            _cboGaDen.Top = 15;
            _cboGaDen.Width = 220;
            _cboGaDen.BorderRadius = 8;
            _cboGaDen.DrawMode = DrawMode.OwnerDrawFixed;
            _cboGaDen.DropDownStyle = ComboBoxStyle.DropDownList;

            var lblNgay = new Label { Text = "Ngày đi", Left = 625, Top = 20, Width = 60 };
            _dtpNgayDi.Left = 690;
            _dtpNgayDi.Top = 15;
            _dtpNgayDi.Width = 150;
            _dtpNgayDi.BorderRadius = 8;
            _dtpNgayDi.Format = DateTimePickerFormat.Short;

            _btnSearch.Text = "T\u00ecm";
            _btnSearch.Left = 860;
            _btnSearch.Top = 14;
            _btnSearch.Width = 90;
            _btnSearch.Height = 36;
            _btnSearch.BorderRadius = 8;
            _btnSearch.FillColor = Color.FromArgb(37, 99, 235);
            _btnSearch.HoverState.FillColor = Color.FromArgb(29, 78, 216);
            _btnSearch.Click += BtnSearch_Click;

            _grid.Dock = DockStyle.Fill;
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _grid.CellDoubleClick += Grid_CellDoubleClick;

            _filterPanel.Controls.Add(lblGaDi);
            _filterPanel.Controls.Add(_cboGaDi);
            _filterPanel.Controls.Add(lblGaDen);
            _filterPanel.Controls.Add(_cboGaDen);
            _filterPanel.Controls.Add(lblNgay);
            _filterPanel.Controls.Add(_dtpNgayDi);
            _filterPanel.Controls.Add(_btnSearch);

            _contentPanel.Controls.Add(_grid);

            Controls.Add(_contentPanel);
            Controls.Add(_filterPanel);

            _loadingOverlay = new LoadingOverlay(this);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _filterPanel.FillColor = UiTheme.Surface;
            _contentPanel.FillColor = UiTheme.Background;
            _btnSearch.FillColor = UiTheme.Primary;
            _btnSearch.HoverState.FillColor = UiTheme.PrimaryHover;
            _btnSearch.ForeColor = Color.White;

            // Adjust Datepicker
            _dtpNgayDi.FillColor = UiTheme.SurfaceVariant;
            _dtpNgayDi.ForeColor = UiTheme.TextPrimary;

            // Adjust Comboboxes
            _cboGaDi.FillColor = UiTheme.SurfaceVariant;
            _cboGaDi.ForeColor = UiTheme.TextPrimary;
            _cboGaDi.BackColor = UiTheme.Surface;

            _cboGaDen.FillColor = UiTheme.SurfaceVariant;
            _cboGaDen.ForeColor = UiTheme.TextPrimary;
            _cboGaDen.BackColor = UiTheme.Surface;

            // Adjust Grid
            _grid.BackgroundColor = UiTheme.Surface;
            _grid.GridColor = UiTheme.Divider;
            _grid.DefaultCellStyle.BackColor = UiTheme.SurfaceVariant;
            _grid.DefaultCellStyle.ForeColor = UiTheme.TextPrimary;
            _grid.DefaultCellStyle.SelectionBackColor = UiTheme.PrimaryLight;
            _grid.DefaultCellStyle.SelectionForeColor = Color.White;
            _grid.ColumnHeadersDefaultCellStyle.BackColor = UiTheme.Surface;
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = UiTheme.TextPrimary;
            _grid.RowHeadersDefaultCellStyle.BackColor = UiTheme.Surface;

            // Update Labels
            foreach (Control control in _filterPanel.Controls)
            {
                if (control is Label label)
                {
                    label.ForeColor = UiTheme.TextPrimary;
                    label.BackColor = Color.Transparent;
                }
            }
        }

        // Load danh mục ga để người dùng chọn điều kiện tìm kiếm.
        private async void FrmSearch_Load(object? sender, EventArgs e)
        {
            var stations = await _dbContext.Stations
                .Where(x => x.IsActive)
                .OrderBy(x => x.StationName)
                .Select(x => new { x.StationID, x.StationName })
                .ToListAsync();

            _cboGaDi.DataSource = stations.ToList();
            _cboGaDi.DisplayMember = "StationName";
            _cboGaDi.ValueMember = "StationID";

            _cboGaDen.DataSource = stations.ToList();
            _cboGaDen.DisplayMember = "StationName";
            _cboGaDen.ValueMember = "StationID";
        }

        // Thực hiện tìm kiếm chuyến và bind lên DataGridView.
        private async void BtnSearch_Click(object? sender, EventArgs e)
        {
            if (_cboGaDi.SelectedValue is not int gaDi || _cboGaDen.SelectedValue is not int gaDen)
            {
                UiNotifier.Info("Vui lòng chọn ga đi và ga đến.");
                return;
            }

            var request = new SearchScheduleDto
            {
                GaDi = gaDi,
                GaDen = gaDen,
                NgayDi = _dtpNgayDi.Value.Date
            };

            try
            {
                _loadingOverlay?.Show("Đang tìm chuyến tàu...");
                var table = await _scheduleService.SearchSchedulesAsync(request);
                _grid.DataSource = table;
                UiNotifier.InfoToast($"Tìm thấy {table.Rows.Count} chuyến phù hợp.");
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Không thể tìm chuyến tàu: {ex.Message}");
            }
            finally
            {
                _loadingOverlay?.Hide();
            }
        }

        // Mở sổ đồ ghế khi double-click vào một chuyến tàu.
        private void Grid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _grid.Rows[e.RowIndex].DataBoundItem == null)
            {
                return;
            }

            var scheduleIdObj = _grid.Rows[e.RowIndex].Cells["ScheduleID"].Value;
            if (scheduleIdObj == null || scheduleIdObj == DBNull.Value)
            {
                return;
            }

            var scheduleId = Convert.ToInt32(scheduleIdObj);
            using var seatMapForm = new frmSeatMap(scheduleId, _scheduleService, _ticketService);
            seatMapForm.ShowDialog(this);
        }
    }
}
