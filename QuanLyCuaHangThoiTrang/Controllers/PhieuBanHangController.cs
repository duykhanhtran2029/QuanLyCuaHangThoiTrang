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
    public class PhieuBanHangController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuBanHang
        public ActionResult Index()
        {
            var phieuBanHangs = db.PhieuBanHangs.Include(p => p.NguoiDung);
            return View(phieuBanHangs.ToList());
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
            return View(phieuBanHang);
        }

        // GET: PhieuBanHang/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            return View();
        }

        // POST: PhieuBanHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuBanHang,NgayBan,MaNguoiDung,TenKhachHang,SoDienThoai,TongTien,Ghichu,NgayChinhSua,IsDeleted")] PhieuBanHang phieuBanHang)
        {
            if (ModelState.IsValid)
            {
                db.PhieuBanHangs.Add(phieuBanHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuBanHang.MaNguoiDung);
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
            return View(phieuBanHang);
        }

        // POST: PhieuBanHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuBanHang,NgayBan,MaNguoiDung,TenKhachHang,SoDienThoai,TongTien,Ghichu,NgayChinhSua,IsDeleted")] PhieuBanHang phieuBanHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuBanHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuBanHang.MaNguoiDung);
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
            PhieuBanHang phieuBanHang = db.PhieuBanHangs.Find(id);
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
