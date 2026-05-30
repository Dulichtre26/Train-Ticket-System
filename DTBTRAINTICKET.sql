-- =====================================================
-- HỆ THỐNG QUẢN LÝ ĐẶT VÉ TÀU HỎA - NÂNG CẤP TOÀN DIỆN
-- Phiên bản: 2.0  |  .NET 8 + EF Core + ADO.NET
-- Cải tiến: Thêm AuditLog, RefreshToken, Discount, Notification,
--           tối ưu Index, thêm View, Function, Trigger
-- =====================================================
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'TrainTicketDB')
BEGIN
    ALTER DATABASE TrainTicketDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TrainTicketDB;
END
GO

CREATE DATABASE TrainTicketDB
    COLLATE Vietnamese_CI_AS;  -- Hỗ trợ tiếng Việt tốt hơn
GO
USE TrainTicketDB;
GO

-- =====================================================
-- NHÓM 1: BẢO MẬT & PHÂN QUYỀN
-- =====================================================

CREATE TABLE Roles (
    RoleID      INT IDENTITY(1,1) PRIMARY KEY,
    RoleName    NVARCHAR(50)  NOT NULL UNIQUE,
    Description NVARCHAR(200) NULL,
    CreatedAt   DATETIME2     DEFAULT GETDATE()
);
GO

CREATE TABLE Users (
    UserID          INT IDENTITY(1,1) PRIMARY KEY,
    FullName        NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(100) NOT NULL UNIQUE,
    PhoneNumber     NVARCHAR(15)  NULL,
    PasswordHash    NVARCHAR(256) NOT NULL,
    IDNumber        NVARCHAR(20)  NULL UNIQUE,
    DateOfBirth     DATE          NULL,
    Gender          NVARCHAR(10)  NULL CHECK (Gender IN (N'Nam', N'Nữ', N'Khác')),
    IsActive        BIT           DEFAULT 1,
    IsDeleted       BIT           DEFAULT 0,
    LastLoginAt     DATETIME2     NULL,
    FailedLoginCount INT          DEFAULT 0,
    LockoutUntil    DATETIME2     NULL,
    CreatedAt       DATETIME2     DEFAULT GETDATE(),
    UpdatedAt       DATETIME2     DEFAULT GETDATE(),
    CreatedBy       INT           NULL
);
GO
ALTER TABLE Users
ADD CONSTRAINT CK_Users_Gender 
CHECK (Gender IN (N'Nam', N'Nữ', N'Khác'));

CREATE TABLE UserRoles (
    UserRoleID  INT IDENTITY(1,1) PRIMARY KEY,
    UserID      INT NOT NULL REFERENCES Users(UserID),
    RoleID      INT NOT NULL REFERENCES Roles(RoleID),
    AssignedAt  DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT UQ_UserRole UNIQUE (UserID, RoleID)
);
GO

-- [MỚI] Bảng RefreshToken cho tính năng "ghi nhớ đăng nhập"
CREATE TABLE RefreshTokens (
    TokenID     INT IDENTITY(1,1) PRIMARY KEY,
    UserID      INT           NOT NULL REFERENCES Users(UserID),
    Token       NVARCHAR(512) NOT NULL UNIQUE,
    ExpiresAt   DATETIME2     NOT NULL,
    IsRevoked   BIT           DEFAULT 0,
    CreatedAt   DATETIME2     DEFAULT GETDATE(),
    DeviceInfo  NVARCHAR(200) NULL
);
GO

-- [MỚI] Bảng AuditLog theo dõi hành động quan trọng
CREATE TABLE AuditLogs (
    LogID       INT IDENTITY(1,1) PRIMARY KEY,
    UserID      INT           NULL REFERENCES Users(UserID),
    Action      NVARCHAR(100) NOT NULL,   -- 'LOGIN', 'BOOK_TICKET', 'CANCEL_TICKET'...
    TableName   NVARCHAR(50)  NULL,
    RecordID    INT           NULL,
    OldValues   NVARCHAR(MAX) NULL,       -- JSON snapshot trước
    NewValues   NVARCHAR(MAX) NULL,       -- JSON snapshot sau
    IPAddress   NVARCHAR(50)  NULL,
    CreatedAt   DATETIME2     DEFAULT GETDATE()
);
GO

-- =====================================================
-- NHÓM 2: DANH MỤC
-- =====================================================
-- Bảng Stations: Ga tàu
CREATE TABLE Stations (
    StationID   INT IDENTITY(1,1) PRIMARY KEY,
    StationCode NVARCHAR(10)  NOT NULL UNIQUE,-- HAN, SGN, DAN...
    StationName NVARCHAR(100) NOT NULL,
    City        NVARCHAR(100) NOT NULL,
    Address     NVARCHAR(200) NULL,
    Latitude    DECIMAL(10,7) NULL,    -- [MỚI] Tọa độ GPS
    Longitude   DECIMAL(10,7) NULL,
    IsActive    BIT           DEFAULT 1,
    CreatedAt   DATETIME2     DEFAULT GETDATE()
);
GO
-- Bảng Trains: Tàu hỏa
CREATE TABLE Trains (
    TrainID     INT IDENTITY(1,1) PRIMARY KEY,
    TrainCode   NVARCHAR(20)  NOT NULL UNIQUE,  -- SE1, SE2, TN1...
    TrainName   NVARCHAR(100) NOT NULL,
    TrainType   NVARCHAR(50)  NOT NULL,
    MaxSpeed    INT           NULL,     -- [MỚI] km/h
    Manufacturer NVARCHAR(100) NULL,   -- [MỚI]
    YearBuilt   INT           NULL,    -- [MỚI]
    IsActive    BIT           DEFAULT 1,
    CreatedAt   DATETIME2     DEFAULT GETDATE()
);
GO

CREATE TABLE Routes (
    RouteID          INT IDENTITY(1,1) PRIMARY KEY,
    RouteName        NVARCHAR(200) NOT NULL,
    DepartureStation INT NOT NULL REFERENCES Stations(StationID),
    ArrivalStation   INT NOT NULL REFERENCES Stations(StationID),
    Distance         DECIMAL(8,2) NULL,
    EstimatedHours   DECIMAL(5,1) NULL,
    IsActive         BIT DEFAULT 1,
    CreatedAt        DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT CHK_Route_Stations CHECK (DepartureStation <> ArrivalStation)
);
GO
GO
-- Bảng Carriages: Toa tàu
CREATE TABLE Carriages (
    CarriageID   INT IDENTITY(1,1) PRIMARY KEY,
    TrainID      INT NOT NULL REFERENCES Trains(TrainID),
    CarriageCode NVARCHAR(10)  NOT NULL,
    CarriageType NVARCHAR(50)  NOT NULL,
    TotalSeats   INT           NOT NULL CHECK (TotalSeats > 0),
    Floor        INT           DEFAULT 1,  -- [MỚI] Tầng của toa
    IsActive     BIT           DEFAULT 1,
    CONSTRAINT UQ_Carriage UNIQUE (TrainID, CarriageCode)
);
GO


