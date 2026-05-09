using TrainTicket.Business.DTOs;
using Guna.UI2.WinForms;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    // Form xác nhận thông tin hành khách — layout rõ ràng với header màu gradient.
    public class frmBookingConfirm : Form, IThemeableForm
    {
        private readonly int _scheduleId;
        private readonly SeatMapDto _seat;
        private readonly ITicketService _ticketService;

        // Header (gradient Indigo)
        private readonly Panel _headerPanel      = new();
        private readonly Label _lblHeaderTitle   = new();
        private readonly Label _lblHeaderSub     = new();

        // Body card
        private readonly Guna2Panel _card = new();

        // Info chips
        private readonly Guna2Panel _chipSeat    = new();
        private readonly Guna2Panel _chipClass   = new();
        private readonly Guna2Panel _chipPrice   = new();

        // Inputs
        private readonly Guna2TextBox   _txtPassengerName  = new();
        private readonly Guna2TextBox   _txtPassengerId    = new();
        private readonly Guna2TextBox   _txtPassengerPhone = new();
        private readonly Guna2ComboBox  _cboPaymentMethod  = new();
        private readonly Guna2Button    _btnConfirm        = new();
        private readonly Guna2Button    _btnCancel         = new();

        private readonly List<Label> _labels = new();

        private bool _isDragging;
        private Point _dragOffset;

        public frmBookingConfirm(int scheduleId, SeatMapDto seat, ITicketService ticketService)
        {
            _scheduleId    = scheduleId;
            _seat          = seat;
            _ticketService = ticketService;
            InitializeUi();
        }

        private void InitializeUi()
        {
            Text            = "Xác nhận đặt vé";
            StartPosition   = FormStartPosition.CenterParent;
            Width           = 540;
            Height          = 520;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox     = false;
            BackColor       = Color.FromArgb(30, 41, 59); // nền shadow effect

            // ── HEADER (gradient Indigo, kéo form) ──────────────────────
            _headerPanel.Left      = 0;
            _headerPanel.Top       = 0;
            _headerPanel.Width     = 540;
            _headerPanel.Height    = 110;
            _headerPanel.BackColor = UiTheme.PrimaryDark;
            _headerPanel.Cursor    = Cursors.SizeAll;

            // Gradient draw
            _headerPanel.Paint += (s, e) =>
            {
                using var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    _headerPanel.ClientRectangle,
                    UiTheme.HeaderGradientStart,
                    UiTheme.HeaderGradientEnd,
                    System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
                e.Graphics.FillRectangle(brush, _headerPanel.ClientRectangle);
            };

            // Icon + title
            var lblIcon = new Label
            {
                Text      = "🎫",
                Font      = new Font("Segoe UI Emoji", 24),
                ForeColor = Color.White,
                Location  = new Point(24, 22),
                AutoSize  = true,
                BackColor = Color.Transparent
            };

            _lblHeaderTitle.Text      = "Xác nhận đặt vé";
            _lblHeaderTitle.Font      = new Font("Segoe UI", 14, FontStyle.Bold);
            _lblHeaderTitle.ForeColor = Color.White;
            _lblHeaderTitle.Location  = new Point(70, 22);
            _lblHeaderTitle.AutoSize  = true;
            _lblHeaderTitle.BackColor = Color.Transparent;

            _lblHeaderSub.Text      = $"Chuyến #{_scheduleId}";
            _lblHeaderSub.Font      = new Font("Segoe UI", 9);
            _lblHeaderSub.ForeColor = Color.FromArgb(196, 181, 253); // indigo-200
            _lblHeaderSub.Location  = new Point(70, 52);
            _lblHeaderSub.AutoSize  = true;
            _lblHeaderSub.BackColor = Color.Transparent;

            // Close button (X)
            var btnClose = new Guna2Button
            {
                Text                  = "✕",
                Size                  = new Size(36, 36),
                Location              = new Point(492, 10),
                BorderRadius          = 18,
                FillColor             = Color.FromArgb(80, 255, 255, 255),
                Font                  = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor             = Color.White
            };
            btnClose.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnClose.Click += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };

            _headerPanel.Controls.AddRange(new Control[] { lblIcon, _lblHeaderTitle, _lblHeaderSub, btnClose });

            // Drag header để di chuyển form
            _headerPanel.MouseDown += (_, e) => { _isDragging = true; _dragOffset = e.Location; };
            _headerPanel.MouseMove += (_, e) =>
            {
                if (!_isDragging) return;
                var pos = PointToScreen(e.Location);
                Location = new Point(pos.X - _dragOffset.X, pos.Y - _dragOffset.Y);
            };
            _headerPanel.MouseUp += (_, _) => _isDragging = false;

            // ── INFO CHIPS (ghế, loại ghế, giá) ────────────────────────
            var chipsRow = new Panel
            {
                Left      = 0,
                Top       = 110,
                Width     = 540,
                Height    = 52,
                BackColor = Color.FromArgb(15, 23, 42) // slate-900
            };

            MakeChip(_chipSeat,  $"🪑 Ghế {_seat.MaToa}-{_seat.SoGhe}", 16,  chipsRow);
            MakeChip(_chipClass, $"🏷️ {_seat.LoaiGhe}",                 190, chipsRow);
            MakeChip(_chipPrice, $"💰 {_seat.GiaVe:N0} ₫",             330, chipsRow);

            // ── CARD (white/surface, bo góc dưới) ───────────────────────
            _card.Left          = 0;
            _card.Top           = 162;
            _card.Width         = 540;
            _card.Height        = 358;
            _card.FillColor     = UiTheme.Surface;
            _card.BorderRadius  = 0;

            int x1 = 32, x2 = 220, w = 272, gap = 64;

            // Họ tên
            AddRow("👤  Họ tên hành khách", _txtPassengerName, x1, x2, w, 24, gap * 0);
            _txtPassengerName.Text = SessionManager.CurrentUser?.FullName ?? string.Empty;

            // CCCD
            AddRow("🪪  Số CCCD / Hộ chiếu", _txtPassengerId, x1, x2, w, 24, gap * 1);

            // Số điện thoại
            AddRow("📱  Số điện thoại", _txtPassengerPhone, x1, x2, w, 24, gap * 2);

            // Thanh toán
            var lblPayment = new Label
            {
                Text      = "💳  Phương thức thanh toán",
                Location  = new Point(x1, 24 + gap * 3),
                AutoSize  = true,
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = UiTheme.TextSecondary,
                BackColor = Color.Transparent
            };
            _labels.Add(lblPayment);
            _card.Controls.Add(lblPayment);

            _cboPaymentMethod.Left         = x2;
            _cboPaymentMethod.Top          = 20 + gap * 3;
            _cboPaymentMethod.Width        = w;
            _cboPaymentMethod.Height       = 38;
            _cboPaymentMethod.BorderRadius = 8;
            _cboPaymentMethod.DropDownStyle= ComboBoxStyle.DropDownList;
            _cboPaymentMethod.Items.AddRange(new object[] { "💵  Tiền mặt", "🏦  Chuyển khoản", "📱  MoMo", "💳  VNPay" });
            _cboPaymentMethod.SelectedIndex = 0;
            _cboPaymentMethod.FillColor    = UiTheme.SurfaceVariant;
            _cboPaymentMethod.ForeColor    = UiTheme.TextPrimary;
            _card.Controls.Add(_cboPaymentMethod);

            // Buttons
            _btnCancel.Text                 = "Hủy";
            _btnCancel.Left                 = 32;
            _btnCancel.Top                  = 24 + gap * 4 + 10;
            _btnCancel.Width                = 100;
            _btnCancel.Height               = 44;
            _btnCancel.BorderRadius         = 10;
            _btnCancel.FillColor            = UiTheme.SurfaceVariant;
            _btnCancel.ForeColor            = UiTheme.TextSecondary;
            _btnCancel.HoverState.FillColor = UiTheme.Border;
            _btnCancel.Click               += (_, _) => { DialogResult = DialogResult.Cancel; Close(); };
            _card.Controls.Add(_btnCancel);

            _btnConfirm.Text                = "✅  Xác nhận đặt vé";
            _btnConfirm.Left                = 300;
            _btnConfirm.Top                 = 24 + gap * 4 + 10;
            _btnConfirm.Width               = 208;
            _btnConfirm.Height              = 44;
            _btnConfirm.BorderRadius        = 10;
            _btnConfirm.FillColor           = UiTheme.Primary;
            _btnConfirm.ForeColor           = Color.White;
            _btnConfirm.HoverState.FillColor= UiTheme.PrimaryHover;
            _btnConfirm.Font                = new Font("Segoe UI", 10, FontStyle.Bold);
            _btnConfirm.Click              += BtnConfirm_Click;
            _card.Controls.Add(_btnConfirm);

            Controls.Add(_headerPanel);
            Controls.Add(chipsRow);
            Controls.Add(_card);

            ApplyTheme();
        }

        private void MakeChip(Guna2Panel chip, string text, int left, Panel parent)
        {
            chip.Left         = left;
            chip.Top          = 8;
            chip.Width        = 160;
            chip.Height       = 36;
            chip.BorderRadius = 18;
            chip.FillColor    = Color.FromArgb(51, 65, 85); // slate-700
            var lbl = new Label
            {
                Text      = text,
                Font      = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(203, 213, 225),
                AutoSize  = false,
                Dock      = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            chip.Controls.Add(lbl);
            parent.Controls.Add(chip);
        }

        private void AddRow(string labelText, Guna2TextBox txt, int x1, int x2, int w, int baseTop, int extraTop)
        {
            var lbl = new Label
            {
                Text      = labelText,
                Location  = new Point(x1, baseTop + extraTop),
                AutoSize  = true,
                Font      = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = UiTheme.TextSecondary,
                BackColor = Color.Transparent
            };
            _labels.Add(lbl);
            _card.Controls.Add(lbl);

            txt.Left          = x2;
            txt.Top           = baseTop + extraTop - 4;
            txt.Width         = w;
            txt.Height        = 38;
            txt.BorderRadius  = 8;
            txt.FillColor     = UiTheme.SurfaceVariant;
            txt.ForeColor     = UiTheme.TextPrimary;
            txt.BorderColor   = UiTheme.Border;
            txt.BorderThickness = 1;
            _card.Controls.Add(txt);
        }

        public void ApplyTheme()
        {
            _card.FillColor     = UiTheme.Surface;
            _btnConfirm.FillColor     = UiTheme.Primary;
            _btnConfirm.HoverState.FillColor = UiTheme.PrimaryHover;
            _btnCancel.FillColor      = UiTheme.SurfaceVariant;
            _btnCancel.ForeColor      = UiTheme.TextSecondary;

            foreach (var lbl in _labels)
            {
                lbl.ForeColor = UiTheme.TextSecondary;
                lbl.BackColor = Color.Transparent;
            }

            foreach (Control c in _card.Controls)
            {
                if (c is Guna2TextBox txt)
                {
                    txt.FillColor   = UiTheme.SurfaceVariant;
                    txt.ForeColor   = UiTheme.TextPrimary;
                    txt.BorderColor = UiTheme.Border;
                }
            }
        }

        private void InitializeComponent() { }

        private async void BtnConfirm_Click(object? sender, EventArgs e)
        {
            if (SessionManager.CurrentUser == null)
            {
                UiNotifier.Info("Phiên đăng nhập đã hết. Vui lòng đăng nhập lại.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtPassengerName.Text) ||
                string.IsNullOrWhiteSpace(_txtPassengerId.Text))
            {
                UiNotifier.Info("Vui lòng nhập đầy đủ họ tên và CCCD.");
                return;
            }

            _btnConfirm.Enabled = false;
            _btnConfirm.Text    = "⏳  Đang xử lý...";

            try
            {
                // Lấy giá trị payment (bỏ icon prefix)
                var paymentRaw    = _cboPaymentMethod.SelectedItem?.ToString() ?? "Cash";
                var paymentMethod = paymentRaw.Contains("Tiền mặt")   ? "Cash"
                                  : paymentRaw.Contains("Chuyển khoản") ? "BankTransfer"
                                  : paymentRaw.Contains("MoMo")       ? "MoMo"
                                  : "VNPay";

                var request = new BookTicketRequestDto
                {
                    UserID          = SessionManager.CurrentUser.UserID,
                    ScheduleID      = _scheduleId,
                    SeatID          = _seat.SeatID,
                    PassengerName   = _txtPassengerName.Text.Trim(),
                    PassengerID     = _txtPassengerId.Text.Trim(),
                    PassengerPhone  = _txtPassengerPhone.Text.Trim(),
                    SeatType        = _seat.LoaiGhe,
                    PaymentMethod   = paymentMethod
                };

                var result = await _ticketService.BookTicketAsync(request);
                if (result == null)
                {
                    UiNotifier.ErrorToast("Đặt vé không thành công.");
                    return;
                }

                UiNotifier.SuccessToast($"🎉 Đặt vé thành công — Mã vé: {result.TicketCode}");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi đặt vé: {ex.Message}");
            }
            finally
            {
                _btnConfirm.Enabled = true;
                _btnConfirm.Text    = "✅  Xác nhận đặt vé";
            }
        }
    }
}
