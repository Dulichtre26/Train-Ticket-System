using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;
using Guna.UI2.WinForms;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmSeatMap_New : Form, IThemeableForm
    {
        private readonly int _scheduleId;
        private readonly IScheduleService _scheduleService;
        private readonly ITicketService _ticketService;

        private List<SeatMapDto> _seats = new();
        private SeatMapDto? _selectedSeat;
        private LoadingOverlay? _loadingOverlay;

        // ---- Confirm panel (slide in from right) ----
        private Panel _confirmPanel = null!;
        private Guna2TextBox _txtName = null!;
        private Guna2TextBox _txtIdCard = null!;
        private Guna2TextBox _txtPhone = null!;
        private Guna2ComboBox _cboPayment = null!;
        private Label _lblConfirmSeat = null!;
        private Guna2Button _btnConfirm = null!;
        private System.Windows.Forms.Timer _slideTimer = new() { Interval = 10 };
        private bool _panelVisible = false;
        private int _targetX;

        public frmSeatMap_New(int scheduleId, IScheduleService scheduleService, ITicketService ticketService)
        {
            InitializeComponent();
            _scheduleId = scheduleId;
            _scheduleService = scheduleService;
            _ticketService = ticketService;

            Text = $"Sơ đồ ghế - Chuyến #{_scheduleId}";
            _loadingOverlay = new LoadingOverlay(this);

            BuildConfirmPanel();
            ApplyTheme();
        }

        // =============================================
        // BUILD CONFIRM PANEL (slide panel on right)
        // =============================================
        private void BuildConfirmPanel()
        {
            const int PW = 360;

            _confirmPanel = new Panel
            {
                Width = PW,
                Height = this.ClientSize.Height - _toolbarPanel.Height,
                Top = 0,
                Left = this.ClientSize.Width,   // hidden off-screen to the right
                BackColor = Color.FromArgb(30, 41, 59),
                Padding = new Padding(20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom
            };

            // Title
            var lblTitle = new Label
            {
                Text = "Xác nhận đặt vé",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = false,
                Width = PW - 40,
                Height = 30,
                Location = new Point(20, 16),
                BackColor = Color.Transparent
            };

            _lblConfirmSeat = new Label
            {
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(148, 163, 184),
                AutoSize = false,
                Width = PW - 40,
                Height = 24,
                Location = new Point(20, 48),
                BackColor = Color.Transparent
            };

            // Divider
            var divider = new Panel { Height = 1, Width = PW - 40, Left = 20, Top = 80, BackColor = Color.FromArgb(51, 65, 85) };

            var lblName = new Label { Text = "Họ tên hành khách", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(203, 213, 225), BackColor = Color.Transparent, AutoSize = true, Location = new Point(20, 96) };
            _txtName = new Guna2TextBox { Location = new Point(20, 118), Width = PW - 40, Height = 36, BorderRadius = 8, Font = new Font("Segoe UI", 9), FillColor = Color.FromArgb(51, 65, 85), ForeColor = Color.White, BorderColor = Color.FromArgb(71, 85, 105) };
            _txtName.Text = SessionManager.CurrentUser?.FullName ?? "";

            var lblId = new Label { Text = "Số CCCD / Hộ chiếu", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(203, 213, 225), BackColor = Color.Transparent, AutoSize = true, Location = new Point(20, 168) };
            _txtIdCard = new Guna2TextBox { Location = new Point(20, 190), Width = PW - 40, Height = 36, BorderRadius = 8, Font = new Font("Segoe UI", 9), FillColor = Color.FromArgb(51, 65, 85), ForeColor = Color.White, BorderColor = Color.FromArgb(71, 85, 105) };

            var lblPhone = new Label { Text = "Số điện thoại", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(203, 213, 225), BackColor = Color.Transparent, AutoSize = true, Location = new Point(20, 240) };
            _txtPhone = new Guna2TextBox { Location = new Point(20, 262), Width = PW - 40, Height = 36, BorderRadius = 8, Font = new Font("Segoe UI", 9), FillColor = Color.FromArgb(51, 65, 85), ForeColor = Color.White, BorderColor = Color.FromArgb(71, 85, 105) };

            // Payment
            var lblPay = new Label { Text = "Phương thức thanh toán", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.FromArgb(203, 213, 225), BackColor = Color.Transparent, AutoSize = true, Location = new Point(20, 312) };
            _cboPayment = new Guna2ComboBox
            {
                Location = new Point(20, 334),
                Width = PW - 40,
                Height = 36,
                BorderRadius = 8,
                Font = new Font("Segoe UI", 9),
                FillColor = Color.FromArgb(51, 65, 85),
                ForeColor = Color.FromArgb(203, 213, 225),
                DropDownStyle = ComboBoxStyle.DropDownList,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 28
            };
            _cboPayment.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "MoMo", "VNPay" });
            _cboPayment.SelectedIndex = 0;

            // Buttons
            _btnConfirm = new Guna2Button
            {
                Text = "Xác nhận đặt vé",
                Location = new Point(20, 390),
                Width = PW - 120,
                Height = 40,
                BorderRadius = 10,
                FillColor = UiTheme.Primary,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                DialogResult = DialogResult.None
            };
            _btnConfirm.HoverState.FillColor = UiTheme.PrimaryHover;
            _btnConfirm.Click += BtnConfirm_Click;

            var btnCancel = new Guna2Button
            {
                Text = "Hủy",
                Location = new Point(PW - 80, 390),
                Width = 60,
                Height = 40,
                BorderRadius = 10,
                FillColor = Color.FromArgb(71, 85, 105),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9)
            };
            btnCancel.HoverState.FillColor = Color.FromArgb(239, 68, 68);
            btnCancel.Click += (_, _) => SlideConfirmPanel(false);

            _confirmPanel.Controls.AddRange(new Control[]
            {
                lblTitle, _lblConfirmSeat, divider,
                lblName, _txtName,
                lblId, _txtIdCard,
                lblPhone, _txtPhone,
                lblPay, _cboPayment,
                _btnConfirm, btnCancel
            });

            // Slide timer
            _slideTimer.Tick += SlideTimer_Tick;

            this.Controls.Add(_confirmPanel);
            _confirmPanel.BringToFront();
        }

        // =============================================
        // SLIDE ANIMATION
        // =============================================
        private void SlideConfirmPanel(bool show)
        {
            // Recalculate size in case form resized
            _confirmPanel.Height = this.ClientSize.Height - _toolbarPanel.Height;
            _confirmPanel.Top = 0;

            _panelVisible = show;
            _targetX = show ? this.ClientSize.Width - _confirmPanel.Width : this.ClientSize.Width;
            _slideTimer.Start();
        }

        private void SlideTimer_Tick(object? sender, EventArgs e)
        {
            int step = (int)((_targetX - _confirmPanel.Left) * 0.25);
            if (step == 0) step = _targetX > _confirmPanel.Left ? 1 : -1;

            _confirmPanel.Left += step;

            if (Math.Abs(_confirmPanel.Left - _targetX) <= 2)
            {
                _confirmPanel.Left = _targetX;
                _slideTimer.Stop();
            }
        }

        // =============================================
        // THEME
        // =============================================
        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _toolbarPanel.FillColor = UiTheme.Surface;
            _lblSelected.ForeColor = UiTheme.TextPrimary;
            _lblSelected.BackColor = Color.Transparent;
            _btnBook.FillColor = UiTheme.Primary;
            _btnBook.HoverState.FillColor = UiTheme.PrimaryHover;
        }

        // =============================================
        // LOAD & SEAT MAP
        // =============================================
        private async void frmSeatMap_New_Load(object sender, EventArgs e)
        {
            await ReloadSeatMapAsync();
        }

        private async Task ReloadSeatMapAsync()
        {
            _selectedSeat = null;
            _lblSelected.Text = "Chưa chọn ghế";
            if (_panelVisible) SlideConfirmPanel(false);

            try
            {
                _loadingOverlay?.Show("Đang tải sơ đồ ghế...");
                _seats = await _scheduleService.GetSeatMapAsync(_scheduleId);

                // KHẮC PHỤC: Tạm dừng cập nhật giao diện để tránh xung đột luồng vẽ khi clear control
                if (_seatPanel.IsHandleCreated)
                {
                    _seatPanel.BeginInvoke(new Action(() => {
                        _seatPanel.SuspendLayout();
                        _seatPanel.Controls.Clear();
                    }));
                }

                var groups = _seats.GroupBy(s => new { s.MaToa, s.LoaiToa });

                foreach (var group in groups)
                {
                    var carriageBox = new Guna2GroupBox
                    {
                        Text = $"Toa tàu: {group.Key.MaToa} - {group.Key.LoaiToa}",
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        Width = 280,
                        AutoSize = false,
                        BorderRadius = 8,
                        Margin = new Padding(12),
                        Padding = new Padding(10, 50, 10, 10),
                        FillColor = UiTheme.SurfaceVariant,
                        ForeColor = UiTheme.TextPrimary,
                        BorderColor = UiTheme.Border
                    };

                    var seatsFlow = new FlowLayoutPanel
                    {
                        Location = new Point(10, 50),
                        Width = 260,
                        WrapContents = true,
                        AutoSize = false,
                        BackColor = Color.Transparent
                    };

                    foreach (var seat in group.OrderBy(s => s.SoGhe))
                    {
                        bool booked = seat.TrangThai == "Booked" || seat.TrangThai == "Used";
                        var btn = new Guna2Button
                        {
                            Width = 110,
                            Height = 54,
                            Margin = new Padding(6),
                            Tag = seat,
                            Text = $"{seat.SoGhe}\n{seat.LoaiGhe}",
                            BorderRadius = 8,
                            FillColor = booked ? Color.Salmon : UiTheme.Secondary,
                            Enabled = !booked,
                            ForeColor = Color.White,
                            Font = new Font("Segoe UI", 9, FontStyle.Bold)
                        };
                        btn.HoverState.FillColor = UiTheme.SecondaryLight;
                        btn.Click += SeatButton_Click;
                        seatsFlow.Controls.Add(btn);
                    }

                    int rows = (int)Math.Ceiling(group.Count() / 2.0);
                    seatsFlow.Height = rows * (54 + 12);
                    carriageBox.Height = seatsFlow.Height + 70;
                    carriageBox.Controls.Add(seatsFlow);

                    // Thêm an toàn vào panel chính
                    _seatPanel.Invoke(new Action(() => _seatPanel.Controls.Add(carriageBox)));
                }
            }
            catch (Exception ex)
            {
                UiNotifier.Error($"Không thể tải sơ đồ ghế: {ex.Message}");
            }
            finally
            {
                // KHẮC PHỤC: Khôi phục vẽ giao diện an toàn
                _seatPanel.Invoke(new Action(() => {
                    _seatPanel.ResumeLayout(true);
                    _seatPanel.Refresh();
                }));
                _loadingOverlay?.Hide();
            }
        }

        private void SeatButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Guna2Button clickedButton || clickedButton.Tag is not SeatMapDto selected)
                return;

            // Reset all available seat colors
            foreach (Control gb in _seatPanel.Controls)
                if (gb is Guna2GroupBox g)
                    foreach (Control flow in g.Controls)
                        if (flow is FlowLayoutPanel pnl)
                            foreach (Control ctrl in pnl.Controls)
                                if (ctrl is Guna2Button b && b.Enabled)
                                    b.FillColor = UiTheme.Secondary;

            clickedButton.FillColor = Color.Khaki;
            _selectedSeat = selected;
            _lblSelected.Text = $"Đang chọn: {selected.MaToa}-{selected.SoGhe} | {selected.LoaiGhe} | Giá: {selected.GiaVe:N0}";

            // Populate confirm panel and slide in
            _lblConfirmSeat.Text = $"Ghế {selected.MaToa}-{selected.SoGhe}  |  {selected.LoaiGhe}  |  {selected.GiaVe:N0} đ";
            _txtName.Text = SessionManager.CurrentUser?.FullName ?? "";
            _txtIdCard.Clear();
            _txtPhone.Clear();
            _cboPayment.SelectedIndex = 0;

            if (!_panelVisible) SlideConfirmPanel(true);
        }

        // =============================================
        // TOOLBAR BUTTONS
        // =============================================
        private async void _btnBook_Click(object sender, EventArgs e)
        {
            // _btnBook acts as "open confirm panel" toggle
            if (_selectedSeat == null)
            {
                UiNotifier.Info("Vui lòng chọn ghế trước.");
                return;
            }
            SlideConfirmPanel(!_panelVisible);
        }

        private async void BtnConfirm_Click(object? sender, EventArgs e)
        {
          if (_selectedSeat == null) return;

    if (string.IsNullOrWhiteSpace(_txtName.Text) || string.IsNullOrWhiteSpace(_txtIdCard.Text))
    {
        UiNotifier.Info("Vui lòng nhập họ tên và số CCCD.");
        return;
    }

    if (SessionManager.CurrentUser == null)
    {
        UiNotifier.Info("Phiên đăng nhập đã hết. Vui lòng đăng nhập lại.");
        return;
    }

    _btnConfirm.Enabled = false;
    _btnConfirm.Text = "Đang xử lý...";

    try
    {
        // 1. CHUẨN HÓA PHƯƠNG THỨC THANH TOÁN (Hỗ trợ quét không dấu/có dấu linh hoạt)
        var paymentRaw = _cboPayment.SelectedItem?.ToString() ?? "Tien mat";
        var paymentMethod = (paymentRaw.IndexOf("Chuyển", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             paymentRaw.IndexOf("Bank", StringComparison.OrdinalIgnoreCase) >= 0) ? "BankTransfer"
                          : paymentRaw.IndexOf("MoMo", StringComparison.OrdinalIgnoreCase) >= 0 ? "MoMo"
                          : paymentRaw.IndexOf("VNPay", StringComparison.OrdinalIgnoreCase) >= 0 ? "VNPay"
                          : "Cash";

        var request = new BookTicketRequestDto
        {
            UserID = SessionManager.CurrentUser.UserId,
            ScheduleID = _scheduleId,
            SeatID = _selectedSeat.SeatID,
            PassengerName = _txtName.Text.Trim(),
            PassengerID = _txtIdCard.Text.Trim(),
            PassengerPhone = _txtPhone.Text.Trim(),
            SeatType = _selectedSeat.LoaiGhe,
            PaymentMethod = paymentMethod
        };

        // 2. Thực thi lưu trữ vé vào Database thông qua Stored Procedure
        var result = await _ticketService.BookTicketAsync(request);
        if (result == null)
        {
            UiNotifier.ErrorToast("Đặt vé không thành công.");
            _btnConfirm.Enabled = true;
            _btnConfirm.Text = "Xác nhận đặt vé";
            return;
        }

        UiNotifier.SuccessToast($"Đặt vé thành công! Mã vé: {result.TicketCode}");
        
        // Ẩn panel xác nhận đặt vé đi sau khi hoàn tất lưu DB
        SlideConfirmPanel(false);

        // 3. === PHÂN LUỒNG XỬ LÝ GIAO DIỆN SAU KHI ĐẶT VÉ THÀNH CÔNG ===
        if (paymentMethod != "Cash")
        {
            // NẾU CHỌN THANH TOÁN ONLINE: Khởi tạo Form QR và truyền TicketID sang
            // Sử dụng ShowDialog để form QR hiển thị đè tập trung lên trên Form sơ đồ ghế
            using (var paymentForm = new frmPayments_New(result.TicketID, _ticketService))
            {
                paymentForm.ShowDialog();
            }
        }
        else
        {
            // NẾU CHỌN TIỀN MẶT: Chỉ cần thông báo nhắc nhở khách hàng qua MessageBox
            MessageBox.Show($"Bạn đã đặt giữ chỗ thành công.\nMã vé: {result.TicketCode}\nTrạng thái: Đang chờ xử lý (Pending).\nVui lòng đến quầy vé tại ga để thanh toán tiền mặt trước giờ tàu chạy!",
                "Thông báo giữ chỗ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 4. Làm mới lại sơ đồ ghế để chuyển ghế vừa chọn sang màu Đỏ (Booked)
        await ReloadSeatMapAsync();
    }
    catch (Exception ex)
    {
        UiNotifier.ErrorToast($"Lỗi đặt vé: {ex.Message}");
    }
    finally
    {
        _btnConfirm.Enabled = true;
        _btnConfirm.Text = "Xác nhận đặt vé";
    }
        }
    }
}
