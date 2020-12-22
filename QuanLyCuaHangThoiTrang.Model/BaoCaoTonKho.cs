namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaoCaoTonKho")]
    public partial class BaoCaoTonKho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaoCaoTonKho()
        {
            ChiTietBaoCaoTonKhoes = new HashSet<ChiTietBaoCaoTonKho>();
        }

        [Key]
        [Display(Name = "Mã Báo Cáo Tồn Kho")]
        public int MaBaoCaoTonKho { get; set; }
        [Display(Name = "Tháng"), Required(ErrorMessage = "Tháng không được trống")]
        public int Thang { get; set; }
        [Display(Name = "Năm"), Required(ErrorMessage = "Năm không được trống")]
        public int Nam { get; set; }
        [Display(Name = "Mã Người Dùng"), Required(ErrorMessage = "Mã Người Dùng không được trống")]
        public int MaNguoiDung { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Lập"), Required(ErrorMessage = "Ngày Lập không được trống")]
        public DateTime NgayLap { get; set; }
        [Display(Name = "Tổng Số Hàng Hóa"), Required(ErrorMessage = "Tổng Số Hàng Hóa không được trống")]
        public int TongSoHangHoa { get; set; }
        public bool IsDeleted { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietBaoCaoTonKho> ChiTietBaoCaoTonKhoes { get; set; }
    }
}
