namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuNhapKho")]
    public partial class PhieuNhapKho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuNhapKho()
        {
            ChiTietPhieuNhapKhoes = new HashSet<ChiTietPhieuNhapKho>();
            PhieuChis = new HashSet<PhieuChi>();
        }

        [Key]
        [Display(Name = "Số Phiếu Nhập Kho"), Required(ErrorMessage = "Số Phiếu Nhập Kh không được trống")]
        public int SoPhieuNhapKho { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Ngày Nhập Kho"), Required(ErrorMessage = "Ngày Nhập Kho không được trống")]
        public DateTime NgayNhapKho { get; set; }

        [Display(Name = "Mã Người Dùng"), Required(ErrorMessage = "Mã Người Dùng không được trống")]
        public int MaNguoiDung { get; set; }

        [Display(Name = "Mã Nhà Cung Cấp"), Required(ErrorMessage = "Mã Nhà Cung Cấp không được trống")]
        public int MaNhaCungCap { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Tổng Tiền"), Required(ErrorMessage = "Tổng Tiền không được trống")]
        public decimal TongTien { get; set; }

        [Display(Name = "Ghi Chú")]
        public string Ghichu { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Ngày Chỉnh Sửa")]
        [Column(TypeName = "date")]
        public DateTime? NgayChinhSua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuNhapKho> ChiTietPhieuNhapKhoes { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        public virtual NhaCungCap NhaCungCap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuChi> PhieuChis { get; set; }
    }
}
