namespace TrainTicket.Business.DTOs
{
    // DTO tham s? l?c báo cáo doanh thu.
    // Có th? l?c theo n?m, tháng ho?c tuy?n (tùy màn hình báo cáo).
    public class ReportFilterDto
    {
        public int Year { get; set; } = DateTime.Now.Year;
        public int? Month { get; set; }
        public int? RouteID { get; set; }
    }
}
