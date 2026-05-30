using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Guna.UI2.WinForms;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmCustomerDashboard_New : Form, IThemeableForm
    {
        private readonly TrainTicketDbContext _dbContext;
        private readonly ITicketService _ticketService;

        public frmCustomerDashboard_New(TrainTicketDbContext dbContext, ITicketService ticketService)
        {
            InitializeComponent();
            _dbContext = dbContext;
            _ticketService = ticketService;

            // Setup Custom Drawings
            _banner.Paint += Banner_Paint;
            _gridPanel.Paint += PaintRoundedPanel;

            // Header titles
            _lblPageTitle.Text = $"Xin chào, {SessionManager.CurrentUser?.FullName} 👋";

            _gridTickets.CellFormatting += GridTickets_CellFormatting;
            _cardRow.Resize += CardRow_Resize;

            // Card custom paints
            _pnlCardTickets.Paint += (s, e) => PaintCardPath(s as Panel, e, Color.FromArgb(59, 130, 246));
            _pnlCardPending.Paint += (s, e) => PaintCardPath(s as Panel, e, Color.FromArgb(245, 158, 11));
            _pnlCardSpent.Paint += (s, e) => PaintCardPath(s as Panel, e, Color.FromArgb(16, 185, 129));
        }

        private void frmCustomerDashboard_New_Load(object sender, EventArgs e)
        {
            _ = LoadDataAsync();
            CardRow_Resize(this, EventArgs.Empty);
        }

        private void CardRow_Resize(object? sender, EventArgs e)
        {
            int totalPad = 16 * 4;
            int cardW = (_cardRow.Width - totalPad) / 3;

            var cards = new[] { _pnlCardTickets, _pnlCardPending, _pnlCardSpent };
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

        private void Banner_Paint(object? sender, PaintEventArgs e)
        {
            var p = (Panel)sender!;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var path = RoundedRect(p.ClientRectangle, 10);
            using var brush = new LinearGradientBrush(
                p.ClientRectangle,
                Color.FromArgb(99, 102, 241),
                Color.FromArgb(139, 92, 246),
                LinearGradientMode.Horizontal);
            e.Graphics.FillPath(brush, path);
        }

        private static void PaintRoundedPanel(object? sender, PaintEventArgs e)
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

        private async Task LoadDataAsync()
        {
            if (SessionManager.CurrentUser == null) return;
            try
            {
                var uid = SessionManager.CurrentUser.UserId;

                var total = await _dbContext.Tickets.CountAsync(t => t.UserId == uid);
                var pending = await _dbContext.Tickets.CountAsync(t => t.UserId == uid && t.Status == "Pending");
                var spent = await _dbContext.Tickets
                                  .Where(t => t.UserId == uid && (t.Status == "Paid" || t.Status == "Completed"))
                                  .SumAsync(t => (decimal?)t.FinalPrice) ?? 0;

                _lblTicketsVal.Text = total.ToString("N0");
                _lblPendingVal.Text = pending.ToString("N0");
                _lblSpentVal.Text = spent.ToString("N0") + " ₫";

                var tickets = await _dbContext.Tickets
                    .Include(t => t.Schedule).ThenInclude(s => s.Route)
                    .Include(t => t.Seat)
                    .Where(t => t.UserId == uid)
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(20)
                    .Select(t => new
                    {
                        TicketId = t.TicketId,
                        TicketCode = t.TicketCode,
                        Route = t.Schedule.Route.RouteName,
                        DepartureTime = t.Schedule.DepartureTime,
                        Seat = t.Seat.SeatNumber,
                        Price = t.FinalPrice,
                        Status = t.Status
                    })
                    .ToListAsync();

                _gridTickets.DataSource = tickets;

                if (_gridTickets.Columns["TicketId"] != null)
                {
                    _gridTickets.Columns["TicketId"].Visible = false;
                }

                if (_gridTickets.Columns.Contains("Price"))
                    _gridTickets.Columns["Price"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi tải dữ liệu: {ex.Message}");
            }
        }

        private void GridTickets_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (_gridTickets.Columns[e.ColumnIndex].Name != "Status") return;

            e.CellStyle.ForeColor = e.Value?.ToString() switch
            {
                "Paid" or "Completed" => Color.FromArgb(16, 185, 129),
                "Pending" => Color.FromArgb(245, 158, 11),
                "Cancelled" => Color.FromArgb(239, 68, 68),
                _ => Color.FromArgb(30, 41, 59)
            };
            e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        private async void _btnCancel_Click(object sender, EventArgs e)
        {
            if (_gridTickets.SelectedRows.Count == 0)
            {
                UiNotifier.ErrorToast("Vui lòng chọn vé cần hủy!");
                return;
            }

            var row = _gridTickets.SelectedRows[0];
            var ticketId = Convert.ToInt32(row.Cells["TicketId"].Value);
            var code = row.Cells["TicketCode"].Value?.ToString();
            var status = row.Cells["Status"].Value?.ToString();

            if (status == "Cancelled") { UiNotifier.ErrorToast("Vé này đã được hủy rồi."); return; }
            if (status == "Completed") { UiNotifier.ErrorToast("Không thể hủy vé đã sử dụng."); return; }

            var confirm = MessageBox.Show(
                $"Bạn chắc chắn muốn hủy vé {code}?\n\n⚠️ Chính sách hoàn tiền phụ thuộc vào thời gian còn lại đến giờ khởi hành.",
                "Xác nhận hủy vé",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            _btnCancel.Enabled = false;
            _btnCancel.Text = "Đang xử lý...";
            try
            {
                var result = await _ticketService.CancelTicketAsync(
                    ticketId,
                    SessionManager.CurrentUser!.UserId,
                    "Khách hàng tự hủy");

                if (result.Success)
                {
                    MessageBox.Show(
                        $"✅ Đã hủy vé thành công!\n\n💸 {result.Message}",
                        "Hủy vé thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    await LoadDataAsync();
                }
                else
                {
                    UiNotifier.ErrorToast($"Không thể hủy vé: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi: {ex.Message}");
            }
            finally
            {
                _btnCancel.Enabled = true;
                _btnCancel.Text = "❌ Hủy vé đã chọn";
            }
        }

        public void ApplyTheme() { /* light-only */ }

        private void _lblSpentVal_Click(object sender, EventArgs e)
        {

        }
    }
}