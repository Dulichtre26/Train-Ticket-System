namespace TrainTicket.Business.DTOs
{
    // DTO điều kiện tìm chuyến tàu.
    // Mapping trực tiếp vào tham số của stored procedure sp_TimChuyenTau.
    public class SearchScheduleDto
    {
        public int GaDi { get; set; }
        public int GaDen { get; set; }
        public DateTime NgayDi { get; set; }
        public string? SeatType { get; set; } // [M?I] filter loại ghế
        public decimal? MaxPrice { get; set; } // [M?I] filter giá t?i ?a
    }
}
