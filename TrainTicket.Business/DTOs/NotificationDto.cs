namespace TrainTicket.Business.DTOs
{
    public class NotificationDto
    {
        public int NotiID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Type { get; set; } = "Info";
        public bool IsRead { get; set; }
        public int? RelatedID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}