namespace TrainTicket.Business.DTOs
{
    // DTO ?i?u ki?n tìm chuy?n tàu.
    // Mapping tr?c ti?p vào tham s? c?a stored procedure sp_TimChuyenTau.
    public class SearchScheduleDto
    {
        public int GaDi { get; set; }
        public int GaDen { get; set; }
        public DateTime NgayDi { get; set; }
        public string? SeatType { get; set; } // [M?I] filter lo?i gh?
        public decimal? MaxPrice { get; set; } // [M?I] filter giá t?i ?a
    }
}
