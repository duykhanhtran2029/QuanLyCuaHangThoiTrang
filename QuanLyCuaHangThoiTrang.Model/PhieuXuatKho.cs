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
        [Display(Name = "Số Phiếu Xuất Kho"), Required(ErrorMessage = "Số Phiếu Xuất Kho không được trống")]
        public int SoPhieuXuatKho { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Xuất Kho"), Required(ErrorMessage = "Ngày Xuất Kho không được trống")]
        public DateTime NgayXuatKho { get; set; }

        [Display(Name = "Mã Người Dùng"), Required(ErrorMessage = "Mã Người Dùng không được trống")]
        public int MaNguoiDung { get; set; }

        [Display(Name = "Lý Do Xuất"), Required(ErrorMessage = "Lý Do Xuất không được trống")]
        public string LyDoXuat { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Tổng Tiền"), Required(ErrorMessage = "Tổng Tiền không được trống")]
        public decimal TongTien { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Ngày Chỉnh Sửa")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NgayChinhSua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuXuatKho> ChiTietPhieuXuatKhoes { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
