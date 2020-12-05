namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaoCaoTonKho")]
    public partial class BaoCaoTonKho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BaoCaoTonKho()
        {
            ChiTietBaoCaoTonKhoes = new HashSet<ChiTietBaoCaoTonKho>();
        }

        [Key]
        public int MaBaoCaoTonKho { get; set; }

        public int Thang { get; set; }

        public int Nam { get; set; }

        public int MaNguoiDung { get; set; }

        public DateTime NgayLap { get; set; }

        public int TongSoHangHoa { get; set; }

        public bool IsDeleted { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietBaoCaoTonKho> ChiTietBaoCaoTonKhoes { get; set; }
    }
}
