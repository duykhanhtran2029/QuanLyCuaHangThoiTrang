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
        public List<ChiTietPhieuDatHang> Cart;
        public ActionResult Index()
        {
            ViewBag.MenWears = db.HangHoas.Where(hh => hh.LoaiHangHoa.GioiTinh == "Nam").ToList();
            ViewBag.WomenWears = db.HangHoas.Where(hh => hh.LoaiHangHoa.GioiTinh == "Nữ").ToList();
            ViewBag.Bags = db.HangHoas.Where(hh => hh.LoaiHangHoa.TenLoaiHangHoa == "Túi xách").ToList();
            ViewBag.FootWears = db.HangHoas.Where(hh => hh.LoaiHangHoa.TenLoaiHangHoa == "Giày Nam" || hh.LoaiHangHoa.TenLoaiHangHoa == "Giày Nữ" 
            || hh.LoaiHangHoa.TenLoaiHangHoa == "Dép").ToList();
            //Load hang hoa
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
            ViewBag.Other = db.LoaiHangHoas.Where(lhh => lhh.GioiTinh != "Nữ" && lhh.GioiTinh != "Nam").ToList();
            return PartialView();
        }

        //Them gio hang
        [HttpPost]
        public void AddtoCart()
        {
            ChiTietPhieuDatHang c = new ChiTietPhieuDatHang();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection form)
        {
            string username = form["username"].ToString();
            string password = form["password"].ToString();

            var nguoiDung = db.NguoiDungs.SingleOrDefault(n => n.UserName == username && n.PassWord == password);
            if(nguoiDung != null)
            {
                //IEnumerable<PhanQuyen> listQuyen = db.PhanQuyens.Where(n => n.MaChucVu == nguoiDung.MaChucVu);
                //string quyen = "";
                //foreach (var item in listQuyen)
                //{
                //    quyen += item.Quyen.TenQuyen + ",";
                //    Console.WriteLine(quyen);
                //}
               // quyen = quyen.Substring(0, quyen.Length - 1);
               // PhanQuyen(nguoiDung.UserName.ToString(), quyen);
                Session["Account"] = nguoiDung;
                HoTen = nguoiDung.TenNguoiDung;
                if (nguoiDung.ChucVu.TenChucVu != "KhachHang")
                    return RedirectToAction("Index", "Manager/Home");
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home"); // Need add notification login not success
        }

        public void PhanQuyen(string username, string quyen)
        {
            FormsAuthentication.Initialize();

            var ticket = new FormsAuthenticationTicket(1, username,
                DateTime.Now, DateTime.Now.AddHours(5),
                false, quyen, FormsAuthentication.FormsCookiePath);

            var cookies = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent)
            {
                cookies.Expires = ticket.Expiration;
            }
            Response.Cookies.Add(cookies);
        }

        [Authorize(Roles = "QuanLyPhanQuyen")]
        public ActionResult DangXuat()
        {
            Session["Account"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public ActionResult DangKy(FormCollection form)
        {
            string username = form["username"].ToString();
            string password = form["password"].ToString();
            string phone = form["phone"].ToString();
            string name = form["name"].ToString();
            var nguoiDung = db.NguoiDungs.SingleOrDefault(n => n.UserName == username);
            if (nguoiDung == null)
            {
                db.NguoiDungs.Add(new NguoiDung
                {
                    TenNguoiDung = name,
                    DiaChi = "",
                    SoDienThoai = phone,
                    Email = "",
                    CMND = "",
                    UserName = username,
                    PassWord = password,
                    IsDeleted = false,
                    MaChucVu = 6, // Customer
                    Avatar = ""
                }) ;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}