-- Bảng Seats: Ghế ngồi
CREATE TABLE Seats (
    SeatID      INT IDENTITY(1,1) PRIMARY KEY,
    CarriageID  INT NOT NULL REFERENCES Carriages(CarriageID),
    SeatNumber  NVARCHAR(10)  NOT NULL,
    SeatType    NVARCHAR(50)  NOT NULL,
    SeatClass   NVARCHAR(20)  DEFAULT 'Economy',  -- [MỚI] Economy/Business/VIP
    HasSocket   BIT           DEFAULT 0,           -- [MỚI] Có ổ cắm điện
    IsActive    BIT           DEFAULT 1,
    CONSTRAINT UQ_Seat UNIQUE (CarriageID, SeatNumber)
);
GO

-- [MỚI] Bảng mã khuyến mãi / giảm giá
CREATE TABLE Discounts (
    DiscountID   INT IDENTITY(1,1) PRIMARY KEY,
    Code         NVARCHAR(20)  NOT NULL UNIQUE,
    Description  NVARCHAR(200) NULL,
    DiscountType NVARCHAR(20)  NOT NULL CHECK (DiscountType IN ('Percent','Fixed')),
    Amount       DECIMAL(12,0) NOT NULL CHECK (Amount > 0),
    MinPrice     DECIMAL(12,0) DEFAULT 0,
    MaxUses      INT           NULL,
    UsedCount    INT           DEFAULT 0,
    ValidFrom    DATETIME2     NOT NULL,
    ValidTo      DATETIME2     NOT NULL,
    IsActive     BIT           DEFAULT 1,
    CreatedAt    DATETIME2     DEFAULT GETDATE()
);
GO

-- =====================================================
-- NHÓM 3: NGHIỆP VỤ CHÍNH
-- =====================================================

CREATE TABLE Schedules (
    ScheduleID        INT IDENTITY(1,1) PRIMARY KEY,
    TrainID           INT           NOT NULL REFERENCES Trains(TrainID),
    RouteID           INT           NOT NULL REFERENCES Routes(RouteID),
    DepartureTime     DATETIME2     NOT NULL,
    ArrivalTime       DATETIME2     NOT NULL,
    Platform          NVARCHAR(10)  NULL,    -- [MỚI] Ke ga
    DelayMinutes      INT           DEFAULT 0, -- [MỚI] Phút trễ thực tế
    Status            NVARCHAR(30)  DEFAULT N'Scheduled'
                      CHECK (Status IN (N'Scheduled', N'Departed', N'Arrived', N'Cancelled', N'Delayed')),
    IsActive          BIT           DEFAULT 1,
    CreatedAt         DATETIME2     DEFAULT GETDATE(),
    CONSTRAINT CHK_Schedule_Time CHECK (ArrivalTime > DepartureTime)
);
GO

-- Bảng SchedulePrices: Giá vé theo lịch trình và loại ghế
CREATE TABLE SchedulePrices (
    PriceID    INT IDENTITY(1,1) PRIMARY KEY,
    ScheduleID INT           NOT NULL REFERENCES Schedules(ScheduleID),
    SeatType   NVARCHAR(50)  NOT NULL,
    Price      DECIMAL(12,0) NOT NULL CHECK (Price >= 0),
    CONSTRAINT UQ_SchedulePrice UNIQUE (ScheduleID, SeatType)
);
GO


-- Bảng Tickets: Vé đặt
CREATE TABLE Tickets (
    TicketID        INT IDENTITY(1,1) PRIMARY KEY,
    TicketCode      NVARCHAR(20)  NOT NULL UNIQUE,
    UserID          INT           NOT NULL REFERENCES Users(UserID),
    ScheduleID      INT           NOT NULL REFERENCES Schedules(ScheduleID),
    SeatID          INT           NOT NULL REFERENCES Seats(SeatID),
    PassengerName   NVARCHAR(100) NOT NULL,
    PassengerID     NVARCHAR(20)  NOT NULL,
    PassengerPhone  NVARCHAR(15)  NULL,
    SeatType        NVARCHAR(50)  NOT NULL,
    OriginalPrice   DECIMAL(12,0) NOT NULL,
    DiscountAmount  DECIMAL(12,0) DEFAULT 0,   -- [MỚI]
    FinalPrice      DECIMAL(12,0) NOT NULL,
    DiscountCode    NVARCHAR(20)  NULL,         -- [MỚI]
    Status          NVARCHAR(20)  DEFAULT N'Pending'
                    CHECK (Status IN (N'Pending', N'Confirmed', N'Cancelled', N'Used', N'Expired')),
    CheckedIn       BIT           DEFAULT 0,   -- [MỚI] Đã check-in
    CheckInAt       DATETIME2     NULL,        -- [MỚI]
    BookedAt        DATETIME2     DEFAULT GETDATE(),
    CancelledAt     DATETIME2     NULL,
    CancelReason    NVARCHAR(200) NULL,
    CreatedAt       DATETIME2     DEFAULT GETDATE(),
    UpdatedAt       DATETIME2     DEFAULT GETDATE(),
    CONSTRAINT UQ_Ticket_Seat UNIQUE (ScheduleID, SeatID)
);
GO

-- =====================================================
-- NHÓM 4: THANH TOÁN
-- =====================================================

CREATE TABLE Payments (
    PaymentID     INT IDENTITY(1,1) PRIMARY KEY,
    TicketID      INT           NOT NULL REFERENCES Tickets(TicketID),
    Amount        DECIMAL(12,0) NOT NULL CHECK (Amount > 0),
    PaymentMethod NVARCHAR(50)  NOT NULL
                  CHECK (PaymentMethod IN (N'Cash', N'BankTransfer', N'MoMo', N'VNPay', N'ZaloPay')),
    Status        NVARCHAR(20)  DEFAULT N'Pending'
                  CHECK (Status IN (N'Pending', N'Success', N'Failed', N'Refunded', N'PartialRefund')),
    TransactionID NVARCHAR(100) NULL,
    GatewayRef    NVARCHAR(200) NULL,   -- [MỚI] Mã tham chiếu cổng TT
    PaidAt        DATETIME2     NULL,
    RefundedAt    DATETIME2     NULL,   -- [MỚI]
    RefundAmount  DECIMAL(12,0) NULL,  -- [MỚI]
    Note          NVARCHAR(200) NULL,
    CreatedAt     DATETIME2     DEFAULT GETDATE()
);
GO

-- [MỚI] Bảng Notification thông báo
CREATE TABLE Notifications (
    NotiID      INT IDENTITY(1,1) PRIMARY KEY,
    UserID      INT           NOT NULL REFERENCES Users(UserID),
    Title       NVARCHAR(200) NOT NULL,
    Body        NVARCHAR(MAX) NOT NULL,
    Type        NVARCHAR(50)  DEFAULT 'Info',  -- Info/Warning/Success/Error
    IsRead      BIT           DEFAULT 0,
    RelatedID   INT           NULL,
    CreatedAt   DATETIME2     DEFAULT GETDATE()
);
GO

-- =====================================================
-- TRIGGERS - PHẢI ĐẶT SAU KHI TẠO XONG BẢNG TICKETS
-- =====================================================

