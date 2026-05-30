using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmMain_New : Form
    {
        private readonly IScheduleService _scheduleService;
        private readonly ITicketService _ticketService;
        private readonly IReportService _reportService;
        private readonly ICatalogService _catalogService;
        private readonly INotificationService _notiService;
        private readonly TrainTicketDbContext _dbContext;

        private Form? _activeForm;
        private bool _sidebarCollapsed = false;
        private readonly System.Windows.Forms.Timer _notiTimer = new();

        public frmMain_New(IScheduleService scheduleService, ITicketService ticketService,
            IReportService reportService, ICatalogService catalogService,
            INotificationService notiService, TrainTicketDbContext dbContext)
        {
            InitializeComponent();

            _scheduleService = scheduleService;
            _ticketService = ticketService;
            _reportService = reportService;
            _catalogService = catalogService;
            _notiService = notiService;
            _dbContext = dbContext;

            this.Load += FrmMain_New_Load;

            btnCollapse.Click += BtnCollapse_Click;
            btnTheme.Click += BtnTheme_Click;
            btnLogout.Click += BtnLogout_Click;
            btnNoti.Click += async (_, _) => await ShowNotificationsAsync();

            if (btnSearch != null) btnSearch.Click += (_, _) => OpenChildFormFromDI<frmSearch_new>("Tìm chuyến tàu");
            if (btnTickets != null) btnTickets.Click += (_, _) => OpenChildFormFromDI<frmTickets_New>("Quản lý vé");
            if (btnPayments != null) btnPayments.Click += (_, _) => OpenChildFormFromDI<frmPendingPayments_New>("Thanh toán");
            if (btnReports != null) btnReports.Click += (_, _) => OpenChildFormFromDI<frmReports_New>("Báo cáo");
            if (btnTrains != null) btnTrains.Click += (_, _) => OpenChildFormFromDI<frmTrains_New>("Tàu hỏa");
            if (btnStations != null) btnStations.Click += (_, _) => OpenChildFormFromDI<frmStations_New>("Nhà ga");
            if (btnRoutes != null) btnRoutes.Click += (_, _) => OpenChildFormFromDI<frmRoutes_New>("Tuyến đường");
            if (btnSchedules != null) btnSchedules.Click += (_, _) => OpenChildFormFromDI<frmSchedules_New>("Lịch trình");
            if (btnPayHistory != null) btnPayHistory.Click += (_, _) => OpenChildFormFromDI<frmPaymentHistory_New>("Lịch sử thanh toán");
            if (btnMyTickets != null) btnMyTickets.Click += (_, _) => OpenChildFormFromDI<frmCustomerDashboard_New>("Vé của tôi");
            if (btnProfile != null) btnProfile.Click += (_, _) => OpenChildFormFromDI<frmCustomerProfile_New>("Hồ sơ cá nhân");
            if (btnChat != null) btnChat.Click += (_, _) => OpenChildFormFromDI<frmChat_New>("Hỗ trợ trò chuyện");

            _notiTimer.Interval = 60_000;
            _notiTimer.Tick += async (_, _) => await RefreshNotiBadgeAsync();
        }

        private async void FrmMain_New_Load(object? sender, EventArgs e)
        {
            var user = SessionManager.CurrentUser;
            var region = SessionManager.CurrentRegion;

            // XÚC DỮ LIỆU LÊN HEADER
            lblWelcome.Text = $"👋 Xin chào, {(user != null ? user.FullName : "Khách")}!";
            lblAvatarLetter.Text = user != null && user.FullName.Length > 0 ? user.FullName.Substring(0, 1).ToUpper() : "?";
            lblRegionBadge.Text = $" {region} ";

            // Tô màu cờ Khu Vực (Vd: North Xanh lục, Central Cam...)
            lblRegionBadge.BackColor = region switch
            {
                "North" => Color.FromArgb(16, 185, 129),
                "Central" => Color.FromArgb(245, 158, 11),
                "South" => Color.FromArgb(59, 130, 246),
                _ => Color.FromArgb(156, 163, 175)
            };

            // LOGIC PHÂN QUYỀN (Che nút)
            bool isAdmin = user?.IsAdmin ?? false;
            bool isStaff = user?.IsStaff ?? false;
            bool isGuest = user?.IsCustomer ?? true;

            if(btnMyTickets != null) btnMyTickets.Visible = isGuest;
            if(btnProfile != null) btnProfile.Visible = isGuest;
            if(btnChat != null) btnChat.Visible = true; // Trả về hiển thị cho cả staff
            if(btnTickets != null) btnTickets.Visible = !isGuest;
            if(btnPayments != null) btnPayments.Visible = !isGuest;
            if(btnReports != null) btnReports.Visible = isStaff || isAdmin;
            if(btnPayHistory != null) btnPayHistory.Visible = isStaff || isAdmin;
            if(btnStations != null) btnStations.Visible = isAdmin;
            if(btnRoutes != null) btnRoutes.Visible = isAdmin;
            if(btnSchedules != null) btnSchedules.Visible = isAdmin;
            if(btnTrains != null) btnTrains.Visible = isAdmin;

            // Start up trơn tru!
            _notiTimer.Start();
            await RefreshNotiBadgeAsync();
            OpenChildFormFromDI<frmSearch_new>("Tìm chuyến tàu");
        }

        // HÀM TẠO ẢO ẢNH CHUYỂN TRANG VÀ BÔI MÀU NÚT ACTIVE
        private void OpenChildFormFromDI<T>(string title) where T : Form
        {
            var childForm = Program.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<T>();
            OpenChildForm(childForm, title);
        }

        private void OpenChildForm(Form childForm, string title)
        {
            if (_activeForm != null) _activeForm.Close();

            lblBreadcrumb.Text = $"🏠 > {title}";
            _activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(childForm);
            pnlContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            HighlightActiveButton(title);
        }

        private void HighlightActiveButton(string title)
        {
            var btnList = new[]
            {
                btnSearch, btnTickets, btnPayments, btnMyTickets, btnProfile, btnChat,
                btnReports, btnPayHistory, btnStations, btnRoutes, btnSchedules, btnTrains
            };

            foreach (var b in btnList)
            {
                if (b != null) b.FillColor = Color.Transparent;
            }

            var hit = Array.Find(btnList, b => b != null && b.Text.Contains(title.Split(' ')[0], StringComparison.OrdinalIgnoreCase));
            if (hit != null) hit.FillColor = Color.FromArgb(50, 99, 102, 241);
        }

        // HÀM HIỂN THỊ POPUP DANH SÁCH THÔNG BÁO
        private async Task ShowNotificationsAsync()
        {
            if (SessionManager.CurrentUser == null) return;
            try
            {
                var notis = await _notiService.GetUnreadAsync(SessionManager.CurrentUser.UserId);

                if (notis.Count == 0)
                {
                    UiNotifier.InfoToast("✅ Bạn không có thông báo chưa đọc.");
                    return;
                }

                // Hiển thị popup danh sách thông báo
                ShowNotiPopup(notis);

                // Đánh dấu tất cả đã đọc
                await _notiService.MarkAllReadAsync(SessionManager.CurrentUser.UserId);
                lblNotiBadge.Visible = false;
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi tải thông báo: {ex.Message}");
            }
        }

        private void ShowNotiPopup(List<NotificationDto> notis)
        {
            var popup = new Form
            {
                Text = $"🔔 Thông báo ({notis.Count} chưa đọc)",
                Size = new Size(460, 480),
                StartPosition = FormStartPosition.Manual,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                BackColor = UiTheme.Surface
            };

            // Đặt popup góc trên phải sát nút chuông
            var btnPos = btnNoti.PointToScreen(Point.Empty);
            popup.Location = new Point(btnPos.X - popup.Width + btnNoti.Width, btnPos.Y + btnNoti.Height + 4);

            var listBox = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = UiTheme.TextPrimary,
                BackColor = UiTheme.Surface,
                BorderStyle = BorderStyle.None,
                ItemHeight = 52,
                DrawMode = DrawMode.OwnerDrawFixed
            };

            foreach (var n in notis)
                listBox.Items.Add(n);

            listBox.DrawItem += (s, e) =>
            {
                if (e.Index < 0) return;
                var n = (NotificationDto)listBox.Items[e.Index];
                e.DrawBackground();

                // Màu nền xen kẽ
                var bgColor = (e.State & DrawItemState.Selected) != 0
                    ? Color.FromArgb(219, 234, 254)
                    : (e.Index % 2 == 0 ? UiTheme.Surface : Color.FromArgb(245, 247, 250));
                using var bgBrush = new SolidBrush(bgColor);
                e.Graphics.FillRectangle(bgBrush, e.Bounds);

                // Icon theo loại
                string icon = n.Type switch
                {
                    "Payment" => "💳",
                    "Ticket"  => "🎫",
                    "Cancel"  => "❌",
                    "System"  => "⚙️",
                    _         => "🔔"
                };

                using var titleFont = new Font("Segoe UI", 10F, FontStyle.Bold);
                using var bodyFont  = new Font("Segoe UI", 8.5F);
                using var timeFont  = new Font("Segoe UI", 7.5F, FontStyle.Italic);
                using var textBrush = new SolidBrush(UiTheme.TextPrimary);
                using var subBrush  = new SolidBrush(UiTheme.TextSecondary);

                int x = e.Bounds.X + 10;
                int y = e.Bounds.Y + 4;
                e.Graphics.DrawString($"{icon} {n.Title}", titleFont, textBrush, x, y);
                e.Graphics.DrawString(n.Body, bodyFont, subBrush, x + 4, y + 20);
                e.Graphics.DrawString(n.CreatedAt.ToString("HH:mm dd/MM/yyyy"), timeFont, subBrush, x + 4, y + 36);

                // Đường kẻ phân cách
                using var pen = new Pen(Color.FromArgb(220, 220, 220));
                e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            };

            // Mất focus thì tự đóng
            popup.Deactivate += (_, _) => popup.Close();
            popup.Controls.Add(listBox);
            popup.Show(this);
        }

        private async Task RefreshNotiBadgeAsync()
        {
            if (SessionManager.CurrentUser == null) return;
            try
            {
                int count = await _notiService.GetUnreadCountAsync(SessionManager.CurrentUser.UserId);
                lblNotiBadge.Text = count > 9 ? "9+" : count.ToString();
                lblNotiBadge.Visible = count > 0;
            }
            catch
            {
                lblNotiBadge.Visible = false;
            }
        }

        private void BtnTheme_Click(object? sender, EventArgs e)
        {
            UiTheme.Toggle();
            btnTheme.Text = UiTheme.IsDark ? "  ☀  Chế độ sáng" : "  🌙  Chế độ tối";
        }

        private void BtnCollapse_Click(object? sender, EventArgs e)
        {
            _sidebarCollapsed = !_sidebarCollapsed;
            pnlSidebar.Width = _sidebarCollapsed ? 64 : 220;
            pnlBrand.Visible = !_sidebarCollapsed; // Ẩn luôn cụm Logo
            btnCollapse.Text = _sidebarCollapsed ? "  ▶" : "  ◀  Thu gọn";

            // Xóa chữ khi kéo hẹp, giữ lại đúng Emoji
            foreach (Control ctrl in pnlSidebar.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2Button btn && btn.Name != "btnCollapse")
                {
                    if (_sidebarCollapsed && btn.Text.Length > 3) btn.Tag = btn.Text;
                    btn.Text = _sidebarCollapsed ? btn.Text.Substring(0, 3) : btn.Tag?.ToString();
                }
            }
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn đăng xuất ư?", "Đăng xuất", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SessionManager.Clear();
                this.Hide();
                var loginForm = Program.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<frmLogin_new>();
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
        }
    }
}