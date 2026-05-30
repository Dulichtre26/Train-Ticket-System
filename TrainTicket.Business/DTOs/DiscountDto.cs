namespace TrainTicket.Business.DTOs
{
    public class DiscountDto
    {
        public int DiscountId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string DiscountType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal MinPrice { get; set; }
        public int? MaxUses { get; set; }
        public int UsedCount { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}