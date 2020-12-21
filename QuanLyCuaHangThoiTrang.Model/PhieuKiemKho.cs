namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuKiemKho")]
    public partial class PhieuKiemKho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuKiemKho()
        {
            ChiTietPhieuKiemKhoes = new HashSet<ChiTietPhieuKiemKho>();
        }

        [Key]
        [Display(Name = "Số Phiếu Kiểm Kho"), Required(ErrorMessage = "Số Phiếu Kiểm Kho không được trống")]
        public int SoPhieuKiemKho { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Kiểm Kho"), Required(ErrorMessage = "Ngày Kiểm Kho không được trống")]
        public DateTime NgayKiemKho { get; set; }

        [Display(Name = "Mã Người Dùng"), Required(ErrorMessage = "Mã Người Dùng không được trống")]
        public int MaNguoiDung { get; set; }

        [Display(Name = "Ghi Chú")]
        public string GhiChu { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Ngày Chỉnh Sửa")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NgayChinhSua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuKiemKho> ChiTietPhieuKiemKhoes { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