CREATE OR ALTER TRIGGER trg_Tickets_UpdatedAt
ON Tickets
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT UPDATE(UpdatedAt)
    BEGIN
        UPDATE t
        SET UpdatedAt = GETDATE()
        FROM Tickets t
        INNER JOIN inserted i ON t.TicketID = i.TicketID;
    END
END
GO

-- =====================================================
-- INDEXES - Tối ưu hiệu năng
-- =====================================================
CREATE INDEX IX_Tickets_ScheduleID   ON Tickets(ScheduleID);
CREATE INDEX IX_Tickets_UserID       ON Tickets(UserID);
CREATE INDEX IX_Tickets_Status       ON Tickets(Status) INCLUDE (TicketCode, FinalPrice);
CREATE INDEX IX_Schedules_TrainID    ON Schedules(TrainID);
CREATE INDEX IX_Schedules_Departure  ON Schedules(DepartureTime, Status) INCLUDE (RouteID);
CREATE INDEX IX_Seats_CarriageID     ON Seats(CarriageID);
CREATE INDEX IX_Payments_TicketID    ON Payments(TicketID);
CREATE INDEX IX_Users_Email          ON Users(Email) WHERE IsDeleted = 0;
CREATE INDEX IX_AuditLogs_UserID     ON AuditLogs(UserID, CreatedAt DESC);
CREATE INDEX IX_Notifications_UserID ON Notifications(UserID, IsRead, CreatedAt DESC);
CREATE INDEX IX_Tickets_TicketCode   ON Tickets(TicketCode);
GO

-- =====================================================
-- VIEWS - Truy vấn nhanh
-- =====================================================

-- View: Vé chi tiết đầy đủ thông tin
-- View 1: Chi tiết vé đầy đủ (rất hay dùng)
CREATE OR ALTER VIEW vw_TicketDetails AS
SELECT
    t.TicketID,
    t.TicketCode,
    t.Status,
    t.BookedAt,
    t.CheckedIn,
    t.CheckInAt,
    t.CancelledAt,
    t.CancelReason,

    -- Thông tin người đặt
    u.FullName        AS BookerName,
    u.Email           AS BookerEmail,
    u.PhoneNumber     AS BookerPhone,

    -- Thông tin hành khách
    t.PassengerName,
    t.PassengerID,
    t.PassengerPhone,

    -- Thông tin vé & giá
    t.SeatType,
    t.OriginalPrice,
    t.DiscountAmount,
    t.FinalPrice,
    t.DiscountCode,

    -- Thông tin chuyến tàu
    sc.DepartureTime                     AS GioDi,
    sc.ArrivalTime                       AS GioDen,
    sc.Platform                          AS KeGa,
    sc.DelayMinutes                      AS TrePhut,
    sc.Status                            AS TripStatus,

    tr.TrainCode                         AS MaTau,
    tr.TrainName                         AS TenTau,
    tr.TrainType,

    dep.StationName                      AS GaDi,
    dep.StationCode                      AS MaGaDi,
    arr.StationName                      AS GaDen,
    arr.StationCode                      AS MaGaDen,

    c.CarriageCode                       AS MaToa,
    c.CarriageType                       AS LoaiToa,
    s.SeatNumber                         AS SoGhe,

    -- Thanh toán
    p.PaymentMethod,
    p.Status                             AS PaymentStatus,
    p.PaidAt,
    p.TransactionID

FROM Tickets t
JOIN Users          u   ON t.UserID       = u.UserID
JOIN Schedules      sc  ON t.ScheduleID   = sc.ScheduleID
JOIN Trains         tr  ON sc.TrainID     = tr.TrainID
JOIN Routes         r   ON sc.RouteID     = r.RouteID
JOIN Stations       dep ON r.DepartureStation = dep.StationID
JOIN Stations       arr ON r.ArrivalStation   = arr.StationID
JOIN Seats          s   ON t.SeatID       = s.SeatID
JOIN Carriages      c   ON s.CarriageID   = c.CarriageID
LEFT JOIN Payments  p   ON t.TicketID     = p.TicketID 
                       AND p.Status = 'Success';
GO
-- View 2: Thống kê doanh thu theo tháng (tối ưu)
CREATE OR ALTER VIEW vw_MonthlyRevenue AS
SELECT
    YEAR(t.BookedAt)                  AS Nam,
    MONTH(t.BookedAt)                 AS Thang,
    r.RouteName,
    dep.StationName + N' → ' + arr.StationName AS TuyenDuong,
    COUNT(*)                          AS SoVe,
    SUM(t.FinalPrice)                 AS DoanhThu,
    AVG(t.FinalPrice)                 AS GiaTrungBinh,
    SUM(t.DiscountAmount)             AS TongGiamGia,
    COUNT(CASE WHEN t.Status = N'Cancelled' THEN 1 END) AS SoVeHuy,
    COUNT(CASE WHEN t.CheckedIn = 1 THEN 1 END)         AS SoVeDaCheckIn
FROM Tickets t
JOIN Schedules sc ON t.ScheduleID = sc.ScheduleID
JOIN Routes    r  ON sc.RouteID   = r.RouteID
JOIN Stations dep ON r.DepartureStation = dep.StationID
JOIN Stations arr ON r.ArrivalStation   = arr.StationID
WHERE t.Status IN (N'Confirmed', N'Used', N'Cancelled')
GROUP BY YEAR(t.BookedAt), MONTH(t.BookedAt), 
         r.RouteName, dep.StationName, arr.StationName;
GO

-- View 3: Trạng thái ghế realtime (SỬA LỖI + TỐI ƯU)
CREATE OR ALTER VIEW vw_SeatAvailability AS
SELECT
    sc.ScheduleID,
    c.CarriageCode,
    c.CarriageType,
    c.Floor,
    se.SeatID,
    se.SeatNumber,
    se.SeatType,
    se.SeatClass,
    se.HasSocket,
    ISNULL(sp.Price, 0)                  AS Price,
    CASE 
        WHEN tk.TicketID IS NOT NULL THEN N'Đã đặt' 
        ELSE N'Trống' 
    END                                  AS TrangThai,
    tk.TicketCode,
    tk.PassengerName
FROM Schedules sc
JOIN Trains         t   ON sc.TrainID     = t.TrainID
JOIN Carriages      c   ON c.TrainID      = t.TrainID AND c.IsActive = 1
JOIN Seats          se  ON se.CarriageID  = c.CarriageID AND se.IsActive = 1
LEFT JOIN SchedulePrices sp ON sp.ScheduleID = sc.ScheduleID 
                           AND sp.SeatType   = se.SeatType
LEFT JOIN Tickets    tk ON tk.ScheduleID = sc.ScheduleID 
                       AND tk.SeatID     = se.SeatID 
                       AND tk.Status NOT IN (N'Cancelled', N'Expired')
WHERE sc.IsActive = 1;
GO
-- =====================================================
-- FUNCTIONS
-- =====================================================

