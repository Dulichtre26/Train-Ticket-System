namespace TrainTicket.Business.DTOs
{
    // DTO dòng dỡ liệu sơ đồ ghế.
    // M?i record t??ng ?ng 1 gh? tr? v? t? sp_XemSoDoGhe.
    public class SeatMapDto
    {
        public string MaToa { get; set; } = string.Empty;
        public string LoaiToa { get; set; } = string.Empty;
        public string SoGhe { get; set; } = string.Empty;
        public string LoaiGhe { get; set; } = string.Empty;
        public string HangGhe { get; set; } = "Economic";
        public bool HasSocket { get; set; } = false;
        public int SeatID { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public decimal GiaVe { get; set; }
        public bool IsAvailable => TrangThai == "Tr?ng";
    }
}
