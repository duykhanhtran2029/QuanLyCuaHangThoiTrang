namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhaCungCap")]
    public partial class NhaCungCap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaCungCap()
        {
            PhieuNhapKhoes = new HashSet<PhieuNhapKho>();
        }

        [Key]
        public int MaNhaCungCap { get; set; }

        [Display(Name = "Tên Nhà Cung Cấp"), Required(ErrorMessage = "Tên Nhà Cung Cấp không được trống")]
        [StringLength(50)]
        public string TenNhaCungCap { get; set; }

        [Display(Name = "Địa Chỉ"), Required(ErrorMessage = "Địa Chỉ không được trống")]
        [StringLength(100)]
        public string DiaChi { get; set; }

        [Display(Name = "Số Điện Thoại"), Required(ErrorMessage = "Số Điện Thoại không được trống")]
        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuNhapKho> PhieuNhapKhoes { get; set; }
    }
}
