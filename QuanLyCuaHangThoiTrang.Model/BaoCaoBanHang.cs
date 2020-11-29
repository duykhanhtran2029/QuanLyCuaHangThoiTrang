namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaoCaoBanHang")]
    public partial class BaoCaoBanHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaoCaoBanHang()
        {
            ChiTietBaoCaoBanHangs = new HashSet<ChiTietBaoCaoBanHang>();
        }

        [Key]
        public int MaBaoCaoBanHang { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public int SoLuongPhieuBanHang { get; set; }

        [Column(TypeName = "money")]
        public decimal TongTienBanHang { get; set; }

        [Column(TypeName = "money")]
        public decimal TongTienNhapHang { get; set; }

        [Column(TypeName = "money")]
        public decimal TongDoanhThu { get; set; }

        public int MaNguoiDung { get; set; }

        public bool TrangThai { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietBaoCaoBanHang> ChiTietBaoCaoBanHangs { get; set; }
    }
}
