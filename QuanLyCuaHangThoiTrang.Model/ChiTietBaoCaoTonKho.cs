namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietBaoCaoTonKho")]
    public partial class ChiTietBaoCaoTonKho
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "Mã Báo Cáo Tồn Kho"), Required(ErrorMessage = "Mã Báo Cáo Tồn Kho không được trống")]
        public int MaBaoCaoTonKho { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Mã Hàng Hóa"), Required(ErrorMessage = "Mã Hàng Hóa không được trống")]
        public int MaHangHoa { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Số Lượng Tồn Đầu"), Required(ErrorMessage = "Số Lượng Tồn Đầu phải nguyên dương và không được trống")]
        public int SoLuongTonDau { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Số Lượng Nhập"), Required(ErrorMessage = "Số Lượng Nhập phải nguyên dương và không được trống")]
        public int SoLuongNhap { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Số Lượng Xuất"), Required(ErrorMessage = "Số Lượng Xuất phải nguyên dương và không được trống")]
        public int SoLuongXuat { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Số Lượng Tồn Cuối"), Required(ErrorMessage = "Số Lượng Tồn Cuối phải nguyên dương và không được trống")]
        public int SoLuongTonCuoi { get; set; }

        [Display(Name = "Tình Trạng Hàng Hóa"), Required(ErrorMessage = "Tình Trạng Hàng Hóa không được trống")]
        [Column(TypeName = "ntext")]
        public string TinhTrangHangHoa { get; set; }

        public virtual BaoCaoTonKho BaoCaoTonKho { get; set; }

        public virtual HangHoa HangHoa { get; set; }
    }
}
