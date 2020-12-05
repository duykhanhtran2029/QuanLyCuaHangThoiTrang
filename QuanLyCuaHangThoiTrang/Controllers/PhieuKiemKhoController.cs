using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Controllers
{
    public class PhieuKiemKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuKiemKho
        public ActionResult Index()
        {
            var phieuKiemKhoes = db.PhieuKiemKhoes.Include(p => p.NguoiDung);
            return View(phieuKiemKhoes.ToList());
        }

        // GET: PhieuKiemKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            if (phieuKiemKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuKiemKho);
        }

        // GET: PhieuKiemKho/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            return View();
        }

        // POST: PhieuKiemKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuKiemKho,NgayKiemKho,MaNguoiDung,GhiChu,IsDeleted,NgayChinhSua")] PhieuKiemKho phieuKiemKho)
        {
            if (ModelState.IsValid)
            {
                db.PhieuKiemKhoes.Add(phieuKiemKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuKiemKho.MaNguoiDung);
            return View(phieuKiemKho);
        }

        // GET: PhieuKiemKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            if (phieuKiemKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuKiemKho.MaNguoiDung);
            return View(phieuKiemKho);
        }

        // POST: PhieuKiemKho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuKiemKho,NgayKiemKho,MaNguoiDung,GhiChu,IsDeleted,NgayChinhSua")] PhieuKiemKho phieuKiemKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuKiemKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuKiemKho.MaNguoiDung);
            return View(phieuKiemKho);
        }

        // GET: PhieuKiemKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            if (phieuKiemKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuKiemKho);
        }

        // POST: PhieuKiemKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            db.PhieuKiemKhoes.Remove(phieuKiemKho);
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
