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
        public int MaBaoCaoTonKho { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaHangHoa { get; set; }

        public int SoLuongTonDau { get; set; }

        public int SoLuongNhap { get; set; }

        public int SoLuongXuat { get; set; }

        public int SoLuongTonCuoi { get; set; }

        [Column(TypeName = "ntext")]
        public string TinhTrangHangHoa { get; set; }

        public virtual BaoCaoTonKho BaoCaoTonKho { get; set; }

        public virtual HangHoa HangHoa { get; set; }
    }
}
