using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager.Controllers
{
    public class PhieuDatHangController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuDatHang
        public ActionResult Index()
        {
            var phieuDatHangs = db.PhieuDatHangs.Include(p => p.NguoiDung);

            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(hh => hh.IsDeleted == false), "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuDatHang = db.ChiTietPhieuDatHangs.ToList();
            return View(phieuDatHangs.ToList());
        }
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
        public ActionResult DanhSachPhieuDatHang(string searchString, int page = 1, int pageSize = 10)
        {
            IList<PhieuDatHang> pdh = db.PhieuDatHangs.Where(nc => nc.IsDeleted != true).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                pdh = db.PhieuDatHangs.Where(
                phieudathang => phieudathang.NguoiDung.TenNguoiDung.Contains(searchString) ||
                phieudathang.TenKhachHang.Contains(searchString)).ToList();
            }
            //Add search later
            return View(pdh.ToPagedList(page, pageSize));
        }

        public ActionResult LoadThongTinHangHoa(int id)
        {
            var result = db.HangHoas.Where(hh => hh.MaHangHoa == id).FirstOrDefault();
            return Json(new
            {
                TenHangHoa = result.TenHangHoa,
                DonViTinh = result.DonViTinh,
                Size = result.Size,
                GiaBan = result.GiaBan,
                GiamGia=result.GiamGia,
                SoLuong=result.SoLuong,
                MoTa=result.MoTa,
                ThoiGianBaoHanh= result.ThoiGianBaoHanh,
                ThuongHieu=result.ThuongHieu
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadChiTietPhieuDatHang(int id)
        {
            var result = db.ChiTietPhieuDatHangs.Where(ct => ct.SoPhieuDatHang == id)
                .Select(ct => new {
                    MaHangHoa = ct.MaHangHoa,
                    TenHangHoa = ct.HangHoa.TenHangHoa,
                    NgungKinhDoanh = ct.HangHoa.IsDeleted,
                    GiamGia = ct.HangHoa.GiamGia,
                    Size = ct.HangHoa.Size,
                    SoLuong = ct.SoLuong,
                    ThanhTien = ct.ThanhTien,
                    GiaBan=(double)ct.HangHoa.GiaBan* (1.0 - ct.HangHoa.GiamGia)
                }).ToList();
            var json = JsonConvert.SerializeObject(result);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public void DeleteAllCTPDH(int id)
        {
            var result = db.ChiTietPhieuDatHangs.Where(ct => ct.SoPhieuDatHang == id).ToList();
            foreach (var item in result)
            {
                db.ChiTietPhieuDatHangs.Remove(item);
            }
            db.SaveChanges();
        }

        public void SaveAllCTPDH(ICollection<ChiTietPhieuDatHang> chiTietPhieuDatHangs, int id)
        {
            foreach (var i in chiTietPhieuDatHangs)
            {
                i.SoPhieuDatHang = id;
                db.ChiTietPhieuDatHangs.Add(i);
                db.SaveChanges();
            }
        }

        [HttpPost]
        public ActionResult LuuPhieuDatHang(PhieuDatHang phieuDatHang)
        {
            PhieuDatHang pdh = new PhieuDatHang
            {
                NgayDat = phieuDatHang.NgayDat,
                MaNguoiDung = phieuDatHang.MaNguoiDung,
                TenKhachHang = phieuDatHang.TenKhachHang,
                TongTien = phieuDatHang.TongTien,
                SoDienThoai=phieuDatHang.SoDienThoai,
                Email=phieuDatHang.Email,
                Diachi=phieuDatHang.Diachi,
                GhiChu = phieuDatHang.GhiChu,
                HinhThucThanhToan=phieuDatHang.HinhThucThanhToan,
                NgayGiao=phieuDatHang.NgayGiao,
                DaThanhToan=phieuDatHang.DaThanhToan,
                DaXacNhan=phieuDatHang.DaXacNhan,
                IsDeleted = phieuDatHang.IsDeleted,
                NgayChinhSua = DateTime.Now.Date
            };
            bool status = false;
            try
            {
                db.PhieuDatHangs.Add(pdh);
                db.SaveChanges();
                SaveAllCTPDH(phieuDatHang.ChiTietPhieuDatHangs, pdh.SoPhieuDatHang);
                SetAlert("Tạo phiếu đặt hàng thành công!", "success");
                status = true;
            }
            catch
            {
                status = false;
                throw;
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public ActionResult SuaPhieuDatHang(PhieuDatHang phieuDatHang)
        {
            bool status = false;
            try
            {
                var phieudathang = db.PhieuDatHangs.SingleOrDefault(pdh => pdh.SoPhieuDatHang == phieuDatHang.SoPhieuDatHang);
                if (phieudathang != null)
                {
                    phieudathang.NgayDat = phieuDatHang.NgayDat;
                    phieudathang.MaNguoiDung = phieuDatHang.MaNguoiDung;
                    phieudathang.TenKhachHang = phieuDatHang.TenKhachHang;
                    phieudathang.TongTien = phieuDatHang.TongTien;
                    phieudathang.SoDienThoai = phieuDatHang.SoDienThoai;
                    phieudathang.Email = phieuDatHang.Email;
                    phieudathang.Diachi = phieuDatHang.Diachi;
                    phieudathang.GhiChu = phieuDatHang.GhiChu;
                    phieudathang.HinhThucThanhToan = phieuDatHang.HinhThucThanhToan;
                    phieudathang.NgayGiao = phieuDatHang.NgayGiao;
                    phieudathang.DaThanhToan = phieuDatHang.DaThanhToan;
                    phieudathang.DaXacNhan = phieuDatHang.DaXacNhan;
                    phieudathang.IsDeleted = phieuDatHang.IsDeleted;
                    phieudathang.NgayChinhSua = DateTime.Now.Date;
                    DeleteAllCTPDH(phieudathang.SoPhieuDatHang);
                    SaveAllCTPDH(phieuDatHang.ChiTietPhieuDatHangs, phieudathang.SoPhieuDatHang);
                    db.SaveChanges();
                    status = true;
                    SetAlert("Sửa phiếu đặt hàng thành công!", "success");
                }
                else
                    status = false;

            }
            catch
            {
                status = false;
                throw;
            }
            return new JsonResult { Data = new { status = status } };
        }

        // GET: PhieuDatHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuDatHang phieuDatHang = db.PhieuDatHangs.Find(id);
            if (phieuDatHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(i => i.IsDeleted != true), "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuDatHang = db.ChiTietPhieuDatHangs.ToList();
            return View(phieuDatHang);
        }

        // GET: PhieuDatHang/Create
        public ActionResult Create()
        {
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(hh => hh.IsDeleted == false), "MaHangHoa", "TenHangHoa");
            return View();
        }

        // POST: PhieuDatHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuDatHang,NgayDat,MaNguoiDung,TenKhachHang,SoDienThoai,Diachi,Email,TongTien,HinhThucThanhToan,GhiChu,NgayGiao,DaXacNhan,DaThanhToan,IsDeleted,NgayChinhSua")] PhieuDatHang phieuDatHang)
        {
            if (ModelState.IsValid)
            {
                db.PhieuDatHangs.Add(phieuDatHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(hh => hh.IsDeleted == false), "MaHangHoa", "TenHangHoa");
            return View(phieuDatHang);
        }

        // GET: PhieuDatHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuDatHang phieuDatHang = db.PhieuDatHangs.Find(id);
            if (phieuDatHang == null)
            {
                return HttpNotFound();
            }
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung"); ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(hh => hh.IsDeleted == false), "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuDatHang = db.ChiTietPhieuDatHangs.ToList();
            return View(phieuDatHang);
        }

        // POST: PhieuDatHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuDatHang,NgayDat,MaNguoiDung,TenKhachHang,SoDienThoai,Diachi,Email,TongTien,HinhThucThanhToan,GhiChu,NgayGiao,DaXacNhan,DaThanhToan,IsDeleted,NgayChinhSua")] PhieuDatHang phieuDatHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuDatHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung"); ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(hh => hh.IsDeleted == false), "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuDatHang = db.ChiTietPhieuDatHangs.ToList();
            return View(phieuDatHang);
        }

        // GET: PhieuDatHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuDatHang phieuDatHang = db.PhieuDatHangs.Find(id);
            if (phieuDatHang == null)
            {
                return HttpNotFound();
            }
            return View(phieuDatHang);
        }

        // POST: PhieuDatHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuDatHang phieuDatHang = db.PhieuDatHangs.Find(id);
            DeleteAllCTPDH(id);
            db.PhieuDatHangs.Remove(phieuDatHang);
            db.SaveChanges();
            SetAlert("Xóa phiếu đặt hàng thành công!", "success");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
