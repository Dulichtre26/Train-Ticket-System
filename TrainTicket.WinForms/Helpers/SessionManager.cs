using TrainTicket.Business.DTOs;
using TrainTicket.Data.Helpers;

namespace TrainTicket.WinForms.Helpers
{
    // Quản lý trạng thái đăng nhập và Phiên làm việc (Session)
    public static class SessionManager
    {
        // User đang đăng nhập.
        public static UserSessionDto? CurrentUser { get; private set; }

        public static string CurrentRegion { get; set; } = RegionHelper.HQ;

        // Role hiện tại
        public static string CurrentRole => CurrentUser?.Roles.FirstOrDefault() ?? string.Empty;

        public static bool IsLoggedIn => CurrentUser is not null;

        // [MỚI] Timeout: session quá 8h thì coi như hết hạn
        public static bool IsSessionExpired =>
            CurrentUser != null && (DateTime.Now - CurrentUser.LoginAt).TotalHours > 8;

        public static void SetSession(UserSessionDto user, string? region = null)
        {
            CurrentUser   = user;
            CurrentRegion = region ?? RegionHelper.HQ;
        }

        public static void Clear()
        {
            CurrentUser   = null;
            CurrentRegion = RegionHelper.HQ;
        }
    }
}
