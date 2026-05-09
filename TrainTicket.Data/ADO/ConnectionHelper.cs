namespace TrainTicket.Data.ADO
{
    // Qu?n lý connection string v?i h? tr? phân m?nh.
    public static class ConnectionHelper
    {
        // Connection default (Main / T?ng)
        public static string DefaultConnection =>
            "Server=localhost;Database=TrainTicketDB;" +
            "Trusted_Connection=True;TrustServerCertificate=True;";

        // Connection Phân m?nh (Site B?c)
        public static string NorthConnection =>
            "Server=localhost;Database=TrainTicketDB_North;" +
            "Trusted_Connection=True;TrustServerCertificate=True;";

        // Connection Phân m?nh (Site Trung)
        public static string CentralConnection =>
            "Server=localhost;Database=TrainTicketDB_Central;" +
            "Trusted_Connection=True;TrustServerCertificate=True;";

        // Connection Phân m?nh (Site Nam)
        public static string SouthConnection =>
            "Server=localhost;Database=TrainTicketDB_South;" +
            "Trusted_Connection=True;TrustServerCertificate=True;";

        // Bi?n l?u tr? Connection String hi?n t?i ???c ch?n (M?c ??nh là Main)
        public static string CurrentConnectionString { get; set; } = DefaultConnection;
    }
}
