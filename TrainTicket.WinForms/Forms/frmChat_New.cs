using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Guna.UI2.WinForms;
using TrainTicket.Data.DbContexts;
using TrainTicket.Data.Entities;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmChat_New : Form, IThemeableForm
    {
        private readonly TrainTicketDbContext _dbContext;
        private int _chatPartnerId;
        private string _chatPartnerName = "";
        private List<(int Id, string Name)> _partners = new();

        public frmChat_New(TrainTicketDbContext dbContext)
        {
            InitializeComponent();
            _dbContext = dbContext;

            _lstUsers.DrawItem += LstUsers_DrawItem;
            _lstUsers.DrawMode = DrawMode.OwnerDrawFixed;
            _lstUsers.ItemHeight = 38;

            // Timer configure
            _pollTimer.Interval = 5000;
            _txtMessage.KeyDown += _txtMessage_KeyDown;

            lblContacts.Text = "👤  Liên hệ";
            _lblChatWith.Text = "Chọn người cần liên hệ từ danh sách bên trái";
            _lstUsers.Click += _lstUsers_Click;
        }

        private async void frmChat_New_Load(object sender, EventArgs e)
        {
            await LoadPartnersAsync();
            _pollTimer.Start();
            ApplyTheme();
        }

        private void LstUsers_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= _partners.Count) return;
            e.DrawBackground();

            var isSelected = (e.State & DrawItemState.Selected) != 0;
            var bg = isSelected ? UiTheme.Primary : UiTheme.Surface;

            using var brush = new SolidBrush(bg);
            e.Graphics.FillRectangle(brush, e.Bounds);

            using var fg = new SolidBrush(isSelected ? Color.White : UiTheme.TextPrimary);
            e.Graphics.DrawString($"  💬  {_partners[e.Index].Name}",
                new Font("Segoe UI", 10), fg,
                e.Bounds.X, e.Bounds.Y + (e.Bounds.Height - 16) / 2);

            e.DrawFocusRectangle();
        }

        private async Task LoadPartnersAsync()
        {
            var me = SessionManager.CurrentUser!;
            _partners.Clear();

            if (me.IsCustomer && !me.IsStaff)
            {
                var staff = await _dbContext.Users
                    .Where(u => u.UserRoles.Any(r => r.Role.RoleName == "Staff" || r.Role.RoleName == "Admin")
                             && u.IsDeleted == false && u.IsActive == true)
                    .Select(u => new { u.UserId, u.FullName })
                    .ToListAsync();
                _partners = staff.Select(s => (s.UserId, s.FullName)).ToList();
                _lblChatWith.Text = "Chọn nhân viên hỗ trợ";
            }
            else
            {
                var customers = await _dbContext.Users
                    .Where(u => u.UserRoles.Any(r => r.Role.RoleName == "Customer" || r.Role.RoleName == "User")
                             && u.IsDeleted == false)
                    .Select(u => new { u.UserId, u.FullName })
                    .ToListAsync();
                _partners = customers.Select(c => (c.UserId, c.FullName)).ToList();
                _lblChatWith.Text = "Chọn khách hàng để hỗ trợ";
            }

            _lstUsers.Items.Clear();
            foreach (var p in _partners) _lstUsers.Items.Add(p.Name);
            if (_partners.Count > 0)
            {
                _lstUsers.SelectedIndex = 0; // Đặt vệt sáng màu tím vào người đầu tiên
                await SelectPartnerAsync();  // Tự động tải tin nhắn của người đó lên luôn
            }
        }

        private async void _lstUsers_Click(object sender, EventArgs e)
        {
            await SelectPartnerAsync();
        }

        private async Task SelectPartnerAsync()
        {
            if (_lstUsers.SelectedIndex < 0) return;

            var p = _partners[_lstUsers.SelectedIndex];
            _chatPartnerId   = p.Id;
            _chatPartnerName = p.Name;
            _lblChatWith.Text = $"💬  Chat với: {_chatPartnerName}";

            await LoadMessagesAsync();

            var me = SessionManager.CurrentUser!.UserId;
            var unread = await _dbContext.ChatMessages
                .Where(m => m.SenderId == _chatPartnerId && m.ReceiverId == me && m.IsRead == false)
                .ToListAsync();

            unread.ForEach(m => m.IsRead = true);
            await _dbContext.SaveChangesAsync();
        }

        private async Task LoadMessagesAsync()
        {
            if (_chatPartnerId == 0) return;
            var me = SessionManager.CurrentUser!.UserId;

            var msgs = await _dbContext.ChatMessages
                .Where(m => (m.SenderId == me && m.ReceiverId == _chatPartnerId)
                         || (m.SenderId == _chatPartnerId && m.ReceiverId == me))
                .OrderBy(m => m.SentAt)
                .Select(m => new { m.SenderId, m.Content, m.SentAt })
                .ToListAsync();

            _chatArea.SuspendLayout();
            _chatArea.Controls.Clear();

            foreach (var msg in msgs)
            {
                bool isMine = msg.SenderId == me;
                AddBubble(msg.Content, isMine, msg.SentAt);
            }

            _chatArea.ResumeLayout();
            _chatArea.PerformLayout();

            if (_chatArea.Controls.Count > 0)
            {
                _chatArea.ScrollControlIntoView(_chatArea.Controls[_chatArea.Controls.Count - 1]);
            }
        }

        private void AddBubble(string text, bool isMine, DateTime? time)
        {
            var timeStr = time.HasValue ? time.Value.ToString("HH:mm") : "";

            // 1. Khởi tạo nhãn hiển thị nội dung tin nhắn
            var lbl = new Label
            {
                Text = $"{text}\n\n{timeStr}", // Thêm khoảng trống xuống dòng để thời gian không đè lên chữ
                Font = new Font("Segoe UI", 10),
                AutoSize = true,
                Padding = new Padding(12, 10, 12, 10),
                BackColor = isMine ? UiTheme.Primary : UiTheme.SurfaceVariant,
                ForeColor = isMine ? Color.White : UiTheme.TextPrimary,
                Cursor = Cursors.Default
            };

            // 2. Khởi tạo Panel chứa để căn lề trái/phải
            var row = new Panel
            {
                Width = _chatArea.Width - 25, // Trừ hao khoảng trống cho thanh cuộn (Scrollbar)
                BackColor = Color.Transparent,
                Padding = new Padding(8, 4, 8, 4)
            };

            // 3. Ép nhãn tính toán lại kích thước dựa trên giới hạn chiều rộng vùng chat
            int maxLabelWidth = (int)(row.Width * 0.65);
            lbl.MaximumSize = new Size(maxLabelWidth, 0);

            // Thêm vào panel để WinForms tính toán toán chính xác lbl.Height thực tế
            row.Controls.Add(lbl);

            // 4. Cập nhật lại chiều cao của Panel theo chiều cao thực tế của nhãn tin nhắn
            row.Height = lbl.Height + 12;

            // 5. Căn vị trí trái/phải cho bong bóng chat
            lbl.Location = isMine
                ? new Point(row.Width - lbl.Width - 8, 4)
                : new Point(8, 4);

            // 6. Đảm bảo co giãn tốt khi người dùng phóng to/thu nhỏ cửa sổ ứng dụng
            row.Resize += (_, _) =>
            {
                row.Width = _chatArea.Width - 25;
                lbl.MaximumSize = new Size((int)(row.Width * 0.65), 0);
                row.Height = lbl.Height + 12;
                lbl.Left = isMine ? row.Width - lbl.Width - 8 : 8;
            };

            // 7. Thêm vào cuối vùng chat (Đảm bảo thứ tự hiển thị tuyến tính từ trên xuống)
            _chatArea.Controls.Add(row);
        }

        private async void _btnSend_Click(object sender, EventArgs e)
        {
            if (_chatPartnerId == 0)
            {
                UiNotifier.ErrorToast("Vui lòng chọn người cần liên hệ trước!");
                return;
            }

            var text = _txtMessage.Text.Trim();
            if (string.IsNullOrWhiteSpace(text)) return;

            var me = SessionManager.CurrentUser!.UserId;
            _dbContext.ChatMessages.Add(new ChatMessage
            {
                SenderId   = me,
                ReceiverId = _chatPartnerId,
                Content    = text,
                SentAt     = DateTime.Now,
                IsRead     = false
            });

            await _dbContext.SaveChangesAsync();
            _txtMessage.Clear();
            await LoadMessagesAsync();
        }

        private void _txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true; 
                _btnSend_Click(sender, EventArgs.Empty);
            }
        }

        private async void _pollTimer_Tick(object sender, EventArgs e)
        {
            if (_chatPartnerId > 0)
            {
                await LoadMessagesAsync();
            }
        }

        private void frmChat_New_FormClosed(object sender, FormClosedEventArgs e)
        {
            _pollTimer.Stop();
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;

            _leftPanel.FillColor  = UiTheme.Surface;
            lblContacts.BackColor = UiTheme.Surface;
            lblContacts.ForeColor = UiTheme.TextPrimary;

            _lstUsers.BackColor = UiTheme.Surface;
            _lstUsers.ForeColor = UiTheme.TextPrimary;

            _rightPanel.FillColor = UiTheme.Background;
            _lblChatWith.BackColor = Color.Transparent;
            _lblChatWith.ForeColor = UiTheme.TextPrimary;

            _chatArea.BackColor = UiTheme.Background;

            _bottomBar.BackColor = UiTheme.Surface;
            _txtMessage.FillColor = UiTheme.SurfaceVariant;
            _txtMessage.ForeColor = UiTheme.TextPrimary;
            _btnSend.FillColor = UiTheme.Primary;
        }
    }
}