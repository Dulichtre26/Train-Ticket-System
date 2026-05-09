using System;

namespace TrainTicket.Business.DTOs
{
    public class TicketHistoryDto
    {
        public int TicketID { get; set; }
        public string TicketCode { get; set; } = string.Empty;
        public string PassengerName { get; set; } = string.Empty;
        public string SeatType { get; set; } = string.Empty;
        public decimal GiaVe { get; set; }
        public string TrangThaiVe { get; set; } = string.Empty;
        public DateTime GioDi { get; set; }
        public DateTime GioDen { get; set; }
        public string GaDi { get; set; } = string.Empty;
        public string GaDen { get; set; } = string.Empty;
        public string MaTau { get; set; } = string.Empty;
        public string MaToa { get; set; } = string.Empty;
        public string SoGhe { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RegionCode { get; set; } = string.Empty;
    }
}
