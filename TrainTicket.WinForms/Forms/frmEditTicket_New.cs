using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Guna.UI2.WinForms;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmEditTicket_New : Form
    {
        private readonly int _ticketId;
        private readonly TrainTicketDbContext _dbContext;

        public bool Saved { get; private set; }

        public frmEditTicket_New(int ticketId, TrainTicketDbContext dbContext)
        {
            InitializeComponent();
            _ticketId = ticketId;
            _dbContext = dbContext;

            Text = $"Sửa thông tin vé #{_ticketId}";
            _lblHeader.Text = $"📝  Sửa thông tin hành khách — Vé #{_ticketId}";
        }

        private async void frmEditTicket_New_Load(object sender, EventArgs e)
        {
            await LoadTicketAsync();
        }

        private async Task LoadTicketAsync()
        {
            var ticket = await _dbContext.Tickets.FindAsync(_ticketId);
            if (ticket == null) { Close(); return; }

            _txtName.Text  = ticket.PassengerName ?? "";
            _txtIdNum.Text = ticket.PassengerId   ?? "";
            _txtPhone.Text = ticket.PassengerPhone ?? "";

            var canEdit = ticket.Status is "Pending" or "Paid" or "Confirmed";
            _btnSave.Enabled = canEdit;
            _lblInfo.Text = canEdit
                ? $"Trạng thái vé: {ticket.Status} — Có thể chỉnh sửa"
                : $"❌  Vé trạng thái \"{ticket.Status}\" — Không thể sửa";
            _lblInfo.ForeColor = canEdit
                ? Color.FromArgb(16, 185, 129)
                : Color.FromArgb(239, 68, 68);
        }

        private async void _btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtName.Text))
            {
                MessageBox.Show("Tên hành khách không được để trống!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ticket = await _dbContext.Tickets.FindAsync(_ticketId);
            if (ticket == null) return;

            ticket.PassengerName  = _txtName.Text.Trim();
            ticket.PassengerId    = _txtIdNum.Text.Trim();
            ticket.PassengerPhone = _txtPhone.Text.Trim();
            ticket.UpdatedAt      = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            Saved = true;
            MessageBox.Show("✅ Đã cập nhật thông tin hành khách thành công!", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void _btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}