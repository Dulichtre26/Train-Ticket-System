using Guna.UI2.WinForms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainTicket.Business.Interfaces;
using TrainTicket.WinForms.Helpers;

namespace TrainTicket.WinForms.Forms
{
    /// <summary>
    /// Trang Thanh toán: Hi?n th? danh sách vé ch? thanh toán (Pending) c?a ng??i dùng hi?n t?i.
    /// Cho phép ch?n vé và m? form xác nh?n thanh toán.
    /// </summary>
    public partial class frmPendingPayments_New : Form, IThemeableForm
    {
        private readonly ITicketService _ticketService;
        private LoadingOverlay? _loading;

        public frmPendingPayments_New(ITicketService ticketService)
        {
            InitializeComponent();
            _ticketService = ticketService;
            _loading = new LoadingOverlay(this);
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _topPanel.FillColor = UiTheme.Surface;
            _lblInfo.ForeColor = UiTheme.TextSecondary;
            _lblInfo.BackColor = Color.Transparent;
            _grid.ThemeStyle.HeaderStyle.BackColor = UiTheme.PrimaryDark;
            _grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
        }

        private async void frmPendingPayments_New_Load(object sender, EventArgs e)
        {
            await LoadPendingTicketsAsync();
        }

        private async Task LoadPendingTicketsAsync()
        {
            _loading?.Show("?ang t?i danh sách vé ch? thanh toán...");
            try
            {
                var userId = SessionManager.CurrentUser?.IsAdmin == true || SessionManager.CurrentUser?.IsStaff == true
                    ? (int?)null
                    : SessionManager.CurrentUser?.UserId;

                var table = await _ticketService.GetTicketsAsync(userId, status: "Pending");

                _grid.DataSource = table;
                FormatColumns();

                int count = table?.Rows.Count ?? 0;
                decimal total = count > 0
                    ? table!.AsEnumerable().Sum(r => r.IsNull("FinalPrice") ? 0m : Convert.ToDecimal(r["FinalPrice"]))
                    : 0m;

                _lblInfo.Text = count > 0
                    ? $"Có {count} vé ch? thanh toán | T?ng ti?n: {total:N0} VN?"
                    : "? Không có vé nào ?ang ch? thanh toán.";

                _btnPay.Enabled = count > 0;
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"L?i t?i danh sách: {ex.Message}");
            }
            finally
            {
                _loading?.Hide();
            }
        }

        private void FormatColumns()
        {
            foreach (DataGridViewColumn col in _grid.Columns)
            {
                col.HeaderText = col.Name switch
                {
                    "TicketID" => "ID",
                    "TicketCode" => "Mã vé",
                    "Status" => "Tr?ng thái",
                    "PassengerName" => "Hành khách",
                    "FinalPrice" => "Giá (VN?)",
                    "GioDi" => "Gi? ?i",
                    "MaTau" => "Tàu",
                    "GaDi" => "T?",
                    "GaDen" => "??n",
                    "SoGhe" => "Gh?",
                    "PaymentMethod" => "Ph??ng th?c TT",
                    "BookedAt" => "Ngày ??t",
                    _ => col.HeaderText
                };
                if (col.Name is "FinalPrice") col.DefaultCellStyle.Format = "N0";
                if (col.Name is "GioDi" or "BookedAt") col.DefaultCellStyle.Format = "HH:mm dd/MM/yy";
            }
        }

        private async void _btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadPendingTicketsAsync();
        }

        private async void _btnPay_Click(object sender, EventArgs e)
        {
            if (_grid.CurrentRow == null || _grid.CurrentRow.Index < 0)
            {
                UiNotifier.ErrorToast("Vui lòng ch?n m?t vé trong danh sách ?? thanh toán!");
                return;
            }

            var currentRow = _grid.CurrentRow;
            var ticketIdObj = currentRow.Cells["TicketID"].Value;
            if (ticketIdObj == null || ticketIdObj == DBNull.Value) return;

            int ticketId = Convert.ToInt32(ticketIdObj);
            string code = currentRow.Cells["TicketCode"].Value?.ToString() ?? $"#{ticketId}";

            using var scope = Program.ServiceProvider.CreateScope();
            var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();

            using var payForm = new frmPayments_New(ticketId, ticketService);
            var result = payForm.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                UiNotifier.SuccessToast($"?? Vé {code} ?ã ???c thanh toán thành công!");
                await LoadPendingTicketsAsync();
            }
        }
    }
}
