using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();
        public string HoTen = "";
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult NavTopBar()
        {
            ViewBag.MenWears = db.LoaiHangHoas.Where(lhh => lhh.GioiTinh == "Nam").ToList();
            ViewBag.WomenWears = db.LoaiHangHoas.Where(lhh => lhh.GioiTinh == "Nữ").ToList();
            return PartialView();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection form)
        {
            string username = form["username"].ToString();
            string password = form["password"].ToString();

            var nguoiDung = db.NguoiDungs.SingleOrDefault(n => n.UserName == username && n.PassWord == password);
            if(nguoiDung != null)
            {
                Session["Account"] = nguoiDung;
            }
            return RedirectToAction("Index", "Home");
        }
    }
}