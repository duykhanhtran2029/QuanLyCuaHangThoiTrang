namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhanQuyen")]
    public partial class PhanQuyen
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaChucVu { get; set; }

        [Key]
        [Column(Order = 1)]
        public int MaQuyen { get; set; }

        [StringLength(100)]
        public string ChuThich { get; set; }

        public virtual ChucVu ChucVu { get; set; }

        public virtual Quyen Quyen { get; set; }
    }
}
