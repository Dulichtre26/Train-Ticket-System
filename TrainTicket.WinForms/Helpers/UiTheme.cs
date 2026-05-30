using System.Text.Json;

namespace TrainTicket.WinForms.Helpers
{
    // Quản lý màu giao diện toàn cục với color palette hài hòa và cân đối.
    // Dựa trên Material Design 3 với primary color xanh dương và accent màu cam.
    public static class UiTheme
    {
        private const string ThemeFileName = "theme.json";

        public static bool IsDark { get; private set; }

        // === PRIMARY — Indigo (tin cậy, chuyên nghiệp) ===
        public static Color Primary      => Color.FromArgb(99,  102, 241); // Indigo-500
        public static Color PrimaryLight => Color.FromArgb(165, 180, 252); // Indigo-300
        public static Color PrimaryDark  => Color.FromArgb(67,  56,  202); // Indigo-700
        public static Color PrimaryHover => Color.FromArgb(79,  70,  229); // Indigo-600

        // === SECONDARY/ACTION COLOR PALETTE ===
        public static Color Secondary      => Color.FromArgb(245, 158, 11);  // Amber-500
        public static Color SecondaryLight => Color.FromArgb(252, 211, 77);  // Amber-300
        public static Color SecondaryDark  => Color.FromArgb(180, 107,  5);  // Amber-700

        // === SURFACE COLORS ===
        public static Color Background => IsDark ? Color.FromArgb(15,  23, 42)  : Color.FromArgb(241, 245, 249); // slate-900 / slate-100
        public static Color Surface => IsDark ? Color.FromArgb(30,  41, 59)  : Color.FromArgb(255, 255, 255); // slate-800 / white
        public static Color SurfaceVariant => IsDark ? Color.FromArgb(51,  65, 85)  : Color.FromArgb(248, 250, 252); // slate-700 / slate-50
        public static Color Sidebar => IsDark ? Color.FromArgb(15,  23, 42)  : Color.FromArgb(30,  41, 59);   // dark always, light dùng slate-800 (n?i b?t)

        // === NAV BUTTON COLORS — trên n?n Sidebar t?i ===
        public static Color NavButton      => Color.FromArgb(51,  65,  85);  // slate-700
        public static Color NavButtonHover => Color.FromArgb(99, 102, 241);  // Primary
        public static Color NavText        => Color.FromArgb(203, 213, 225); // slate-300

        // === TEXT COLORS ===
        public static Color TextPrimary => IsDark ? Color.FromArgb(226, 232, 240) : Color.FromArgb(15,  23, 42);  // slate-200 / slate-900
        public static Color TextSecondary => IsDark ? Color.FromArgb(148, 163, 184) : Color.FromArgb(71,  85, 105); // slate-400 / slate-600
        public static Color TextTertiary => Color.FromArgb(100, 116, 139);                                         // slate-500

        // === HEADER GRADIENT START/END (dùng cho form header) ===
        public static Color HeaderGradientStart => Color.FromArgb(67, 56, 202);  // Indigo-700
        public static Color HeaderGradientEnd   => Color.FromArgb(99, 102, 241); // Indigo-500

        // === SEMANTIC COLORS ===
        public static Color Success => Color.FromArgb(76, 175, 80);       // Green 500
        public static Color Error => Color.FromArgb(244, 67, 54);        // Red 500
        public static Color Warning => Color.FromArgb(255, 152, 0);      // Orange 500
        public static Color Info => Color.FromArgb(33, 150, 243);        // Blue 500

        // === BORDER & DIVIDER ===
        public static Color Border => IsDark ? Color.FromArgb(55, 55, 55) : Color.FromArgb(224, 224, 224);
        public static Color Divider => IsDark ? Color.FromArgb(45, 45, 45) : Color.FromArgb(238, 238, 238);

                public static void StyleGrid(Guna.UI2.WinForms.Guna2DataGridView grid)
        {
            grid.ThemeStyle.HeaderStyle.BackColor = PrimaryDark;
            grid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            grid.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            
            grid.ThemeStyle.RowsStyle.BackColor = Background;
            grid.ThemeStyle.RowsStyle.ForeColor = TextPrimary;
            grid.ThemeStyle.RowsStyle.SelectionBackColor = Primary;
            grid.ThemeStyle.RowsStyle.SelectionForeColor = Color.White;

            grid.ThemeStyle.AlternatingRowsStyle.BackColor = SurfaceVariant;
            grid.ThemeStyle.AlternatingRowsStyle.ForeColor = TextPrimary;
            grid.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Primary;
            grid.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.White;
            
            grid.GridColor = Border;
        }

        public static void Toggle()
        {
            IsDark = !IsDark;
            Save();
        }

        public static void Load()
        {
            try
            {
                var path = GetThemeFilePath();
                if (!File.Exists(path))
                {
                    IsDark = false;
                    return;
                }

                var json = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<ThemeState>(json);
                IsDark = data?.IsDark ?? false;
            }
            catch
            {
                IsDark = false;
            }
        }

        public static void Save()
        {
            try
            {
                var path = GetThemeFilePath();
                var data = new ThemeState { IsDark = IsDark };
                var json = JsonSerializer.Serialize(data);
                File.WriteAllText(path, json);
            }
            catch
            {
                // Ignore save errors
            }
        }

        private static string GetThemeFilePath()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(appData, "TrainTicket");
            Directory.CreateDirectory(appFolder);
            return Path.Combine(appFolder, ThemeFileName);
        }

        private class ThemeState
        {
            public bool IsDark { get; set; }
        }
    }
}
