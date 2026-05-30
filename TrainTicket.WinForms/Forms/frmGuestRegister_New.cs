using System;
using System.Drawing;
using System.Windows.Forms;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmGuestRegister_New : Form
    {
        // 1. Khai báo Interface của Service tạo tài khoản
        private readonly IAuthService _authService;

        public frmGuestRegister_New(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            // Ép màu nút bấm theo Theme chuẩn của ứng dụng thay vì phải tự mò bằng tay
            btnRegister.FillColor = UiTheme.Primary;
            btnRegister.ForeColor = Color.White;

            // 2. Gắn kết sự kiện (Event) cho 2 nút
            btnRegister.Click += BtnRegister_Click;
            btnCancel.Click += (s, e) => this.Close(); // Bấm Hủy thì đóng form
        }

        // 3. Hàm xử lý logic khi người dùng bấm nút "Đăng Ký"
        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            // Kiểm tra: Không được để trống 3 ô bắt buộc
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                // Dùng thông báo Toast (kiểu nhỏ nhỏ bật lên) chuẩn của ứng dụng 
                UiNotifier.ErrorToast("Vui lòng nhập đầy đủ Tên, Email và Mật khẩu!");
                return;
            }

            // Đóng gói thông tin thu được từ Giao diện (Form) vào một thùng chứa DTO
            var request = new RegisterRequestDto
            {
                FullName = txtFullName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text,
                PhoneNumber = txtPhone.Text.Trim()
            };

            // Khóa nút đăng ký lại, đổi thành "Đang xử lý..." để người dùng không bấm liên tục nhiều lần
            btnRegister.Enabled = false;
            btnRegister.Text = "Đang xử lý...";

            try
            {
                // 4. BẮT ĐẦU GIAO TIẾP VỚI DATABASE (Chạy ngầm - Bất đồng bộ)
                var success = await _authService.RegisterAsync(request);

                if (success)
                {
                    // Thông báo dùng cửa sổ mặc định của Windows
                    MessageBox.Show("Tạo tài khoản thành công! Bạn có thể dùng email này để đăng nhập.",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Tạo xong rồi thì tự động tắt Form này đi
                    this.Close();
                }
                else
                {
                    UiNotifier.ErrorToast("Đăng ký thất bại. Email có thể đã tồn tại.");
                }
            }
            catch (Exception ex)
            {
                // Bắt lỗi hệ thống (ví dụ: đứt cáp mạng, server hỏng)
                UiNotifier.ErrorToast($"Lỗi: {ex.Message}");
            }
            finally
            {
                // Mở lại nút bấm khi chạy xong mọi thứ
                btnRegister.Enabled = true;
                btnRegister.Text = "Đăng ký";
            }
        }
    }
}