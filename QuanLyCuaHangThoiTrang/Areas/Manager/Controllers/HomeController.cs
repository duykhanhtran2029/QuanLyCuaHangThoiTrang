using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager.Controllers
{
    public class HomeController : Controller
    {
        // GET: Manager/Home
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();
        public ActionResult Index()
        {
            //Session["Account"] = db.NguoiDungs.Find(2);
            ViewBag.SoPhieuDatHang = db.PhieuDatHangs.Count();
            ViewBag.SoPhieuBanHang = db.PhieuBanHangs.Count();
            ViewBag.SoPhieuChi = db.PhieuChis.Count();
            ViewBag.SoLuongTonKho = db.HangHoas.Sum(hh => (int?)hh.SoLuong) ?? 0;

            ViewBag.PDHXacNhan = db.PhieuDatHangs.Where(pdh => pdh.DaXacNhan).Count();
            ViewBag.TongTienBanHang = db.PhieuBanHangs.Sum(pbh => (decimal?)pbh.TongTien) ?? 0;
            ViewBag.TongTienChi = db.PhieuChis.Sum(pc => (decimal?)pc.TongTienChi) ?? 0;
            ViewBag.SoLuongNhapKho = db.ChiTietPhieuNhapKhoes.Sum(ct => (decimal?)ct.SoLuong) ?? 0;
            ViewBag.SoLuongXuatKho = db.ChiTietPhieuXuatKhoes.Sum(ct => (decimal?)ct.SoLuong) ?? 0;

            ViewBag.HetHang = db.HangHoas.Where(hh => hh.SoLuong == 0).Count();
            ViewBag.SapHetHang = db.HangHoas.Where(hh => hh.SoLuong < 5 && hh.SoLuong > 0).Count();

            ViewBag.SanPham = db.HangHoas.Count() + "/" + db.LoaiHangHoas.Count();
            ViewBag.KinhDoanh = db.HangHoas.Where(hh => hh.IsDeleted == false).Count() + "/" + db.HangHoas.Where(hh => hh.IsDeleted == true).Count();
            return View();
        }


        public ActionResult DangXuat()
        {
            Session["Account"] = null;
            FormsAuthentication.SignOut();
            return Redirect("~/Home/Index");
        }

        public class Quyen
        {
            public int MaChucVu { set; get; }
            public int MaQuyen { set; get; }
            public string TenQuyen { set; get; }
        }
        public ActionResult GetMenu()
        {
            var db = new QuanLyCuaHangThoiTrangDbContext();
            List<string> quyenStr = new List<string>();
            var user = (NguoiDung)Session["Account"];
            var quyen = (List<Quyen>) null;
            if (user != null)
            {
                quyen = (from pq in db.PhanQuyens
                             join q in db.Quyens on pq.MaQuyen equals q.MaQuyen
                             where pq.MaChucVu == user.MaChucVu
                             select new Quyen
                             {
                                 MaChucVu = pq.MaChucVu,
                                 MaQuyen = pq.MaQuyen,
                                 TenQuyen = q.TenQuyen
                             }).ToList();

                foreach (var item in quyen)
                {
                    quyenStr.Add(item.TenQuyen);
                }
                TempData["ListMenu"] = quyenStr;
            }
            return null;
        }
    }
}