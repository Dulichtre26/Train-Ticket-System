using System.Data;
using Microsoft.EntityFrameworkCore;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Helpers;
using Guna.UI2.WinForms;

namespace TrainTicket.WinForms.Forms
{
    public partial class frmCustomers_New : Form, IThemeableForm
    {
        private readonly IAuthService _authService;
        private readonly TrainTicketDbContext _dbContext;

        public frmCustomers_New(IAuthService authService, TrainTicketDbContext dbContext)
        {
            InitializeComponent();
            _authService = authService;
            _dbContext = dbContext;

            ApplyTheme();
        }

        public void ApplyTheme()
        {
            BackColor = UiTheme.Background;
            _leftPanel.BackColor = UiTheme.Surface;
            _rightPanel.BackColor = UiTheme.Background;

            _btnRegister.FillColor = UiTheme.Primary;
            _btnRegister.ForeColor = Color.White;

            _txtFullName.FillColor = UiTheme.SurfaceVariant;
            _txtEmail.FillColor = UiTheme.SurfaceVariant;
            _txtPassword.FillColor = UiTheme.SurfaceVariant;
            _txtPhone.FillColor = UiTheme.SurfaceVariant;

            _txtFullName.ForeColor = UiTheme.TextPrimary;
            _txtEmail.ForeColor = UiTheme.TextPrimary;
            _txtPassword.ForeColor = UiTheme.TextPrimary;
            _txtPhone.ForeColor = UiTheme.TextPrimary;

            lblTitle.ForeColor = UiTheme.TextPrimary;
            lblTitle.BackColor = Color.Transparent;

            _grid.BackgroundColor = UiTheme.Surface;
            _grid.GridColor = UiTheme.Divider;
            _grid.DefaultCellStyle.BackColor = UiTheme.SurfaceVariant;
            _grid.DefaultCellStyle.ForeColor = UiTheme.TextPrimary;
            _grid.DefaultCellStyle.SelectionBackColor = UiTheme.PrimaryLight;
            _grid.DefaultCellStyle.SelectionForeColor = Color.White;
            _grid.ColumnHeadersDefaultCellStyle.BackColor = UiTheme.Surface;
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = UiTheme.TextPrimary;
            _grid.RowHeadersDefaultCellStyle.BackColor = UiTheme.Surface;
        }

        private async void frmCustomers_New_Load(object sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            var customersRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleName == "Customer" || r.RoleName == "User");
            if (customersRole == null) return;

            var customers = await _dbContext.Users
                .Where(u => u.UserRoles.Any(ur => ur.RoleId == customersRole.RoleId) && u.IsDeleted == false)
                .Select(u => new 
                {
                    u.UserId,
                    u.FullName,
                    u.Email,
                    u.PhoneNumber,
                    u.CreatedAt
                })
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            _grid.DataSource = customers;
            _grid.Columns["UserId"].HeaderText = "ID";
            _grid.Columns["FullName"].HeaderText = "Họ và tên";
            _grid.Columns["Email"].HeaderText = "Email";
            _grid.Columns["PhoneNumber"].HeaderText = "Số điện thoại";
            _grid.Columns["CreatedAt"].HeaderText = "Ngày tạo";
        }

        private async void _btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtFullName.Text) || 
                string.IsNullOrWhiteSpace(_txtEmail.Text) || 
                string.IsNullOrWhiteSpace(_txtPassword.Text))
            {
                UiNotifier.ErrorToast("Vui lòng nhập đầy đủ Tên, Email và Mật khẩu!");
                return;
            }

            var request = new RegisterRequestDto
            {
                FullName = _txtFullName.Text.Trim(),
                Email = _txtEmail.Text.Trim(),
                Password = _txtPassword.Text,
                PhoneNumber = _txtPhone.Text.Trim()
            };

            _btnRegister.Enabled = false;
            try
            {
                var success = await _authService.RegisterAsync(request);
                if (success)
                {
                    UiNotifier.InfoToast("Đăng ký thành công!");
                    _txtFullName.Clear();
                    _txtEmail.Clear();
                    _txtPassword.Clear();
                    _txtPhone.Clear();
                    await LoadCustomersAsync();
                }
                else
                {
                    UiNotifier.ErrorToast("Đăng ký thất bại. Email có thể đã tồn tại.");
                }
            }
            catch (Exception ex)
            {
                UiNotifier.ErrorToast($"Lỗi hệ thống: {ex.Message}");
            }
            finally
            {
                _btnRegister.Enabled = true;
            }
        }
    }
}