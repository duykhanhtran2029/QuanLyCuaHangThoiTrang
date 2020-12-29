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
    public class PhieuBanHangController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuBanHang
        public ActionResult Index()
        {
            var phieuBanHangs = db.PhieuBanHangs.Include(p => p.NguoiDung);
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(hh=>hh.IsDeleted == false), "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuBanHang = db.ChiTietPhieuBanHangs.ToList();
            return View(phieuBanHangs.ToList());
        }

        public ActionResult DanhSachPhieuBanHang(string searchString, int page = 1, int pageSize = 10)
        {
            IList<PhieuBanHang> pbh = db.PhieuBanHangs.Where(nc => nc.IsDeleted != true).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                pbh = db.PhieuBanHangs.Where(
                phieubanhang => phieubanhang.NguoiDung.TenNguoiDung.Contains(searchString) ||
                phieubanhang.TenKhachHang.Contains(searchString)).ToList();
            }
            //Add search later
            return View(pbh.ToPagedList(page, pageSize));
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
                GiamGia = result.GiamGia,
                SoLuong = result.SoLuong,
                MoTa = result.MoTa,
                ThoiGianBaoHanh = result.ThoiGianBaoHanh,
                ThuongHieu = result.ThuongHieu
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadChiTietPhieuBanHang(int id)
        {
            var result = db.ChiTietPhieuBanHangs.Where(ct => ct.SoPhieuBanHang == id)
                .Select(ct => new {
                    MaHangHoa = ct.MaHangHoa,
                    TenHangHoa = ct.HangHoa.TenHangHoa,
                    GiamGia = ct.HangHoa.GiamGia,
                    Size = ct.HangHoa.Size,
                    SoLuong = ct.SoLuong,
                    ThanhTien = ct.ThanhTien,
                    GiaBan = (double)ct.HangHoa.GiaBan * (1.0 - ct.HangHoa.GiamGia)
                }).ToList();
            var json = JsonConvert.SerializeObject(result);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public void DeleteAllCTPBH(int id)
        {
            var result = db.ChiTietPhieuBanHangs.Where(ct => ct.SoPhieuBanHang == id).ToList();
            foreach (var item in result)
            {
                db.ChiTietPhieuBanHangs.Remove(item);
            }
            db.SaveChanges();
        }

        public void SaveAllCTPBH(ICollection<ChiTietPhieuBanHang> chiTietPhieuBanHangs, int id)
        {
            foreach (var i in chiTietPhieuBanHangs)
            {
                var hanghoa = db.HangHoas.Where(hh => hh.MaHangHoa == i.MaHangHoa).FirstOrDefault();
                i.SoPhieuBanHang = id;
                db.ChiTietPhieuBanHangs.Add(i);
                db.SaveChanges();

                if (hanghoa != null)
                {
                    hanghoa.SoLuong -= i.SoLuong;
                    db.SaveChanges();
                }
            }
        }

        public bool CheckSoLuong(PhieuBanHang pbh)
        {
            foreach (var i in pbh.ChiTietPhieuBanHangs)
            {
                var hanghoa = db.HangHoas.Where(hh => hh.MaHangHoa == i.MaHangHoa).FirstOrDefault();
                if (i.SoLuong > hanghoa.SoLuong)
                {
                    return false;
                }
   
            }
            return true;
        }

        [HttpPost]
        public ActionResult LuuPhieuBanHang(PhieuBanHang phieuBanHang)
        {
            PhieuBanHang pbh = new PhieuBanHang
            {
                NgayBan = phieuBanHang.NgayBan,
                MaNguoiDung = phieuBanHang.MaNguoiDung,
                TenKhachHang = phieuBanHang.TenKhachHang,
                TongTien = phieuBanHang.TongTien,
                SoDienThoai = phieuBanHang.SoDienThoai,
                GhiChu = phieuBanHang.GhiChu,
                IsDeleted = phieuBanHang.IsDeleted,
                NgayChinhSua = DateTime.Now.Date
            };
            bool status = false;
            try
            {
                db.PhieuBanHangs.Add(pbh);
                db.SaveChanges();
                SaveAllCTPBH(phieuBanHang.ChiTietPhieuBanHangs, pbh.SoPhieuBanHang);
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
        public ActionResult LuuPhieuBanHangTuPhieuDatHang(PhieuBanHang phieuBanHang)
        {
            var idnguoidung = -1;
            if (Session["Account"] != null)
            {
                if (!CheckSoLuong(phieuBanHang))
                {
                    return new JsonResult { Data = new { status = false } };
                }
                NguoiDung a = Session["Account"] as NguoiDung;
                idnguoidung = a.MaNguoiDung;
                PhieuBanHang pbh = new PhieuBanHang
                {
                    NgayBan = phieuBanHang.NgayBan,
                    MaNguoiDung = idnguoidung,
                    TenKhachHang = phieuBanHang.TenKhachHang,
                    TongTien = phieuBanHang.TongTien,
                    SoDienThoai = phieuBanHang.SoDienThoai,
                    GhiChu = phieuBanHang.GhiChu,
                    IsDeleted = phieuBanHang.IsDeleted,
                    NgayChinhSua = DateTime.Now.Date
                };
                bool status = false;
                try
                {
                    db.PhieuBanHangs.Add(pbh);
                    db.SaveChanges();
                    SaveAllCTPBH(phieuBanHang.ChiTietPhieuBanHangs, pbh.SoPhieuBanHang);
                    status = true;
                }
                catch
                {
                    status = false;
                    throw;
                }
                return new JsonResult { Data = new { status = status } };
            }
            else //NO SESSION
            {
                if (!CheckSoLuong(phieuBanHang))
                {
                    return new JsonResult { Data = new { status = false } };
                }
                PhieuBanHang pbh = new PhieuBanHang
                {
                    NgayBan = phieuBanHang.NgayBan,
                    MaNguoiDung = phieuBanHang.MaNguoiDung,
                    TenKhachHang = phieuBanHang.TenKhachHang,
                    TongTien = phieuBanHang.TongTien,
                    SoDienThoai = phieuBanHang.SoDienThoai,
                    GhiChu = phieuBanHang.GhiChu,
                    IsDeleted = phieuBanHang.IsDeleted,
                    NgayChinhSua = DateTime.Now.Date
                };
                bool status = false;
                try
                {
                    db.PhieuBanHangs.Add(pbh);
                    db.SaveChanges();
                    SaveAllCTPBH(phieuBanHang.ChiTietPhieuBanHangs, pbh.SoPhieuBanHang);
                    status = true;
                }
                catch
                {
                    status = false;
                    throw;
                }
                return new JsonResult { Data = new { status = status } };
            }
            
        }

        [HttpPost]
        public ActionResult SuaPhieuBanHang(PhieuBanHang phieuBanHang)
        {
            bool status = false;
            try
            {
                var phieubanhang = db.PhieuBanHangs.SingleOrDefault(pbh => pbh.SoPhieuBanHang == phieuBanHang.SoPhieuBanHang);
                if (phieubanhang != null)
                {
                    phieubanhang.NgayBan = phieuBanHang.NgayBan;
                    phieubanhang.MaNguoiDung = phieuBanHang.MaNguoiDung;
                    phieubanhang.TenKhachHang = phieuBanHang.TenKhachHang;
                    phieubanhang.TongTien = phieuBanHang.TongTien;
                    phieubanhang.SoDienThoai = phieuBanHang.SoDienThoai;
                    phieubanhang.GhiChu = phieuBanHang.GhiChu;
                    phieubanhang.IsDeleted = phieuBanHang.IsDeleted;
                    phieubanhang.NgayChinhSua = DateTime.Now.Date;
                    db.SaveChanges();
                    status = true;
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

        // GET: PhieuBanHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuBanHang phieuBanHang = db.PhieuBanHangs.Find(id);
            if (phieuBanHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(i => i.IsDeleted != true), "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuBanHang = db.ChiTietPhieuBanHangs.ToList();
            return View(phieuBanHang);
        }

        // GET: PhieuBanHang/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");

            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(hh => hh.IsDeleted == false), "MaHangHoa", "TenHangHoa");

            return View();
        }

        // POST: PhieuBanHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuBanHang,NgayBan,MaNguoiDung,TenKhachHang,SoDienThoai,TongTien,GhiChu,NgayChinhSua,IsDeleted")] PhieuBanHang phieuBanHang)
        {
            if (ModelState.IsValid)
            {
                db.PhieuBanHangs.Add(phieuBanHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuBanHang.MaNguoiDung);
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(hh => hh.IsDeleted == false), "MaHangHoa", "TenHangHoa");
            return View(phieuBanHang);
        }

        // GET: PhieuBanHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuBanHang phieuBanHang = db.PhieuBanHangs.Find(id);
            if (phieuBanHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuBanHang.MaNguoiDung);
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(hh => hh.IsDeleted == false), "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuBanHang = db.ChiTietPhieuBanHangs.ToList();
            return View(phieuBanHang);
        }

        // POST: PhieuBanHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuBanHang,NgayBan,MaNguoiDung,TenKhachHang,SoDienThoai,TongTien,GhiChu,NgayChinhSua,IsDeleted")] PhieuBanHang phieuBanHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuBanHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuBanHang.MaNguoiDung);
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(hh => hh.IsDeleted == false), "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuBanHang = db.ChiTietPhieuBanHangs.ToList();
            return View(phieuBanHang);
        }

        // GET: PhieuBanHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuBanHang phieuBanHang = db.PhieuBanHangs.Find(id);
            if (phieuBanHang == null)
            {
                return HttpNotFound();
            }
            return View(phieuBanHang);
        }

        // POST: PhieuBanHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeleteAllCTPBH(id);
            PhieuBanHang phieuBanHang = db.PhieuBanHangs.Find(id);
            DeleteAllCTPBH(id);
            db.PhieuBanHangs.Remove(phieuBanHang);
            db.SaveChanges();
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
