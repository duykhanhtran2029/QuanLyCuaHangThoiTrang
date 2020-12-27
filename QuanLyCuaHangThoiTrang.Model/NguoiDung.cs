namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NguoiDung")]
    public partial class NguoiDung
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NguoiDung()
        {
            BaoCaoBanHangs = new HashSet<BaoCaoBanHang>();
            BaoCaoTonKhoes = new HashSet<BaoCaoTonKho>();
            PhieuBanHangs = new HashSet<PhieuBanHang>();
            PhieuChis = new HashSet<PhieuChi>();
            PhieuDatHangs = new HashSet<PhieuDatHang>();
            PhieuKiemKhoes = new HashSet<PhieuKiemKho>();
            PhieuNhapKhoes = new HashSet<PhieuNhapKho>();
            PhieuXuatKhoes = new HashSet<PhieuXuatKho>();
        }

        [Key]
        public int MaNguoiDung { get; set; }


        [StringLength(50)]
        [Display(Name = "Tên người dùng"), Required(ErrorMessage = "Tên người dùng không được trống!")]
        public string TenNguoiDung { get; set; }

        [StringLength(100)]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Số Điện Thoại"), Required(ErrorMessage = "Số Điện Thoại không được trống")]
        [StringLength(11, ErrorMessage = "Số Điện Thoại không được quá 11 chữ số")]
        [RegularExpression(@"[0-9]{7,11}", ErrorMessage = "Số Điện Thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [StringLength(30)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(10)]
        [Display(Name = "CMND")]
        public string CMND { get; set; }

        [Required(ErrorMessage = "Tên người dùng không được trống")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được trống")]
        [StringLength(50)]
        public string PassWord { get; set; }

        public bool IsDeleted { get; set; }

        public int MaChucVu { get; set; }

        public string Avatar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaoCaoBanHang> BaoCaoBanHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaoCaoTonKho> BaoCaoTonKhoes { get; set; }

        public virtual ChucVu ChucVu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuBanHang> PhieuBanHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuChi> PhieuChis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuDatHang> PhieuDatHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuKiemKho> PhieuKiemKhoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuNhapKho> PhieuNhapKhoes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuXuatKho> PhieuXuatKhoes { get; set; }
    }
}
