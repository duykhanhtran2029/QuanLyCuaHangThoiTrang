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
    public class ChiTietBaoCaoBanHangController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: ChiTietBaoCaoBanHang
        public ActionResult Index()
        {
            var chiTietBaoCaoBanHangs = db.ChiTietBaoCaoBanHangs.Include(c => c.BaoCaoBanHang);
            return View(chiTietBaoCaoBanHangs.ToList());
        }

        // GET: ChiTietBaoCaoBanHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietBaoCaoBanHang chiTietBaoCaoBanHang = db.ChiTietBaoCaoBanHangs.Find(id);
            if (chiTietBaoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietBaoCaoBanHang);
        }

        // GET: ChiTietBaoCaoBanHang/Create
        public ActionResult Create()
        {
            ViewBag.MaBaoCaoBanHang = new SelectList(db.BaoCaoBanHangs, "MaBaoCaoBanHang", "MaBaoCaoBanHang");
            return View();
        }

        // POST: ChiTietBaoCaoBanHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBaoCaoBanHang,Ngay,SoLuongPhieuBanHang,DoanhThu,TiLe")] ChiTietBaoCaoBanHang chiTietBaoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietBaoCaoBanHangs.Add(chiTietBaoCaoBanHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaBaoCaoBanHang = new SelectList(db.BaoCaoBanHangs, "MaBaoCaoBanHang", "MaBaoCaoBanHang", chiTietBaoCaoBanHang.MaBaoCaoBanHang);
            return View(chiTietBaoCaoBanHang);
        }

        // GET: ChiTietBaoCaoBanHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietBaoCaoBanHang chiTietBaoCaoBanHang = db.ChiTietBaoCaoBanHangs.Find(id);
            if (chiTietBaoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaBaoCaoBanHang = new SelectList(db.BaoCaoBanHangs, "MaBaoCaoBanHang", "MaBaoCaoBanHang", chiTietBaoCaoBanHang.MaBaoCaoBanHang);
            return View(chiTietBaoCaoBanHang);
        }

        // POST: ChiTietBaoCaoBanHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBaoCaoBanHang,Ngay,SoLuongPhieuBanHang,DoanhThu,TiLe")] ChiTietBaoCaoBanHang chiTietBaoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietBaoCaoBanHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaBaoCaoBanHang = new SelectList(db.BaoCaoBanHangs, "MaBaoCaoBanHang", "MaBaoCaoBanHang", chiTietBaoCaoBanHang.MaBaoCaoBanHang);
            return View(chiTietBaoCaoBanHang);
        }

        // GET: ChiTietBaoCaoBanHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietBaoCaoBanHang chiTietBaoCaoBanHang = db.ChiTietBaoCaoBanHangs.Find(id);
            if (chiTietBaoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietBaoCaoBanHang);
        }

        // POST: ChiTietBaoCaoBanHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietBaoCaoBanHang chiTietBaoCaoBanHang = db.ChiTietBaoCaoBanHangs.Find(id);
            db.ChiTietBaoCaoBanHangs.Remove(chiTietBaoCaoBanHang);
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
