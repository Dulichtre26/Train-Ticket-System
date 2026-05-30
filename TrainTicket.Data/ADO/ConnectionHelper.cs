// ============================================================
// FILE: TrainTicket.Data/ADO/ConnectionHelper.cs — NANG CAP
// Cai tien:
//   - Doc connection string tu appsettings.json neu co
//   - Ho tro connection string voi server/database tuy chinh
//   - Them helper BuildConnectionString
// ============================================================
using System.Text.Json;

namespace TrainTicket.Data.ADO
{
    public static class ConnectionHelper
    {
        private static string _current = DefaultConnection;

        // Connection strings mac dinh
        public static string DefaultConnection =>
            BuildConnectionString("localhost", "TrainTicketDB");

        public static string NorthConnection =>
            BuildConnectionString("localhost", "TrainTicketDB_North");

        public static string CentralConnection =>
            BuildConnectionString("localhost", "TrainTicketDB_Central");

        public static string SouthConnection =>
            BuildConnectionString("localhost", "TrainTicketDB_South");

        /// <summary>Connection string đang được chọn (có thể thay đổ  i runtime)</summary>
        public static string CurrentConnectionString
        {
            get => _current;
            set => _current = value;
        }

        /// <summary>Tạo connection string chuẩn từ server + database</summary>
        public static string BuildConnectionString(string server, string database,
            string? user = null, string? password = null)
        {
            if (user != null && password != null)
                return $"Server={server};Database={database};User Id={user};Password={password};" +
                       "TrustServerCertificate=True;";

            return $"Server={server};Database={database};" +
                   "Trusted_Connection=True;TrustServerCertificate=True;";
        }

        /// <summary>Load connection string tu file config neu ton tai</summary>
        public static void LoadFromConfig(string configPath = "appsettings.json")
        {
            try
            {
                if (!File.Exists(configPath)) return;
                var json  = File.ReadAllText(configPath);
                var doc   = JsonDocument.Parse(json);
                var conn  = doc.RootElement
                               .GetProperty("ConnectionStrings")
                               .GetProperty("DefaultConnection")
                               .GetString();
                if (!string.IsNullOrEmpty(conn))
                    _current = conn;
            }
            catch { /* Giữ nguyên default nếu lỗi */ }
        }

        /// <summary>Validate connection string hiện tại</summary>
        public static bool IsValid()
        {
            try
            {
                var helper = new AdoHelper(_current);
                return helper.TestConnection();
            }
            catch { return false; }
        }
    }
}
