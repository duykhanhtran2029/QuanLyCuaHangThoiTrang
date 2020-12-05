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
        public int MaHangHoa { get; set; }

        public int SoLuongHienTai { get; set; }

        public int SoLuongKiemTra { get; set; }

        [Column(TypeName = "ntext")]
        public string TinhTrangHangHoa { get; set; }

        public virtual HangHoa HangHoa { get; set; }

        public virtual PhieuKiemKho PhieuKiemKho { get; set; }
    }
}
