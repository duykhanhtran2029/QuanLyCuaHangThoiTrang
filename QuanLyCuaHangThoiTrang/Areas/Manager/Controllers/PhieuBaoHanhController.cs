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
    public class PhieuBaoHanhController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuBaoHanh
        public ActionResult Index()
        {
            var phieuBaoHanhs = db.PhieuBaoHanhs.Include(p => p.PhieuBanHang);
            return View(phieuBaoHanhs.ToList());
        }

        // GET: PhieuBaoHanh/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuBaoHanh phieuBaoHanh = db.PhieuBaoHanhs.Find(id);
            if (phieuBaoHanh == null)
            {
                return HttpNotFound();
            }
            return View(phieuBaoHanh);
        }

        // GET: PhieuBaoHanh/Create
        public ActionResult Create()
        {
            ViewBag.SoPhieuBanHang = new SelectList(db.PhieuBanHangs, "SoPhieuBanHang", "TenKhachHang");
            return View();
        }

        // POST: PhieuBaoHanh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuBaoHanh,TenKhachHang,SoDienThoai,TongTien,GhiChu,NgayChinhSua,IsDeleted,SoPhieuBanHang")] PhieuBaoHanh phieuBaoHanh)
        {
            if (ModelState.IsValid)
            {
                db.PhieuBaoHanhs.Add(phieuBaoHanh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SoPhieuBanHang = new SelectList(db.PhieuBanHangs, "SoPhieuBanHang", "TenKhachHang", phieuBaoHanh.SoPhieuBanHang);
            return View(phieuBaoHanh);
        }

        // GET: PhieuBaoHanh/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuBaoHanh phieuBaoHanh = db.PhieuBaoHanhs.Find(id);
            if (phieuBaoHanh == null)
            {
                return HttpNotFound();
            }
            ViewBag.SoPhieuBanHang = new SelectList(db.PhieuBanHangs, "SoPhieuBanHang", "TenKhachHang", phieuBaoHanh.SoPhieuBanHang);
            return View(phieuBaoHanh);
        }

        // POST: PhieuBaoHanh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuBaoHanh,TenKhachHang,SoDienThoai,TongTien,GhiChu,NgayChinhSua,IsDeleted,SoPhieuBanHang")] PhieuBaoHanh phieuBaoHanh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuBaoHanh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SoPhieuBanHang = new SelectList(db.PhieuBanHangs, "SoPhieuBanHang", "TenKhachHang", phieuBaoHanh.SoPhieuBanHang);
            return View(phieuBaoHanh);
        }

        // GET: PhieuBaoHanh/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuBaoHanh phieuBaoHanh = db.PhieuBaoHanhs.Find(id);
            if (phieuBaoHanh == null)
            {
                return HttpNotFound();
            }
            return View(phieuBaoHanh);
        }

        // POST: PhieuBaoHanh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuBaoHanh phieuBaoHanh = db.PhieuBaoHanhs.Find(id);
            db.PhieuBaoHanhs.Remove(phieuBaoHanh);
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
