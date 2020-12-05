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
    public class ChiTietPhieuDatHangController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: ChiTietPhieuDatHang
        public ActionResult Index()
        {
            var chiTietPhieuDatHangs = db.ChiTietPhieuDatHangs.Include(c => c.HangHoa).Include(c => c.PhieuDatHang);
            return View(chiTietPhieuDatHangs.ToList());
        }

        // GET: ChiTietPhieuDatHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuDatHang chiTietPhieuDatHang = db.ChiTietPhieuDatHangs.Find(id);
            if (chiTietPhieuDatHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuDatHang);
        }

        // GET: ChiTietPhieuDatHang/Create
        public ActionResult Create()
        {
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.SoPhieuDatHang = new SelectList(db.PhieuDatHangs, "SoPhieuDatHang", "TenKhachHang");
            return View();
        }

        // POST: ChiTietPhieuDatHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuDatHang,MaHangHoa,SoLuong,Gia,ThanhTien")] ChiTietPhieuDatHang chiTietPhieuDatHang)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietPhieuDatHangs.Add(chiTietPhieuDatHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuDatHang.MaHangHoa);
            ViewBag.SoPhieuDatHang = new SelectList(db.PhieuDatHangs, "SoPhieuDatHang", "TenKhachHang", chiTietPhieuDatHang.SoPhieuDatHang);
            return View(chiTietPhieuDatHang);
        }

        // GET: ChiTietPhieuDatHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuDatHang chiTietPhieuDatHang = db.ChiTietPhieuDatHangs.Find(id);
            if (chiTietPhieuDatHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuDatHang.MaHangHoa);
            ViewBag.SoPhieuDatHang = new SelectList(db.PhieuDatHangs, "SoPhieuDatHang", "TenKhachHang", chiTietPhieuDatHang.SoPhieuDatHang);
            return View(chiTietPhieuDatHang);
        }

        // POST: ChiTietPhieuDatHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuDatHang,MaHangHoa,SoLuong,Gia,ThanhTien")] ChiTietPhieuDatHang chiTietPhieuDatHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietPhieuDatHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuDatHang.MaHangHoa);
            ViewBag.SoPhieuDatHang = new SelectList(db.PhieuDatHangs, "SoPhieuDatHang", "TenKhachHang", chiTietPhieuDatHang.SoPhieuDatHang);
            return View(chiTietPhieuDatHang);
        }

        // GET: ChiTietPhieuDatHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuDatHang chiTietPhieuDatHang = db.ChiTietPhieuDatHangs.Find(id);
            if (chiTietPhieuDatHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuDatHang);
        }

        // POST: ChiTietPhieuDatHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietPhieuDatHang chiTietPhieuDatHang = db.ChiTietPhieuDatHangs.Find(id);
            db.ChiTietPhieuDatHangs.Remove(chiTietPhieuDatHang);
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
