using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Guna.UI2.WinForms;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmDashboard_New : Form, IThemeableForm
    {
        private readonly TrainTicketDbContext _dbContext;

        public frmDashboard_New(TrainTicketDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;

            _header.Resize += Header_Resize;
            _cardRow.Resize += CardRow_Resize;

            _gridPanel.Paint += PaintRoundedWhitePanel;
            _pnlCardTickets.Paint += (s, e) => PaintCardPath(s as Panel, e, Color.FromArgb(59, 130, 246));
            _pnlCardUsers.Paint += (s, e) => PaintCardPath(s as Panel, e, Color.FromArgb(16, 185, 129));
            _pnlCardSchedules.Paint += (s, e) => PaintCardPath(s as Panel, e, Color.FromArgb(245, 158, 11));
            _pnlCardRevenue.Paint += (s, e) => PaintCardPath(s as Panel, e, Color.FromArgb(239, 68, 68));

            _gridRecent.CellFormatting += GridRecent_CellFormatting;
        }

        private void Header_Resize(object? sender, EventArgs e)
        {
            _lblBreadcrumb.Top = (_header.Height - _lblBreadcrumb.Height) / 2;
            _lblBreadcrumb.Left = _header.Width - _lblBreadcrumb.Width - 20;
        }

        private void CardRow_Resize(object? sender, EventArgs e)
        {
            int totalPad = 16 * 5;
            int cardW = (_cardRow.Width - totalPad) / 4;

            var cards = new[] { _pnlCardTickets, _pnlCardUsers, _pnlCardSchedules, _pnlCardRevenue };
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].Width = cardW;
                cards[i].Height = _cardRow.Height - 32;
                cards[i].Left = 16 + i * (cardW + 16);
                cards[i].Top = 0;
            }
        }

        private void PaintCardPath(Panel? p, PaintEventArgs e, Color bgColor)
        {
            if (p == null) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = RoundedRect(p.ClientRectangle, 12);
            e.Graphics.FillPath(new SolidBrush(bgColor), path);
        }

        private static void PaintRoundedWhitePanel(object? sender, PaintEventArgs e)
        {
            if (sender is not Panel p) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = RoundedRect(p.ClientRectangle, 10);
            e.Graphics.FillPath(Brushes.White, path);
            using var pen = new Pen(Color.FromArgb(229, 231, 235), 1);
            e.Graphics.DrawPath(pen, path);
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        private async void frmDashboard_New_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
            Header_Resize(this, EventArgs.Empty);
            CardRow_Resize(this, EventArgs.Empty);
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var users = await _dbContext.Users.CountAsync(u => u.IsDeleted == false);
                var schedules = await _dbContext.Schedules.CountAsync(s => s.IsActive == true);
                var tickets = await _dbContext.Tickets.CountAsync();
                var revenue = await _dbContext.Tickets
                                    .Where(t => t.Status == "Paid" || t.Status == "Completed")
                                    .SumAsync(t => (decimal?)t.FinalPrice) ?? 0;

                _lblUsersVal.Text = users.ToString("N0");
                _lblSchedulesVal.Text = schedules.ToString("N0");
                _lblTicketsVal.Text = tickets.ToString("N0");
                _lblRevenueVal.Text = revenue.ToString("N0") + " ₫";

                var recent = await _dbContext.Tickets
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(20)
                    .Select(t => new
                    {
                        MaVe = t.TicketCode,
                        HanhKhach = t.PassengerName,
                        SoDienThoai = t.PassengerPhone,
                        NgayDat = t.CreatedAt,
                        GiaVe = t.FinalPrice,
                        TrangThai = t.Status
                    })
                    .ToListAsync();

                _gridRecent.DataSource = recent;

                if (_gridRecent.Columns.Contains("GiaVe"))
                    _gridRecent.Columns["GiaVe"].DefaultCellStyle.Format = "N0";
                if (_gridRecent.Columns.Contains("MaVe"))
                    _gridRecent.Columns["MaVe"].HeaderText = "Mã vé";
                if (_gridRecent.Columns.Contains("HanhKhach"))
                    _gridRecent.Columns["HanhKhach"].HeaderText = "Hành khách";
                if (_gridRecent.Columns.Contains("SoDienThoai"))
                    _gridRecent.Columns["SoDienThoai"].HeaderText = "SĐT";
                if (_gridRecent.Columns.Contains("NgayDat"))
                    _gridRecent.Columns["NgayDat"].HeaderText = "Ngày đặt";
                if (_gridRecent.Columns.Contains("GiaVe"))
                    _gridRecent.Columns["GiaVe"].HeaderText = "Giá vé";
                if (_gridRecent.Columns.Contains("TrangThai"))
                    _gridRecent.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi tải Dashboard: {ex.Message}");
            }
        }

        private void GridRecent_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (_gridRecent.Columns[e.ColumnIndex].Name != "TrangThai") return;
            e.CellStyle.ForeColor = e.Value?.ToString() switch
            {
                "Paid" or "Completed" => Color.FromArgb(16, 185, 129),
                "Pending" => Color.FromArgb(245, 158, 11),
                "Cancelled" => Color.FromArgb(239, 68, 68),
                _ => Color.FromArgb(30, 41, 59)
            };
            e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _header.BackColor = UiTheme.Surface;
            _gridPanel.Refresh();
        }
    }
}