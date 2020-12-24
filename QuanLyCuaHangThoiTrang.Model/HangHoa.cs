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
            ChiTietPhieuDatHangs = new HashSet<ChiTietPhieuDatHang>();
            ChiTietPhieuKiemKhoes = new HashSet<ChiTietPhieuKiemKho>();
            ChiTietPhieuNhapKhoes = new HashSet<ChiTietPhieuNhapKho>();
            ChiTietPhieuXuatKhoes = new HashSet<ChiTietPhieuXuatKho>();
        }

        [Key]
        [Display(Name = "Mã Hàng Hóa")]
        public int MaHangHoa { get; set; }

        [Display(Name = "Tên Hàng Hóa"), Required(ErrorMessage = "Tên Hàng Hóa không được trống")]
        public string TenHangHoa { get; set; }

        [Display(Name = "Giảm Giá"), Required(ErrorMessage = "Giảm Giá không được trống")]
        public double GiamGia { get; set; }

        [Display(Name = "Giá Bán"), Required(ErrorMessage = "Giá Bán không được trống")]
        [Column(TypeName = "money")]
        public decimal? GiaBan { get; set; }
        [Display(Name = "Số Lượng"), Required(ErrorMessage = "Số Lượng không được trống")]
        public int SoLuong { get; set; }

        [StringLength(1)]
        public string Size { get; set; }

        [Display(Name = "Đơn Vị Tính"), Required(ErrorMessage = "Đơn Vị Tính không được trống")]
        [StringLength(50)]
        public string DonViTinh { get; set; }

        [Display(Name = "Mô Tả"), Required(ErrorMessage = "Mô Tả không được trống")]
        public string MoTa { get; set; }

        [Display(Name = "Thời Gian Bảo Hành"), Required(ErrorMessage = "Thời Gian Bảo Hành không được trống")]
        public int ThoiGianBaoHanh { get; set; }

        [Display(Name = "Hình Ảnh"), Required(ErrorMessage = "Hình Ảnh không được trống")]
        public string HinhAnh { get; set; }
        [Display(Name = "Thương Hiệu")]
        public string ThuongHieu { get; set; }

        public int MaLoaiHangHoa { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietBaoCaoTonKho> ChiTietBaoCaoTonKhoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuBanHang> ChiTietPhieuBanHangs { get; set; }

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
