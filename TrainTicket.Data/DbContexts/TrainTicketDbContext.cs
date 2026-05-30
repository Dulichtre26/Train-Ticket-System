using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TrainTicket.Data.Entities;

namespace TrainTicket.Data.DbContexts;

public partial class TrainTicketDbContext : DbContext
{
    public TrainTicketDbContext()
    {
    }

    public TrainTicketDbContext(DbContextOptions<TrainTicketDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog>     AuditLogs    { get; set; }
    public virtual DbSet<ChatMessage>  ChatMessages { get; set; }

    public virtual DbSet<Carriage> Carriages { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<SchedulePrice> SchedulePrices { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Train> Trains { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<VwMonthlyRevenue> VwMonthlyRevenues { get; set; }

    public virtual DbSet<VwSeatAvailability> VwSeatAvailabilities { get; set; }

    public virtual DbSet<VwTicketDetail> VwTicketDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=TrainTicketDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Vietnamese_CI_AS");

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AuditLog__5E5499A86818ABB3");

            entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "IX_AuditLogs_UserID").IsDescending(false, true);

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.RecordId).HasColumnName("RecordID");
            entity.Property(e => e.TableName).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__AuditLogs__UserI__3E52440B");
        });

        modelBuilder.Entity<Carriage>(entity =>
        {
            entity.HasKey(e => e.CarriageId).HasName("PK__Carriage__17FE2DB09E74D948");

            entity.HasIndex(e => new { e.TrainId, e.CarriageCode }, "UQ_Carriage").IsUnique();

            entity.Property(e => e.CarriageId).HasColumnName("CarriageID");
            entity.Property(e => e.CarriageCode).HasMaxLength(10);
            entity.Property(e => e.CarriageType).HasMaxLength(50);
            entity.Property(e => e.Floor).HasDefaultValue(1);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TrainId).HasColumnName("TrainID");

            entity.HasOne(d => d.Train).WithMany(p => p.Carriages)
                .HasForeignKey(d => d.TrainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Carriages__Train__534D60F1");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PK__Discount__E43F6DF6EDFE4558");

            entity.HasIndex(e => e.Code, "UQ__Discount__A25C5AA744399C9A").IsUnique();

            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.DiscountType).HasMaxLength(20);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MinPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)");
            entity.Property(e => e.UsedCount).HasDefaultValue(0);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotiId).HasName("PK__Notifica__EDC08EF2A1422F8B");

            entity.HasIndex(e => new { e.UserId, e.IsRead, e.CreatedAt }, "IX_Notifications_UserID").IsDescending(false, false, true);

            entity.Property(e => e.NotiId).HasColumnName("NotiID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.RelatedId).HasColumnName("RelatedID");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasDefaultValue("Info");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__0B91BA14");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A586B96BB2A");

            entity.HasIndex(e => e.TicketId, "IX_Payments_TicketID");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.GatewayRef).HasMaxLength(200);
            entity.Property(e => e.Note).HasMaxLength(200);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.RefundAmount).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .HasColumnName("TransactionID");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Payments)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Ticket__03F0984C");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__RefreshT__658FEE8A289E86B8");

            entity.HasIndex(e => e.Token, "UQ__RefreshT__1EB4F81794100FF8").IsUnique();

            entity.Property(e => e.TokenId).HasColumnName("TokenID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeviceInfo).HasMaxLength(200);
            entity.Property(e => e.IsRevoked).HasDefaultValue(false);
            entity.Property(e => e.Token).HasMaxLength(512);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__UserI__38996AB5");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AE26B9D34");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61602A445A04").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PK__Routes__80979AADA428E5FA");

            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Distance).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.EstimatedHours).HasColumnType("decimal(5, 1)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RouteName).HasMaxLength(200);

            entity.HasOne(d => d.ArrivalStationNavigation).WithMany(p => p.RouteArrivalStationNavigations)
                .HasForeignKey(d => d.ArrivalStation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Routes__ArrivalS__4CA06362");

            entity.HasOne(d => d.DepartureStationNavigation).WithMany(p => p.RouteDepartureStationNavigations)
                .HasForeignKey(d => d.DepartureStation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Routes__Departur__4BAC3F29");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B69AD2C9657");

            entity.HasIndex(e => new { e.DepartureTime, e.Status }, "IX_Schedules_Departure");

            entity.HasIndex(e => e.TrainId, "IX_Schedules_TrainID");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DelayMinutes).HasDefaultValue(0);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Platform).HasMaxLength(10);
            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValue("Scheduled");
            entity.Property(e => e.TrainId).HasColumnName("TrainID");

            entity.HasOne(d => d.Route).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedules__Route__693CA210");

            entity.HasOne(d => d.Train).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.TrainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Schedules__Train__68487DD7");
        });

        modelBuilder.Entity<SchedulePrice>(entity =>
        {
            entity.HasKey(e => e.PriceId).HasName("PK__Schedule__4957584FF74400F6");

            entity.HasIndex(e => new { e.ScheduleId, e.SeatType }, "UQ_SchedulePrice").IsUnique();

            entity.Property(e => e.PriceId).HasColumnName("PriceID");
            entity.Property(e => e.Price).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.SeatType).HasMaxLength(50);

            entity.HasOne(d => d.Schedule).WithMany(p => p.SchedulePrices)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ScheduleP__Sched__72C60C4A");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PK__Seats__311713D3E0F74898");

            entity.HasIndex(e => e.CarriageId, "IX_Seats_CarriageID");

            entity.HasIndex(e => new { e.CarriageId, e.SeatNumber }, "UQ_Seat").IsUnique();

            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.CarriageId).HasColumnName("CarriageID");
            entity.Property(e => e.HasSocket).HasDefaultValue(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SeatClass)
                .HasMaxLength(20)
                .HasDefaultValue("Economy");
            entity.Property(e => e.SeatNumber).HasMaxLength(10);
            entity.Property(e => e.SeatType).HasMaxLength(50);

            entity.HasOne(d => d.Carriage).WithMany(p => p.Seats)
                .HasForeignKey(d => d.CarriageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seats__CarriageI__59FA5E80");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.StationId).HasName("PK__Stations__E0D8A6DDB632FA50");

            entity.HasIndex(e => e.StationCode, "UQ__Stations__D388561871BE6808").IsUnique();

            entity.Property(e => e.StationId).HasColumnName("StationID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Latitude).HasColumnType("decimal(10, 7)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(10, 7)");
            entity.Property(e => e.StationCode).HasMaxLength(10);
            entity.Property(e => e.StationName).HasMaxLength(100);
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC6272214858F");

            entity.ToTable(tb => tb.HasTrigger("trg_Tickets_UpdatedAt"));

            entity.HasIndex(e => e.ScheduleId, "IX_Tickets_ScheduleID");

            entity.HasIndex(e => e.Status, "IX_Tickets_Status");

            entity.HasIndex(e => e.TicketCode, "IX_Tickets_TicketCode");

            entity.HasIndex(e => e.UserId, "IX_Tickets_UserID");

            entity.HasIndex(e => new { e.ScheduleId, e.SeatId }, "UQ_Ticket_Seat").IsUnique();

            entity.HasIndex(e => e.TicketCode, "UQ__Tickets__598CF7A35375400F").IsUnique();

            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.BookedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CancelReason).HasMaxLength(200);
            entity.Property(e => e.CheckedIn).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 0)");
            entity.Property(e => e.DiscountCode).HasMaxLength(20);
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.PassengerId)
                .HasMaxLength(20)
                .HasColumnName("PassengerID");
            entity.Property(e => e.PassengerName).HasMaxLength(100);
            entity.Property(e => e.PassengerPhone).HasMaxLength(15);
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.SeatType).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TicketCode).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tickets__Schedul__797309D9");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tickets__SeatID__7A672E12");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tickets__UserID__787EE5A0");
        });

        modelBuilder.Entity<Train>(entity =>
        {
            entity.HasKey(e => e.TrainId).HasName("PK__Trains__8ED2725AD939E2D3");

            entity.HasIndex(e => e.TrainCode, "UQ__Trains__2AED9B9822062834").IsUnique();

            entity.Property(e => e.TrainId).HasColumnName("TrainID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
            entity.Property(e => e.TrainCode).HasMaxLength(20);
            entity.Property(e => e.TrainName).HasMaxLength(100);
            entity.Property(e => e.TrainType).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC49E6C5AC");

            entity.HasIndex(e => e.Email, "IX_Users_Email").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.Idnumber, "UQ__Users__564DB08ABD71DACC").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534829EC2BB").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FailedLoginCount).HasDefaultValue(0);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Idnumber)
                .HasMaxLength(20)
                .HasColumnName("IDNumber");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__3D978A55C1DE3A5D");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "UQ_UserRole").IsUnique();

            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
            entity.Property(e => e.AssignedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__RoleI__33D4B598");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__UserI__32E0915F");
        });

        modelBuilder.Entity<VwMonthlyRevenue>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_MonthlyRevenue");

            entity.Property(e => e.DoanhThu).HasColumnType("decimal(38, 0)");
            entity.Property(e => e.GiaTrungBinh).HasColumnType("decimal(38, 6)");
            entity.Property(e => e.RouteName).HasMaxLength(200);
            entity.Property(e => e.TongGiamGia).HasColumnType("decimal(38, 0)");
            entity.Property(e => e.TuyenDuong).HasMaxLength(203);
        });

        modelBuilder.Entity<VwSeatAvailability>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_SeatAvailability");

            entity.Property(e => e.CarriageCode).HasMaxLength(10);
            entity.Property(e => e.CarriageType).HasMaxLength(50);
            entity.Property(e => e.PassengerName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.SeatClass).HasMaxLength(20);
            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.SeatNumber).HasMaxLength(10);
            entity.Property(e => e.SeatType).HasMaxLength(50);
            entity.Property(e => e.TicketCode).HasMaxLength(20);
            entity.Property(e => e.TrangThai).HasMaxLength(6);
        });

        modelBuilder.Entity<VwTicketDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_TicketDetails");

            entity.Property(e => e.BookerEmail).HasMaxLength(100);
            entity.Property(e => e.BookerName).HasMaxLength(100);
            entity.Property(e => e.BookerPhone).HasMaxLength(15);
            entity.Property(e => e.CancelReason).HasMaxLength(200);
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.DiscountCode).HasMaxLength(20);
            entity.Property(e => e.FinalPrice).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.GaDen).HasMaxLength(100);
            entity.Property(e => e.GaDi).HasMaxLength(100);
            entity.Property(e => e.KeGa).HasMaxLength(10);
            entity.Property(e => e.LoaiToa).HasMaxLength(50);
            entity.Property(e => e.MaGaDen).HasMaxLength(10);
            entity.Property(e => e.MaGaDi).HasMaxLength(10);
            entity.Property(e => e.MaTau).HasMaxLength(20);
            entity.Property(e => e.MaToa).HasMaxLength(10);
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(12, 0)");
            entity.Property(e => e.PassengerId)
                .HasMaxLength(20)
                .HasColumnName("PassengerID");
            entity.Property(e => e.PassengerName).HasMaxLength(100);
            entity.Property(e => e.PassengerPhone).HasMaxLength(15);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentStatus).HasMaxLength(20);
            entity.Property(e => e.SeatType).HasMaxLength(50);
            entity.Property(e => e.SoGhe).HasMaxLength(10);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TenTau).HasMaxLength(100);
            entity.Property(e => e.TicketCode).HasMaxLength(20);
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.TrainType).HasMaxLength(50);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .HasColumnName("TransactionID");
            entity.Property(e => e.TripStatus).HasMaxLength(30);
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.MessageId);
            entity.Property(e => e.Content).HasMaxLength(2000);
            entity.Property(e => e.SentAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsRead).HasDefaultValue(false);

            entity.HasOne(e => e.Sender)
                  .WithMany(u => u.SentMessages)
                  .HasForeignKey(e => e.SenderId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_Chat_Sender");

            entity.HasOne(e => e.Receiver)
                  .WithMany(u => u.ReceivedMessages)
                  .HasForeignKey(e => e.ReceiverId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_Chat_Receiver");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
