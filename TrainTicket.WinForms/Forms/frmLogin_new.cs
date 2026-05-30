using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.ADO;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmLogin_new : Form
    {
        private readonly IAuthService _authService;
        private System.Windows.Forms.Timer _fadeTimer = new();

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        public frmLogin_new(IAuthService authService)
        {
            InitializeComponent();
            _authService = authService;

            this.Shown += Form_Shown;

            if (this.Controls.Find("cardShadow", true).Length > 0)
            {
                var card = this.Controls.Find("cardShadow", true)[0];
                card.MouseDown += CardShadow_MouseDown;
            }

            btnTogglePwd.Click += BtnTogglePwd_Click;
            btnLogin.Click += BtnLogin_Click;
            txtPassword.KeyDown += TxtPassword_KeyDown;

            if (this.Controls.Find("lnkRegister", true).Length > 0)
            {
                var lnk = (LinkLabel)this.Controls.Find("lnkRegister", true)[0];
                lnk.LinkClicked += LnkRegister_LinkClicked;
            }
        }

        // ==========================================
        //  CÁC HÀM XỬ LÝ GIAO DIỆN (UI EFFECTS)
        // ==========================================

        // Hiệu ứng mờ ảo hiện lên dần dần khi mở Form
        private void Form_Shown(object sender, EventArgs e)
        {
            this.Opacity = 0; // Bắt đầu từ trạng thái trong suốt
            _fadeTimer.Interval = 20; // Tốc độ chạy timer (ms)
            _fadeTimer.Tick += (s, ev) =>
            {
                // Nếu độ rõ nét đã đạt 100% (1) thì dừng Timer
                if (this.Opacity >= 1) { _fadeTimer.Stop(); return; }
                this.Opacity += 0.05; // Mỗi lần tăng độ rõ lên 5%
            };
            _fadeTimer.Start();
        }

        // Kéo Form bằng chuột
        private void CardShadow_MouseDown(object sender, MouseEventArgs e)
        {
            // Chỉ bắt sự kiện khi nhấn chuột trái
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0); // Lệnh gọi OS cho phép kéo Form
            }
        }

        // Tắt / Mở hiển thị mật khẩu
        private void BtnTogglePwd_Click(object sender, EventArgs e)
        {
            // Nếu đang là ký tự trống ('\0') thì đổi thành dấu chấm đen, ngược lại
            txtPassword.PasswordChar = txtPassword.PasswordChar == '\0' ? '●' : '\0';
        }

        // Bấm Enter ở ô Mật khẩu thì cũng gọi hàm Đăng Nhập
        private void TxtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnLogin_Click(sender, e);
            }
        }

        // Mở Form Đăng ký tài khoản dành cho Khách hàng
        private void LnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using var registerForm = new frmGuestRegister_New(_authService);
            registerForm.ShowDialog(this);
        }

        // Hiển thị dòng thông báo lỗi chữ đỏ trên màn hình
        private void ShowError(string msg)
        {
            lblStatus.Text = "⚠ " + msg;
            lblStatus.Visible = true;
        }

        // Chặn người dùng spam nút (Bấm nhiều lần liên tục) trong lúc đợi load mạng
        private void SetLoading(bool loading)
        {
            btnLogin.Text = loading ? "Đang đăng nhập..." : "ĐĂNG NHẬP";
            btnLogin.Enabled = !loading; // Khóa / Mở khóa nút bấm
        }

        // ==========================================
        //  CÁC HÀM XỬ LÝ NGHIỆP VỤ (LOGIC & DATABASE)
        // ==========================================

        // Hàm Đăng Nhập chính (Sử dụng Async/Await để không bị treo Form)
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            // 1. Lấy dữ liệu người dùng nhập
            var email = txtEmail.Text.Trim();
            var pwd = txtPassword.Text;

            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pwd))
            {
                ShowError("Vui lòng nhập Email và Mật khẩu.");
                return; // Dừng lại không chạy tiếp
            }

            // Đổi trạng thái Form sang đang chờ
            SetLoading(true);
            lblStatus.Text = string.Empty; // Xóa thông báo lỗi cũ

            // 2. Xác định chi nhánh Cơ sở dữ liệu (Database Region)
            var regionMap = new[] { "HQ", "North", "Central", "South" };
            int selectedIndex = cboRegion.SelectedIndex < 0 ? 0 : cboRegion.SelectedIndex;
            var region = regionMap[selectedIndex];

            // Thiết lập chuỗi kết nối vào đúng Server vùng mà người dùng chọn
            ConnectionHelper.CurrentConnectionString = region switch
            {
                "North" => ConnectionHelper.NorthConnection,
                "Central" => ConnectionHelper.CentralConnection,
                "South" => ConnectionHelper.SouthConnection,
                _ => ConnectionHelper.DefaultConnection
            };

            try 
            {
                // 3. Đóng gói dữ liệu (DTO - Data Transfer Object) để gửi xuống tầng Business
                var request = new LoginRequestDto
                {
                    Email = email,
                    Password = pwd,
                    RememberMe = chkRemember.Checked,
                    Region = region
                };

                // 4. Gọi Service kiểm tra CSDL ngầm (Bất đồng bộ)
                var session = await _authService.LoginAsync(request);

                // Nếu trả về null nghĩa là sai mật khẩu / tài khoản không tồn tại
                if (session == null)
                {
                    ShowError("Email hoặc mật khẩu không đúng.");
                    txtPassword.Clear();
                    txtPassword.Focus(); // Đưa con trỏ chuột về lại ô mật khẩu
                    return;
                }

                // 5. Nếu Đăng nhập THÀNH CÔNG -> Lưu thông tin phiên hoạt động toàn cục
                SessionManager.SetSession(session);
                SessionManager.CurrentRegion = region; 

                // 6. Mở màn hình chính (Main Form)
                var mainForm = Program.ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<frmMain_New>();

                await FadeOutAsync(); // Chạy hiệu ứng mờ dần để tắt Form
                this.Hide(); // Ẩn form đăng nhập

                // Khi Main Form bị đóng, ứng dụng sẽ tắt luôn form Đăng nhập để thoát hoàn toàn
                mainForm.FormClosed += (s, ev) => this.Close();
                mainForm.Show();
            } 
            catch (InvalidOperationException ex)
            {
                // Bắt các lỗi nghiệp vụ (Vd: Tài khoản bị khóa, trùng lặp...)
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                // Bắt các lỗi hệ thống (Vd: Mất mạng, CSDL sập...)
                ShowError($"Lỗi kết nối: {ex.Message}");
            }
            finally
            {
                // Dù thành công hay thất bại, cuối cùng cũng phải mở khóa nút bấm lại
                SetLoading(false);
            }
        }

        // Hiệu ứng mờ dần rồi biến mất
        private async Task FadeOutAsync()
        {
            // Giảm độ trong suốt từ 1 (100%) về 0
            for (double o = 1; o >= 0; o -= 0.05)
            {
                this.Opacity = o;
                await Task.Delay(15); // Nghỉ 15ms giữa các lần vẽ để tạo cảm giác mượt mà
            }
        }
    }
}