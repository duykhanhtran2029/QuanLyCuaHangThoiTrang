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
        [Display(Name = "Số Phiếu Đặt Hàng")]
        public int SoPhieuDatHang { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Ngày Đặt")]
        [Required(ErrorMessage = "Ngày Đặt không được trống")]
        public DateTime NgayDat { get; set; }

        [Display(Name = "Mã Người Dùng")]
        public int? MaNguoiDung { get; set; }

        [Display(Name = "Tên Khách Hàng"), Required(ErrorMessage = "Tên Hàng Hóa không được trống")]
        public string TenKhachHang { get; set; }

        [Required(ErrorMessage = "Số Điện Thoại không được trống")]
        [Display(Name = "Số Điện Thoại")]
        [StringLength(11, ErrorMessage = "Số Điện Thoại không được quá 11 chữ số")]
        [RegularExpression(@"[0-9]{7,11}", ErrorMessage = "Số Điện Thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Địa Chỉ không được trống")]
        [Display(Name = "Địa Chỉ")]
        public string Diachi { get; set; }

        [StringLength(30)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Tổng Tiền"), Required(ErrorMessage = "Tổng Tiền không được trống")]
        [RegularExpression(@"[0-9]{1,100}", ErrorMessage = "Giá Bán không hợp lệ")]
        [Column(TypeName = "money")]
        public decimal TongTien { get; set; }

        [Required(ErrorMessage = "Hình Thức Thanh Toán không được trống")]
        [Display(Name = "Hình Thức Thanh Toán")]
        public string HinhThucThanhToan { get; set; }

        [Display(Name = "Ghi Chú")]
        public string GhiChu { get; set; }

        [Display(Name = "Ngày Giao")]
        [Required(ErrorMessage = "Ngày Giao không được trống")]
        [Column(TypeName = "date")]
        public DateTime NgayGiao { get; set; }

        [Display(Name = "Trạng Thái Xác Nhận")]
        [Required(ErrorMessage = "Trạng Thái Xác Nhận không được trống")]
        public bool DaXacNhan { get; set; }

        [Display(Name = "Trạng Thái Thanh Toán")]
        [Required(ErrorMessage = "Trạng Thái Xác Nhận không được trống")]
        public bool DaThanhToan { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Ngày Chỉnh Sửa")]
        public DateTime? NgayChinhSua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuDatHang> ChiTietPhieuDatHangs { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
