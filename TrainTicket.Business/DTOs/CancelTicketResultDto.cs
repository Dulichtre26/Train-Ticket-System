// ============================================================
// FILE: TrainTicket.Business/DTOs/CancelTicketResultDto.cs (M?I)
// ============================================================
namespace TrainTicket.Business.DTOs
{
    public class CancelTicketResultDto
    {
        public bool    Success        { get; set; }
        public decimal RefundAmount   { get; set; }
        public decimal RefundPercent  { get; set; }
        public string  Message        { get; set; } = string.Empty;
    }
}