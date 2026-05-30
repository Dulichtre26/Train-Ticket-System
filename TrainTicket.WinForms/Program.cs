using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrainTicket.Business.Interfaces;
using TrainTicket.Business.Services;
using TrainTicket.Data.ADO;
using TrainTicket.Data.DbContexts;
using TrainTicket.WinForms.Forms;
using TrainTicket.WinForms.Helpers;
using System.Globalization;

namespace TrainTicket.WinForms
{
    internal static class Program
    {
        // ServiceProvider toàn cục để resolve dependency giữa các Form/Service.
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        [STAThread]
        static void Main()
        {
            // Khoi tao cau hình mac ddnh WinForms (.NET 8).
            ApplicationConfiguration.Initialize();

            // Đọc theme đã lưu từ lần chạy trước.
            UiTheme.Load();

            // 1) Tạo DI container.
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Seed data if empty
            // using (var scopeInit = ServiceProvider.CreateScope())
            // {
            //     var context = scopeInit.ServiceProvider.GetRequiredService<TrainTicketDbContext>();
            //     DataSeeder.Initialize(context);
            // }

            // 2) Resolve form đăng nhập từ DI và chạy ứng dụng.
            using var scope = ServiceProvider.CreateScope();
            var loginForm = scope.ServiceProvider.GetRequiredService<frmLogin_new>();
            Application.Run(loginForm);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            // Tenant Provider cho Global Query Filter ho?c filter c?c m?c d? lieu
            services.AddSingleton<ITenantProvider, TenantProvider>();

            // ??ng ký DbContext (EF Core) s? d?ng lambda ?? l?y chu?i k?t n?i hi?n t?i thay vì lock 1 chu?i.
            services.AddDbContext<TrainTicketDbContext>((provider, options) =>
                options.UseSqlServer(ConnectionHelper.CurrentConnectionString), ServiceLifetime.Scoped);

            // ??ng ký helper ADO.NET d?a tr?n chu?i connection hi?n t?i.
            services.AddScoped(_ => new AdoHelper(ConnectionHelper.CurrentConnectionString));

            // đăng ký Business Services.
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<INotificationService, NotificationService>();

            // đăng ký các form chính.
            services.AddScoped<frmLogin_new>();
            services.AddScoped<frmRegister_New>();
            services.AddScoped<frmMain_New>();
            services.AddScoped<frmSearch_new>();
            services.AddScoped<frmSeatMap_New>();
            services.AddScoped<frmBookingConfirm_New>();
            services.AddScoped<frmReports_New>();
            services.AddScoped<frmTickets_New>();
            services.AddScoped<frmPayments_New>();
            services.AddScoped<frmPendingPayments_New>();
            services.AddScoped<frmPaymentHistory_New>();
            services.AddScoped<frmCustomerDashboard_New>();
            services.AddScoped<frmCustomerProfile_New>();
            services.AddScoped<frmChat_New>();
            services.AddScoped<frmTrains_New>();
            services.AddScoped<frmStations_New>();
            services.AddScoped<frmRoutes_New>();
            services.AddScoped<frmSchedules_New>();
        }
    }
}

