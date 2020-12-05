namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuDatHang")]
    public partial class PhieuDatHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuDatHang()
        {
            ChiTietPhieuDatHangs = new HashSet<ChiTietPhieuDatHang>();
        }

        [Key]
        public int SoPhieuDatHang { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayDat { get; set; }

        public int? MaNguoiDung { get; set; }

        [Required]
        public string TenKhachHang { get; set; }

        [Required]
        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [Required]
        public string Diachi { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        [Column(TypeName = "money")]
        public decimal TongTien { get; set; }

        [Required]
        public string HinhThucThanhToan { get; set; }

        public string Ghichu { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayGiao { get; set; }

        public bool DaXacNhan { get; set; }

        public bool DaThanhToan { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? NgayChinhSua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuDatHang> ChiTietPhieuDatHangs { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
