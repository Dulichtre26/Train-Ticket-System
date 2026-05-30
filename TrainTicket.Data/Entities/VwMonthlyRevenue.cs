using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class VwMonthlyRevenue
{
    public int? Nam { get; set; }

    public int? Thang { get; set; }

    public string RouteName { get; set; } = null!;

    public string TuyenDuong { get; set; } = null!;

    public int? SoVe { get; set; }

    public decimal? DoanhThu { get; set; }

    public decimal? GiaTrungBinh { get; set; }

    public decimal? TongGiamGia { get; set; }

    public int? SoVeHuy { get; set; }

    public int? SoVeDaCheckIn { get; set; }
}