CREATE OR ALTER FUNCTION fn_TinhGiaVe
(
    @OriginalPrice DECIMAL(12,0),
    @DiscountCode  NVARCHAR(20)
)
RETURNS DECIMAL(12,0)
AS
BEGIN
    DECLARE @FinalPrice DECIMAL(12,0) = @OriginalPrice;

    IF @DiscountCode IS NOT NULL
    BEGIN
        DECLARE @Type      NVARCHAR(20),
                @Amount    DECIMAL(12,0),
                @MinPrice  DECIMAL(12,0),
                @ValidTo   DATETIME2,
                @MaxUses   INT,
                @UsedCount INT;

        SELECT 
            @Type      = DiscountType,
            @Amount    = Amount,
            @MinPrice  = MinPrice,
            @ValidTo   = ValidTo,
            @MaxUses   = MaxUses,
            @UsedCount = UsedCount
        FROM Discounts
        WHERE Code = @DiscountCode 
          AND IsActive = 1
          AND GETDATE() BETWEEN ValidFrom AND ValidTo;   -- Kiểm tra thời hạn tốt hơn

        IF @Type IS NOT NULL 
           AND @OriginalPrice >= @MinPrice
           AND (@MaxUses IS NULL OR @UsedCount < @MaxUses)
        BEGIN
            IF @Type = 'Percent'
                SET @FinalPrice = @OriginalPrice * (1 - @Amount / 100.0);
            ELSE
                SET @FinalPrice = @OriginalPrice - @Amount;

            IF @FinalPrice < 0 
                SET @FinalPrice = 0;
        END
    END

    RETURN @FinalPrice;
END
GO

-- Function 2: Đếm số ghế còn trống (TỐI ƯU HIỆU NĂNG)
CREATE OR ALTER FUNCTION dbo.fn_SoGheTrong(@ScheduleID INT)
RETURNS INT
AS
BEGIN
    DECLARE @TotalSeats INT, @BookedSeats INT;

    -- Lấy tổng số ghế của tàu trong lịch trình
    SELECT @TotalSeats = COUNT(*)
    FROM Carriages c
    JOIN Seats se ON se.CarriageID = c.CarriageID
    WHERE c.TrainID = (SELECT TrainID FROM Schedules WHERE ScheduleID = @ScheduleID)
      AND c.IsActive = 1 
      AND se.IsActive = 1;

    -- Đếm ghế đã đặt
    SELECT @BookedSeats = COUNT(*)
    FROM Tickets
    WHERE ScheduleID = @ScheduleID
      AND Status NOT IN (N'Cancelled', N'Expired');

    RETURN ISNULL(@TotalSeats, 0) - ISNULL(@BookedSeats, 0);
END
GO
SELECT TOP 1 * FROM SchedulePrices
-- =====================================================
-- STORED PROCEDURES NÂNG CẤP
-- =====================================================
USE TrainTicketDB;
GO


-- SP 1: Tìm chuyến tàu (tối ưu + thêm thông tin)
CREATE OR ALTER PROCEDURE sp_TimChuyenTau
    @GaDi   INT,
    @GaDen  INT,
    @NgayDi DATE
AS
BEGIN  
    SET NOCOUNT ON;

    SELECT
        s.ScheduleID,
        t.TrainCode                         AS MaTau,
        t.TrainName                         AS TenTau,
        dep.StationName                     AS GaDi,
        arr.StationName                     AS GaDen,
        s.DepartureTime                     AS GioDi,
        s.ArrivalTime                       AS GioDen,
        DATEDIFF(MINUTE, s.DepartureTime, s.ArrivalTime) AS ThoiGianDi_Phut,
        r.Distance                          AS KhoangCach_KM,
        s.Status                            AS TrangThai,
        s.Platform                          AS KeGa,           -- [MỚI]
        s.DelayMinutes                      AS TrePhut,        -- [MỚI]
        dbo.fn_SoGheTrong(s.ScheduleID)    AS SoGheTrong,     -- Dùng function
        -- Giá thấp nhất
        (SELECT MIN(Price) FROM SchedulePrices WHERE ScheduleID = s.ScheduleID) AS GiaThapNhat
    FROM Schedules s
    JOIN Trains    t   ON s.TrainID  = t.TrainID
    JOIN Routes    r   ON s.RouteID  = r.RouteID
    JOIN Stations  dep ON r.DepartureStation = dep.StationID
    JOIN Stations  arr ON r.ArrivalStation   = arr.StationID
    WHERE r.DepartureStation = @GaDi
      AND r.ArrivalStation   = @GaDen
      AND CAST(s.DepartureTime AS DATE) = @NgayDi
      AND s.Status           = N'Scheduled'
      AND s.IsActive         = 1
    ORDER BY s.DepartureTime;
END
GO
DROP PROCEDURE IF EXISTS sp_XemSoDoGhe;   -- <--- Thêm dòng này để an toàn
GO
-- SP 2: Xem sơ đồ ghế
CREATE PROCEDURE sp_XemSoDoGhe
  @ScheduleID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.CarriageCode      AS MaToa,
        c.CarriageType      AS LoaiToa,
        se.SeatNumber       AS SoGhe,
        se.SeatType         AS LoaiGhe,
        ISNULL(se.SeatClass, N'Economy') AS HangGhe,      -- Sửa an toàn
        ISNULL(se.HasSocket, 0)          AS CoO_Cam,      -- Sửa an toàn
        se.SeatID,
        CASE WHEN tk.TicketID IS NOT NULL 
             THEN N'Đã đặt' 
             ELSE N'Trống' 
        END AS TrangThai,
        ISNULL(sp.Price, 0) AS GiaVe
    FROM Schedules sc
    JOIN Trains    t   ON sc.TrainID = t.TrainID
    JOIN Carriages c   ON c.TrainID = t.TrainID AND c.IsActive = 1
    JOIN Seats     se  ON se.CarriageID = c.CarriageID AND se.IsActive = 1
    LEFT JOIN SchedulePrices sp ON sp.ScheduleID = sc.ScheduleID 
                               AND sp.SeatType = se.SeatType
    LEFT JOIN Tickets tk ON tk.ScheduleID = sc.ScheduleID 
                        AND tk.SeatID = se.SeatID 
                        AND tk.Status NOT IN (N'Cancelled', N'Expired')
    WHERE sc.ScheduleID = @ScheduleID
    ORDER BY c.CarriageCode, se.SeatNumber;
