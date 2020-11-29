namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuBanHang")]
    public partial class ChiTietPhieuBanHang
    {
        [Key]
        [Column(Order = 0)]
        public int SoPhieuBanHang { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaHangHoa { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "money")]
        public decimal Gia { get; set; }

        [Column(TypeName = "money")]
        public decimal ThanhTien { get; set; }

        public virtual HangHoa HangHoa { get; set; }

        public virtual PhieuBanHang PhieuBanHang { get; set; }
    }
}
