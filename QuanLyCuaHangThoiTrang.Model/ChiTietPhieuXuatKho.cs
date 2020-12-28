namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuXuatKho")]
    public partial class ChiTietPhieuXuatKho
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SoPhieuXuatKho { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Mã Hàng Hóa"), Required(ErrorMessage = "Mã Hàng Hóa không được trống")]
        public int MaHangHoa { get; set; }
        [Display(Name = "Số Lượng"), Required(ErrorMessage = "Số Lượng không được trống")]
        public int SoLuong { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Giá"), Required(ErrorMessage = "Giá không được trống")]
        public decimal Gia { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Thành Tiền"), Required(ErrorMessage = "Thành Tiền không được trống")]
        public decimal ThanhTien { get; set; }

        public virtual HangHoa HangHoa { get; set; }

        public virtual PhieuXuatKho PhieuXuatKho { get; set; }
    }
}
