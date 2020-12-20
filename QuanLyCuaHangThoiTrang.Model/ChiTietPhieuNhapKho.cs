namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuNhapKho")]
    public partial class ChiTietPhieuNhapKho
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Số Phiếu Nhập Kho"), Required(ErrorMessage = "Số Phiếu Nhập Kho không được trống")]
        public int SoPhieuNhapKho { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Mã Hàng Hóa"), Required(ErrorMessage = "Mã Hàng Hóa không được trống")]
        public int MaHangHoa { get; set; }

        [Display(Name = "Số Lượng"), Required(ErrorMessage = "Số Lượng không được trống")]
        public int SoLuong { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Giá Nhập"), Required(ErrorMessage = "Giá Nhập không được trống")]
        public decimal GiaNhap { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Thành Tiền"), Required(ErrorMessage = "Thành Tiền không được trống")]
        public decimal ThanhTien { get; set; }

        public virtual HangHoa HangHoa { get; set; }

        public virtual PhieuNhapKho PhieuNhapKho { get; set; }
    }
}
