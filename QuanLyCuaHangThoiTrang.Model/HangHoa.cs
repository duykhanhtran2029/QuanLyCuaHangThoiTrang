namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HangHoa")]
    public partial class HangHoa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HangHoa()
        {
            ChiTietBaoCaoTonKhoes = new HashSet<ChiTietBaoCaoTonKho>();
            ChiTietPhieuBanHangs = new HashSet<ChiTietPhieuBanHang>();
            ChiTietPhieuBaoHanhs = new HashSet<ChiTietPhieuBaoHanh>();
            ChiTietPhieuDatHangs = new HashSet<ChiTietPhieuDatHang>();
            ChiTietPhieuKiemKhoes = new HashSet<ChiTietPhieuKiemKho>();
            ChiTietPhieuNhapKhoes = new HashSet<ChiTietPhieuNhapKho>();
            ChiTietPhieuXuatKhoes = new HashSet<ChiTietPhieuXuatKho>();
        }

        [Key]
        public int MaHangHoa { get; set; }

        [Required]
        public string TenHangHoa { get; set; }

        [Column(TypeName = "money")]
        public decimal GiaNhap { get; set; }

        public double GiamGia { get; set; }

        [Column(TypeName = "money")]
        public decimal? GiaBan { get; set; }

        public int SoLuong { get; set; }

        [StringLength(1)]
        public string Size { get; set; }

        [Required]
        [StringLength(50)]
        public string DonViTinh { get; set; }

        [Required]
        public string MoTa { get; set; }

        public int ThoiGianBaoHanh { get; set; }

        [Required]
        public string HinhAnh { get; set; }

        public string ThuongHieu { get; set; }

        public int MaLoaiHangHoa { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietBaoCaoTonKho> ChiTietBaoCaoTonKhoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuBanHang> ChiTietPhieuBanHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuBaoHanh> ChiTietPhieuBaoHanhs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuDatHang> ChiTietPhieuDatHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuKiemKho> ChiTietPhieuKiemKhoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuNhapKho> ChiTietPhieuNhapKhoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuXuatKho> ChiTietPhieuXuatKhoes { get; set; }

        public virtual LoaiHangHoa LoaiHangHoa { get; set; }
    }
}
