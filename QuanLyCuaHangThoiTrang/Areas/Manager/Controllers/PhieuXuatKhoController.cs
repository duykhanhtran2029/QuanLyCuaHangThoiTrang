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
    public class PhieuXuatKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuXuatKho
        public ActionResult Index()
        {
            var phieuXuatKhoes = db.PhieuXuatKhoes.Include(p => p.NguoiDung);
            return View(phieuXuatKhoes.ToList());
        }

        // GET: PhieuXuatKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuXuatKho phieuXuatKho = db.PhieuXuatKhoes.Find(id);
            if (phieuXuatKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuXuatKho);
        }

        // GET: PhieuXuatKho/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            return View();
        }

        // POST: PhieuXuatKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuXuatKho,NgayXuatKho,MaNguoiDung,LyDoXuat,TongTien,IsDeleted,NgayChinhSua")] PhieuXuatKho phieuXuatKho)
        {
            if (ModelState.IsValid)
            {
                db.PhieuXuatKhoes.Add(phieuXuatKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuXuatKho.MaNguoiDung);
            return View(phieuXuatKho);
        }

        // GET: PhieuXuatKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuXuatKho phieuXuatKho = db.PhieuXuatKhoes.Find(id);
            if (phieuXuatKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuXuatKho.MaNguoiDung);
            return View(phieuXuatKho);
        }

        // POST: PhieuXuatKho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuXuatKho,NgayXuatKho,MaNguoiDung,LyDoXuat,TongTien,IsDeleted,NgayChinhSua")] PhieuXuatKho phieuXuatKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuXuatKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuXuatKho.MaNguoiDung);
            return View(phieuXuatKho);
        }

        // GET: PhieuXuatKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuXuatKho phieuXuatKho = db.PhieuXuatKhoes.Find(id);
            if (phieuXuatKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuXuatKho);
        }

        // POST: PhieuXuatKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuXuatKho phieuXuatKho = db.PhieuXuatKhoes.Find(id);
            db.PhieuXuatKhoes.Remove(phieuXuatKho);
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
