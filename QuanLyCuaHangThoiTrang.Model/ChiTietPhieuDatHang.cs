namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuDatHang")]
    public partial class ChiTietPhieuDatHang
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SoPhieuDatHang { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaHangHoa { get; set; }

        [Display(Name = "Số Lượng"), Required(ErrorMessage = "Số Lượng không được trống")]
        [RegularExpression(@"[0-9]", ErrorMessage = "Số Lượng không hợp lệ")]
        public int SoLuong { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Giá"), Required(ErrorMessage = "Giá không được trống")]
        [RegularExpression(@"[0-9]{1,100}", ErrorMessage = "Giá không hợp lệ")]
        public decimal Gia { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Thành Tiền"), Required(ErrorMessage = "Thành Tiền không được trống")]
        [RegularExpression(@"[0-9]{1,100}", ErrorMessage = "Thành Tiền không hợp lệ")]
        public decimal ThanhTien { get; set; }

        public virtual HangHoa HangHoa { get; set; }

        public virtual PhieuDatHang PhieuDatHang { get; set; }
    }
}
