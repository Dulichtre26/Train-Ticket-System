using Microsoft.Extensions.DependencyInjection;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;
using Guna.UI2.WinForms;

namespace TrainTicket.WinForms.Forms
{
    public class frmRegister : Form
    {
        private readonly IAuthService _authService;
        private readonly Guna2TextBox _txtFullName = new();
        private readonly Guna2TextBox _txtEmail = new();
        private readonly Guna2TextBox _txtPassword = new();
        private readonly Guna2TextBox _txtPhone = new();
        private readonly Guna2Button _btnRegister = new();
        private readonly Label _lblStatus = new();

        public frmRegister(IAuthService authService)
        {
            _authService = authService;
            InitializeUi();
        }

        private void InitializeUi()
        {
            Text = "??ng ký";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 500;
            Height = 450;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.FromArgb(15, 23, 42);

            var elipse = new Guna2Elipse { BorderRadius = 16, TargetControl = this };
            Controls.Add(new Panel { Width = 0, Height = 0, Tag = elipse });

            var card = new Guna2ShadowPanel
            {
                Width = 420,
                Height = 380,
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
                Text = "??ng ký tài kho?n m?i",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(100, 116, 139),
                Left = 26,
                Top = 52,
                Width = 250
            };

            var lblFullName = new Label { Text = "H? tên", Left = 28, Top = 78, Width = 100 };
            _txtFullName.Left = 28; _txtFullName.Top = 98; _txtFullName.Width = 360; _txtFullName.BorderRadius = 8; _txtFullName.PlaceholderText = "Nh?p h? tên";

            var lblEmail = new Label { Text = "Email", Left = 28, Top = 132, Width = 100 };
            _txtEmail.Left = 28; _txtEmail.Top = 152; _txtEmail.Width = 360; _txtEmail.BorderRadius = 8; _txtEmail.PlaceholderText = "Nh?p email";

            var lblPassword = new Label { Text = "M?t kh?u", Left = 28, Top = 186, Width = 100 };
            _txtPassword.Left = 28; _txtPassword.Top = 206; _txtPassword.Width = 360; _txtPassword.BorderRadius = 8; _txtPassword.PlaceholderText = "Nh?p m?t kh?u"; _txtPassword.UseSystemPasswordChar = true;

            var lblPhone = new Label { Text = "S? ?i?n tho?i", Left = 28, Top = 240, Width = 100 };
            _txtPhone.Left = 28; _txtPhone.Top = 260; _txtPhone.Width = 360; _txtPhone.BorderRadius = 8; _txtPhone.PlaceholderText = "Nh?p s? ?i?n tho?i";

            _btnRegister.Text = "??ng ký"; _btnRegister.Left = 250; _btnRegister.Top = 310; _btnRegister.Width = 138; _btnRegister.Height = 38; _btnRegister.BorderRadius = 10; _btnRegister.FillColor = Color.FromArgb(37, 99, 235); _btnRegister.HoverState.FillColor = Color.FromArgb(29, 78, 216); _btnRegister.Click += BtnRegister_Click;

            _lblStatus.Left = 28; _lblStatus.Top = 350; _lblStatus.Width = 360; _lblStatus.ForeColor = Color.Firebrick;

            card.Controls.Add(lblTitle); card.Controls.Add(lblSubTitle); card.Controls.Add(lblFullName); card.Controls.Add(_txtFullName); card.Controls.Add(lblEmail); card.Controls.Add(_txtEmail); card.Controls.Add(lblPassword); card.Controls.Add(_txtPassword); card.Controls.Add(lblPhone); card.Controls.Add(_txtPhone); card.Controls.Add(_btnRegister); card.Controls.Add(_lblStatus);

            var btnClose = new Guna2ControlBox { FillColor = Color.Transparent, IconColor = Color.White, Anchor = AnchorStyles.Top | AnchorStyles.Right, Left = Width - 52, Top = 8 };
            Controls.Add(btnClose); Controls.Add(card);
        }

        private async void BtnRegister_Click(object? sender, EventArgs e)
        {
            _lblStatus.Text = string.Empty;
            _btnRegister.Enabled = false;

            try
            {
                var request = new RegisterRequestDto
                {
                    FullName = _txtFullName.Text.Trim(),
                    Email = _txtEmail.Text.Trim(),
                    Password = _txtPassword.Text,
                    PhoneNumber = _txtPhone.Text.Trim()
                };

                bool success = await _authService.RegisterAsync(request);
                if (success)
                {
                    MessageBox.Show("??ng ký thành công! Vui lòng ??ng nh?p.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    _lblStatus.Text = "Email ?ã t?n t?i.";
                }
            }
            catch (Exception ex)
            {
                _lblStatus.Text = $"L?i ??ng ký: {ex.Message}";
            }
            finally
            {
                _btnRegister.Enabled = true;
            }
        }
    }
}