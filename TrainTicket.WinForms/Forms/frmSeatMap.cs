using TrainTicket.Business.DTOs;
using Guna.UI2.WinForms;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    // Form hiển thị sơ đồ ghế động với màu trạng thái hiện đại.
    public class frmSeatMap : Form, IThemeableForm
    {
        private readonly int _scheduleId;
        private readonly IScheduleService _scheduleService;
        private readonly ITicketService _ticketService;

        private readonly Guna2Panel _toolbarPanel = new();
        private readonly FlowLayoutPanel _seatPanel = new();
        private readonly Label _lblSelected = new();
        private readonly Guna2Button _btnBook = new();

        private List<SeatMapDto> _seats = new();
        private SeatMapDto? _selectedSeat;
        private LoadingOverlay? _loadingOverlay;

        public frmSeatMap(int scheduleId, IScheduleService scheduleService, ITicketService ticketService)
        {
            _scheduleId = scheduleId;
            _scheduleService = scheduleService;
            _ticketService = ticketService;

            InitializeUi();
            Load += FrmSeatMap_Load;
        }

        private void InitializeUi()
        {
            Text = $"Sơ đồ ghế - Chuyến #{_scheduleId}";
            Width = 980;
            Height = 620;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = UiTheme.Background;

            _toolbarPanel.Dock = DockStyle.Bottom;
            _toolbarPanel.Height = 72;
            _toolbarPanel.FillColor = UiTheme.Surface;
            _toolbarPanel.ShadowDecoration.Enabled = true;

            _seatPanel.Dock = DockStyle.Fill;
            _seatPanel.Padding = new Padding(20);
            _seatPanel.AutoScroll = true;

            _lblSelected.Left = 20;
            _lblSelected.Top = 24;
            _lblSelected.Width = 620;
            _lblSelected.Text = "Chưa chọn ghế";
            _lblSelected.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _lblSelected.ForeColor = UiTheme.TextPrimary;

            _btnBook.Left = 760;
            _btnBook.Top = 16;
            _btnBook.Width = 180;
            _btnBook.Height = 40;
            _btnBook.BorderRadius = 10;
            _btnBook.FillColor = UiTheme.Primary;
            _btnBook.HoverState.FillColor = Color.FromArgb(5, 150, 105);
            _btnBook.Text = "Đặt vé ghế đang chọn";
            _btnBook.Click += BtnBook_Click;

            _toolbarPanel.Controls.Add(_lblSelected);
            _toolbarPanel.Controls.Add(_btnBook);

            Controls.Add(_seatPanel);
            Controls.Add(_toolbarPanel);

            _loadingOverlay = new LoadingOverlay(this);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _toolbarPanel.FillColor = UiTheme.Surface;
            _lblSelected.ForeColor = UiTheme.TextPrimary;
            _btnBook.FillColor = UiTheme.Primary;
            _btnBook.HoverState.FillColor = UiTheme.PrimaryHover;
        }

        // T?i s? ?? gh? t? stored procedure.
        private async void FrmSeatMap_Load(object? sender, EventArgs e)
        {
            await ReloadSeatMapAsync();
        }

        private async Task ReloadSeatMapAsync()
        {
            _selectedSeat = null;
            _lblSelected.Text = "Chưa chọn ghế";

            try
            {
                _loadingOverlay?.Show("Đang tải sơ đồ ghế...");
                _seats = await _scheduleService.GetSeatMapAsync(_scheduleId);
                _seatPanel.Controls.Clear();

                var groupedByCarriage = _seats.GroupBy(s => new { s.MaToa, s.LoaiToa });

                foreach (var group in groupedByCarriage)
                {
                    // Panel mô tả từng toa tàu
                    var carriageBox = new Guna2GroupBox
                    {
                        Text = $"Toa tàu: {group.Key.MaToa} - {group.Key.LoaiToa}",
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        Width = 260,
                        AutoSize = true,
                        BorderRadius = 8,
                        Margin = new Padding(10),
                        Padding = new Padding(10, 50, 10, 10),
                        FillColor = UiTheme.SurfaceVariant,
                        ForeColor = UiTheme.TextPrimary,
                        BorderColor = UiTheme.Border
                    };

                    var seatsFlow = new FlowLayoutPanel
                    {
                        Dock = DockStyle.Top,
                        AutoSize = true,
                        MaximumSize = new Size(240, 0), // Ép 2 ghế 1 hàng => Lối đi ở giữa
                        BackColor = Color.Transparent,
                        Margin = new Padding(0)
                    };

                    foreach (var seat in group.OrderBy(s => s.SoGhe))
                    {
                        var btn = new Guna2Button
                        {
                            Width = 100,
                            Height = 52,
                            Margin = new Padding(8),
                            Tag = seat,
                            Text = $"{seat.SoGhe}\n{seat.LoaiGhe}",
                            BorderRadius = 8,
                            FillColor = seat.TrangThai.Contains("Đã") ? Color.Salmon : UiTheme.Secondary,
                            Enabled = !seat.TrangThai.Contains("Đã")
                        };

                        btn.ForeColor = Color.White;
                        btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                        btn.HoverState.FillColor = UiTheme.SecondaryLight;

                        btn.Click += SeatButton_Click;
                        seatsFlow.Controls.Add(btn);
                    }

                    carriageBox.Controls.Add(seatsFlow);
                    _seatPanel.Controls.Add(carriageBox);
                }
            }
            catch (Exception ex)
            {
                UiNotifier.Error($"Không thể tải sơ đồ ghế: {ex.Message}");
            }
            finally
            {
                _loadingOverlay?.Hide();
            }
        }

        // ?ánh d?u gh? ?ang ch?n.
        private void SeatButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Guna2Button clickedButton || clickedButton.Tag is not SeatMapDto selected)
            {
                return;
            }

            foreach (Control control in _seatPanel.Controls)
            {
                if (control is Guna2Button btn && btn.Enabled && btn.Tag is SeatMapDto seat)
                {
                    btn.FillColor = seat.TrangThai.Contains("Đã") ? Color.Salmon : Color.LightGreen;
                }
            }

            clickedButton.FillColor = Color.Khaki;
            _selectedSeat = selected;
            _lblSelected.Text = $"Đang chọn: {selected.MaToa}-{selected.SoGhe} | {selected.LoaiGhe} | Giá: {selected.GiaVe:N0}";
        }

        // Mở form xác nhận đặt vé cho ghế đã chọn.
        private async void BtnBook_Click(object? sender, EventArgs e)
        {
            if (_selectedSeat == null)
            {
                UiNotifier.Info("Vui lòng chọn ghế trước khi đặt vé.");
                return;
            }

            using var confirmForm = new frmBookingConfirm(_scheduleId, _selectedSeat, _ticketService);
            var result = confirmForm.ShowDialog(this);

            // Nếu đặt vé thành công thì tải lại trạng thái ghế.
            if (result == DialogResult.OK)
            {
                await ReloadSeatMapAsync();
            }
        }
    }
}
