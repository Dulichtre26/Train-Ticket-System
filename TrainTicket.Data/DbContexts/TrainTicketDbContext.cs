using Microsoft.EntityFrameworkCore;
using TrainTicket.Data.Entities;

namespace TrainTicket.Data.DbContexts
{
    // Tenant provider interface trong c?ng Data ?? khng b? l?i ph? thu?c ngu?c
    public interface ITenantProviderData
    {
        string GetCurrentRegion();
    }

    // DbContext trung tm c?a h? th?ng.
    public class TrainTicketDbContext : DbContext
    {
        private readonly string _tenantRegion;

        public TrainTicketDbContext(DbContextOptions<TrainTicketDbContext> options, ITenantProviderData? tenantProvider = null)
            : base(options) 
        { 
            _tenantRegion = tenantProvider?.GetCurrentRegion() ?? "HQ";
        }

        // DbSets  m?i ci t??ng ?ng 1 b?ng
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Station> Stations => Set<Station>();
        public DbSet<Train> Trains => Set<Train>();
        public DbSet<Entities.Route> Routes => Set<Entities.Route>();
        public DbSet<Carriage> Carriages => Set<Carriage>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<SchedulePrice> SchedulePrices => Set<SchedulePrice>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        public DbSet<Discount> Discounts => Set<Discount>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserRole — khóa unique composite
            modelBuilder.Entity<UserRole>(e => {
                e.HasIndex(x => new { x.UserID, x.RoleID }).IsUnique();
                e.HasOne(x => x.User)
                 .WithMany(x => x.UserRoles)
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Role)
                 .WithMany(x => x.UserRoles)
                 .HasForeignKey(x => x.RoleID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Route — 2 FK ??n Station, c?n ??t tên rõ
            modelBuilder.Entity<Entities.Route>(e => {
                e.HasOne(x => x.DepartureStationNav)
                 .WithMany()
                 .HasForeignKey(x => x.DepartureStation)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.ArrivalStationNav)
                 .WithMany()
                 .HasForeignKey(x => x.ArrivalStation)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Carriage
            modelBuilder.Entity<Carriage>(e => {
                e.HasIndex(x => new { x.TrainID, x.CarriageCode }).IsUnique();
                e.HasOne(x => x.Train)
                 .WithMany(x => x.Carriages)
                 .HasForeignKey(x => x.TrainID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Seat
            modelBuilder.Entity<Seat>(e => {
                e.HasIndex(x => new { x.CarriageID, x.SeatNumber }).IsUnique();
                e.HasOne(x => x.Carriage)
                 .WithMany(x => x.Seats)
                 .HasForeignKey(x => x.CarriageID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Schedule
            modelBuilder.Entity<Schedule>(e => {
                e.HasOne(x => x.Train)
                 .WithMany(x => x.Schedules)
                 .HasForeignKey(x => x.TrainID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Route)
                 .WithMany(x => x.Schedules)
                 .HasForeignKey(x => x.RouteID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // SchedulePrice — unique (ScheduleID, SeatType)
            modelBuilder.Entity<SchedulePrice>(e => {
                e.HasIndex(x => new { x.ScheduleID, x.SeatType }).IsUnique();
                e.HasOne(x => x.Schedule)
                 .WithMany(x => x.SchedulePrices)
                 .HasForeignKey(x => x.ScheduleID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Ticket — unique (ScheduleID, SeatID) = 1 gh? 1 chuy?n
            modelBuilder.Entity<Ticket>(e => {
                e.HasIndex(x => x.TicketCode).IsUnique();
                e.HasIndex(x => new { x.ScheduleID, x.SeatID }).IsUnique();
                e.HasOne(x => x.User)
                 .WithMany(x => x.Tickets)
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Schedule)
                 .WithMany(x => x.Tickets)
                 .HasForeignKey(x => x.ScheduleID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Seat)
                 .WithMany()
                 .HasForeignKey(x => x.SeatID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Payment  1-1 v?i Ticket
            modelBuilder.Entity<Payment>(e => {
                e.HasOne(x => x.Ticket)
                 .WithOne(x => x.Payment)
                 .HasForeignKey<Payment>(x => x.TicketID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Notification
            modelBuilder.Entity<Notification>(e => {
                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // RefreshToken
            modelBuilder.Entity<RefreshToken>(e => {
                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // AuditLog
            modelBuilder.Entity<AuditLog>(e => {
                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // Discount - no FK, but index on Code
            modelBuilder.Entity<Discount>(e => {
                e.HasIndex(x => x.Code).IsUnique();
            });

            // GLOBAL QUERY FILTERS - MULTI-TENANT (BY REGION)
            modelBuilder.Entity<Station>().HasQueryFilter(x => x.RegionCode == "HQ" || x.RegionCode == _tenantRegion || _tenantRegion == "HQ");
            modelBuilder.Entity<Train>().HasQueryFilter(x => x.RegionCode == "HQ" || x.RegionCode == _tenantRegion || _tenantRegion == "HQ");
            modelBuilder.Entity<Entities.Route>().HasQueryFilter(x => x.RegionCode == "HQ" || x.RegionCode == _tenantRegion || _tenantRegion == "HQ");
            modelBuilder.Entity<Schedule>().HasQueryFilter(x => x.RegionCode == "HQ" || x.RegionCode == _tenantRegion || _tenantRegion == "HQ");
            modelBuilder.Entity<Carriage>().HasQueryFilter(x => x.RegionCode == "HQ" || x.RegionCode == _tenantRegion || _tenantRegion == "HQ");
            modelBuilder.Entity<Seat>().HasQueryFilter(x => x.RegionCode == "HQ" || x.RegionCode == _tenantRegion || _tenantRegion == "HQ");
            modelBuilder.Entity<SchedulePrice>().HasQueryFilter(x => x.RegionCode == "HQ" || x.RegionCode == _tenantRegion || _tenantRegion == "HQ");
            modelBuilder.Entity<Ticket>().HasQueryFilter(x => x.RegionCode == "HQ" || x.RegionCode == _tenantRegion || _tenantRegion == "HQ");
            // Removed filter for User to avoid issues if RegionCode column is missing
            // modelBuilder.Entity<User>().HasQueryFilter(x => x.RegionCode == "HQ" || x.RegionCode == _tenantRegion || _tenantRegion == "HQ");
        }
    }
}
