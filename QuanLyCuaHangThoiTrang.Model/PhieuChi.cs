namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuChi")]
    public partial class PhieuChi
    {
        [Key]
        public int SoPhieuChi { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime NgayChi { get; set; }

        public int MaNguoiDung { get; set; }

        [Display(Name = "Số Phiếu Nhập Kho"), Required(ErrorMessage = "Số Phiếu Nhập Kho không được trống")]
        public int SoPhieuNhapKho { get; set; }

        [Column(TypeName = "money")]
        public decimal TongTienChi { get; set; }

        public string GhiChu { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NgayChinhSua { get; set; }

        public bool IsDeleted { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        public virtual PhieuNhapKho PhieuNhapKho { get; set; }
    }
}
