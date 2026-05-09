using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    // Form chính sau đăng nhập với layout sidebar hiện đại.
    public class frmMain : Form
    {
        private readonly IScheduleService _scheduleService;
        private readonly ITicketService _ticketService;
        private readonly IReportService _reportService;
        private readonly ICatalogService _catalogService;
        private readonly TrainTicketDbContext _dbContext;

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
        private readonly Guna2Button _btnTheme     = new();
        private readonly Guna2Button _btnTrains    = new();
        private readonly Guna2Button _btnCollapse  = new();

        // Header elements
        private readonly Label _lblWelcome = new();
        private readonly Panel _avatarCircle = new();
        private readonly Label _lblAvatarLetter = new();

        // Sidebar brand area
        private readonly Panel _brandPanel = new();
        private readonly Label _lblBrandIcon = new();
        private readonly Label _lblBrandName = new();
        private readonly Label _lblBrandSub  = new();

        private bool _sidebarCollapsed = false;

        public frmMain(
            IScheduleService scheduleService,
            ITicketService ticketService,
            IReportService reportService,
            ICatalogService catalogService,
            TrainTicketDbContext dbContext)
        {
            _scheduleService = scheduleService;
            _ticketService   = ticketService;
            _reportService   = reportService;
            _catalogService  = catalogService;
            _dbContext       = dbContext;

            InitializeUi();
            ApplyRolePermission();
            this.Resize += FrmMain_Resize;
        }

        private void InitializeUi()
        {
            Text            = "TrainTicket";
            StartPosition   = FormStartPosition.CenterScreen;
            Width           = 1200;
            Height          = 750;
            MinimumSize     = new Size(900, 600);
            BackColor       = UiTheme.Background;
            FormBorderStyle = FormBorderStyle.Sizable;

            BuildSidebar();
            BuildHeader();
            BuildContent();

            Controls.Add(_content);
            Controls.Add(_header);
            Controls.Add(_sidebar);
        }

        // ─── SIDEBAR ────────────────────────────────────────────────
        private void BuildSidebar()
        {
            _sidebar.Dock      = DockStyle.Left;
            _sidebar.Width     = 240;
            _sidebar.FillColor = UiTheme.Sidebar; // slate-800 trên light, slate-900 trên dark — luôn tối

            // Brand area — logo + tên app
            _brandPanel.Left        = 0;
            _brandPanel.Top         = 0;
            _brandPanel.Width       = 240;
            _brandPanel.Height      = 72;
            _brandPanel.BackColor   = Color.FromArgb(15, 23, 42); // slate-900 — đậm hơn 1 tông
            _brandPanel.Cursor      = Cursors.Default;

            _lblBrandIcon.Text      = "🚂";
            _lblBrandIcon.Font      = new Font("Segoe UI Emoji", 20);
            _lblBrandIcon.ForeColor = Color.White;
            _lblBrandIcon.Location  = new Point(16, 14);
            _lblBrandIcon.AutoSize  = true;
            _lblBrandIcon.BackColor = Color.Transparent;

            _lblBrandName.Text      = "TRAIN TICKET";
            _lblBrandName.Font      = new Font("Segoe UI", 11, FontStyle.Bold);
            _lblBrandName.ForeColor = Color.White;
            _lblBrandName.Location  = new Point(56, 16);
            _lblBrandName.AutoSize  = true;
            _lblBrandName.BackColor = Color.Transparent;

            _lblBrandSub.Text       = "Hệ thống đặt vé";
            _lblBrandSub.Font       = new Font("Segoe UI", 8);
            _lblBrandSub.ForeColor  = Color.FromArgb(148, 163, 184); // slate-400
            _lblBrandSub.Location   = new Point(56, 38);
            _lblBrandSub.AutoSize   = true;
            _lblBrandSub.BackColor  = Color.Transparent;

            _brandPanel.Controls.Add(_lblBrandIcon);
            _brandPanel.Controls.Add(_lblBrandName);
            _brandPanel.Controls.Add(_lblBrandSub);

            // Divider under brand
            var divider = new Panel
            {
                Left      = 16,
                Top       = 72,
                Width     = 208,
                Height    = 1,
                BackColor = Color.FromArgb(51, 65, 85) // slate-700
            };

            // Nav buttons — luôn dùng màu nav cố định vì sidebar luôn tối
            _btnSearch.Text    = "🔍  Tìm chuyến";
            _btnTickets.Text   = "🎫  Quản lý vé";
            _btnPayments.Text  = "💳  Thanh toán";
            _btnReports.Text   = "📊  Báo cáo";
            _btnTrains.Text    = "🚂  Quản lý tàu";
            _btnStations.Text  = "🏙️  Quản lý ga";
            _btnRoutes.Text    = "🛣️  Quản lý tuyến";
            _btnSchedules.Text = "⏰  Lịch trình";
            _btnTheme.Text     = "🎨  Giao diện";

            ConfigureNavButton(_btnSearch,    86,  OpenSearchForm);
            ConfigureNavButton(_btnTickets,   138, OpenTicketsForm);
            ConfigureNavButton(_btnPayments,  190, OpenPaymentsForm);
            ConfigureNavButton(_btnReports,   242, OpenReportsForm);
            ConfigureNavButton(_btnTrains,    294, OpenTrainsForm);
            ConfigureNavButton(_btnStations,  346, OpenStationsForm);
            ConfigureNavButton(_btnRoutes,    398, OpenRoutesForm);
            ConfigureNavButton(_btnSchedules, 450, OpenSchedulesForm);
            ConfigureNavButton(_btnTheme,     502, ToggleTheme);

            // Collapse button
            _btnCollapse.Text                = "◀";
            _btnCollapse.Size                = new Size(28, 28);
            _btnCollapse.Location            = new Point(_sidebar.Width - 38, 22);
            _btnCollapse.BorderRadius        = 14;
            _btnCollapse.FillColor           = Color.FromArgb(51, 65, 85);
            _btnCollapse.HoverState.FillColor= UiTheme.Primary;
            _btnCollapse.Font                = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnCollapse.ForeColor           = Color.White;
            _btnCollapse.Click              += BtnCollapse_Click;

            // Add all to sidebar
            _sidebar.Controls.Add(_brandPanel);
            _sidebar.Controls.Add(divider);
            _sidebar.Controls.Add(_btnSearch);
            _sidebar.Controls.Add(_btnTickets);
            _sidebar.Controls.Add(_btnPayments);
            _sidebar.Controls.Add(_btnReports);
            _sidebar.Controls.Add(_btnTrains);
            _sidebar.Controls.Add(_btnStations);
            _sidebar.Controls.Add(_btnRoutes);
            _sidebar.Controls.Add(_btnSchedules);
            _sidebar.Controls.Add(_btnTheme);
            _sidebar.Controls.Add(_btnCollapse);
        }

        private static void ConfigureNavButton(Guna2Button button, int top, Action onClick)
        {
            button.Left                  = 16;
            button.Top                   = top;
            button.Width                 = 208;
            button.Height                = 42;
            button.BorderRadius          = 10;
            button.FillColor             = UiTheme.NavButton;
            button.HoverState.FillColor  = UiTheme.NavButtonHover;
            button.Font                  = new Font("Segoe UI", 9, FontStyle.Bold);
            button.ForeColor             = UiTheme.NavText;
            button.HoverState.ForeColor  = Color.White;
            button.TextAlign             = HorizontalAlignment.Left;
            button.Click                += (_, _) => onClick();
        }

        // ─── HEADER ─────────────────────────────────────────────────
        private void BuildHeader()
        {
            _header.Dock                      = DockStyle.Top;
            _header.Height                    = 64;
            _header.FillColor                 = UiTheme.Surface;
            _header.ShadowDecoration.Enabled  = true;
            _header.ShadowDecoration.Color    = Color.FromArgb(30, 0, 0, 0);
            _header.ShadowDecoration.Depth    = 4;

            // Welcome text
            var roleName     = MapRoleName(SessionManager.CurrentRole);
            var userFullName = SessionManager.CurrentUser?.FullName ?? "User";
            var welcomeText  = $"Xin chào, {userFullName}";
            var roleText     = userFullName == roleName ? "" : $" — {roleName}";

            _lblWelcome.Text      = welcomeText;
            _lblWelcome.Font      = new Font("Segoe UI", 11, FontStyle.Bold);
            _lblWelcome.ForeColor = UiTheme.TextPrimary;
            _lblWelcome.AutoSize  = true;
            _lblWelcome.Left      = 24;
            _lblWelcome.Top       = 22;

            var lblRole = new Label
            {
                Text      = roleText,
                Font      = new Font("Segoe UI", 9),
                ForeColor = UiTheme.TextSecondary,
                AutoSize  = true,
                Left      = _lblWelcome.Left + TextRenderer.MeasureText(welcomeText, _lblWelcome.Font).Width,
                Top       = 25
            };

            // Avatar circle (góc phải header)
            _avatarCircle.Width     = 40;
            _avatarCircle.Height    = 40;
            _avatarCircle.BackColor = UiTheme.Primary;
            _avatarCircle.Cursor    = Cursors.Hand;

            _lblAvatarLetter.Text      = (SessionManager.CurrentUser?.FullName ?? "U").Substring(0, 1).ToUpper();
            _lblAvatarLetter.Font      = new Font("Segoe UI", 14, FontStyle.Bold);
            _lblAvatarLetter.ForeColor = Color.White;
            _lblAvatarLetter.AutoSize  = false;
            _lblAvatarLetter.TextAlign = ContentAlignment.MiddleCenter;
            _lblAvatarLetter.Dock      = DockStyle.Fill;
            _lblAvatarLetter.BackColor = Color.Transparent;

            _avatarCircle.Controls.Add(_lblAvatarLetter);
            _avatarCircle.Paint += (s, e) =>
            {
                // Vẽ hình tròn thay vì vuông
                using var path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(0, 0, _avatarCircle.Width - 1, _avatarCircle.Height - 1);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using var brush = new SolidBrush(UiTheme.Primary);
                e.Graphics.FillPath(brush, path);
            };

            // Anchor avatar về bên phải
            _header.Controls.Add(_lblWelcome);
            _header.Controls.Add(lblRole);
            _header.Controls.Add(_avatarCircle);

            _header.Resize += (_, _) =>
            {
                _avatarCircle.Left = _header.Width - 64;
                _avatarCircle.Top  = (_header.Height - 40) / 2;
            };
        }

        private static string MapRoleName(string? role) => role switch
        {
            "Admin"    => "Quản Trị Viên",
            "Staff"    => "Nhân Viên",
            "Manager"  => "Quản lý",
            "Customer" => "Khách Hàng",
            _          => role ?? ""
        };

        // ─── CONTENT ────────────────────────────────────────────────
        private void BuildContent()
        {
            _content.Dock      = DockStyle.Fill;
            _content.FillColor = UiTheme.Background;
        }

        // ─── THEME TOGGLE ────────────────────────────────────────────
        private void ToggleTheme()
        {
            UiTheme.Toggle();
            BackColor          = UiTheme.Background;
            _header.FillColor  = UiTheme.Surface;
            _content.FillColor = UiTheme.Background;
            _lblWelcome.ForeColor = UiTheme.TextPrimary;

            if (_content.Controls.OfType<IThemeableForm>().FirstOrDefault() is IThemeableForm t)
                t.ApplyTheme();
        }

        // ─── SIDEBAR COLLAPSE ────────────────────────────────────────
        private void BtnCollapse_Click(object? sender, EventArgs e) => ToggleSidebar();

        private void ToggleSidebar()
        {
            _sidebarCollapsed = !_sidebarCollapsed;

            if (_sidebarCollapsed)
            {
                _sidebar.Width          = 64;
                _btnCollapse.Text       = "▶";
                _btnCollapse.Location   = new Point(18, 22);
                HideButtonTexts();
            }
            else
            {
                _sidebar.Width          = 240;
                _btnCollapse.Text       = "◀";
                _btnCollapse.Location   = new Point(_sidebar.Width - 38, 22);
                ShowButtonTexts();
            }
        }

        private void FrmMain_Resize(object? sender, EventArgs e)
        {
            // WinForms Dock tự xử lý; chỉ cần cập nhật khi resize form con
            foreach (Control c in _content.Controls)
            {
                c.Width  = _content.Width;
                c.Height = _content.Height;
            }
        }

        // ─── ROLE PERMISSIONS ────────────────────────────────────────
        private void ApplyRolePermission()
        {
            var roles      = SessionManager.CurrentUser?.Roles ?? new List<string>();
            bool isAdmin   = roles.Contains("Admin");
            bool isStaff   = roles.Contains("Staff");
            bool isManager = roles.Contains("Manager");
            bool isCustomer= roles.Contains("User");

            _btnSearch.Visible    = isAdmin || isStaff || isCustomer;
            _btnTickets.Visible   = isAdmin || isStaff || isCustomer;
            _btnPayments.Visible  = isAdmin || isStaff || isCustomer;
            _btnReports.Visible   = isAdmin || isManager;
            _btnTrains.Visible    = isAdmin;
            _btnStations.Visible  = isAdmin;
            _btnRoutes.Visible    = isAdmin;
            _btnSchedules.Visible = isAdmin;
            _btnTheme.Visible     = true;

            // Sắp xếp lại vị trí Y cho các nút hiển thị
            int y   = 86;
            int gap = 52;
            foreach (var btn in new[] { _btnSearch, _btnTickets, _btnPayments, _btnReports,
                                        _btnTrains, _btnStations, _btnRoutes, _btnSchedules, _btnTheme })
            {
                if (btn.Visible) { btn.Top = y; y += gap; }
            }

            if (isCustomer) OpenSearchForm();
        }

        // ─── OPEN CHILD FORMS ────────────────────────────────────────
        private void OpenChildForm(Form form)
        {
            foreach (Control c in _content.Controls) c.Dispose();
            _content.Controls.Clear();

            form.TopLevel        = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock            = DockStyle.Fill;
            _content.Controls.Add(form);

            if (form is IThemeableForm t) t.ApplyTheme();
            form.Show();
        }

        private void OpenSearchForm()    => OpenChildForm(new frmSearch(_scheduleService, _ticketService, _dbContext));
        private void OpenTicketsForm()   => OpenChildForm(new frmTickets(_ticketService, _dbContext));
        private void OpenPaymentsForm()  => OpenChildForm(new frmTickets(_ticketService, _dbContext));
        private void OpenReportsForm()   => OpenChildForm(new frmReports(_reportService));
        private void OpenTrainsForm()    => OpenChildForm(new frmTrains(_catalogService));
        private void OpenStationsForm()  => OpenChildForm(new frmStations(_catalogService));
        private void OpenRoutesForm()    => OpenChildForm(new frmRoutes(_catalogService));
        private void OpenSchedulesForm() => OpenChildForm(new frmSchedules(
            Program.ServiceProvider.GetRequiredService<ICatalogService>()));

        // ─── COLLAPSED SIDEBAR TEXT ──────────────────────────────────
        private void HideButtonTexts()
        {
            _btnSearch.Text = "🔍"; _btnTickets.Text = "🎫"; _btnPayments.Text = "💳";
            _btnReports.Text = "📊"; _btnTrains.Text = "🚂"; _btnStations.Text = "🏙️";
            _btnRoutes.Text = "🛣️"; _btnSchedules.Text = "⏰"; _btnTheme.Text = "🎨";
            foreach (var btn in new[] { _btnSearch, _btnTickets, _btnPayments, _btnReports,
                                        _btnTrains, _btnStations, _btnRoutes, _btnSchedules, _btnTheme })
            {
                btn.Width       = 32;
                btn.Left        = 16;
                btn.TextAlign   = HorizontalAlignment.Center;
            }
            _brandPanel.Controls[0].Visible = true; // icon vẫn hiện
            _lblBrandName.Visible = false;
            _lblBrandSub.Visible  = false;
            _brandPanel.Width     = 64;
        }

        private void ShowButtonTexts()
        {
            _btnSearch.Text = "🔍  Tìm chuyến";    _btnTickets.Text = "🎫  Quản lý vé";
            _btnPayments.Text = "💳  Thanh toán";  _btnReports.Text = "📊  Báo cáo";
            _btnTrains.Text = "🚂  Quản lý tàu";   _btnStations.Text = "🏙️  Quản lý ga";
            _btnRoutes.Text = "🛣️  Quản lý tuyến"; _btnSchedules.Text = "⏰  Lịch trình";
            _btnTheme.Text = "🎨  Giao diện";
            foreach (var btn in new[] { _btnSearch, _btnTickets, _btnPayments, _btnReports,
                                        _btnTrains, _btnStations, _btnRoutes, _btnSchedules, _btnTheme })
            {
                btn.Width     = 208;
                btn.Left      = 16;
                btn.TextAlign = HorizontalAlignment.Left;
            }
            _lblBrandName.Visible = true;
            _lblBrandSub.Visible  = true;
            _brandPanel.Width     = 240;
        }
    }
}
