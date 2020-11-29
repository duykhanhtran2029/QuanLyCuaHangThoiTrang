namespace QuanLyCuaHangThoiTrang.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class QuanLyCuaHangThoiTrangDbContext : DbContext
    {
        public QuanLyCuaHangThoiTrangDbContext()
            : base("name=QuanLyCuaHangThoiTrangDbContext")
        {
        }

        public virtual DbSet<BaoCaoBanHang> BaoCaoBanHangs { get; set; }
        public virtual DbSet<BaoCaoTonKho> BaoCaoTonKhoes { get; set; }
        public virtual DbSet<ChiTietBaoCaoBanHang> ChiTietBaoCaoBanHangs { get; set; }
        public virtual DbSet<ChiTietBaoCaoTonKho> ChiTietBaoCaoTonKhoes { get; set; }
        public virtual DbSet<ChiTietPhieuBanHang> ChiTietPhieuBanHangs { get; set; }
        public virtual DbSet<ChiTietPhieuBaoHanh> ChiTietPhieuBaoHanhs { get; set; }
        public virtual DbSet<ChiTietPhieuDatHang> ChiTietPhieuDatHangs { get; set; }
        public virtual DbSet<ChiTietPhieuKiemKho> ChiTietPhieuKiemKhoes { get; set; }
        public virtual DbSet<ChiTietPhieuNhapKho> ChiTietPhieuNhapKhoes { get; set; }
        public virtual DbSet<ChiTietPhieuXuatKho> ChiTietPhieuXuatKhoes { get; set; }
        public virtual DbSet<ChucVu> ChucVus { get; set; }
        public virtual DbSet<HangHoa> HangHoas { get; set; }
        public virtual DbSet<LoaiHangHoa> LoaiHangHoas { get; set; }
        public virtual DbSet<NguoiDung> NguoiDungs { get; set; }
        public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }
        public virtual DbSet<PhieuBanHang> PhieuBanHangs { get; set; }
        public virtual DbSet<PhieuBaoHanh> PhieuBaoHanhs { get; set; }
        public virtual DbSet<PhieuChi> PhieuChis { get; set; }
        public virtual DbSet<PhieuDatHang> PhieuDatHangs { get; set; }
        public virtual DbSet<PhieuKiemKho> PhieuKiemKhoes { get; set; }
        public virtual DbSet<PhieuNhapKho> PhieuNhapKhoes { get; set; }
        public virtual DbSet<PhieuXuatKho> PhieuXuatKhoes { get; set; }
        public virtual DbSet<Quyen> Quyens { get; set; }
        public virtual DbSet<ThamSo> ThamSoes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaoCaoBanHang>()
                .Property(e => e.TongTienBanHang)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BaoCaoBanHang>()
                .Property(e => e.TongTienNhapHang)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BaoCaoBanHang>()
                .Property(e => e.TongDoanhThu)
                .HasPrecision(19, 4);

            modelBuilder.Entity<BaoCaoBanHang>()
                .HasMany(e => e.ChiTietBaoCaoBanHangs)
                .WithRequired(e => e.BaoCaoBanHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BaoCaoTonKho>()
                .HasMany(e => e.ChiTietBaoCaoTonKhoes)
                .WithRequired(e => e.BaoCaoTonKho)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ChiTietBaoCaoBanHang>()
                .Property(e => e.DoanhThu)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTietPhieuBanHang>()
                .Property(e => e.Gia)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTietPhieuBanHang>()
                .Property(e => e.ThanhTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTietPhieuBaoHanh>()
                .Property(e => e.Gia)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTietPhieuBaoHanh>()
                .Property(e => e.ThanhTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTietPhieuDatHang>()
                .Property(e => e.Gia)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTietPhieuDatHang>()
                .Property(e => e.ThanhTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTietPhieuNhapKho>()
                .Property(e => e.GiaNhap)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTietPhieuNhapKho>()
                .Property(e => e.ThanhTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTietPhieuXuatKho>()
                .Property(e => e.Gia)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChiTietPhieuXuatKho>()
                .Property(e => e.ThanhTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<ChucVu>()
                .HasMany(e => e.NguoiDungs)
                .WithRequired(e => e.ChucVu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ChucVu>()
                .HasMany(e => e.PhanQuyens)
                .WithRequired(e => e.ChucVu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HangHoa>()
                .Property(e => e.GiaBan)
                .HasPrecision(19, 4);

            modelBuilder.Entity<HangHoa>()
                .HasMany(e => e.ChiTietBaoCaoTonKhoes)
                .WithRequired(e => e.HangHoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HangHoa>()
                .HasMany(e => e.ChiTietPhieuBanHangs)
                .WithRequired(e => e.HangHoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HangHoa>()
                .HasMany(e => e.ChiTietPhieuBaoHanhs)
                .WithRequired(e => e.HangHoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HangHoa>()
                .HasMany(e => e.ChiTietPhieuDatHangs)
                .WithRequired(e => e.HangHoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HangHoa>()
                .HasMany(e => e.ChiTietPhieuKiemKhoes)
                .WithRequired(e => e.HangHoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HangHoa>()
                .HasMany(e => e.ChiTietPhieuNhapKhoes)
                .WithRequired(e => e.HangHoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HangHoa>()
                .HasMany(e => e.ChiTietPhieuXuatKhoes)
                .WithRequired(e => e.HangHoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LoaiHangHoa>()
                .HasMany(e => e.HangHoas)
                .WithRequired(e => e.LoaiHangHoa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDung>()
                .Property(e => e.SoDienThoai)
                .IsUnicode(false);

            modelBuilder.Entity<NguoiDung>()
                .Property(e => e.CMND)
                .IsUnicode(false);

            modelBuilder.Entity<NguoiDung>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<NguoiDung>()
                .Property(e => e.PassWord)
                .IsUnicode(false);

            modelBuilder.Entity<NguoiDung>()
                .HasMany(e => e.BaoCaoBanHangs)
                .WithRequired(e => e.NguoiDung)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDung>()
                .HasMany(e => e.BaoCaoTonKhoes)
                .WithRequired(e => e.NguoiDung)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDung>()
                .HasMany(e => e.PhieuBanHangs)
                .WithRequired(e => e.NguoiDung)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDung>()
                .HasMany(e => e.PhieuBaoHanhs)
                .WithRequired(e => e.NguoiDung)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDung>()
                .HasMany(e => e.PhieuChis)
                .WithRequired(e => e.NguoiDung)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDung>()
                .HasMany(e => e.PhieuDatHangs)
                .WithRequired(e => e.NguoiDung)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDung>()
                .HasMany(e => e.PhieuKiemKhoes)
                .WithRequired(e => e.NguoiDung)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDung>()
                .HasMany(e => e.PhieuNhapKhoes)
                .WithRequired(e => e.NguoiDung)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NguoiDung>()
                .HasMany(e => e.PhieuXuatKhoes)
                .WithRequired(e => e.NguoiDung)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NhaCungCap>()
                .Property(e => e.SoDienThoai)
                .IsUnicode(false);

            modelBuilder.Entity<NhaCungCap>()
                .HasMany(e => e.PhieuNhapKhoes)
                .WithRequired(e => e.NhaCungCap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhieuBanHang>()
                .Property(e => e.SoDienThoai)
                .IsUnicode(false);

            modelBuilder.Entity<PhieuBanHang>()
                .Property(e => e.TongTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PhieuBanHang>()
                .HasMany(e => e.ChiTietPhieuBanHangs)
                .WithRequired(e => e.PhieuBanHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhieuBaoHanh>()
                .Property(e => e.SoDienThoai)
                .IsUnicode(false);

            modelBuilder.Entity<PhieuBaoHanh>()
                .Property(e => e.TongTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PhieuBaoHanh>()
                .HasMany(e => e.ChiTietPhieuBaoHanhs)
                .WithRequired(e => e.PhieuBaoHanh)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhieuChi>()
                .Property(e => e.TongTienChi)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PhieuDatHang>()
                .Property(e => e.SoDienThoai)
                .IsUnicode(false);

            modelBuilder.Entity<PhieuDatHang>()
                .Property(e => e.TongTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PhieuDatHang>()
                .HasMany(e => e.ChiTietPhieuDatHangs)
                .WithRequired(e => e.PhieuDatHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhieuKiemKho>()
                .HasMany(e => e.ChiTietPhieuKiemKhoes)
                .WithRequired(e => e.PhieuKiemKho)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhieuNhapKho>()
                .Property(e => e.TongTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PhieuNhapKho>()
                .HasMany(e => e.ChiTietPhieuNhapKhoes)
                .WithRequired(e => e.PhieuNhapKho)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhieuNhapKho>()
                .HasMany(e => e.PhieuChis)
                .WithRequired(e => e.PhieuNhapKho)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PhieuXuatKho>()
                .Property(e => e.TongTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PhieuXuatKho>()
                .HasMany(e => e.ChiTietPhieuXuatKhoes)
                .WithRequired(e => e.PhieuXuatKho)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quyen>()
                .HasMany(e => e.PhanQuyens)
                .WithRequired(e => e.Quyen)
                .WillCascadeOnDelete(false);
        }
    }
}
