using System;
using System.Drawing;
using System.Windows.Forms;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;
using Guna.UI2.WinForms;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmRegister_New : Form
    {
        private readonly IAuthService _authService;

        public frmRegister_New(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private async void _btnRegister_Click(object sender, EventArgs e)
        {
            _lblStatus.Text = string.Empty;
            _btnRegister.Enabled = false;

            try
            {
                if (string.IsNullOrWhiteSpace(_txtFullName.Text) ||
                    string.IsNullOrWhiteSpace(_txtEmail.Text) ||
                    string.IsNullOrWhiteSpace(_txtPassword.Text))
                {
                    _lblStatus.Text = "⚠ Vui lòng điền đầy đủ thông tin.";
                    return;
                }

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
                    MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
                else
                {
                    _lblStatus.Text = "⚠ Email đã tồn tại.";
                }
            }
            catch (Exception ex)
            {
                _lblStatus.Text = $"Lỗi đăng ký: {ex.Message}";
            }
            finally
            {
                _btnRegister.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}