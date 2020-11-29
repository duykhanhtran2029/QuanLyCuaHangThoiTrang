namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuChi")]
    public partial class PhieuChi
    {
        [Key]
        public int SoPhieuChi { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayChi { get; set; }

        public int MaNguoiDung { get; set; }

        public int SoPhieuNhapKho { get; set; }

        [Column(TypeName = "money")]
        public decimal TongTienChi { get; set; }

        public string GhiChu { get; set; }

        public DateTime? NgayChinhSua { get; set; }

        public bool TrangThai { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        public virtual PhieuNhapKho PhieuNhapKho { get; set; }
    }
}
