namespace TrainTicket.Business.DTOs
{
    // DTO d? li?u ??u vào khi th?c hi?n ??t vé.
    // Dùng ?? truy?n ??y ?? tham s? cho sp_DatVe.
    public class BookTicketRequestDto
    {
        public int UserID { get; set; }
        public int ScheduleID { get; set; }
        public int SeatID { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string PassengerID { get; set; } = string.Empty;
        public string? PassengerPhone { get; set; }
        public string SeatType { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = "Cash";
        public string? DiscountCode { get; set; }   // [M?I]
    }
}
