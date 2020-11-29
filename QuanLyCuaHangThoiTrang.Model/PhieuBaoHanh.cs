namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuBaoHanh")]
    public partial class PhieuBaoHanh
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuBaoHanh()
        {
            ChiTietPhieuBaoHanhs = new HashSet<ChiTietPhieuBaoHanh>();
        }

        [Key]
        public int SoPhieuBaoHanh { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayLap { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayGiao { get; set; }

        public int MaNguoiDung { get; set; }

        [Required]
        public string TenKhachHang { get; set; }

        [Required]
        [StringLength(50)]
        public string SoDienThoai { get; set; }

        [Column(TypeName = "money")]
        public decimal TongTien { get; set; }

        public string GhiChu { get; set; }

        public bool DaGiao { get; set; }

        public DateTime? NgayChinhSua { get; set; }

        public bool TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuBaoHanh> ChiTietPhieuBaoHanhs { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
