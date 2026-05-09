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
        // ServiceProvider toàn c?c ?? resolve dependency gi?a các Form/Service.
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        [STAThread]
        static void Main()
        {
            // Kh?i t?o c?u hình m?c ??nh WinForms (.NET 8).
            ApplicationConfiguration.Initialize();

            // ??c theme ?ã l?u t? l?n ch?y tr??c.
            UiTheme.Load();

            // 1) T?o DI container.
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Seed data if empty
            using (var scopeInit = ServiceProvider.CreateScope())
            {
                var context = scopeInit.ServiceProvider.GetRequiredService<TrainTicketDbContext>();
                DataSeeder.Initialize(context);
            }

            // 2) Resolve form ??ng nh?p t? DI và ch?y ?ng d?ng.
            using var scope = ServiceProvider.CreateScope();
            var loginForm = scope.ServiceProvider.GetRequiredService<frmLogin>();
            Application.Run(loginForm);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            // Tenant Provider cho Global Query Filter ho?c filter c?c m?c d? li?u
            services.AddSingleton<ITenantProvider, TenantProvider>();
            services.AddSingleton<ITenantProviderData, TenantProvider>();

            // ??ng ký DbContext (EF Core) s? d?ng lambda ?? l?y chu?i k?t n?i hi?n t?i thay vì lock 1 chu?i.
            services.AddDbContext<TrainTicketDbContext>((provider, options) =>
                options.UseSqlServer(ConnectionHelper.CurrentConnectionString), ServiceLifetime.Scoped);

            // ??ng ký helper ADO.NET d?a tr?n chu?i connection hi?n t?i.
            services.AddScoped(_ => new AdoHelper(ConnectionHelper.CurrentConnectionString));

            // ??ng ký Business Services.
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<INotificationService, NotificationService>();

            // ??ng ký các form chính.
            services.AddScoped<frmLogin>();
            services.AddScoped<frmRegister>();
            services.AddScoped<frmMain>();
            services.AddScoped<frmSearch>();
            services.AddScoped<frmSeatMap>();
            services.AddScoped<frmBookingConfirm>();
            services.AddScoped<frmReports>();
            services.AddScoped<frmTickets>();
            services.AddScoped<frmPayments>();
            services.AddScoped<frmTrains>();
            services.AddScoped<frmStations>();
            services.AddScoped<frmRoutes>();
            services.AddScoped<frmSchedules>();
        }
    }
}
