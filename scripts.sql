/****** Object:  Table [dbo].[BaoCaoBanHang]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaoCaoBanHang](
	[MaBaoCaoBanHang] [int] IDENTITY(1,1) NOT NULL,
	[NgayBatDau] [datetime] NOT NULL,
	[NgayKetThuc] [datetime] NOT NULL,
	[SoLuongPhieuBanHang] [int] NOT NULL,
	[TongTienBanHang] [money] NOT NULL,
	[TongTienNhapHang] [money] NOT NULL,
	[TongDoanhThu] [money] NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[TrangThai] [bit] NOT NULL,
 CONSTRAINT [PK_BCBH] PRIMARY KEY CLUSTERED 
(
	[MaBaoCaoBanHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BaoCaoTonKho]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaoCaoTonKho](
	[MaBaoCaoTonKho] [int] IDENTITY(1,1) NOT NULL,
	[Thang] [int] NOT NULL,
	[Nam] [int] NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[NgayLap] [datetime] NOT NULL,
	[TongSoHangHoa] [int] NOT NULL,
	[TrangThai] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaBaoCaoTonKho] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietBaoCaoBanHang]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietBaoCaoBanHang](
	[MaBaoCaoBanHang] [int] IDENTITY(1,1) NOT NULL,
	[Ngay] [datetime] NOT NULL,
	[SoLuongPhieuBanHang] [int] NOT NULL,
	[DoanhThu] [money] NOT NULL,
	[TiLe] [float] NOT NULL,
 CONSTRAINT [PK_CTBCBH] PRIMARY KEY CLUSTERED 
(
	[MaBaoCaoBanHang] ASC,
	[Ngay] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietBaoCaoTonKho]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietBaoCaoTonKho](
	[MaBaoCaoTonKho] [int] IDENTITY(1,1) NOT NULL,
	[MaHangHoa] [int] NOT NULL,
	[SoLuongTonDau] [int] NOT NULL,
	[SoLuongNhap] [int] NOT NULL,
	[SoLuongXuat] [int] NOT NULL,
	[SoLuongTonCuoi] [int] NOT NULL,
	[TinhTrangHangHoa] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaBaoCaoTonKho] ASC,
	[MaHangHoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietPhieuBanHang]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPhieuBanHang](
	[SoPhieuBanHang] [int] IDENTITY(1,1) NOT NULL,
	[MaHangHoa] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[Gia] [money] NOT NULL,
	[ThanhTien] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuBanHang] ASC,
	[MaHangHoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietPhieuBaoHanh]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPhieuBaoHanh](
	[SoPhieuBaoHanh] [int] IDENTITY(1,1) NOT NULL,
	[MaHangHoa] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[Gia] [money] NOT NULL,
	[ThanhTien] [money] NOT NULL,
	[NoiDungBaoHanh] [nvarchar](max) NOT NULL,
	[GhiChu] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuBaoHanh] ASC,
	[MaHangHoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietPhieuDatHang]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPhieuDatHang](
	[SoPhieuDatHang] [int] NOT NULL,
	[MaHangHoa] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[Gia] [money] NOT NULL,
	[ThanhTien] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuDatHang] ASC,
	[MaHangHoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietPhieuKiemKho]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPhieuKiemKho](
	[SoPhieuKiemKho] [int] NOT NULL,
	[MaHangHoa] [int] NOT NULL,
	[SoLuongHienTai] [int] NOT NULL,
	[SoLuongKiemTra] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuKiemKho] ASC,
	[MaHangHoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietPhieuNhapKho]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPhieuNhapKho](
	[SoPhieuNhapKho] [int] NOT NULL,
	[MaHangHoa] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[GiaNhap] [money] NOT NULL,
	[MaNhaCungCap] [int] NOT NULL,
	[ThanhTien] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuNhapKho] ASC,
	[MaHangHoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietPhieuXuatKho]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietPhieuXuatKho](
	[SoPhieuXuatKho] [int] NOT NULL,
	[MaHangHoa] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[Gia] [money] NOT NULL,
	[ThanhTien] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuXuatKho] ASC,
	[MaHangHoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChucVu]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChucVu](
	[MaChucVu] [int] IDENTITY(1,1) NOT NULL,
	[TenChucVu] [nvarchar](100) NOT NULL,
	[TrangThai] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaChucVu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HangHoa]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HangHoa](
	[MaHangHoa] [int] IDENTITY(1,1) NOT NULL,
	[TenHangHoa] [nvarchar](max) NOT NULL,
	[GiaBan] [money] NOT NULL,
	[GiamGia] [float] NOT NULL,
	[SoLuongTon] [int] NOT NULL,
	[Size] [nvarchar](1) NULL,
	[DonViTinh] [nvarchar](50) NOT NULL,
	[MoTa] [nvarchar](max) NOT NULL,
	[ThongSoKyThuat] [ntext] NOT NULL,
	[XuatXu] [nvarchar](max) NOT NULL,
	[ThoiGianBaoHanh] [int] NOT NULL,
	[HinhAnh] [nvarchar](max) NOT NULL,
	[ThuongHieu] [nvarchar](max) NULL,
	[MaLoaiHangHoa] [int] NOT NULL,
	[TrangThai] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHangHoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoaiHangHoa]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiHangHoa](
	[MaLoaiHangHoa] [int] IDENTITY(1,1) NOT NULL,
	[TenLoaiHangHoa] [nvarchar](50) NOT NULL,
	[GioiTinh] [nvarchar](50) NOT NULL,
	[TrangThai] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLoaiHangHoa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NguoiDung]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NguoiDung](
	[MaNguoiDung] [int] IDENTITY(1,1) NOT NULL,
	[TenNguoiDung] [nvarchar](50) NOT NULL,
	[DiaChi] [nvarchar](100) NULL,
	[SoDienThoai] [varchar](15) NOT NULL,
	[Email] [nvarchar](30) NULL,
	[CMND] [varchar](10) NULL,
	[UserName] [varchar](100) NOT NULL,
	[PassWord] [varchar](50) NOT NULL,
	[TrangThai] [bit] NOT NULL,
	[MaChucVu] [int] NOT NULL,
	[Avatar] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNguoiDung] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhaCungCap]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhaCungCap](
	[MaNhaCungCap] [int] IDENTITY(1,1) NOT NULL,
	[TenNhaCungCap] [nvarchar](50) NOT NULL,
	[DiaChi] [nvarchar](100) NOT NULL,
	[SoDienThoai] [varchar](15) NOT NULL,
	[Email] [nvarchar](30) NULL,
	[TrangThai] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNhaCungCap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhanQuyen]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhanQuyen](
	[MaChucVu] [int] NOT NULL,
	[MaQuyen] [int] IDENTITY(1,1) NOT NULL,
	[ChuThich] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaChucVu] ASC,
	[MaQuyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuBanHang]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuBanHang](
	[SoPhieuBanHang] [int] IDENTITY(1,1) NOT NULL,
	[NgayBan] [date] NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[TenKhachHang] [nvarchar](max) NOT NULL,
	[SoDienThoai] [varchar](15) NOT NULL,
	[TongTien] [money] NOT NULL,
	[Ghichu] [nvarchar](max) NULL,
	[NgayChinhSua] [datetime] NULL,
	[TrangThai] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuBanHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuBaoHanh]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuBaoHanh](
	[SoPhieuBaoHanh] [int] IDENTITY(1,1) NOT NULL,
	[NgayLap] [date] NOT NULL,
	[NgayGiao] [date] NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[TenKhachHang] [nvarchar](max) NOT NULL,
	[SoDienThoai] [varchar](50) NOT NULL,
	[TongTien] [money] NOT NULL,
	[GhiChu] [nvarchar](max) NULL,
	[DaGiao] [bit] NOT NULL,
	[NgayChinhSua] [datetime] NULL,
	[TrangThai] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuBaoHanh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuChi]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuChi](
	[SoPhieuChi] [int] IDENTITY(1,1) NOT NULL,
	[NgayChi] [date] NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[SoPhieuNhapKho] [int] NOT NULL,
	[TongTienChi] [money] NOT NULL,
	[GhiChu] [nvarchar](max) NULL,
	[NgayChinhSua] [datetime] NULL,
	[TrangThai] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuChi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuDatHang]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuDatHang](
	[SoPhieuDatHang] [int] IDENTITY(1,1) NOT NULL,
	[NgayDat] [date] NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[TenKhachHang] [nvarchar](max) NOT NULL,
	[SoDienThoai] [varchar](15) NOT NULL,
	[Diachi] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](30) NULL,
	[TongTien] [money] NOT NULL,
	[HinhThucThanhToan] [nvarchar](max) NOT NULL,
	[Ghichu] [nvarchar](max) NULL,
	[NgayGiao] [date] NOT NULL,
	[DaXacNhan] [bit] NOT NULL,
	[DaThanhToan] [bit] NOT NULL,
	[TrangThai] [bit] NOT NULL,
	[NgayChinhSua] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuDatHang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuKiemKho]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuKiemKho](
	[SoPhieuKiemKho] [int] IDENTITY(1,1) NOT NULL,
	[NgayKiemKho] [date] NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[GhiChu] [nvarchar](max) NULL,
	[TrangThai] [bit] NOT NULL,
	[NgayChinhSua] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuKiemKho] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuNhapKho]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuNhapKho](
	[SoPhieuNhapKho] [int] IDENTITY(1,1) NOT NULL,
	[NgayNhap] [date] NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[MaNhaCungCap] [int] NOT NULL,
	[TongTien] [money] NOT NULL,
	[Ghichu] [nvarchar](max) NULL,
	[TrangThai] [bit] NOT NULL,
	[NgayChinhSua] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuNhapKho] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhieuXuatKho]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhieuXuatKho](
	[SoPhieuXuatKho] [int] IDENTITY(1,1) NOT NULL,
	[NgayXuat] [date] NOT NULL,
	[MaNguoiDung] [int] NOT NULL,
	[LyDoXuat] [nvarchar](max) NOT NULL,
	[TongTien] [money] NOT NULL,
	[TrangThai] [bit] NOT NULL,
	[NgayChinhSua] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SoPhieuXuatKho] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Quyen]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quyen](
	[MaQuyen] [int] IDENTITY(1,1) NOT NULL,
	[TenQuyen] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaQuyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThamSo]    Script Date: 11/16/2020 1:45:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThamSo](
	[MaThamSo] [int] IDENTITY(1,1) NOT NULL,
	[TenThamSo] [nvarchar](200) NOT NULL,
	[GiaTri] [float] NOT NULL,
	[NgayApDung] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaThamSo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BaoCaoBanHang]  WITH CHECK ADD  CONSTRAINT [FK_BaoCaoBanHang_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[BaoCaoBanHang] CHECK CONSTRAINT [FK_BaoCaoBanHang_NguoiDung]
GO
ALTER TABLE [dbo].[BaoCaoTonKho]  WITH CHECK ADD  CONSTRAINT [FK_BaoCaoTonKho_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[BaoCaoTonKho] CHECK CONSTRAINT [FK_BaoCaoTonKho_NguoiDung]
GO
ALTER TABLE [dbo].[ChiTietBaoCaoBanHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietBaoCaoBanHang_BaoCaoBanHang] FOREIGN KEY([MaBaoCaoBanHang])
REFERENCES [dbo].[BaoCaoBanHang] ([MaBaoCaoBanHang])
GO
ALTER TABLE [dbo].[ChiTietBaoCaoBanHang] CHECK CONSTRAINT [FK_ChiTietBaoCaoBanHang_BaoCaoBanHang]
GO
ALTER TABLE [dbo].[ChiTietBaoCaoTonKho]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietBaoCaoTonKho_BaoCaoTonKho] FOREIGN KEY([MaBaoCaoTonKho])
REFERENCES [dbo].[BaoCaoTonKho] ([MaBaoCaoTonKho])
GO
ALTER TABLE [dbo].[ChiTietBaoCaoTonKho] CHECK CONSTRAINT [FK_ChiTietBaoCaoTonKho_BaoCaoTonKho]
GO
ALTER TABLE [dbo].[ChiTietBaoCaoTonKho]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietBaoCaoTonKho_HangHoa] FOREIGN KEY([MaHangHoa])
REFERENCES [dbo].[HangHoa] ([MaHangHoa])
GO
ALTER TABLE [dbo].[ChiTietBaoCaoTonKho] CHECK CONSTRAINT [FK_ChiTietBaoCaoTonKho_HangHoa]
GO
ALTER TABLE [dbo].[ChiTietPhieuBanHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuBanHang_HangHoa] FOREIGN KEY([MaHangHoa])
REFERENCES [dbo].[HangHoa] ([MaHangHoa])
GO
ALTER TABLE [dbo].[ChiTietPhieuBanHang] CHECK CONSTRAINT [FK_ChiTietPhieuBanHang_HangHoa]
GO
ALTER TABLE [dbo].[ChiTietPhieuBanHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuBanHang_PhieuBanHang] FOREIGN KEY([SoPhieuBanHang])
REFERENCES [dbo].[PhieuBanHang] ([SoPhieuBanHang])
GO
ALTER TABLE [dbo].[ChiTietPhieuBanHang] CHECK CONSTRAINT [FK_ChiTietPhieuBanHang_PhieuBanHang]
GO
ALTER TABLE [dbo].[ChiTietPhieuBaoHanh]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuBaoHanh_HangHoa] FOREIGN KEY([MaHangHoa])
REFERENCES [dbo].[HangHoa] ([MaHangHoa])
GO
ALTER TABLE [dbo].[ChiTietPhieuBaoHanh] CHECK CONSTRAINT [FK_ChiTietPhieuBaoHanh_HangHoa]
GO
ALTER TABLE [dbo].[ChiTietPhieuBaoHanh]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuBaoHanh_PhieuBaoHanh] FOREIGN KEY([SoPhieuBaoHanh])
REFERENCES [dbo].[PhieuBaoHanh] ([SoPhieuBaoHanh])
GO
ALTER TABLE [dbo].[ChiTietPhieuBaoHanh] CHECK CONSTRAINT [FK_ChiTietPhieuBaoHanh_PhieuBaoHanh]
GO
ALTER TABLE [dbo].[ChiTietPhieuDatHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuDatHang_HangHoa] FOREIGN KEY([MaHangHoa])
REFERENCES [dbo].[HangHoa] ([MaHangHoa])
GO
ALTER TABLE [dbo].[ChiTietPhieuDatHang] CHECK CONSTRAINT [FK_ChiTietPhieuDatHang_HangHoa]
GO
ALTER TABLE [dbo].[ChiTietPhieuDatHang]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuDatHang_PhieuDatHang] FOREIGN KEY([SoPhieuDatHang])
REFERENCES [dbo].[PhieuDatHang] ([SoPhieuDatHang])
GO
ALTER TABLE [dbo].[ChiTietPhieuDatHang] CHECK CONSTRAINT [FK_ChiTietPhieuDatHang_PhieuDatHang]
GO
ALTER TABLE [dbo].[ChiTietPhieuKiemKho]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuKiemKho_HangHoa] FOREIGN KEY([MaHangHoa])
REFERENCES [dbo].[HangHoa] ([MaHangHoa])
GO
ALTER TABLE [dbo].[ChiTietPhieuKiemKho] CHECK CONSTRAINT [FK_ChiTietPhieuKiemKho_HangHoa]
GO
ALTER TABLE [dbo].[ChiTietPhieuKiemKho]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuKiemKho_PhieuKiemKho] FOREIGN KEY([SoPhieuKiemKho])
REFERENCES [dbo].[PhieuKiemKho] ([SoPhieuKiemKho])
GO
ALTER TABLE [dbo].[ChiTietPhieuKiemKho] CHECK CONSTRAINT [FK_ChiTietPhieuKiemKho_PhieuKiemKho]
GO
ALTER TABLE [dbo].[ChiTietPhieuNhapKho]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuNhapKho_HangHoa] FOREIGN KEY([MaHangHoa])
REFERENCES [dbo].[HangHoa] ([MaHangHoa])
GO
ALTER TABLE [dbo].[ChiTietPhieuNhapKho] CHECK CONSTRAINT [FK_ChiTietPhieuNhapKho_HangHoa]
GO
ALTER TABLE [dbo].[ChiTietPhieuNhapKho]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuNhapKho_PhieuNhapKho] FOREIGN KEY([SoPhieuNhapKho])
REFERENCES [dbo].[PhieuNhapKho] ([SoPhieuNhapKho])
GO
ALTER TABLE [dbo].[ChiTietPhieuNhapKho] CHECK CONSTRAINT [FK_ChiTietPhieuNhapKho_PhieuNhapKho]
GO
ALTER TABLE [dbo].[ChiTietPhieuXuatKho]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuXuatKho_HangHoa] FOREIGN KEY([MaHangHoa])
REFERENCES [dbo].[HangHoa] ([MaHangHoa])
GO
ALTER TABLE [dbo].[ChiTietPhieuXuatKho] CHECK CONSTRAINT [FK_ChiTietPhieuXuatKho_HangHoa]
GO
ALTER TABLE [dbo].[ChiTietPhieuXuatKho]  WITH CHECK ADD  CONSTRAINT [FK_ChiTietPhieuXuatKho_PhieuXuatKho] FOREIGN KEY([SoPhieuXuatKho])
REFERENCES [dbo].[PhieuXuatKho] ([SoPhieuXuatKho])
GO
ALTER TABLE [dbo].[ChiTietPhieuXuatKho] CHECK CONSTRAINT [FK_ChiTietPhieuXuatKho_PhieuXuatKho]
GO
ALTER TABLE [dbo].[HangHoa]  WITH CHECK ADD  CONSTRAINT [FK_HangHoa_LoaiHangHoa] FOREIGN KEY([MaLoaiHangHoa])
REFERENCES [dbo].[LoaiHangHoa] ([MaLoaiHangHoa])
GO
ALTER TABLE [dbo].[HangHoa] CHECK CONSTRAINT [FK_HangHoa_LoaiHangHoa]
GO
ALTER TABLE [dbo].[NguoiDung]  WITH CHECK ADD  CONSTRAINT [FK_NguoiDung_ChucVu] FOREIGN KEY([MaChucVu])
REFERENCES [dbo].[ChucVu] ([MaChucVu])
GO
ALTER TABLE [dbo].[NguoiDung] CHECK CONSTRAINT [FK_NguoiDung_ChucVu]
GO
ALTER TABLE [dbo].[PhanQuyen]  WITH CHECK ADD  CONSTRAINT [FK_PhanQuyen_ChucVu] FOREIGN KEY([MaChucVu])
REFERENCES [dbo].[ChucVu] ([MaChucVu])
GO
ALTER TABLE [dbo].[PhanQuyen] CHECK CONSTRAINT [FK_PhanQuyen_ChucVu]
GO
ALTER TABLE [dbo].[PhanQuyen]  WITH CHECK ADD  CONSTRAINT [FK_PhanQuyen_Quyen] FOREIGN KEY([MaQuyen])
REFERENCES [dbo].[Quyen] ([MaQuyen])
GO
ALTER TABLE [dbo].[PhanQuyen] CHECK CONSTRAINT [FK_PhanQuyen_Quyen]
GO
ALTER TABLE [dbo].[PhieuBanHang]  WITH CHECK ADD  CONSTRAINT [FK_PhieuBanHang_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[PhieuBanHang] CHECK CONSTRAINT [FK_PhieuBanHang_NguoiDung]
GO
ALTER TABLE [dbo].[PhieuBaoHanh]  WITH CHECK ADD  CONSTRAINT [FK_PhieuBaoHanh_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[PhieuBaoHanh] CHECK CONSTRAINT [FK_PhieuBaoHanh_NguoiDung]
GO
ALTER TABLE [dbo].[PhieuChi]  WITH CHECK ADD  CONSTRAINT [FK_PhieuChi_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[PhieuChi] CHECK CONSTRAINT [FK_PhieuChi_NguoiDung]
GO
ALTER TABLE [dbo].[PhieuChi]  WITH CHECK ADD  CONSTRAINT [FK_PhieuChi_PhieuNhapKho] FOREIGN KEY([SoPhieuNhapKho])
REFERENCES [dbo].[PhieuNhapKho] ([SoPhieuNhapKho])
GO
ALTER TABLE [dbo].[PhieuChi] CHECK CONSTRAINT [FK_PhieuChi_PhieuNhapKho]
GO
ALTER TABLE [dbo].[PhieuDatHang]  WITH CHECK ADD  CONSTRAINT [FK_PhieuDatHang_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[PhieuDatHang] CHECK CONSTRAINT [FK_PhieuDatHang_NguoiDung]
GO
ALTER TABLE [dbo].[PhieuKiemKho]  WITH CHECK ADD  CONSTRAINT [FK_PhieuKiemKho_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[PhieuKiemKho] CHECK CONSTRAINT [FK_PhieuKiemKho_NguoiDung]
GO
ALTER TABLE [dbo].[PhieuNhapKho]  WITH CHECK ADD  CONSTRAINT [FK_PhieuNhapKho_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[PhieuNhapKho] CHECK CONSTRAINT [FK_PhieuNhapKho_NguoiDung]
GO
ALTER TABLE [dbo].[PhieuNhapKho]  WITH CHECK ADD  CONSTRAINT [FK_PhieuNhapKho_NhaCungCap] FOREIGN KEY([MaNhaCungCap])
REFERENCES [dbo].[NhaCungCap] ([MaNhaCungCap])
GO
ALTER TABLE [dbo].[PhieuNhapKho] CHECK CONSTRAINT [FK_PhieuNhapKho_NhaCungCap]
GO
ALTER TABLE [dbo].[PhieuXuatKho]  WITH CHECK ADD  CONSTRAINT [FK_PhieuXuatKho_NguoiDung] FOREIGN KEY([MaNguoiDung])
REFERENCES [dbo].[NguoiDung] ([MaNguoiDung])
GO
ALTER TABLE [dbo].[PhieuXuatKho] CHECK CONSTRAINT [FK_PhieuXuatKho_NguoiDung]
GO
