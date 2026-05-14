using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public class frmMain : Form
    {
        private readonly IScheduleService  _scheduleService;
        private readonly ITicketService    _ticketService;
        private readonly IReportService    _reportService;
        private readonly ICatalogService   _catalogService;
        private readonly INotificationService _notiService;   // [MỚI]
        private readonly TrainTicketDbContext  _dbContext;

        // Layout panels
        private readonly Guna2Panel _sidebar  = new();
        private readonly Guna2Panel _header   = new();
        private readonly Guna2Panel _content  = new();

        // Nav buttons
        private readonly Guna2Button _btnSearch    = new();
        private readonly Guna2Button _btnTickets   = new();
        private readonly Guna2Button _btnPayments  = new();
        private readonly Guna2Button _btnReports   = new();
        private readonly Guna2Button _btnStations  = new();
        private readonly Guna2Button _btnRoutes    = new();
        private readonly Guna2Button _btnSchedules = new();
        private readonly Guna2Button _btnTrains    = new();
        private readonly Guna2Button _btnTheme     = new();
        private readonly Guna2Button _btnCollapse  = new();
        private readonly Guna2Button _btnLogout    = new();

        // Header elements
        private readonly Label  _lblWelcome    = new();
        private readonly Label  _lblBreadcrumb = new();   // [MỚI]
        private readonly Panel  _avatarCircle  = new();
        private readonly Label  _lblAvatarLetter = new();
        private readonly Label  _lblRegionBadge  = new();  // [MỚI]

        // Notification bell [MỚI]
        private readonly Guna2Button _btnNoti    = new();
        private readonly Label       _lblNotiBadge = new();
        private readonly System.Windows.Forms.Timer _notiTimer = new();

        // Sidebar brand
        private readonly Panel _brandPanel  = new();
        private readonly Label _lblBrandIcon = new();
        private readonly Label _lblBrandName = new();
        private readonly Label _lblBrandSub  = new();

        private bool _sidebarCollapsed = false;
        private const int SidebarExpanded  = 220;
        private const int SidebarCollapsed = 64;

        public frmMain(
            IScheduleService scheduleService, ITicketService ticketService,
            IReportService reportService, ICatalogService catalogService,
            INotificationService notiService, TrainTicketDbContext dbContext)
        {
            _scheduleService = scheduleService;
            _ticketService   = ticketService;
            _reportService   = reportService;
            _catalogService  = catalogService;
            _notiService     = notiService;
            _dbContext       = dbContext;

            InitializeUi();
            ApplyRolePermission();
            this.Resize  += (_, _) => LayoutPanels();
            this.Shown   += async (_, _) => await RefreshNotiBadgeAsync();
        }

        private void InitializeUi()
        {
            Text            = "TrainTicket v2.0";
            StartPosition   = FormStartPosition.CenterScreen;
            Width           = 1280;
            Height          = 780;
            MinimumSize     = new Size(960, 600);
            BackColor       = UiTheme.Background;
            FormBorderStyle = FormBorderStyle.Sizable;

            BuildSidebar();
            BuildHeader();
            BuildContent();
            LayoutPanels();

            Controls.Add(_content);
            Controls.Add(_header);
            Controls.Add(_sidebar);
        }

        private void BuildSidebar()
        {
            _sidebar.Width     = SidebarExpanded;
            _sidebar.Dock      = DockStyle.Left;
            _sidebar.FillColor = UiTheme.Sidebar;

            // Brand
            _brandPanel.Width     = SidebarExpanded;
            _brandPanel.Height    = 70;
            _brandPanel.BackColor = Color.Transparent;
            _brandPanel.Top       = 0;

            _lblBrandIcon.Text      = "🚂";
            _lblBrandIcon.Font      = new Font("Segoe UI Emoji", 20);
            _lblBrandIcon.ForeColor = Color.White;
            _lblBrandIcon.Left      = 16; _lblBrandIcon.Top = 14;
            _lblBrandIcon.AutoSize  = true;

            _lblBrandName.Text      = "TrainTicket";
            _lblBrandName.Font      = new Font("Segoe UI", 13, FontStyle.Bold);
            _lblBrandName.ForeColor = Color.White;
            _lblBrandName.Left      = 56; _lblBrandName.Top = 14;
            _lblBrandName.AutoSize  = true;

            _lblBrandSub.Text      = "v2.0";
            _lblBrandSub.Font      = new Font("Segoe UI", 8);
            _lblBrandSub.ForeColor = Color.FromArgb(148, 163, 184);
            _lblBrandSub.Left      = 58; _lblBrandSub.Top = 38;
            _lblBrandSub.AutoSize  = true;

            _brandPanel.Controls.AddRange(new Control[] { _lblBrandIcon, _lblBrandName, _lblBrandSub });

            // Nav buttons
            var navItems = new[] 
            {
                (_btnSearch,    "🔍", "Tìm chuyến tàu",  74),
                (_btnTickets,   "🎫", "Quản lý vé",      118),
                (_btnPayments,  "💳", "Thanh toán",       162),
                (_btnReports,   "📊", "Báo cáo",          206),
                (_btnStations,  "🏛", "Ga tàu",           280),
                (_btnRoutes,    "🗺", "Tuyến đường",      324),
                (_btnSchedules, "📅", "Lịch trình",       368),
                (_btnTrains,    "🚃", "Đoàn tàu",         412),
            };

            foreach (var (btn, icon, text, top) in navItems)
            {
                btn.Left         = 8;
                btn.Top          = top;
                btn.Width        = SidebarExpanded - 16;
                btn.Height       = 40;
                btn.BorderRadius = 10;
                btn.FillColor    = Color.Transparent;
                btn.HoverState.FillColor = Color.FromArgb(99, 102, 241, 40);
                btn.ForeColor    = UiTheme.NavText;
                btn.Font         = new Font("Segoe UI", 10);
                btn.Text         = $"  {icon}  {text}";
                btn.TextAlign    = HorizontalAlignment.Left;
                btn.Cursor       = Cursors.Hand;
            }

            // Collapse button
            _btnCollapse.Left = 8; _btnCollapse.Top = 500;
            _btnCollapse.Width = SidebarExpanded - 16; _btnCollapse.Height = 36;
            _btnCollapse.BorderRadius = 10;
            _btnCollapse.FillColor = Color.Transparent;
            _btnCollapse.ForeColor = UiTheme.NavText;
            _btnCollapse.Text = "  ◀  Thu gọn";
            _btnCollapse.Font = new Font("Segoe UI", 9);
            _btnCollapse.TextAlign = HorizontalAlignment.Left;
            _btnCollapse.Click += BtnCollapse_Click;

            // Theme toggle
            _btnTheme.Left = 8; _btnTheme.Top = 540;
            _btnTheme.Width = SidebarExpanded - 16; _btnTheme.Height = 36;
            _btnTheme.BorderRadius = 10;
            _btnTheme.FillColor = Color.Transparent;
            _btnTheme.ForeColor = UiTheme.NavText;
            _btnTheme.Text = UiTheme.IsDark ? "  🌞  Chế độ sáng" : "  🌙  Chế độ tối";
            _btnTheme.Font = new Font("Segoe UI", 9);
            _btnTheme.TextAlign = HorizontalAlignment.Left;
            _btnTheme.Click += (_, _) =>
            {
                UiTheme.Toggle();
                _btnTheme.Text = UiTheme.IsDark ? "  🌞  Chế độ sáng" : "  🌙  Chế độ tối";
                ApplyThemeToAll();
            };

            // Logout
            _btnLogout.Left = 8; _btnLogout.Top = 580;
            _btnLogout.Width = SidebarExpanded - 16; _btnLogout.Height = 36;
            _btnLogout.BorderRadius = 10;
            _btnLogout.FillColor = Color.Transparent;
            _btnLogout.ForeColor = Color.FromArgb(248, 113, 113);
            _btnLogout.Text = "  🚪  Đăng xuất";
            _btnLogout.Font = new Font("Segoe UI", 9);
            _btnLogout.TextAlign = HorizontalAlignment.Left;
            _btnLogout.Click += BtnLogout_Click;

            // Wire nav events
            _btnSearch.Click    += (_, _) => OpenChild(new frmSearch(_scheduleService, _ticketService, _dbContext), "Tìm chuyến tàu");
            _btnTickets.Click   += (_, _) => OpenChild(new frmTickets(_ticketService, _dbContext), "Quản lý vé");
            _btnPayments.Click  += (_, _) => MessageBox.Show("Chọn vé từ màn hình Quản lý vé để thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _btnReports.Click   += (_, _) => OpenChild(new frmReports(_reportService), "Báo cáo doanh thu");

            _btnStations.Click  += (_, _) => OpenChild(new frmStations(_catalogService), "Ga tàu");
            _btnRoutes.Click    += (_, _) => OpenChild(new frmRoutes(_catalogService), "Tuyến đường");
            _btnSchedules.Click += (_, _) => OpenChild(new frmSchedules(_catalogService), "Lịch trình");
            _btnTrains.Click    += (_, _) => OpenChild(new frmTrains(_catalogService), "Đoàn tàu");

            _sidebar.Controls.AddRange(new Control[] 
            {
                _brandPanel,
                _btnSearch, _btnTickets, _btnPayments, _btnReports,
                _btnStations, _btnRoutes, _btnSchedules, _btnTrains,
                _btnCollapse, _btnTheme, _btnLogout
            });
        }

        private void BuildHeader()
        {
            _header.Height    = 60;
            _header.Dock      = DockStyle.Top;
            _header.FillColor = UiTheme.Surface;
            _header.ShadowDecoration.Enabled = true;
            _header.ShadowDecoration.Depth   = 4;

            // Welcome
            _lblWelcome.Text      = $"Xin chào, {SessionManager.CurrentUser?.FullName ?? ""}";
            _lblWelcome.Font      = new Font("Segoe UI", 10, FontStyle.Bold);
            _lblWelcome.ForeColor = UiTheme.TextPrimary;
            _lblWelcome.Left      = 20; _lblWelcome.Top = 10;
            _lblWelcome.AutoSize  = true;

            // Breadcrumb [MỚI]
            _lblBreadcrumb.Text      = "Dashboard";
            _lblBreadcrumb.Font      = new Font("Segoe UI", 9);
            _lblBreadcrumb.ForeColor = UiTheme.TextSecondary;
            _lblBreadcrumb.Left      = 20; _lblBreadcrumb.Top = 32;
            _lblBreadcrumb.AutoSize  = true;

            // Region badge [MỚI]
            _lblRegionBadge.Text      = "🌐 " + (SessionManager.CurrentRegion ?? "HQ");
            _lblRegionBadge.Font      = new Font("Segoe UI", 8, FontStyle.Bold);
            _lblRegionBadge.ForeColor = Color.White;
            _lblRegionBadge.BackColor = UiTheme.Primary;
            _lblRegionBadge.Left      = 200; _lblRegionBadge.Top = 18;
            _lblRegionBadge.AutoSize  = false;
            _lblRegionBadge.Width     = 80; _lblRegionBadge.Height = 22;
            _lblRegionBadge.TextAlign = ContentAlignment.MiddleCenter;

            // Notification bell [MỚI]
            _btnNoti.Text         = "🔔";
            _btnNoti.Font         = new Font("Segoe UI Emoji", 14);
            _btnNoti.Width        = 44; _btnNoti.Height = 44;
            _btnNoti.BorderRadius = 22;
            _btnNoti.FillColor    = Color.Transparent;
            _btnNoti.Anchor       = AnchorStyles.Top | AnchorStyles.Right;
            _btnNoti.Click       += async (_, _) => await ShowNotificationsAsync();

            _lblNotiBadge.Width     = 18; _lblNotiBadge.Height = 18;
            _lblNotiBadge.BackColor = Color.FromArgb(239, 68, 68);
            _lblNotiBadge.ForeColor = Color.White;
            _lblNotiBadge.Font      = new Font("Segoe UI", 7, FontStyle.Bold);
            _lblNotiBadge.TextAlign = ContentAlignment.MiddleCenter;
            _lblNotiBadge.Visible   = false;
            _lblNotiBadge.Anchor    = AnchorStyles.Top | AnchorStyles.Right;

            // Avatar
            _avatarCircle.Width     = 38; _avatarCircle.Height = 38;
            _avatarCircle.BackColor = UiTheme.Primary;
            _avatarCircle.Anchor    = AnchorStyles.Top | AnchorStyles.Right;

            _lblAvatarLetter.Text      = SessionManager.CurrentUser?.AvatarLetter ?? "?";
            _lblAvatarLetter.Font      = new Font("Segoe UI", 14, FontStyle.Bold);
            _lblAvatarLetter.ForeColor = Color.White;
            _lblAvatarLetter.Dock      = DockStyle.Fill;
            _lblAvatarLetter.TextAlign = ContentAlignment.MiddleCenter;
            _avatarCircle.Controls.Add(_lblAvatarLetter);
            _avatarCircle.Paint += (s, e) =>
            {
                var p = (Panel)s!;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, p.Width - 1, p.Height - 1);
                e.Graphics.SetClip(path);
                e.Graphics.FillEllipse(new SolidBrush(UiTheme.Primary), 0, 0, p.Width - 1, p.Height - 1);
            };

            _header.Controls.AddRange(new Control[] 
            {
                _lblWelcome, _lblBreadcrumb, _lblRegionBadge,
                _btnNoti, _lblNotiBadge, _avatarCircle
            });

            // Định vị động khi resize
            _header.Resize += (_, _) =>
            {
                _avatarCircle.Left = _header.Width - 56;
                _avatarCircle.Top  = 11;
                _btnNoti.Left      = _header.Width - 110;
                _btnNoti.Top       = 8;
                _lblNotiBadge.Left = _header.Width - 90;
                _lblNotiBadge.Top  = 8;
            };

            // Notification polling mỗi 60 giây
            _notiTimer.Interval = 60_000;
            _notiTimer.Tick    += async (_, _) => await RefreshNotiBadgeAsync();
            _notiTimer.Start();
        }

        private void BuildContent()
        {
            _content.Dock       = DockStyle.Fill;
            _content.FillColor  = UiTheme.Background;
            _content.Padding    = new Padding(16);
            ShowDashboard();
        }

        private void ShowDashboard()
        {
            _content.Controls.Clear();
            var lbl = new Label
            {
                Text      = $"👋 Chào mừng, {SessionManager.CurrentUser?.FullName}!\n\nChọn chức năng từ menu bên trái để bắt đầu.",
                Font      = new Font("Segoe UI", 13),
                ForeColor = UiTheme.TextSecondary,
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            _content.Controls.Add(lbl);
            _lblBreadcrumb.Text = "Dashboard";
        }

        private void OpenChild(Form child, string title)
        {
            _content.Controls.Clear();
            child.TopLevel   = false;
            child.FormBorderStyle = FormBorderStyle.None;
            child.Dock       = DockStyle.Fill;
            _content.Controls.Add(child);
            child.Show();
            _lblBreadcrumb.Text = $"🏠 > {title}";
            HighlightActiveButton(title);
        }

        private void HighlightActiveButton(string title)
        {
            var all = new[] { _btnSearch, _btnTickets, _btnPayments,
                              _btnReports, _btnStations, _btnRoutes, _btnSchedules, _btnTrains };
            foreach (var b in all) b.FillColor = Color.Transparent;

            var hit = all.FirstOrDefault(b => b.Text.Contains(title.Split(' ')[0], StringComparison.OrdinalIgnoreCase));
            if (hit != null) hit.FillColor = Color.FromArgb(99, 102, 241, 50);
        }

        private void LayoutPanels()
        {
            _header.Width = Width - _sidebar.Width;
            _header.Left  = _sidebar.Width;
        }

        private async Task RefreshNotiBadgeAsync()
        {
            if (SessionManager.CurrentUser == null) return;
            try
            {
                var count = await _notiService.GetUnreadCountAsync(SessionManager.CurrentUser.UserID);
                _lblNotiBadge.Visible = count > 0;
                _lblNotiBadge.Text    = count > 9 ? "9+" : count.ToString();
            }
            catch { /* Không crash nếu lỗi noti */ }
        }

        private async Task ShowNotificationsAsync()
        {
            if (SessionManager.CurrentUser == null) return;
            var notis = await _notiService.GetUnreadAsync(SessionManager.CurrentUser.UserID);
            if (notis.Count == 0) { MessageBox.Show("Không có thông báo mới.", "Thông báo"); return; }

            var msg = string.Join("\n\n", notis.Take(5).Select(n => $"[{n.Type}] {n.Title}\n{n.Body}"));
            MessageBox.Show(msg, $"Thông báo ({notis.Count})", MessageBoxButtons.OK, MessageBoxIcon.Information);

            await _notiService.MarkAllReadAsync(SessionManager.CurrentUser.UserID);
            await RefreshNotiBadgeAsync();
        }

        private void BtnCollapse_Click(object? sender, EventArgs e)
        {
            _sidebarCollapsed = !_sidebarCollapsed;
            _sidebar.Width = _sidebarCollapsed ? SidebarCollapsed : SidebarExpanded;

            var btnList = new[] { _btnSearch, _btnTickets, _btnPayments, _btnReports,
                                  _btnStations, _btnRoutes, _btnSchedules, _btnTrains,
                                  _btnCollapse, _btnTheme, _btnLogout };
            foreach (var b in btnList)
            {
                b.Width = _sidebar.Width - 16;
                b.Text  = _sidebarCollapsed ? b.Text.Split(' ')[1] : b.Text; // chỉ giữ icon
            }

            _lblBrandName.Visible = !_sidebarCollapsed;
            _lblBrandSub.Visible  = !_sidebarCollapsed;
            _btnCollapse.Text     = _sidebarCollapsed ? "▶" : "  ◀  Thu gọn";
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            _notiTimer.Stop();
            SessionManager.Clear();
            Close();
        }

        private void ApplyRolePermission()
        {
            var isStaff = SessionManager.CurrentUser?.IsStaff ?? false;
            var isAdmin = SessionManager.CurrentUser?.IsAdmin ?? false;

            _btnReports.Visible   = isStaff;
            _btnStations.Visible  = isAdmin;
            _btnRoutes.Visible    = isAdmin;
            _btnSchedules.Visible = isAdmin;
            _btnTrains.Visible    = isAdmin;
        }

        private void ApplyThemeToAll()
        {
            BackColor            = UiTheme.Background;
            _sidebar.FillColor   = UiTheme.Sidebar;
            _header.FillColor    = UiTheme.Surface;
            _content.FillColor   = UiTheme.Background;
            _lblWelcome.ForeColor    = UiTheme.TextPrimary;
            _lblBreadcrumb.ForeColor = UiTheme.TextSecondary;
            UiTheme.Save();
        }
    }
}
