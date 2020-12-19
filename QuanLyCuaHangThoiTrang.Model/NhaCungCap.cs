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
        [Display(Name = "Mã Nhà Cung Cấp")]
        public int MaNhaCungCap { get; set; }

        [Display(Name = "Tên Nhà Cung Cấp"), Required(ErrorMessage = "Tên Nhà Cung Cấp không được trống")]
        public string TenNhaCungCap { get; set; }

        [Display(Name = "Địa Chỉ"), Required(ErrorMessage = "Địa Chỉ không được trống")]
        public string DiaChi { get; set; }

        [Display(Name = "Số Điện Thoại"), Required(ErrorMessage = "Số Điện Thoại không được trống")]
        [StringLength(11, ErrorMessage = "Số Điện Thoại không được quá 11 chữ số")]
        [RegularExpression(@"[0-9]{7,11}", ErrorMessage = "Số Điện Thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuNhapKho> PhieuNhapKhoes { get; set; }
    }
}
