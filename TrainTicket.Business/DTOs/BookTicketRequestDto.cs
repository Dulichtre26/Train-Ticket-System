namespace TrainTicket.Business.DTOs
{
    /// DTO dữ liệu đầu vào khi thực hiện đặt vé
    /// Dùng để truyền đầy đủ tham số cho Stored Procedure sp_DatVe
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
        public string? DiscountCode { get; set; }   
    }
}
