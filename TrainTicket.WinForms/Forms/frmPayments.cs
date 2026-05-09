using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;
using Guna.UI2.WinForms;
using QRCoder;
using System.Drawing;

namespace TrainTicket.WinForms.Forms
{
    // Form xác nhận thanh toán cho vé đang Pending. Gọi sp_XacNhanThanhToan để đổi trạng thái Pending -> Confirmed.
    public class frmPayments : Form, IThemeableForm
    {
        private readonly int _ticketId;
        private readonly ITicketService _ticketService;

        private readonly Label _lblInfo = new();
        private readonly Guna2ComboBox _cboPaymentMethod = new();
        private readonly Guna2TextBox _txtTransactionId = new();
        private readonly Guna2Button _btnConfirm = new();
        private readonly PictureBox _picQrCode = new();
        private LoadingOverlay? _loadingOverlay;

        public frmPayments(int ticketId, ITicketService ticketService)
        {
            _ticketId = ticketId;
            _ticketService = ticketService;

            InitializeUi();
        }

        private void InitializeUi()
        {
            Text = $"Xác nhận thanh toán - Vé #{_ticketId}";
            StartPosition = FormStartPosition.CenterParent;
            Width = 600;
            Height = 350;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            BackColor = UiTheme.Background;

            var card = new Guna2Panel
            {
                Left = 15,
                Top = 15,
                Width = 570,
                Height = 320,
                FillColor = UiTheme.Surface,
                BorderRadius = 12
            };

            _lblInfo.Text = $"Xác nhận thanh toán cho vé số {_ticketId}.";
            _lblInfo.Font = new Font("Arial", 10, FontStyle.Bold);
            _lblInfo.Left = 18;
            _lblInfo.Top = 18;
            _lblInfo.Width = 384;
            _lblInfo.Height = 30;
            card.Controls.Add(_lblInfo);

            // Left side (Form)
            card.Controls.Add(new Label { Text = "Phương thức thanh toán", Left = 20, Top = 65, Width = 150 });
            _cboPaymentMethod.Left = 20;
            _cboPaymentMethod.Top = 85;
            _cboPaymentMethod.Width = 300;
            _cboPaymentMethod.BorderRadius = 8;
            _cboPaymentMethod.DrawMode = DrawMode.OwnerDrawFixed;
            _cboPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            _cboPaymentMethod.Items.AddRange(new object[] { "Cash", "BankTransfer", "MoMo", "VNPay" });
            _cboPaymentMethod.SelectedIndex = 1;
            _cboPaymentMethod.SelectedIndexChanged += (_, _) => UpdateQrCode();
            card.Controls.Add(_cboPaymentMethod);

            card.Controls.Add(new Label { Text = "Mã giao dịch (nếu có)", Left = 20, Top = 135, Width = 150 });
            _txtTransactionId.Left = 20;
            _txtTransactionId.Top = 155;
            _txtTransactionId.Width = 300;
            _txtTransactionId.BorderRadius = 8;
            _txtTransactionId.PlaceholderText = "Nhập mã giao dịch từ cổng TT";
            card.Controls.Add(_txtTransactionId);

            _btnConfirm.Text = "Xác nhận thanh toán";
            _btnConfirm.Left = 160;
            _btnConfirm.Top = 230;
            _btnConfirm.Width = 160;
            _btnConfirm.Height = 40;
            _btnConfirm.BorderRadius = 10;
            _btnConfirm.FillColor = Color.FromArgb(37, 99, 235);
            _btnConfirm.Click += BtnConfirm_Click;
            card.Controls.Add(_btnConfirm);

            // Right side (QR Code)
            card.Controls.Add(new Label { Text = "Quét mã để thanh toán", Left = 360, Top = 65, Width = 180, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
            
            _picQrCode.Left = 360;
            _picQrCode.Top = 90;
            _picQrCode.Width = 160;
            _picQrCode.Height = 160;
            _picQrCode.SizeMode = PictureBoxSizeMode.Zoom;
            _picQrCode.BackColor = Color.White;
            card.Controls.Add(_picQrCode);

            var closeButton = new Guna2ControlBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FillColor = Color.Transparent,
                IconColor = Color.White,
                Left = 550,
                Top = 6
            };

            Controls.Add(closeButton);
            Controls.Add(card);

            _loadingOverlay = new LoadingOverlay(this);
            ApplyTheme();
            
            UpdateQrCode();
        }

        private void UpdateQrCode()
        {
            string paymentMethod = _cboPaymentMethod.SelectedItem?.ToString() ?? "BankTransfer";
            
            // Generate standard text or VietQR simulation
            string qrText = paymentMethod == "Cash" 
                ? "Thanh toán tiền mặt, không cần quét." 
                : $"VNPAY/MOMO/BANK\nVE: {_ticketId}\nGUI DEN: Cong Ty Duong Sat VN";

            if (paymentMethod != "Cash")
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new QRCode(qrData);
                _picQrCode.Image = qrCode.GetGraphic(5);
            }
            else
            {
                _picQrCode.Image = null; // No QR for Cash
            }
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _lblInfo.ForeColor = UiTheme.TextPrimary;
            _btnConfirm.FillColor = UiTheme.Primary;
            _btnConfirm.HoverState.FillColor = UiTheme.PrimaryHover;
        }

        private async void BtnConfirm_Click(object? sender, EventArgs e)
        {
            _btnConfirm.Enabled = false;

            try
            {
                _loadingOverlay?.Show("Đang xác nhận thanh toán...");
                var transactionId = string.IsNullOrWhiteSpace(_txtTransactionId.Text) ? null : _txtTransactionId.Text.Trim();
                var success = await _ticketService.ConfirmPaymentAsync(_ticketId, transactionId);
                if (success)
                {
                    UiNotifier.SuccessToast("Thanh toán thành công. Vé đã được xác nhận.");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    UiNotifier.ErrorToast("Thanh toán thất bại. Vé không tồn tại hoặc đã xác nhận.");
                }
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi thanh toán: {ex.Message}");
            }
            finally
            {
                _loadingOverlay?.Hide();
                _btnConfirm.Enabled = true;
            }
        }
    }
}