END
GO
-- SP3: ĐẶT VÉ (TỐI ƯU CAO - HỖ TRỢ ĐẶT NHIỀU GHẾ)
CREATE OR ALTER PROCEDURE sp_DatVe
    @UserID          INT,
    @ScheduleID      INT,
    @SeatIDs         NVARCHAR(MAX),      -- '5,7,12,15'
    @PassengerNames  NVARCHAR(MAX),      -- 'Nguyễn Văn A|Trần Thị B|...'
    @PassengerIDs    NVARCHAR(MAX),
    @PassengerPhones NVARCHAR(MAX) = NULL,
    @PaymentMethod   NVARCHAR(50),
    @DiscountCode    NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @TicketTable TABLE (TicketID INT, TicketCode NVARCHAR(20));
    DECLARE @CurrentTime DATETIME2 = GETDATE();
    DECLARE @TotalAmount DECIMAL(12,0) = 0;
    DECLARE @FinalPricePerTicket DECIMAL(12,0);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validation: Lịch trình hợp lệ
        IF NOT EXISTS (
            SELECT 1 FROM Schedules 
            WHERE ScheduleID = @ScheduleID 
              AND Status = N'Scheduled' 
              AND IsActive = 1 
              AND DepartureTime > @CurrentTime
        )
            RAISERROR(N'Lịch trình không hợp lệ hoặc đã khởi hành.', 16, 1);

        -- Tạo bảng tạm chứa danh sách ghế + hành khách
        CREATE TABLE #BookingTemp (
            SeatID         INT,
            PassengerName  NVARCHAR(100),
            PassengerID    NVARCHAR(20),
            PassengerPhone NVARCHAR(15)
        );

		-- Parse chuỗi thành bảng và gán thông tin hành khách
		-- Với đặt 1 ghế: @SeatIDs='5', @PassengerNames='Nguyen Van A', @PassengerIDs='012345'
		-- Với nhiều ghế: '5,7' | 'Nguyen Van A|Tran Thi B' | '012|034'
		;WITH SeatList AS (
			SELECT TRY_CAST(value AS INT) AS SeatID,
				   ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rn
			FROM STRING_SPLIT(@SeatIDs, ',')
		),
		NameList AS (
			SELECT value AS PassengerName,
				   ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rn
			FROM STRING_SPLIT(@PassengerNames, '|')
		),
		IDList AS (
			SELECT value AS PassengerID,
				   ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rn
			FROM STRING_SPLIT(@PassengerIDs, '|')
		),
		PhoneList AS (
			SELECT value AS PassengerPhone,
				   ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rn
			FROM STRING_SPLIT(ISNULL(@PassengerPhones, ''), '|')
		)
		INSERT INTO #BookingTemp (SeatID, PassengerName, PassengerID, PassengerPhone)
		SELECT sl.SeatID, nl.PassengerName, il.PassengerID,
			   NULLIF(ISNULL(pl.PassengerPhone, ''), '')
		FROM SeatList sl
		LEFT JOIN NameList  nl ON nl.rn  = sl.rn
		LEFT JOIN IDList    il ON il.rn  = sl.rn
		LEFT JOIN PhoneList pl ON pl.rn  = sl.rn;

        -- Kiểm tra ghế còn trống
        IF EXISTS (
            SELECT 1 FROM #BookingTemp t
            JOIN Tickets tk ON tk.SeatID = t.SeatID AND tk.ScheduleID = @ScheduleID
            WHERE tk.Status NOT IN (N'Cancelled', N'Expired')
        )
            RAISERROR(N'Một hoặc nhiều ghế đã được đặt.', 16, 1);

        -- Lấy giá vé
        SELECT @FinalPricePerTicket = dbo.fn_TinhGiaVe(sp.Price, @DiscountCode)
        FROM SchedulePrices sp
        WHERE sp.ScheduleID = @ScheduleID 
          AND sp.SeatType = (SELECT TOP 1 SeatType FROM Seats WHERE SeatID IN (SELECT SeatID FROM #BookingTemp));

        IF @FinalPricePerTicket IS NULL
            RAISERROR(N'Không tìm thấy giá vé.', 16, 1);

        SET @TotalAmount = @FinalPricePerTicket * (SELECT COUNT(*) FROM #BookingTemp);

        -- Tạo vé
        DECLARE @BaseCode NVARCHAR(20) = 'VE' + FORMAT(@CurrentTime, 'yyyyMMdd');
        DECLARE @MaxSeq INT;

        SELECT @MaxSeq = ISNULL(MAX(CAST(SUBSTRING(TicketCode, 11, 5) AS INT)), 0)
        FROM Tickets WHERE TicketCode LIKE @BaseCode + '%';

        INSERT INTO Tickets (
            TicketCode, UserID, ScheduleID, SeatID, 
            PassengerName, PassengerID, PassengerPhone,
            SeatType, OriginalPrice, DiscountAmount, FinalPrice,
            DiscountCode, Status, BookedAt
        )
        OUTPUT INSERTED.TicketID, INSERTED.TicketCode INTO @TicketTable
        SELECT 
            @BaseCode + RIGHT('0000' + CAST(@MaxSeq + ROW_NUMBER() OVER(ORDER BY bt.SeatID) AS NVARCHAR(5)), 5),
            @UserID, @ScheduleID, bt.SeatID,
            bt.PassengerName, bt.PassengerID, bt.PassengerPhone,
            s.SeatType, @FinalPricePerTicket, 0, @FinalPricePerTicket,
            @DiscountCode, N'Pending', @CurrentTime
        FROM #BookingTemp bt
        JOIN Seats s ON s.SeatID = bt.SeatID;

        -- Thanh toán
        INSERT INTO Payments (TicketID, Amount, PaymentMethod, Status)
        SELECT TicketID, @TotalAmount, @PaymentMethod, N'Pending'
        FROM @TicketTable;

        -- Cập nhật mã giảm giá
        IF @DiscountCode IS NOT NULL
            UPDATE Discounts SET UsedCount += 1 WHERE Code = @DiscountCode;

        -- Audit Log & Notification
        INSERT INTO AuditLogs (UserID, Action, TableName, RecordID, NewValues)
        SELECT @UserID, 'BOOK_TICKET', 'Tickets', TicketID, 
               JSON_MODIFY('{}', '$.TotalAmount', @TotalAmount)
        FROM @TicketTable;

        INSERT INTO Notifications (UserID, Title, Body, Type, RelatedID)
        VALUES (@UserID, N'Đặt vé thành công!', 
                N'Bạn đã đặt ' + CAST((SELECT COUNT(*) FROM @TicketTable) AS NVARCHAR) + 
                N' vé. Tổng tiền: ' + FORMAT(@TotalAmount, 'N0') + N' VNĐ', 
                'Success', (SELECT MIN(TicketID) FROM @TicketTable));

        COMMIT TRANSACTION;

        -- Trả kết quả
        SELECT t.TicketID, t.TicketCode, t.PassengerName, t.FinalPrice, 
               s.SeatNumber, c.CarriageCode, dep.StationName AS GaDi, arr.StationName AS GaDen
        FROM @TicketTable tt
        JOIN Tickets t ON t.TicketID = tt.TicketID
        JOIN Seats s ON s.SeatID = t.SeatID
        JOIN Carriages c ON c.CarriageID = s.CarriageID
        JOIN Schedules sc ON sc.ScheduleID = t.ScheduleID
        JOIN Routes r ON r.RouteID = sc.RouteID
        JOIN Stations dep ON dep.StationID = r.DepartureStation
        JOIN Stations arr ON arr.StationID = r.ArrivalStation;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- SP 4: Hủy vé (hỗ trợ hoàn tiền theo %)
CREATE OR ALTER PROCEDURE sp_HuyVe
    @TicketID     INT,
    @UserID       INT,
    @CancelReason NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Status    NVARCHAR(20);
        DECLARE @FinalPrice DECIMAL(12,0);
        DECLARE @GioDi     DATETIME2;

        SELECT @Status = t.Status, @FinalPrice = t.FinalPrice, @GioDi = sc.DepartureTime
        FROM Tickets t
        JOIN Schedules sc ON t.ScheduleID = sc.ScheduleID
        WHERE t.TicketID = @TicketID AND t.UserID = @UserID;

        IF @Status IS NULL
        BEGIN ROLLBACK; RAISERROR(N'Không tìm thấy vé.', 16, 1); RETURN; END

        IF @Status = N'Cancelled'
        BEGIN ROLLBACK; RAISERROR(N'Vé đã bị hủy trước đó.', 16, 1); RETURN; END

        IF @Status = N'Used'
        BEGIN ROLLBACK; RAISERROR(N'Không thể hủy vé đã sử dụng.', 16, 1); RETURN; END

        -- Tính phí hoàn trả theo thời gian trước giờ đi
        DECLARE @HoursLeft FLOAT = DATEDIFF(HOUR, GETDATE(), @GioDi);
        DECLARE @RefundPct DECIMAL(5,2) =
            CASE
                WHEN @HoursLeft >= 72 THEN 100  -- Hoàn 100% nếu hủy trước 3 ngày
                WHEN @HoursLeft >= 24 THEN 70   -- Hoàn 70% nếu hủy trước 1 ngày
                WHEN @HoursLeft >= 4  THEN 30   -- Hoàn 30% nếu hủy trước 4 giờ
                ELSE 0                           -- Không hoàn nếu quá gần giờ đi
            END;
        DECLARE @RefundAmount DECIMAL(12,0) = @FinalPrice * @RefundPct / 100;

        -- Hủy vé
        UPDATE Tickets
        SET Status      = N'Cancelled',
            CancelledAt = GETDATE(),
            CancelReason = @CancelReason,
            UpdatedAt   = GETDATE()
        WHERE TicketID = @TicketID;

        -- Cập nhật thanh toán -> Refunded
        UPDATE Payments
        SET Status = CASE WHEN @RefundAmount > 0 THEN N'Refunded' ELSE N'Failed' END,
            RefundedAt   = GETDATE(),
            RefundAmount = @RefundAmount
        WHERE TicketID = @TicketID AND Status = N'Success';

        -- Audit log
        INSERT INTO AuditLogs (UserID, Action, TableName, RecordID, NewValues)
        VALUES (@UserID, 'CANCEL_TICKET', 'Tickets', @TicketID,
                N'{"CancelReason":"' + ISNULL(@CancelReason, '') + N'","RefundAmount":' + CAST(@RefundAmount AS NVARCHAR) + N'}');

        -- Thông báo
        INSERT INTO Notifications (UserID, Title, Body, Type, RelatedID)
        SELECT UserID, N'Hủy vé thành công',
               N'Vé #' + TicketCode + N' đã hủy. Hoàn tiền: ' + FORMAT(@RefundAmount,'N0') + N' VNĐ (' + CAST(@RefundPct AS NVARCHAR) + N'%)',
               'Info', @TicketID
        FROM Tickets WHERE TicketID = @TicketID;

        COMMIT TRANSACTION;

        -- Trả kết quả
        SELECT 1 AS Success, @RefundAmount AS RefundAmount, @RefundPct AS RefundPercent;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

-- SP 5: Xác nhận thanh toán
CREATE OR ALTER PROCEDURE sp_XacNhanThanhToan
    @TicketID      INT,
    @TransactionID NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Kiểm tra vé tồn tại và đang Pending
        IF NOT EXISTS (SELECT 1 FROM Tickets WHERE TicketID = @TicketID AND Status = N'Pending')
        BEGIN
            ROLLBACK;
            RAISERROR(N'Vé không ở trạng thái Pending.', 16, 1);
            RETURN;
        END

        -- Cập nhật vé -> Confirmed
        UPDATE Tickets
        SET Status = N'Confirmed', UpdatedAt = GETDATE()
        WHERE TicketID = @TicketID;

        -- Cập nhật thanh toán -> Success
        UPDATE Payments
        SET Status = N'Success', PaidAt = GETDATE(),
            TransactionID = @TransactionID
        WHERE TicketID = @TicketID AND Status = N'Pending';

        COMMIT TRANSACTION;
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO

-- SP 6: Lấy danh sách vé (filter linh hoạt)
CREATE OR ALTER PROCEDURE sp_LayDanhSachVe
    @UserID   INT  = NULL,
    @Status   NVARCHAR(20) = NULL,
    @TuNgay   DATE = NULL,
    @DenNgay  DATE = NULL,
    @MaVe     NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        t.TicketID, t.TicketCode, t.Status,
        t.PassengerName, t.PassengerID,
        t.SeatType, t.FinalPrice,
        t.BookedAt, t.CancelledAt,
        sc.DepartureTime AS GioDi, sc.ArrivalTime AS GioDen,
        tr.TrainCode AS MaTau,
        dep.StationName AS GaDi, arr.StationName AS GaDen,
        c.CarriageCode AS MaToa, se.SeatNumber AS SoGhe,
        p.PaymentMethod, p.Status AS TrangThaiTT,
        u.FullName AS NguoiDat
    FROM Tickets t
    JOIN Users       u   ON t.UserID     = u.UserID
    JOIN Schedules   sc  ON t.ScheduleID = sc.ScheduleID
    JOIN Trains      tr  ON sc.TrainID   = tr.TrainID
    JOIN Routes      r   ON sc.RouteID   = r.RouteID
    JOIN Stations    dep ON r.DepartureStation = dep.StationID
    JOIN Stations    arr ON r.ArrivalStation   = arr.StationID
    JOIN Seats       se  ON t.SeatID     = se.SeatID
    JOIN Carriages   c   ON se.CarriageID = c.CarriageID
    LEFT JOIN Payments p ON t.TicketID   = p.TicketID
    WHERE (@UserID IS NULL OR t.UserID = @UserID)
      AND (@Status IS NULL OR t.Status = @Status)
      AND (@TuNgay IS NULL OR CAST(t.BookedAt AS DATE) >= @TuNgay)
      AND (@DenNgay IS NULL OR CAST(t.BookedAt AS DATE) <= @DenNgay)
      AND (@MaVe IS NULL OR t.TicketCode LIKE '%' + @MaVe + '%')
    ORDER BY t.BookedAt DESC;
END
GO

-- SP 7: Báo cáo doanh thu nâng cao
CREATE OR ALTER PROCEDURE sp_BaoCaoDoanhThu
    @Nam     INT,
    @Thang   INT  = NULL,
    @RouteID INT  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Doanh thu tổng hợp
    SELECT
        YEAR(t.BookedAt)   AS Nam,
        MONTH(t.BookedAt)  AS Thang,
        r.RouteName,
        dep.StationName + N' → ' + arr.StationName AS TuyenDuong,
        COUNT(*)           AS SoVeBan,
        SUM(t.FinalPrice)  AS DoanhThu,
        AVG(t.FinalPrice)  AS GiaTrungBinh,
        SUM(t.DiscountAmount) AS TongGiamGia,
        COUNT(CASE WHEN t.Status = N'Cancelled' THEN 1 END) AS SoVeHuy
    FROM Tickets t
    JOIN Schedules sc ON t.ScheduleID = sc.ScheduleID
    JOIN Routes    r  ON sc.RouteID   = r.RouteID
    JOIN Stations dep ON r.DepartureStation = dep.StationID
    JOIN Stations arr ON r.ArrivalStation   = arr.StationID
    WHERE YEAR(t.BookedAt) = @Nam
      AND (@Thang  IS NULL OR MONTH(t.BookedAt) = @Thang)
      AND (@RouteID IS NULL OR r.RouteID = @RouteID)
      AND t.Status IN (N'Confirmed', N'Used', N'Cancelled')
    GROUP BY YEAR(t.BookedAt), MONTH(t.BookedAt),
             r.RouteName, dep.StationName, arr.StationName
    ORDER BY Nam, Thang, DoanhThu DESC;
END
GO

-- SP 8: Check-in vé (MỚI)
CREATE OR ALTER PROCEDURE sp_CheckIn
    @TicketCode NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TicketID  INT;
    DECLARE @Status    NVARCHAR(20);
    DECLARE @GioDi     DATETIME2;

    SELECT @TicketID = t.TicketID, @Status = t.Status, @GioDi = sc.DepartureTime
    FROM Tickets t
    JOIN Schedules sc ON t.ScheduleID = sc.ScheduleID
    WHERE t.TicketCode = @TicketCode;

    IF @TicketID IS NULL
    BEGIN RAISERROR(N'Không tìm thấy vé.', 16, 1); RETURN; END

    IF @Status != N'Confirmed'
    BEGIN RAISERROR(N'Vé phải ở trạng thái Confirmed để check-in.', 16, 1); RETURN; END

    -- Check-in chỉ được phép trong vòng 2 giờ trước giờ đi
    IF ABS(DATEDIFF(HOUR, GETDATE(), @GioDi)) > 2
    BEGIN RAISERROR(N'Check-in chỉ được phép trong vòng 2 giờ trước giờ tàu.', 16, 1); RETURN; END

    UPDATE Tickets
    SET CheckedIn = 1, CheckInAt = GETDATE(), Status = N'Used', UpdatedAt = GETDATE()
    WHERE TicketID = @TicketID;

    SELECT 1 AS Success, @TicketCode AS TicketCode, GETDATE() AS CheckInAt;
END
GO


-- =====================================================
-- DỮ LIỆU MẪU
-- =====================================================

INSERT INTO Roles (RoleName, Description) VALUES
(N'Admin',    N'Quản trị viên hệ thống, toàn quyền'),
(N'Staff',    N'Nhân viên bán vé tại quầy'),
(N'Customer', N'Khách hàng đặt vé online');
GO

INSERT INTO Users (FullName, Email, PhoneNumber, PasswordHash, IDNumber, Gender) VALUES
(N'Quản Trị Viên',  'admin@trainticket.vn',   '0900000001', '$2a$11$hashedAdmin123',  '001099000001', N'Nam'),
(N'Nguyễn Văn An',  'staff1@trainticket.vn',  '0900000002', '$2a$11$hashedStaff123',  '001099000002', N'Nam'),
(N'Trần Thị Bình',  'customer1@gmail.com',    '0901234567', '$2a$11$hashedPass123',   '001099000003', N'Nữ'),
(N'Lê Văn Cường',   'customer2@gmail.com',    '0912345678', '$2a$11$hashedPass123',   '001099000004', N'Nam');
GO
INSERT INTO UserRoles (UserID, RoleID) VALUES (1,1),(2,2),(3,3),(4,3);
GO

INSERT INTO Stations (StationCode, StationName, City, Address, Latitude, Longitude) VALUES
(N'HAN', N'Ga Hà Nội', N'Hà Nội', N'120 Lê Duẩn, Hoàn Kiếm', 21.0245, 105.8412),
(N'GIA', N'Ga Giáp Bát', N'Hà Nội', N'366 Giải Phóng, Định Công', 20.9800, 105.8500),
(N'PHU', N'Ga Phủ Lý', N'Hà Nam', N'Phủ Lý', 20.5500, 105.9000),
(N'NAM', N'Ga Nam Định', N'Nam Định', N'Nam Định', 20.4200, 106.1700),
(N'NIN', N'Ga Ninh Bình', N'Ninh Bình', N'Ninh Bình', 20.2500, 105.9800),
(N'THA', N'Ga Thanh Hóa', N'Thanh Hóa', N'Thanh Hóa', 19.8000, 105.7800),
(N'VIN', N'Ga Vinh', N'Nghệ An', N'Lê Nin, TP. Vinh', 18.6796, 105.6813),
(N'DHO', N'Ga Đồng Hới', N'Quảng Bình', N'Đồng Hới', 17.4800, 106.6000),
(N'HUE', N'Ga Huế', N'Thừa Thiên Huế', N'2 Bùi Thị Xuân, TP. Huế', 16.4637, 107.5909),
(N'DAN', N'Ga Đà Nẵng', N'Đà Nẵng', N'202 Hải Phòng, Thanh Khê', 16.0678, 108.2208),
(N'TAM', N'Ga Tam Kỳ', N'Quảng Nam', N'Tam Kỳ', 15.5700, 108.5000),
(N'QNG', N'Ga Quảng Ngãi', N'Quảng Ngãi', N'Quảng Ngãi', 15.1200, 108.8000),
(N'DIE', N'Ga Diêu Trì', N'Bình Định', N'Tuy Phước, Bình Định', 13.9500, 109.1000),
(N'TUY', N'Ga Tuy Hòa', N'Phú Yên', N'Tuy Hòa', 13.0800, 109.3000),
(N'NHA', N'Ga Nha Trang', N'Khánh Hòa', N'17 Thái Nguyên, TP. Nha Trang', 12.2388, 109.1967),
(N'TCH', N'Ga Tháp Chàm', N'Ninh Thuận', N'Tháp Chàm', 11.6000, 108.9500),
(N'BTH', N'Ga Bình Thuận', N'Bình Thuận', N'Phan Thiết', 10.9300, 108.1000),
(N'BIH', N'Ga Biên Hòa', N'Đồng Nai', N'Biên Hòa', 10.9500, 106.8200),
(N'SGN', N'Ga Sài Gòn', N'TP.HCM', N'1 Nguyễn Thông, Q.3', 10.7727, 106.6808)
GO

INSERT INTO Trains (TrainCode, TrainName, TrainType, MaxSpeed) VALUES
(N'SE1', N'Super Express SE1', N'SE', 120),
(N'SE2', N'Super Express SE2', N'SE', 120),
(N'SE3', N'Super Express SE3', N'SE', 120),
(N'SE4', N'Super Express SE4', N'SE', 120),
(N'SE5', N'Super Express SE5', N'SE', 110),
(N'SE6', N'Super Express SE6', N'SE', 110),
(N'SE7', N'Super Express SE7', N'SE', 110),
(N'SE8', N'Super Express SE8', N'SE', 110),
(N'TN1', N'Thống Nhất TN1', N'TN', 90),
(N'SNT1', N'Sài Gòn - Nha Trang SNT1', N'Local', 100),
(N'SPT1', N'Sài Gòn - Phan Thiết SPT1', N'Tourist', 90);
GO

-- Routes (Dùng StationID thay vì StationCode)
INSERT INTO Routes (RouteName, DepartureStation, ArrivalStation, Distance, EstimatedHours) VALUES
(N'Hà Nội - Sài Gòn', 1, 19, 1726.0, 32.0),
(N'Sài Gòn - Hà Nội', 19, 1, 1726.0, 32.0),
(N'Hà Nội - Đà Nẵng', 1, 10, 791.0, 14.5),
(N'Đà Nẵng - Sài Gòn', 10, 19, 935.0, 17.5),
(N'Hà Nội - Nha Trang', 1, 15, 1315.0, 24.0),
(N'Hà Nội - Vinh', 1, 7, 319.0, 6.0),
(N'Ga Diêu Trì - Ga Sài Gòn',  13, 19, 620.0,  8.0),
(N'Ga Sài Gòn - Ga Diêu Trì',  19, 13, 620.0,  8.0),
(N'Ga Hà Nội - Ga Diêu Trì',    1, 13, 1065.0, 15.0),
(N'Ga Diêu Trì - Ga Hà Nội',   13,  1, 1065.0, 15.0),
(N'Ga Đà Nẵng - Ga Sài Gòn',   10, 19, 935.0,  12.0),
(N'Ga Sài Gòn - Ga Đà Nẵng',   19, 10, 935.0,  12.0),
(N'Ga Hà Nội - Ga Huế',         1,  9, 688.0,  10.0),
(N'Ga Hà Nội - Ga Nha Trang',   1, 15, 1315.0, 18.0),
(N'Ga Nha Trang - Ga Sài Gòn', 15, 19, 411.0,   6.0);
GO
INSERT INTO Carriages (TrainID, CarriageCode, CarriageType, TotalSeats) VALUES
(1,N'T1',N'Ngồi mềm',64),(1,N'T2',N'Ngồi mềm',64),
(1,N'T3',N'Nằm cứng 6',42),(1,N'T4',N'Nằm cứng 6',42),
(1,N'T5',N'Nằm mềm 4',28),(1,N'T6',N'Nằm mềm 4',28);
GO

-- Tạo ghế toa T1
DECLARE @cid INT = 1, @r CHAR(1), @col INT, @rows NVARCHAR(8) = 'ABCD', @i INT = 1;
WHILE @i <= 4
BEGIN
    SET @r = SUBSTRING(@rows, @i, 1); SET @col = 1;
    WHILE @col <= 16
    BEGIN
        INSERT INTO Seats (CarriageID,SeatNumber,SeatType) VALUES (@cid, @r+CAST(@col AS NVARCHAR), N'Ngồi mềm');
        SET @col = @col + 1;
    END
    SET @i = @i + 1;
END
GO

-- Toa T3 nằm cứng
DECLARE @cid3 INT = 3, @r3 CHAR(1), @rows3 NVARCHAR(8)='ABCDEF', @j INT=1;
WHILE @j <= 7
BEGIN
    SET @r3 = SUBSTRING(@rows3, @j, 1);
    INSERT INTO Seats (CarriageID,SeatNumber,SeatType) VALUES (@cid3,@r3+N'1',N'Nằm cứng tầng 1');
    INSERT INTO Seats (CarriageID,SeatNumber,SeatType) VALUES (@cid3,@r3+N'2',N'Nằm cứng tầng 2');
    INSERT INTO Seats (CarriageID,SeatNumber,SeatType) VALUES (@cid3,@r3+N'3',N'Nằm cứng tầng 3');
    SET @j = @j + 1;
END
GO

SELECT * FROM Routes;
INSERT INTO Schedules (TrainID, RouteID, DepartureTime, ArrivalTime, Status, Platform) VALUES
(1, 1, '2026-06-19 06:00', '2026-06-20 14:00', N'Scheduled', '2'),   -- HAN -> SGN
(1, 1, '2026-06-21 06:00', '2026-06-22 14:00', N'Scheduled', '2'),
(2, 3, '2026-06-20 08:00', '2026-06-20 22:30', N'Scheduled', '1'),   -- HAN -> DAN
(1, 5, '2026-06-23 07:00', '2026-06-24 07:00', N'Scheduled', '3'),   -- HAN -> NHA
(3, 2, '2026-06-22 09:00', '2026-06-23 17:00', N'Scheduled', '4'),   -- SGN -> HAN
(1, 7, '2026-06-19 14:00', '2026-06-19 22:00', N'Scheduled', '1'),   -- DTR -> SGN
(1, 7, '2026-06-21 14:00', '2026-06-21 22:00', N'Scheduled', '1'),
(1, 9, '2026-06-19 06:00', '2026-06-19 21:00', N'Scheduled', '2'),   -- HAN -> DTR
(2, 11,'2026-06-20 10:00', '2026-06-20 22:00', N'Scheduled', '3'),   -- DAN -> SGN
(1, 14,'2026-06-23 06:00', '2026-06-24 00:00', N'Scheduled', '2'),   -- HAN -> NHA
(3, 8, '2026-06-22 06:00', '2026-06-22 14:00', N'Scheduled', '4'),   -- SGN -> DTR
(1, 15,'2026-06-20 06:00', '2026-06-20 12:00', N'Scheduled', '1');   -- NHA -> SGN
GO

INSERT INTO SchedulePrices (ScheduleID,SeatType,Price)
SELECT s.ScheduleID, p.SeatType, p.Price
FROM Schedules s
CROSS JOIN (
    SELECT DISTINCT se.SeatType,
        CASE se.SeatType
            WHEN N'Ngồi mềm'          THEN 600000
            WHEN N'Nằm cứng tầng 1'   THEN 900000
            WHEN N'Nằm cứng tầng 2'   THEN 850000
            WHEN N'Nằm cứng tầng 3'   THEN 800000
            ELSE 500000
        END AS Price
    FROM Seats se
) AS p;
GO

-- Mã giảm giá mẫu
INSERT INTO Discounts (Code,Description,DiscountType,Amount,MinPrice,MaxUses,ValidFrom,ValidTo) VALUES
(N'SALE20','Giảm 20% cho vé mới','Percent',20,300000,100,'2025-01-01','2025-12-31'),
(N'VIP100K','Giảm 100K cho VIP','Fixed',100000,500000,50,'2025-01-01','2025-12-31');
GO

PRINT N'=== TrainTicketDB v2.0 tạo thành công! ===';
PRINT N'Tổng bảng: 15 | SPs: 8 | Views: 3 | Functions: 2 | Triggers: 1';
GO

SELECT 
    'Users' AS Bang, COUNT(*) AS SoDong FROM Users
UNION ALL
SELECT 'Stations', COUNT(*) FROM Stations
UNION ALL
SELECT 'Routes', COUNT(*) FROM Routes
UNION ALL
SELECT 'Schedules', COUNT(*) FROM Schedules
UNION ALL
SELECT 'Seats', COUNT(*) FROM Seats
UNION ALL
SELECT 'Tickets', COUNT(*) FROM Tickets;