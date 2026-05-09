namespace TrainTicket.Data.Helpers
{
    public static class RegionHelper
    {
        public const string HQ = "HQ";
        public const string North = "North";
        public const string Central = "Central";
        public const string South = "South";

        public static string GetRegionName(string code)
        {
            return code switch
            {
                "North" => "Mi?n B?c",
                "Central" => "Mi?n Trung",
                "South" => "Mi?n Nam",
                _ => "Tr? s? chính"
            };
        }
    }
}