using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager
{
    public class TonKho
    {
        public int MaBaoCaoTonKho { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
        public DateTime NgayLap { get; set; }
        public int MaNguoiDung { get; set; }
        public int TongSoHangHoa { get; set; }
        public bool IsDeleted { get; set; }
        public List<ChiTietBaoCaoTonKho> chiTietBaoCaoTonKhoes { get; set; }

    }
}