namespace TrainTicket.Business.DTOs
{
    // DTO dòng d? li?u s? ?? gh?.
    // M?i record t??ng ?ng 1 gh? tr? v? t? sp_XemSoDoGhe.
    public class SeatMapDto
    {
        public string MaToa { get; set; } = string.Empty;
        public string LoaiToa { get; set; } = string.Empty;
        public string SoGhe { get; set; } = string.Empty;
        public string LoaiGhe { get; set; } = string.Empty;
        public int SeatID { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public decimal GiaVe { get; set; }
    }
}
