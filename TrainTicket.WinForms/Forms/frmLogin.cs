// ============================================================
// FILE: frmLogin.cs — NÂNG CẤP TOÀN DIỆN
// Cải tiến:
//   - Hiển thị lỗi rõ ràng (lockout, disabled, wrong pass)
//   - Checkbox "Ghi nhớ đăng nhập"
//   - Fade-in animation mượt hơn
//   - Chọn region có icon
// ============================================================
using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.ADO;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public class frmLogin : Form
    {
        private readonly IAuthService _authService;

        private readonly Guna2TextBox   _txtEmail      = new();
        private readonly Guna2TextBox   _txtPassword   = new();
        private readonly Guna2ComboBox  _cboRegion     = new();
        private readonly Guna2Button    _btnLogin      = new();
        private readonly Guna2Button    _btnTogglePwd  = new();   // [MỚI] show/hide pwd
        private readonly CheckBox       _chkRemember   = new();   // [MỚI]
        private readonly Label          _lblStatus     = new();
        private readonly Label          _lblVersion    = new();
        private readonly System.Windows.Forms.Timer _fadeTimer = new();

        public frmLogin(IAuthService authService)
        {
            _authService = authService;
            InitializeUi();
            Shown += (_, _) => StartFadeIn();
        }

        private void InitializeUi()
        {
            Text            = "Đăng nhập — TrainTicket";
            StartPosition   = FormStartPosition.CenterScreen;
            Width           = 500;
            Height          = 420;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox     = false;
            BackColor       = Color.FromArgb(15, 23, 42);
            Opacity         = 0;

            // Bo tròn cửa sổ
            var elipse = new Guna2Elipse { BorderRadius = 18, TargetControl = this };
            Controls.Add(new Panel { Width = 0, Height = 0, Tag = elipse });

            // Card trắng giữa màn hình
            var card = new Guna2ShadowPanel
            {
                Width      = 430,
                Height     = 350,
                FillColor  = Color.White,
                Radius     = 16,
                ShadowColor = Color.FromArgb(60, 0, 0, 0),
                ShadowDepth = 80,
                Location   = new Point(35, 30)
            };

            // Logo / tiêu đề
            var lblIcon = new Label
            {
                Text      = "🚂",
                Font      = new Font("Segoe UI Emoji", 26),
                Left = 24, Top = 18, AutoSize = true
            };

            var lblTitle = new Label
            {
                Text      = "TRAIN TICKET",
                Font      = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Left = 70, Top = 22, AutoSize = true
            };

            var lblSub = new Label
            {
                Text      = "Hệ thống quản lý đặt vé tàu hỏa",
                Font      = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 116, 139),
                Left = 70, Top = 56, Width = 300
            };

            // Divider
            var divider = new Panel
            {
                Left = 24, Top = 85, Width = 382, Height = 1,
                BackColor = Color.FromArgb(226, 232, 240)
            };

            // Email
            card.Controls.Add(new Label { Text = "Email", Left = 26, Top = 98, Width = 80,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105) });
            _txtEmail.Left = 26; _txtEmail.Top = 116; _txtEmail.Width = 378;
            _txtEmail.BorderRadius = 8;
            _txtEmail.PlaceholderText = "Nhập địa chỉ email";
            _txtEmail.Text = "admin@trainticket.vn";
            _txtEmail.Font = new Font("Segoe UI", 10);
            _txtEmail.FocusedState.BorderColor = UiTheme.Primary;

            // Password (có nút show/hide)
            card.Controls.Add(new Label { Text = "Mật khẩu", Left = 26, Top = 152, Width = 100,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105) });
            _txtPassword.Left = 26; _txtPassword.Top = 170; _txtPassword.Width = 340;
            _txtPassword.BorderRadius = 8;
            _txtPassword.PlaceholderText = "Nhập mật khẩu";
            _txtPassword.PasswordChar = '●';
            _txtPassword.Font = new Font("Segoe UI", 10);
            _txtPassword.FocusedState.BorderColor = UiTheme.Primary;
            _txtPassword.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(null!, null!); };

            _btnTogglePwd.Left = 370; _btnTogglePwd.Top = 170;
            _btnTogglePwd.Width = 34; _btnTogglePwd.Height = 36;
            _btnTogglePwd.BorderRadius = 8;
            _btnTogglePwd.FillColor = Color.FromArgb(241, 245, 249);
            _btnTogglePwd.Text = "👁";
            _btnTogglePwd.Font = new Font("Segoe UI Emoji", 10);
            _btnTogglePwd.Click += (_, _) =>
            {
                _txtPassword.PasswordChar = _txtPassword.PasswordChar == '\0' ? '●' : '\0';
            };

            // Region
            card.Controls.Add(new Label { Text = "Khu vực", Left = 26, Top = 216, Width = 100,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105) });
            _cboRegion.Left = 26; _cboRegion.Top = 234; _cboRegion.Width = 200;
            _cboRegion.BorderRadius = 8;
            _cboRegion.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboRegion.Items.AddRange(new object[]
            {
                "🏢 HQ — Tổng cục",
                "🔵 Bắc",
                "🟡 Trung",
                "🔴 Nam"
            });
            _cboRegion.SelectedIndex = 0;

            // Remember me
            _chkRemember.Left = 240; _chkRemember.Top = 238;
            _chkRemember.Width = 160; _chkRemember.Text = "Ghi nhớ đăng nhập";
            _chkRemember.Font = new Font("Segoe UI", 9);
            _chkRemember.ForeColor = Color.FromArgb(71, 85, 105);

            // Status label
            _lblStatus.Left = 26; _lblStatus.Top = 276;
            _lblStatus.Width = 378; _lblStatus.Height = 32;
            _lblStatus.Font = new Font("Segoe UI", 9);
            _lblStatus.ForeColor = Color.FromArgb(220, 38, 38);
            _lblStatus.Visible = false;

            // Login button
            _btnLogin.Left = 26; _btnLogin.Top = 300;
            _btnLogin.Width = 378; _btnLogin.Height = 40;
            _btnLogin.BorderRadius = 10;
            _btnLogin.FillColor = UiTheme.Primary;
            _btnLogin.HoverState.FillColor = UiTheme.PrimaryHover;
            _btnLogin.Text = "Đăng nhập";
            _btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            _btnLogin.ForeColor = Color.White;
            _btnLogin.Click += BtnLogin_Click;

            card.Controls.AddRange(new Control[]
            {
                lblIcon, lblTitle, lblSub, divider,
                _txtEmail, _txtPassword, _btnTogglePwd,
                _cboRegion, _chkRemember,
                _lblStatus, _btnLogin
            });

            // Version bottom-right
            _lblVersion.Text = "v2.0";
            _lblVersion.Font = new Font("Segoe UI", 8);
            _lblVersion.ForeColor = Color.FromArgb(100, 116, 139);
            _lblVersion.Left = 440; _lblVersion.Top = 390;
            _lblVersion.AutoSize = true;

            Controls.Add(card);
            Controls.Add(_lblVersion);

            // Kéo form
            card.MouseDown += (_, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(Handle, 0xA1, 0x2, 0); } };
        }

        private async void BtnLogin_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtEmail.Text) ||
                string.IsNullOrWhiteSpace(_txtPassword.Text))
            {
                ShowError("Vui lòng nhập email và mật khẩu.");
                return;
            }

            SetLoading(true);

            // Chọn connection string theo region
            var regionMap = new[] { "HQ", "North", "Central", "South" };
            var region    = regionMap[Math.Max(0, _cboRegion.SelectedIndex)];
            ConnectionHelper.CurrentConnectionString = region switch
            {
                "North"   => ConnectionHelper.NorthConnection,
                "Central" => ConnectionHelper.CentralConnection,
                "South"   => ConnectionHelper.SouthConnection,
                _         => ConnectionHelper.DefaultConnection
            };

            try
            {
                var request = new LoginRequestDto
                {
                    Email      = _txtEmail.Text.Trim(),
                    Password   = _txtPassword.Text,
                    RememberMe = _chkRemember.Checked,
                    Region     = region
                };
                var session = await _authService.LoginAsync(request);

                if (session == null)
                {
                    ShowError("Email hoặc mật khẩu không đúng.");
                    _txtPassword.Clear();
                    _txtPassword.Focus();
                    return;
                }

                SessionManager.SetSession(session);
                SessionManager.CurrentRegion = region;

                // Mở frmMain qua DI
                var mainForm = Program.ServiceProvider
                    .CreateScope().ServiceProvider.GetRequiredService<frmMain>();

                // Fade out rồi chuyển form
                await FadeOutAsync();
                Hide();
                mainForm.FormClosed += (_, _) => Close();
                mainForm.Show();
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi kết nối: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void ShowError(string msg)
        {
            _lblStatus.Text    = "⚠ " + msg;
            _lblStatus.Visible = true;
        }

        private void SetLoading(bool loading)
        {
            _btnLogin.Text    = loading ? "Đang đăng nhập..." : "Đăng nhập";
            _btnLogin.Enabled = !loading;
        }

        private void StartFadeIn()
        {
            _fadeTimer.Interval = 20;
            _fadeTimer.Tick += (_, _) =>
            {
                if (Opacity >= 1) { _fadeTimer.Stop(); return; }
                Opacity += 0.05;
            };
            _fadeTimer.Start();
        }

        private async Task FadeOutAsync()
        {
            for (double o = 1; o >= 0; o -= 0.05)
            {
                Opacity = o;
                await Task.Delay(15);
            }
        }

        // P/Invoke để kéo form không border
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    }
}
