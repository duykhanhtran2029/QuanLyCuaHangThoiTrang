namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoaiHangHoa")]
    public partial class LoaiHangHoa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiHangHoa()
        {
            HangHoas = new HashSet<HangHoa>();
        }

        [Key]
        public int MaLoaiHangHoa { get; set; }

        [Display(Name = "Tên Loại Hàng Hóa"), Required(ErrorMessage = "Tên Loại Hàng Hóa không được trống")]
        [StringLength(50, ErrorMessage = "Tên Loại Hàng Hóa không được quá 50 ký tự")]
        public string TenLoaiHangHoa { get; set; }

        [Display(Name = "Giới Tính"), Required(ErrorMessage = "Giới Tính không được trống")]
        [StringLength(50, ErrorMessage = "Giới Tính không được quá 50 ký tự")]
        public string GioiTinh { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HangHoa> HangHoas { get; set; }
    }
}
