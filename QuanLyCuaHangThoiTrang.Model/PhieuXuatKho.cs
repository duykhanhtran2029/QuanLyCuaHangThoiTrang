namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuXuatKho")]
    public partial class PhieuXuatKho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuXuatKho()
        {
            ChiTietPhieuXuatKhoes = new HashSet<ChiTietPhieuXuatKho>();
        }

        [Key]
        public int SoPhieuXuatKho { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayXuat { get; set; }

        public int MaNguoiDung { get; set; }

        [Required]
        public string LyDoXuat { get; set; }

        [Column(TypeName = "money")]
        public decimal TongTien { get; set; }

        public bool TrangThai { get; set; }

        public DateTime? NgayChinhSua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuXuatKho> ChiTietPhieuXuatKhoes { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
