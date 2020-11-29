namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuBaoHanh")]
    public partial class ChiTietPhieuBaoHanh
    {
        [Key]
        [Column(Order = 0)]
        public int SoPhieuBaoHanh { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaHangHoa { get; set; }

        public int SoLuong { get; set; }

        [Column(TypeName = "money")]
        public decimal Gia { get; set; }

        [Column(TypeName = "money")]
        public decimal ThanhTien { get; set; }

        [Required]
        public string NoiDungBaoHanh { get; set; }

        public string GhiChu { get; set; }

        public virtual HangHoa HangHoa { get; set; }

        public virtual PhieuBaoHanh PhieuBaoHanh { get; set; }
    }
}
