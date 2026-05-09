using System;

namespace TrainTicket.Business.DTOs
{
    public class TrainDto
    {
        public int TrainID { get; set; }
        public string TrainCode { get; set; } = string.Empty;
        public string TrainName { get; set; } = string.Empty;
        public string TrainType { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string RegionCode { get; set; } = "HQ";
    }
}
