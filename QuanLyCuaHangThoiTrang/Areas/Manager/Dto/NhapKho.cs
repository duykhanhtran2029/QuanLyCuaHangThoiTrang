using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager
{
    public class NhapKho
    {
        public int SoPhieuNhapKho { get; set; }
        public DateTime NgayNhapKho { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaNhaCungCap { get; set; }
        public decimal TongTien { get; set; }
        public string GhiChu { get; set; }
        public bool IsDeleted { get; set; }
        public List<ChiTietPhieuNhapKho> chiTietPhieuNhapKhoes { get; set; }
    }
}