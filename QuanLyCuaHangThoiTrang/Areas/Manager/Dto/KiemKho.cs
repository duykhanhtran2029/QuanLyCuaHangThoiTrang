using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager
{
    public class KiemKho
    {
        public int SoPhieuKiemKho { get; set; }
        public DateTime NgayKiemKho { get; set; }
        public int MaNguoiDung { get; set; }
        public string GhiChu { get; set; }
        public bool IsDeleted { get; set; }
        public List<ChiTietPhieuKiemKho> chiTietPhieuKiemKhoes { get; set; }
    }
}