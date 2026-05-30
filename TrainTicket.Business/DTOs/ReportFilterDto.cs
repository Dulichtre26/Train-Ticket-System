namespace TrainTicket.Business.DTOs
{
    // DTO tham số lọc báo cáo doanh thu.
    // Có thể lọc theo năm, tháng hoặc tuyến (tùy màn hình báo cáo).
    public class ReportFilterDto
    {
        public int Year { get; set; } = DateTime.Now.Year;
        public int? Month { get; set; }
        public int? RouteID { get; set; }
    }
}
