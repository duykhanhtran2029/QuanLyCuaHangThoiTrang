using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager.Controllers
{
    public class PhieuChiController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuChi
        public ActionResult Index()
        {
            var phieuChis = db.PhieuChis.Include(p => p.NguoiDung).Include(p => p.PhieuNhapKho);
            return View(phieuChis.ToList());
        }

        // GET: PhieuChi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            if (phieuChi == null)
            {
                return HttpNotFound();
            }
            return View(phieuChi);
        }

        // GET: PhieuChi/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            ViewBag.SoPhieuNhapKho = new SelectList(db.PhieuNhapKhoes, "SoPhieuNhapKho", "Ghichu");
            return View();
        }

        // POST: PhieuChi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuChi,NgayChi,MaNguoiDung,SoPhieuNhapKho,TongTienChi,GhiChu,NgayChinhSua,IsDeleted")] PhieuChi phieuChi)
        {
            if (ModelState.IsValid)
            {
                db.PhieuChis.Add(phieuChi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuChi.MaNguoiDung);
            ViewBag.SoPhieuNhapKho = new SelectList(db.PhieuNhapKhoes, "SoPhieuNhapKho", "Ghichu", phieuChi.SoPhieuNhapKho);
            return View(phieuChi);
        }

        // GET: PhieuChi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            if (phieuChi == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuChi.MaNguoiDung);
            ViewBag.SoPhieuNhapKho = new SelectList(db.PhieuNhapKhoes, "SoPhieuNhapKho", "Ghichu", phieuChi.SoPhieuNhapKho);
            return View(phieuChi);
        }

        // POST: PhieuChi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuChi,NgayChi,MaNguoiDung,SoPhieuNhapKho,TongTienChi,GhiChu,NgayChinhSua,IsDeleted")] PhieuChi phieuChi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuChi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuChi.MaNguoiDung);
            ViewBag.SoPhieuNhapKho = new SelectList(db.PhieuNhapKhoes, "SoPhieuNhapKho", "Ghichu", phieuChi.SoPhieuNhapKho);
            return View(phieuChi);
        }

        // GET: PhieuChi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            if (phieuChi == null)
            {
                return HttpNotFound();
            }
            return View(phieuChi);
        }

        // POST: PhieuChi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            db.PhieuChis.Remove(phieuChi);
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
