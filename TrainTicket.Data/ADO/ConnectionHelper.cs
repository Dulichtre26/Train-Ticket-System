// ============================================================
// FILE: TrainTicket.Data/ADO/ConnectionHelper.cs — NÂNG C?P
// C?i ti?n:
//   - ??c connection string t? appsettings.json n?u có
//   - H? tr? connection string v?i server/database tùy ch?nh
//   - Thêm helper BuildConnectionString
// ============================================================
using System.Text.Json;

namespace TrainTicket.Data.ADO
{
    public static class ConnectionHelper
    {
        private static string _current = DefaultConnection;

        // Connection strings m?c ??nh
        public static string DefaultConnection =>
            BuildConnectionString("localhost", "TrainTicketDB");

        public static string NorthConnection =>
            BuildConnectionString("localhost", "TrainTicketDB_North");

        public static string CentralConnection =>
            BuildConnectionString("localhost", "TrainTicketDB_Central");

        public static string SouthConnection =>
            BuildConnectionString("localhost", "TrainTicketDB_South");

        /// <summary>Connection string ?ang ???c ch?n (có th? thay ??i runtime)</summary>
        public static string CurrentConnectionString
        {
            get => _current;
            set => _current = value;
        }

        /// <summary>T?o connection string chu?n t? server + database</summary>
        public static string BuildConnectionString(string server, string database,
            string? user = null, string? password = null)
        {
            if (user != null && password != null)
                return $"Server={server};Database={database};User Id={user};Password={password};" +
                       "TrustServerCertificate=True;";

            return $"Server={server};Database={database};" +
                   "Trusted_Connection=True;TrustServerCertificate=True;";
        }

        /// <summary>Load connection string t? file config n?u t?n t?i</summary>
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
            catch { /* Gi? nguyên default n?u l?i */ }
        }

        /// <summary>Validate connection string hi?n t?i</summary>
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
