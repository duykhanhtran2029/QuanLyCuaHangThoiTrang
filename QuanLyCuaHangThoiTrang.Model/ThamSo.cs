namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThamSo")]
    public partial class ThamSo
    {
        [Key]
        public int MaThamSo { get; set; }

        [Required]
        [StringLength(200)]
        public string TenThamSo { get; set; }

        public double GiaTri { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayApDung { get; set; }
    }
}
