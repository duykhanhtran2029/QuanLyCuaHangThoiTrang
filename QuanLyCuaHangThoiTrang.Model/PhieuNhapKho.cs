namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuNhapKho")]
    public partial class PhieuNhapKho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuNhapKho()
        {
            ChiTietPhieuNhapKhoes = new HashSet<ChiTietPhieuNhapKho>();
            PhieuChis = new HashSet<PhieuChi>();
        }

        [Key]
        public int SoPhieuNhapKho { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayNhapKho { get; set; }

        public int MaNguoiDung { get; set; }

        public int MaNhaCungCap { get; set; }

        [Column(TypeName = "money")]
        public decimal TongTien { get; set; }

        public string Ghichu { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? NgayChinhSua { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuNhapKho> ChiTietPhieuNhapKhoes { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }

        public virtual NhaCungCap NhaCungCap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuChi> PhieuChis { get; set; }
    }
}
