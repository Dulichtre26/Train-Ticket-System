using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TrainTicket.Data.Entities;
using Route = TrainTicket.Data.Entities.Route;
using BCrypt.Net;

namespace TrainTicket.Data.DbContexts
{
    public static class DataSeeder
    {
        public static void Initialize(TrainTicketDbContext context)
        {
            // Đảm bảo Database đã được tạo
            context.Database.EnsureCreated();

            // 1. Tạo Stored Procedures và Functions (Quan trọng để đồng bộ logic Business)
            SeedStoreProcedures(context);

            try
            {
                // 2. Tạo Roles nếu chưa có
                if (!context.Roles.Any())
                {
                    var roleAdmin = new Role { RoleName = "Admin", Description = "Administrator" };
                    var roleUser = new Role { RoleName = "User", Description = "Customer" };
                    var roleStaff = new Role { RoleName = "Staff", Description = "Staff" };
                    context.Roles.AddRange(roleAdmin, roleUser, roleStaff);
                    context.SaveChanges();
                }

                // 3. Tạo Users mẫu nếu chưa có
                if (!context.Users.Any())
                {
                    var roleAdmin = context.Roles.FirstOrDefault(r => r.RoleName == "Admin");
                    var roleUser = context.Roles.FirstOrDefault(r => r.RoleName == "User");

                    // Tài khoản Admin: Admin@123
                    var adminUser = new User
                    {
                        Email = "admin@trainticket.vn",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                        FullName = "Quản Trị Viên",
                        PhoneNumber = "0123456789",
                        IsActive = true,
                        RegionCode = "HQ",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    // Tài khoản Khách hàng: 123456
                    var customerUser = new User
                    {
                        Email = "kh@trainticket.vn",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                        FullName = "Khách Hàng Mẫu",
                        PhoneNumber = "0987654321",
                        IsActive = true,
                        RegionCode = "HQ",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    context.Users.AddRange(adminUser, customerUser);
                    context.SaveChanges();

                    // Gán quyền
                    if (roleAdmin != null)
                        context.UserRoles.Add(new UserRole { UserID = adminUser.UserID, RoleID = roleAdmin.RoleID });
                    if (roleUser != null)
                        context.UserRoles.Add(new UserRole { UserID = customerUser.UserID, RoleID = roleUser.RoleID });

                    context.SaveChanges();
                }

                // Nếu đã có Stations từ SQL file thì không nạp thêm dữ liệu mẫu bằng C# để tránh trùng lặp
                if (context.Stations.Any()) return;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("=== SEEDER EXCEPTION ===");
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }

        private static void SeedStoreProcedures(TrainTicketDbContext context)
        {
            // SP Lấy danh sách vé (Đồng bộ với TicketHistoryDto)
            var spLayDanhSachVe = @"
CREATE OR ALTER PROCEDURE sp_LayDanhSachVe
    @UserID INT = NULL,
    @Status NVARCHAR(20) = NULL,
    @TuNgay DATE = NULL,
    @DenNgay DATE = NULL,
    @MaVe NVARCHAR(20) = NULL
AS
BEGIN
    SELECT t.TicketID, t.TicketCode, t.Status, t.PassengerName, t.PassengerID,
           t.SeatType, t.FinalPrice, t.BookedAt, t.CancelledAt,
           sch.DepartureTime AS GioDi, sch.ArrivalTime AS GioDen,
           tr.TrainCode AS MaTau, dep.StationName AS GaDi, arr.StationName AS GaDen,
           c.CarriageCode AS MaToa, s.SeatNumber AS SoGhe, u.FullName AS NguoiDat
    FROM Tickets t
    JOIN Users u ON t.UserID = u.UserID
    JOIN Schedules sch ON t.ScheduleID = sch.ScheduleID
    JOIN Trains tr ON sch.TrainID = tr.TrainID
    JOIN Routes r ON sch.RouteID = r.RouteID
    JOIN Stations dep ON r.DepartureStation = dep.StationID
    JOIN Stations arr ON r.ArrivalStation = arr.StationID
    JOIN Seats s ON t.SeatID = s.SeatID
    JOIN Carriages c ON s.CarriageID = c.CarriageID
    WHERE (@UserID IS NULL OR t.UserID = @UserID)
      AND (@Status IS NULL OR t.Status = @Status)
      AND (@TuNgay IS NULL OR CAST(t.BookedAt AS DATE) >= @TuNgay)
      AND (@DenNgay IS NULL OR CAST(t.BookedAt AS DATE) <= @DenNgay)
      AND (@MaVe IS NULL OR t.TicketCode LIKE '%' + @MaVe + '%')
    ORDER BY t.BookedAt DESC;
END";

            var spCheckIn = @"
CREATE OR ALTER PROCEDURE sp_CheckIn
    @TicketCode NVARCHAR(20)
AS
BEGIN
    UPDATE Tickets SET CheckedIn = 1, CheckInAt = GETDATE(), Status = 'Used' 
    WHERE TicketCode = @TicketCode AND Status = 'Confirmed';
    SELECT @@ROWCOUNT AS Result;
END";

            // Function tính giá vé (Sửa tên cột khớp với Entity Discount và SQL Table)
            var fnTinhGiaVe = @"
CREATE OR ALTER FUNCTION fn_TinhGiaVe(@BasePrice DECIMAL(12,0), @DiscountCode NVARCHAR(20))
RETURNS DECIMAL(12,0)
AS
BEGIN
    DECLARE @FinalPrice DECIMAL(12,0) = @BasePrice;
    DECLARE @Amount DECIMAL(12,0) = 0;
    DECLARE @Type NVARCHAR(20);

    SELECT @Amount = Amount, @Type = DiscountType
    FROM Discounts 
    WHERE Code = @DiscountCode AND IsActive = 1 
      AND GETDATE() BETWEEN ValidFrom AND ValidTo;

    IF @Type = 'Percent'
        SET @FinalPrice = @BasePrice - (@BasePrice * @Amount / 100);
    ELSE IF @Type = 'Fixed'
        SET @FinalPrice = @BasePrice - @Amount;

    IF @FinalPrice < 0 SET @FinalPrice = 0;
    RETURN @FinalPrice;
END";

            var alterUsers = @"
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = 'RegionCode')
BEGIN
    ALTER TABLE [dbo].[Users] ADD [RegionCode] NVARCHAR(20) DEFAULT 'HQ' WITH VALUES;
END";

            // SP Báo cáo doanh thu (Sửa trạng thái lọc từ Paid thành Confirmed)
            var spBaoCaoDoanhThu = @"
CREATE OR ALTER PROCEDURE sp_BaoCaoDoanhThu
    @Nam INT,
    @Thang INT = NULL,
    @RouteID INT = NULL
AS
BEGIN
    SELECT MONTH(t.BookedAt) AS Thang, 
           COUNT(t.TicketID) AS SoVeBan, 
           SUM(t.FinalPrice) AS TongDoanhThu
    FROM Tickets t
    JOIN Schedules sch ON t.ScheduleID = sch.ScheduleID
    WHERE YEAR(t.BookedAt) = @Nam
      AND (@Thang IS NULL OR MONTH(t.BookedAt) = @Thang)
      AND (@RouteID IS NULL OR sch.RouteID = @RouteID)
      AND t.Status IN ('Confirmed', 'Used')
    GROUP BY MONTH(t.BookedAt)
    ORDER BY MONTH(t.BookedAt);
END";

            try
            {
                // Thực thi SQL Raw để tạo object trong DB
                context.Database.ExecuteSqlRaw(spLayDanhSachVe);
                context.Database.ExecuteSqlRaw(spCheckIn);
                context.Database.ExecuteSqlRaw(fnTinhGiaVe);
                context.Database.ExecuteSqlRaw(alterUsers);
                context.Database.ExecuteSqlRaw(spBaoCaoDoanhThu);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error seeding SPs: " + ex.Message);
            }
        }
    }
}