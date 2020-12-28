namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuKiemKho")]
    public partial class ChiTietPhieuKiemKho
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SoPhieuKiemKho { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Mã Hàng Hóa"), Required(ErrorMessage = "Mã Hàng Hóa không được trống")]
        public int MaHangHoa { get; set; }

        [Display(Name = "Số Lượng Tồn"), Required(ErrorMessage = "Số Lượng Tồn không được trống")]
        public int SoLuongHienTai { get; set; }

        [Display(Name = "Số Lượng Kiểm Tra"), Required(ErrorMessage = "Số Lượng Kiểm Tra không được trống")]
        public int SoLuongKiemTra { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Tình Trạng Hàng Hóa"), Required(ErrorMessage = "Tình Trạng Hàng Hóa không được trống")]
        public string TinhTrangHangHoa { get; set; }

        public virtual HangHoa HangHoa { get; set; }

        public virtual PhieuKiemKho PhieuKiemKho { get; set; }
    }
}
