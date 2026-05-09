using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    // Form ??ng nh?p kh?i ??ng c?a ?ng d?ng v?i giao di?n Guna hi?n ??i.
    public class frmLogin : Form
    {
        private readonly IAuthService _authService;
        private readonly Guna2TextBox _txtEmail = new();
        private readonly Guna2TextBox _txtPassword = new();
        private readonly Guna2ComboBox _cboRegion = new();
        private readonly Guna2Button _btnLogin = new();
        private readonly Label _lblStatus = new();
        private readonly System.Windows.Forms.Timer _fadeTimer = new();

        public frmLogin(IAuthService authService)
        {
            _authService = authService;
            InitializeUi();
            Shown += FrmLogin_Shown;
        }

        private void InitializeUi()
        {
            Text = "??ng nh?p";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 500;
            Height = 360;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            BackColor = Color.FromArgb(15, 23, 42);

            var elipse = new Guna2Elipse { BorderRadius = 16, TargetControl = this };
            Controls.Add(new Panel { Width = 0, Height = 0, Tag = elipse });

            var card = new Guna2ShadowPanel
            {
                Width = 420,
                Height = 290,
                FillColor = Color.White,
                Radius = 14,
                ShadowColor = Color.Black,
                ShadowDepth = 60,
                Location = new Point(40, 30)
            };

            var lblTitle = new Label
            {
                Text = "TRAIN TICKET",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 41, 59),
                Left = 24,
                Top = 16,
                Width = 260,
                AutoSize = true
            };

            var lblSubTitle = new Label
            {
                Text = "??ng nh?p ?? ti?p t?c",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 116, 139),
                Left = 26,
                Top = 52,
                Width = 250
            };

            var lblEmail = new Label
            {
                Text = "Email",
                Left = 28,
                Top = 88,
                Width = 100
            };

            _txtEmail.Left = 28;
            _txtEmail.Top = 108;
            _txtEmail.Width = 360;
            _txtEmail.BorderRadius = 8;
            _txtEmail.PlaceholderText = "Nh?p email";
            _txtEmail.Text = "admin@trainticket.vn";

            var lblPassword = new Label
            {
                Text = "M?t kh?u",
                Left = 28,
                Top = 142,
                Width = 100
            };

            _txtPassword.Left = 28;
            _txtPassword.Top = 162;
            _txtPassword.Width = 360;
            _txtPassword.BorderRadius = 8;
            _txtPassword.PlaceholderText = "Nh?p m?t kh?u";
            _txtPassword.UseSystemPasswordChar = true;
            _txtPassword.Text = "Admin@123";

            var lblRegion = new Label
            {
                Text = "Khu v?c (Site)",
                Left = 28,
                Top = 206,
                Width = 150
            };

            _cboRegion.Left = 28;
            _cboRegion.Top = 226;
            _cboRegion.Width = 360;
            _cboRegion.BorderRadius = 8;
            _cboRegion.Items.AddRange(new object[] { "Tr? s? chính (T?ng)", "Site Mi?n B?c", "Site Mi?n Trung", "Site Mi?n Nam" });
            _cboRegion.SelectedIndex = 0;

            _btnLogin.Text = "??ng nh?p";
            _btnLogin.Left = 250;
            _btnLogin.Top = 280;
            _btnLogin.Width = 138;
            _btnLogin.Height = 38;
            _btnLogin.BorderRadius = 10;
            _btnLogin.FillColor = Color.FromArgb(37, 99, 235);
            _btnLogin.HoverState.FillColor = Color.FromArgb(29, 78, 216);
            _btnLogin.Click += BtnLogin_Click;

            _lblStatus.Left = 28;
            _lblStatus.Top = 290;
            _lblStatus.Width = 210;
            _lblStatus.ForeColor = Color.Firebrick;

            card.Height = 340;
            Height = 410;

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblSubTitle);
            card.Controls.Add(lblEmail);
            card.Controls.Add(_txtEmail);
            card.Controls.Add(lblPassword);
            card.Controls.Add(_txtPassword);
            card.Controls.Add(lblRegion);
            card.Controls.Add(_cboRegion);
            card.Controls.Add(_btnLogin);
            card.Controls.Add(_lblStatus);

            var btnClose = new Guna2ControlBox
            {
                FillColor = Color.Transparent,
                IconColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Left = Width - 52,
                Top = 8
            };

            Controls.Add(btnClose);
            Controls.Add(card);

            _fadeTimer.Interval = 16;
            _fadeTimer.Tick += (_, _) =>
            {
                Opacity += 0.08;
                if (Opacity >= 1)
                {
                    Opacity = 1;
                    _fadeTimer.Stop();
                }
            };
        }

        private void FrmLogin_Shown(object? sender, EventArgs e)
        {
            Opacity = 0;
            _fadeTimer.Start();
        }

        // X? lý ??ng nh?p: xác th?c, l?u session, m? màn hình chính.
        private async void BtnLogin_Click(object? sender, EventArgs e)
        {
            _lblStatus.Text = string.Empty;
            _btnLogin.Enabled = false;

            // X? lý l?u Region thay vì Connection String
            switch (_cboRegion.SelectedIndex)
            {
                case 1: TrainTicket.WinForms.Helpers.SessionManager.CurrentRegion = TrainTicket.Data.Helpers.RegionHelper.North; break;
                case 2: TrainTicket.WinForms.Helpers.SessionManager.CurrentRegion = TrainTicket.Data.Helpers.RegionHelper.Central; break;
                case 3: TrainTicket.WinForms.Helpers.SessionManager.CurrentRegion = TrainTicket.Data.Helpers.RegionHelper.South; break;
                default: TrainTicket.WinForms.Helpers.SessionManager.CurrentRegion = TrainTicket.Data.Helpers.RegionHelper.HQ; break;
            }

            // Restore connection string v? Default vì chúng ta ?ang dùng Single Database + RegionCode
            TrainTicket.Data.ADO.ConnectionHelper.CurrentConnectionString = TrainTicket.Data.ADO.ConnectionHelper.DefaultConnection;

            try
            {
                var request = new LoginRequestDto
                {
                    Email = _txtEmail.Text.Trim(),
                    Password = _txtPassword.Text
                };

                var session = await _authService.LoginAsync(request);
                if (session == null)
                {
                    _lblStatus.Text = "Sai tài kho?n ho?c m?t kh?u.";
                    return;
                }

                SessionManager.SetSession(session);

                var mainForm = Program.ServiceProvider.GetRequiredService<frmMain>();
                Hide();
                mainForm.ShowDialog(this);
                Show();

                // Khi form chính ?óng thì quay v? tr?ng thái ch?a ??ng nh?p.
                SessionManager.Clear();
                _txtPassword.Clear();
            }
            catch (Exception ex)
            {
                _lblStatus.Text = $"L?i ??ng nh?p: {ex.Message}";
            }
            finally
            {
                _btnLogin.Enabled = true;
            }
        }
    }
}
