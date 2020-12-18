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
        public ActionResult Index()
        {
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