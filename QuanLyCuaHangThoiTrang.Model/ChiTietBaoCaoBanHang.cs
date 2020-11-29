namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietBaoCaoBanHang")]
    public partial class ChiTietBaoCaoBanHang
    {
        [Key]
        [Column(Order = 0)]
        public int MaBaoCaoBanHang { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime Ngay { get; set; }

        public int SoLuongPhieuBanHang { get; set; }

        [Column(TypeName = "money")]
        public decimal DoanhThu { get; set; }

        public double TiLe { get; set; }

        public virtual BaoCaoBanHang BaoCaoBanHang { get; set; }
    }
}
