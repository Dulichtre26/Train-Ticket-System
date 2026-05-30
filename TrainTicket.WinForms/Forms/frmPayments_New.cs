using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmPayments_New : Form, IThemeableForm
    {
        private readonly int _ticketId;
        private readonly ITicketService _ticketService;
        private LoadingOverlay? _loadingOverlay;
        private bool _isInitialized = false; // Cờ ngăn chặn các sự kiện kích hoạt sớm trước khi Form Load xong

        public frmPayments_New(int ticketId, ITicketService ticketService)
        {
            InitializeComponent();
            _ticketId = ticketId;
            _ticketService = ticketService;

            // Thiết lập trạng thái hiển thị ban đầu cho Form
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;

            Text = $"Xác nhận thanh toán - Vé #{_ticketId}";
            _lblInfo.Text = $"Xác nhận thanh toán cho vé số {_ticketId}.";

            // Ép ảnh QR luôn co giãn tỉ lệ chuẩn, không bị vỡ hay mất góc cụm QR
            if (_picQrCode != null)
            {
                _picQrCode.SizeMode = PictureBoxSizeMode.Zoom;
            }

            // Đăng ký thủ công sự kiện Load của Form
            this.Load += frmPayments_New_Load;

            // Đăng ký sự kiện ComboBox an toàn
            _cboPaymentMethod.SelectedIndexChanged -= _cboPaymentMethod_SelectedIndexChanged;
            _cboPaymentMethod.SelectedIndexChanged += _cboPaymentMethod_SelectedIndexChanged;

            ApplyTheme();
        }

        private async void frmPayments_New_Load(object sender, EventArgs e)
        {
            // Triệt tiêu thuộc tính tự sụp Form của các nút bấm nếu lỡ bị cấu hình nhầm trong Designer
            _btnConfirm.DialogResult = DialogResult.None;
            if (btnClose != null) btnClose.DialogResult = DialogResult.None;

            // Khởi tạo lớp phủ Loading sau khi Handle đồ họa của Form đã sẵn sàng
            _loadingOverlay = new LoadingOverlay(this);

            // Gán dữ liệu mặc định an toàn cho ComboBox
            if (_cboPaymentMethod.Items.Count > 0 && _cboPaymentMethod.SelectedIndex < 0)
            {
                _cboPaymentMethod.SelectedIndex = 0;
            }

            // Bật cờ cho phép nạp QR
            _isInitialized = true;

            // Bắt đầu gọi nạp mã QR động từ mạng về
            await UpdateQrCodeAsync();
        }

        /// <summary>
        /// Hàm lấy số tiền từ DB và hiển thị ảnh QR chuẩn VietQR cho khách quét
        /// </summary>
        private async System.Threading.Tasks.Task UpdateQrCodeAsync()
        {
            if (!_isInitialized) return;

            string paymentMethod = _cboPaymentMethod.SelectedItem?.ToString() ?? "BankTransfer";

            // Xử lý nếu quay xe chọn tiền mặt
            if (paymentMethod == "Cash" || paymentMethod.Contains("Tiền mặt") || paymentMethod.Contains("Tien mat"))
            {
                _picQrCode.Image = null;
                _lblInfo.Text = $"Vé #{_ticketId}: Thanh toán bằng tiền mặt tại quầy ga.";
                return;
            }

            try
            {
                _loadingOverlay?.Show("Đang khởi tạo mã QR thanh toán...");

                // Lấy thông tin từ Database tầng Business
                var ticket = await _ticketService.GetTicketByIdAsync(_ticketId);
                if (ticket == null)
                {
                    UiNotifier.ErrorToast("Không tìm thấy thông tin vé trên hệ thống.");
                    return;
                }

                decimal soTien = ticket.GiaVe;
                string maVe = ticket.TicketCode;

                _lblInfo.Text = $"Vé #{_ticketId} ({maVe}) | Số tiền: {soTien:N0} VNĐ";

                // Cấu hình ngân hàng đích của nhà ga
                string nganHang = "970415"; // VietinBank
                string soTaiKhoan = "123456789";
                string tenTaiKhoan = Uri.EscapeDataString("CONG TY DUONG SAT TRAINTICKET");
                string noiDung = Uri.EscapeDataString($"THANHTOAN VE {maVe}");

                // Định dạng :F0 ép số tiền về dạng chuỗi số nguyên sạch (Ví dụ: 150000 chứ không phải 150,000.00)
                string urlVietQr = $"https://api.vietqr.io/image/{nganHang}-{soTaiKhoan}-compact.jpg?amount={soTien:F0}&addInfo={noiDung}&accountName={tenTaiKhoan}";

                // Nạp bất đồng bộ thông qua ImageLocation của WinForms giúp tránh đơ giao diện
                _picQrCode.ImageLocation = urlVietQr;
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi nạp mã QR: {ex.Message}");
            }
            finally
            {
                // Giữ vòng quay Loading thêm 200ms để đồ họa PictureBox kịp thời nạp xong luồng ảnh
                await System.Threading.Tasks.Task.Delay(200);
                _loadingOverlay?.Hide();
            }
        }

        private async void _cboPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            await UpdateQrCodeAsync();
        }

        private async void _btnConfirm_Click(object sender, EventArgs e)
        {
            _btnConfirm.Enabled = false;

            try
            {
                _loadingOverlay?.Show("Đang xác nhận thanh toán...");

                var transactionId = string.IsNullOrWhiteSpace(_txtTransactionId.Text)
                    ? $"ONLINE_PAY_{_ticketId}"
                    : _txtTransactionId.Text.Trim();

                // Cập nhật trạng thái sang Paid
                var success = await _ticketService.ConfirmPaymentAsync(_ticketId, transactionId);

                if (success)
                {
                    UiNotifier.SuccessToast("🎉 Thanh toán thành công! Vé đã được kích hoạt.");
                    this.DialogResult = DialogResult.OK; // Đặt kết quả thành công
                    this.Close(); // Chủ động đóng form
                }
                else
                {
                    UiNotifier.ErrorToast("Xác nhận thất bại. Vé không tồn tại hoặc đã được xử lý.");
                }
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi hệ thống: {ex.Message}");
            }
            finally
            {
                _loadingOverlay?.Hide();
                _btnConfirm.Enabled = true;
            }
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _lblInfo.ForeColor = UiTheme.TextPrimary;
            _lblInfo.BackColor = Color.Transparent;
            _btnConfirm.FillColor = UiTheme.Primary;
            _btnConfirm.HoverState.FillColor = UiTheme.PrimaryHover;
            _card.FillColor = UiTheme.Surface;

            if (lblPaymentMethod != null) lblPaymentMethod.ForeColor = UiTheme.TextSecondary;
            if (lblTransactionId != null) lblTransactionId.ForeColor = UiTheme.TextSecondary;
            if (lblQrCode != null) lblQrCode.ForeColor = UiTheme.TextSecondary;
            if (btnClose != null) btnClose.ForeColor = UiTheme.TextPrimary;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}