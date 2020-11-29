namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuKiemKho")]
    public partial class PhieuKiemKho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuKiemKho()
        {
            ChiTietPhieuKiemKhoes = new HashSet<ChiTietPhieuKiemKho>();
        }

        [Key]
        public int SoPhieuKiemKho { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayKiemKho { get; set; }

        public int MaNguoiDung { get; set; }

        public string GhiChu { get; set; }

        public bool TrangThai { get; set; }

        public DateTime? NgayChinhSua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuKiemKho> ChiTietPhieuKiemKhoes { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
