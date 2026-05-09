using TrainTicket.Business.DTOs;
using TrainTicket.Data.Helpers;

namespace TrainTicket.WinForms.Helpers
{
    // Qu?n lý tr?ng thái ??ng nh?p và Phiên làm vi?c (Session)
    public static class SessionManager
    {
        // User ?ang ??ng nh?p.
        public static UserSessionDto? CurrentUser { get; private set; }

        public static string CurrentRegion { get; set; } = RegionHelper.HQ;

        // Role hi?n t?i
        public static string CurrentRole => CurrentUser?.Roles.FirstOrDefault() ?? string.Empty;

        public static bool IsLoggedIn => CurrentUser is not null;

        public static void SetSession(UserSessionDto user)
        {
            CurrentUser = user;
        }

        public static void Clear()
        {
            CurrentUser = null;
        }
    }
}
