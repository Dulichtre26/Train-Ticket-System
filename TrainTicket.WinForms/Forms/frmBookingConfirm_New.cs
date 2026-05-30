using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmBookingConfirm_New : Form, IThemeableForm
    {
        private readonly int _scheduleId;
        private readonly SeatMapDto _seat;
        private readonly ITicketService _ticketService;

        private bool _isDragging;
        private Point _dragOffset;

        public frmBookingConfirm_New(int scheduleId, SeatMapDto seat, ITicketService ticketService)
        {
            InitializeComponent();
            _scheduleId = scheduleId;
            _seat = seat;
            _ticketService = ticketService;

            _lblHeaderSub.Text = $"Chuyến #{_scheduleId}";
            _txtPassengerName.Text = SessionManager.CurrentUser?.FullName ?? string.Empty;
            _cboPaymentMethod.SelectedIndex = 0;

            _lblChipSeat.Text = $"🪑 Ghế {_seat.MaToa}-{_seat.SoGhe}";
            _lblChipClass.Text = $"⭐ {_seat.LoaiGhe}";
            _lblChipPrice.Text = $"💰 {_seat.GiaVe:N0} đ";

            // Làm cho Form không viền có thể kéo di chuyển được bằng thanh Header
            _headerPanel.MouseDown += Header_MouseDown;
            _headerPanel.MouseMove += Header_MouseMove;
            _headerPanel.MouseUp += Header_MouseUp;

            ApplyTheme();
        }

        /// <summary>
        /// LUỒNG XỬ LÝ: BẤM XÁC NHẬN -> LƯU DB -> ẨN FORM ĐẶT VÉ -> HIỆN FORM CHUYỂN KHOẢN/QR
        /// </summary>
        private async void _btnConfirm_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra trạng thái đăng nhập hệ thống
            if (SessionManager.CurrentUser == null)
            {
                UiNotifier.Info("Phiên đăng nhập đã hết. Vui lòng đăng nhập lại.");
                return;
            }

            // 2. Kiểm tra ràng buộc nhập liệu thông tin hành khách
            if (string.IsNullOrWhiteSpace(_txtPassengerName.Text) ||
                string.IsNullOrWhiteSpace(_txtPassengerId.Text))
            {
                UiNotifier.Info("Vui lòng nhập đầy đủ họ tên và số CCCD để đặt vé.");
                return;
            }

            // Vô hiệu hóa nút để tránh khách hàng click nhiều lần gây trùng lặp dữ liệu
            _btnConfirm.Enabled = false;
            _btnConfirm.Text = "⏳  Đang xử lý...";

            try
            {
                // 3. Chuẩn hóa phương thức thanh toán dựa trên lựa chọn ComboBox
                var paymentRaw = _cboPaymentMethod.SelectedItem?.ToString() ?? "Cash";
                var paymentMethod = paymentRaw.Contains("Tiền mặt") ? "Cash"
                                  : paymentRaw.Contains("Chuyển khoản") ? "BankTransfer"
                                  : paymentRaw.Contains("MoMo") ? "MoMo"
                                  : "VNPay";

                var request = new BookTicketRequestDto
                {
                    UserID = SessionManager.CurrentUser.UserId,
                    ScheduleID = _scheduleId,
                    SeatID = _seat.SeatID,
                    PassengerName = _txtPassengerName.Text.Trim(),
                    PassengerID = _txtPassengerId.Text.Trim(),
                    PassengerPhone = _txtPassengerPhone.Text.Trim(),
                    SeatType = _seat.LoaiGhe,
                    PaymentMethod = paymentMethod
                };

                // 4. Thực thi gọi tầng nghiệp vụ để ghi nhận thông tin đặt vé vào SQL Server
                var result = await _ticketService.BookTicketAsync(request);

                if (result == null)
                {
                    UiNotifier.ErrorToast("Đặt vé không thành công. Hệ thống bận.");
                    _btnConfirm.Enabled = true;
                    _btnConfirm.Text = "✅  Xác nhận đặt vé";
                    return;
                }

                UiNotifier.SuccessToast($"🎉 Đặt chỗ thành công — Mã vé: {result.TicketCode}");

                // 5. PHÂN LUỒNG HIỂN THỊ SAU KHI BẤM XÁC NHẬN THÀNH CÔNG
                if (paymentMethod != "Cash")
                {
                    // ẨN LẬP TỨC form xác nhận đặt vé hiện tại theo đúng yêu cầu của bạn
                    this.Hide();

                    // BẬT FORM CHUYỂN KHOẢN (frmPayments_New) lên và truyền trực tiếp mã TicketID vừa sinh ra
                    // Việc dùng toán tử 'new' thủ công giúp loại bỏ hoàn toàn lỗi gãy DI Container (System.Int32)
                    using (var paymentForm = new frmPayments_New(result.TicketID, _ticketService))
                    {
                        // Hiển thị form thanh toán QR dưới dạng hộp thoại Modal đè lên trên tập trung tương tác
                        paymentForm.ShowDialog();
                    }
                }
                else
                {
                    // Nếu chọn thanh toán bằng Tiền mặt tại quầy ga
                    MessageBox.Show($"Bạn đã đặt giữ chỗ thành công.\nMã vé: {result.TicketCode}\nVui lòng đến quầy ga thanh toán tiền mặt trước giờ tàu chạy!",
                        "Thông báo giữ chỗ thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Cài đặt trạng thái hoàn tất và giải phóng bộ nhớ form đặt vé
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi hệ thống khi đặt vé: {ex.Message}");
                _btnConfirm.Enabled = true;
                _btnConfirm.Text = "✅  Xác nhận đặt vé";
            }
        }

        private void Header_MouseDown(object sender, MouseEventArgs e)
        {
            _isDragging = true;
            _dragOffset = e.Location;
        }

        private void Header_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;
            var pos = PointToScreen(e.Location);
            Location = new Point(pos.X - _dragOffset.X, pos.Y - _dragOffset.Y);
        }

        private void Header_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        private void _headerPanel_Paint(object sender, PaintEventArgs e)
        {
            using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                _headerPanel.ClientRectangle,
                UiTheme.HeaderGradientStart,
                UiTheme.HeaderGradientEnd,
                System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, _headerPanel.ClientRectangle);
        }

        public void ApplyTheme()
        {
            _card.FillColor = UiTheme.Surface;
            _btnConfirm.FillColor = UiTheme.Primary;
            _btnConfirm.HoverState.FillColor = UiTheme.PrimaryHover;
            _btnCancel.FillColor = UiTheme.SurfaceVariant;
            _btnCancel.ForeColor = UiTheme.TextSecondary;
            _btnCancel.HoverState.FillColor = UiTheme.Border;

            lblPassengerName.ForeColor = UiTheme.TextSecondary;
            lblPassengerName.BackColor = Color.Transparent;
            lblPassengerId.ForeColor = UiTheme.TextSecondary;
            lblPassengerId.BackColor = Color.Transparent;
            lblPassengerPhone.ForeColor = UiTheme.TextSecondary;
            lblPassengerPhone.BackColor = Color.Transparent;
            lblPayment.ForeColor = UiTheme.TextSecondary;
            lblPayment.BackColor = Color.Transparent;

            foreach (Control c in _card.Controls)
            {
                if (c is Guna2TextBox txt)
                {
                    txt.FillColor = UiTheme.SurfaceVariant;
                    txt.ForeColor = UiTheme.TextPrimary;
                    txt.BorderColor = UiTheme.Border;
                }
            }
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void _btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}