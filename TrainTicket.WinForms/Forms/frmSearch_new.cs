using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmSearch_new : Form, IThemeableForm
    {
        private readonly IScheduleService _scheduleService;
        private readonly ITicketService _ticketService;
        private readonly TrainTicketDbContext _dbContext;
        private LoadingOverlay? _loadingOverlay;

        public frmSearch_new(
            IScheduleService scheduleService,
            ITicketService ticketService,
            TrainTicketDbContext dbContext)
        {
            InitializeComponent();
            _scheduleService = scheduleService;
            _ticketService = ticketService;
            _dbContext = dbContext;

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

            _dtpNgayDi.FillColor = UiTheme.SurfaceVariant;
            _dtpNgayDi.ForeColor = UiTheme.TextPrimary;

            _cboGaDi.FillColor = UiTheme.SurfaceVariant;
            _cboGaDi.ForeColor = UiTheme.TextPrimary;
            _cboGaDi.BackColor = UiTheme.Surface;

            _cboGaDen.FillColor = UiTheme.SurfaceVariant;
            _cboGaDen.ForeColor = UiTheme.TextPrimary;
            _cboGaDen.BackColor = UiTheme.Surface;

            _grid.BackgroundColor = UiTheme.Surface;
            _grid.GridColor = UiTheme.Divider;
            _grid.DefaultCellStyle.BackColor = UiTheme.SurfaceVariant;
            _grid.DefaultCellStyle.ForeColor = UiTheme.TextPrimary;
            _grid.DefaultCellStyle.SelectionBackColor = UiTheme.PrimaryLight;
            _grid.DefaultCellStyle.SelectionForeColor = Color.White;
            _grid.ColumnHeadersDefaultCellStyle.BackColor = UiTheme.Surface;
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = UiTheme.TextPrimary;

            lblGaDi.ForeColor = UiTheme.TextPrimary;
            lblGaDi.BackColor = Color.Transparent;
            lblGaDen.ForeColor = UiTheme.TextPrimary;
            lblGaDen.BackColor = Color.Transparent;
            lblNgay.ForeColor = UiTheme.TextPrimary;
            lblNgay.BackColor = Color.Transparent;
        }

        private async void frmSearch_new_Load(object sender, EventArgs e)
        {
            var stations = await _dbContext.Stations
                .Where(x => x.IsActive == true)
                .OrderBy(x => x.StationName)
                .Select(x => new { x.StationId, x.StationName })
                .ToListAsync();

            _cboGaDi.DataSource = stations.ToList();
            _cboGaDi.DisplayMember = "StationName";
            _cboGaDi.ValueMember = "StationId";

            _cboGaDen.DataSource = stations.ToList();
            _cboGaDen.DisplayMember = "StationName";
            _cboGaDen.ValueMember = "StationId";
        }

        private async void _btnSearch_Click(object sender, EventArgs e)
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

        private void _grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _grid.Rows[e.RowIndex].DataBoundItem == null) return;

            if (!_grid.Columns.Contains("ScheduleID")) return;
            var scheduleIdObj = _grid.Rows[e.RowIndex].Cells["ScheduleID"].Value;
            if (scheduleIdObj == null || scheduleIdObj == DBNull.Value) return;

            var scheduleId = Convert.ToInt32(scheduleIdObj);
            using var seatMapForm = new frmSeatMap_New(scheduleId, _scheduleService, _ticketService);
            seatMapForm.ShowDialog(this);
        }
    }
}
