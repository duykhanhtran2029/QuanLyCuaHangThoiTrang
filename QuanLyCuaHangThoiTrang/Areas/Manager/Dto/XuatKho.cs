using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager
{
    public class XuatKho
    {
        public int SoPhieuXuatKho { get; set; }
        public DateTime NgayXuatKho { get; set; }
        public int MaNguoiDung { get; set; }
        public string LyDoXuat { get; set; }
        public decimal TongTien { get; set; }
        public bool IsDeleted { get; set; }
        public List<ChiTietPhieuXuatKho> chiTietPhieuXuatKhoes { get; set; }
    }
}