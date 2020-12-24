namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuBanHang")]
    public partial class PhieuBanHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuBanHang()
        {
            ChiTietPhieuBanHangs = new HashSet<ChiTietPhieuBanHang>();
        }

        [Key]
        public int SoPhieuBanHang { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Ngày Bán")]
        [Required(ErrorMessage = "Ngày Bán không được trống")]
        public DateTime NgayBan { get; set; }

        public int? MaNguoiDung { get; set; }

        [Display(Name = "Tên Khách Hàng"), Required(ErrorMessage = "Tên Hàng Hóa không được trống")]
        public string TenKhachHang { get; set; }

        [Required(ErrorMessage = "Số Điện Thoại không được trống")]
        [Display(Name = "Số Điện Thoại")]
        [StringLength(11, ErrorMessage = "Số Điện Thoại không được quá 11 chữ số")]
        [RegularExpression(@"[0-9]{7,11}", ErrorMessage = "Số Điện Thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Tổng Tiền"), Required(ErrorMessage = "Tổng Tiền không được trống")]
        [RegularExpression(@"[0-9]{1,100}", ErrorMessage = "Giá Bán không hợp lệ")]
        public decimal TongTien { get; set; }

        public string GhiChu { get; set; }

        public DateTime? NgayChinhSua { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuBanHang> ChiTietPhieuBanHangs { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
