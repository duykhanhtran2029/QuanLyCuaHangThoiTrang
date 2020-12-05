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
    public class BaoCaoBanHangController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: BaoCaoBanHang
        public ActionResult Index()
        {
            var baoCaoBanHangs = db.BaoCaoBanHangs.Include(b => b.NguoiDung);
            return View(baoCaoBanHangs.ToList());
        }

        // GET: BaoCaoBanHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            if (baoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoBanHang);
        }

        // GET: BaoCaoBanHang/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            return View();
        }

        // POST: BaoCaoBanHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBaoCaoBanHang,NgayBatDau,NgayKetThuc,SoLuongPhieuBanHang,TongTienBanHang,TongTienNhapHang,TongDoanhThu,MaNguoiDung,IsDeleted")] BaoCaoBanHang baoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                db.BaoCaoBanHangs.Add(baoCaoBanHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoBanHang.MaNguoiDung);
            return View(baoCaoBanHang);
        }

        // GET: BaoCaoBanHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            if (baoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoBanHang.MaNguoiDung);
            return View(baoCaoBanHang);
        }

        // POST: BaoCaoBanHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBaoCaoBanHang,NgayBatDau,NgayKetThuc,SoLuongPhieuBanHang,TongTienBanHang,TongTienNhapHang,TongDoanhThu,MaNguoiDung,IsDeleted")] BaoCaoBanHang baoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(baoCaoBanHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoBanHang.MaNguoiDung);
            return View(baoCaoBanHang);
        }

        // GET: BaoCaoBanHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            if (baoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoBanHang);
        }

        // POST: BaoCaoBanHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            db.BaoCaoBanHangs.Remove(baoCaoBanHang);
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
