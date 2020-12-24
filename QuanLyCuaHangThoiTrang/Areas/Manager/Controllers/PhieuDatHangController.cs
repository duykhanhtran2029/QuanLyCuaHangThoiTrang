using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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

            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuDatHang = db.ChiTietPhieuDatHangs.ToList();
            return View(phieuDatHangs.ToList());
        }

        public ActionResult DanhSachPhieuDatHang(string searchString, int page = 1, int pageSize = 10)
        {
            IList<PhieuDatHang> pdh = db.PhieuDatHangs.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                pdh = db.PhieuDatHangs.Where(
                phieudathang => phieudathang.NguoiDung.TenNguoiDung.Contains(searchString) ||
                phieudathang.TenKhachHang.Contains(searchString)).ToList();
            }
            //Add search later
            return View(pdh.ToPagedList(page, pageSize));
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
            return View(phieuDatHang);
        }

        // GET: PhieuDatHang/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");

            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
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

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuDatHang.MaNguoiDung);
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
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuDatHang.MaNguoiDung);
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
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuDatHang.MaNguoiDung);
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
            db.PhieuDatHangs.Remove(phieuDatHang);
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
