using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QuanLyCuaHangThoiTrang.Extension;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();
        public string HoTen = "";
        public List<ChiTietPhieuDatHang> Cart = new List<ChiTietPhieuDatHang>();
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
                TempData["AlertType"] = "alert-success";
            else if (type == "warning")
                TempData["AlertType"] = "alert-warning";
            else if (type == "error")
                TempData["AlertType"] = "alert-danger";
        }

        public ActionResult Index()
        {
            ViewBag.MenWears = db.HangHoas.Where(hh => hh.LoaiHangHoa.GioiTinh == "Nam").ToList();
            ViewBag.WomenWears = db.HangHoas.Where(hh => hh.LoaiHangHoa.GioiTinh == "Nữ").ToList();
            ViewBag.Bags = db.HangHoas.Where(hh => hh.LoaiHangHoa.TenLoaiHangHoa == "Túi xách").ToList();
            ViewBag.FootWears = db.HangHoas.Where(hh => hh.LoaiHangHoa.TenLoaiHangHoa == "Giày Nam" || hh.LoaiHangHoa.TenLoaiHangHoa == "Giày Nữ"
            || hh.LoaiHangHoa.TenLoaiHangHoa == "Dép").ToList();
            //Load hang hoa
            ViewBag.MenWears_Sale = db.HangHoas.Where(hh => hh.LoaiHangHoa.GioiTinh == "Nam" && hh.GiamGia > 0).ToList();
            ViewBag.WomenWears_Sale = db.HangHoas.Where(hh => hh.LoaiHangHoa.GioiTinh == "Nữ" && hh.GiamGia > 0).ToList();
            ViewBag.Bags_Sale = db.HangHoas.Where(hh => hh.LoaiHangHoa.TenLoaiHangHoa == "Túi xách" && hh.GiamGia > 0).ToList();
            ViewBag.FootWears_Sale = db.HangHoas.Where(hh => (hh.LoaiHangHoa.TenLoaiHangHoa == "Giày Nam" && hh.GiamGia != 0) || (hh.LoaiHangHoa.TenLoaiHangHoa == "Giày Nữ" && hh.GiamGia != 0)
            || (hh.LoaiHangHoa.TenLoaiHangHoa == "Dép" && hh.GiamGia != 0)).ToList();
            //load hang hoa sale
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

        [Authorize(Roles = "KhachHang")]
        public ActionResult UserProfile()
        {
            if(Session["Account"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.UserProfile = (NguoiDung)Session["Account"];
            }
            return View();
        }

        public ActionResult NavTopBar()
        {
            ViewBag.MenWears = db.LoaiHangHoas.Where(lhh => lhh.GioiTinh == "Nam").ToList();
            ViewBag.WomenWears = db.LoaiHangHoas.Where(lhh => lhh.GioiTinh == "Nữ").ToList();
            ViewBag.Other = db.LoaiHangHoas.Where(lhh => lhh.GioiTinh != "Nữ" && lhh.GioiTinh != "Nam").ToList();
            return PartialView();
        }

        [HttpPost]
        public void AddToCart(int MAHANGHOA, float GIA, float GIAMGIA)
        {
            if (!Cart.Exists(o => o.MaHangHoa == MAHANGHOA))
            {
                ChiTietPhieuDatHang ctpdh = new ChiTietPhieuDatHang();
                ctpdh.MaHangHoa = MAHANGHOA;
                ctpdh.SoLuong = 1;
                ctpdh.Gia = (decimal)(GIA * (1 - GIAMGIA));
                ctpdh.ThanhTien = (decimal)(GIA * (1 - GIAMGIA)); ;
                Cart.Add(ctpdh);
            }
            else
            {
                for (int i = 0; i < Cart.Count; i++)
                {
                    if (Cart[i].MaHangHoa == MAHANGHOA)
                        Cart[i].SoLuong += 1;
                }
            }
            
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection form)
        {
            string username = form["username"].ToString();
            string password = MD5Encode.CreateMD5(form["password"].ToString()); 

            var nguoiDung = db.NguoiDungs.SingleOrDefault(n => n.UserName == username && n.PassWord == password);
            if(nguoiDung != null)
            {
                IEnumerable<ChucVu> listQuyen = db.ChucVus.Where(n => n.MaChucVu == nguoiDung.MaChucVu);
                string quyen = "";
                foreach (var item in listQuyen)
                {
                    quyen += item.TenChucVu + ",";
                }
                quyen = quyen.Substring(0, quyen.Length - 1);
                PhanQuyen(nguoiDung.UserName.ToString(), quyen);
                Session["Account"] = nguoiDung;
                HoTen = nguoiDung.TenNguoiDung;
                if (nguoiDung.ChucVu.TenChucVu != "KhachHang")
                    return RedirectToAction("Index", "Manager/Home");
                return RedirectToAction("Index", "Home");
            }
            SetAlert("Sai tài khoản hoặc mật khẩu!", "error");
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

        [Authorize(Roles = "Admin")]
        public ActionResult DangXuat()
        {
            Session["Account"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult DangKy(FormCollection form)
        {
            string username = form["username"].ToString();
            string password = MD5Encode.CreateMD5(form["password"].ToString());
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
                    MaChucVu = 2, // Customer
                    Avatar = ""
                }) ;
                db.SaveChanges();
                SetAlert("Tạo tài khoản thành công!", "success");
            }
            else
            {
                SetAlert("Tài khoản này đã có người sử dụng!", "error");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "KhachHang")]
        public ActionResult CapNhat(FormCollection form)
        {
            var user = (NguoiDung)Session["Account"];
            string name = form["name"].ToString();
            string phone = form["phone"].ToString();
            string email = form["email"].ToString();
            string address = form["address"].ToString();
            string password = form["password"].ToString();

            var nguoidung = db.NguoiDungs.SingleOrDefault(n => n.UserName == user.UserName);
            if (nguoidung != null)
            {
                nguoidung.TenNguoiDung = name;
                nguoidung.SoDienThoai = phone;
                nguoidung.Email = email;
                nguoidung.DiaChi = address;
                if (!String.IsNullOrEmpty(password))
                {               
                    nguoidung.PassWord = MD5Encode.CreateMD5(password);
                }
                db.SaveChanges();
                SetAlert("Cập nhật thông tin thành công!", "success");
                Session["Account"] = nguoidung;
            }
            else
            {
                SetAlert("Không thể cập nhật thông tin", "error");
            }
            return RedirectToAction("UserProfile");
        }
    }
}