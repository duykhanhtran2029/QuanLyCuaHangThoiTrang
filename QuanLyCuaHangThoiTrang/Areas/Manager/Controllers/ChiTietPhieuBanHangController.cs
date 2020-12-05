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
    public class ChiTietPhieuBanHangController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: ChiTietPhieuBanHang
        public ActionResult Index()
        {
            var chiTietPhieuBanHangs = db.ChiTietPhieuBanHangs.Include(c => c.HangHoa).Include(c => c.PhieuBanHang);
            return View(chiTietPhieuBanHangs.ToList());
        }

        // GET: ChiTietPhieuBanHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuBanHang chiTietPhieuBanHang = db.ChiTietPhieuBanHangs.Find(id);
            if (chiTietPhieuBanHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuBanHang);
        }

        // GET: ChiTietPhieuBanHang/Create
        public ActionResult Create()
        {
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.SoPhieuBanHang = new SelectList(db.PhieuBanHangs, "SoPhieuBanHang", "TenKhachHang");
            return View();
        }

        // POST: ChiTietPhieuBanHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuBanHang,MaHangHoa,SoLuong,Gia,ThanhTien")] ChiTietPhieuBanHang chiTietPhieuBanHang)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietPhieuBanHangs.Add(chiTietPhieuBanHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuBanHang.MaHangHoa);
            ViewBag.SoPhieuBanHang = new SelectList(db.PhieuBanHangs, "SoPhieuBanHang", "TenKhachHang", chiTietPhieuBanHang.SoPhieuBanHang);
            return View(chiTietPhieuBanHang);
        }

        // GET: ChiTietPhieuBanHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuBanHang chiTietPhieuBanHang = db.ChiTietPhieuBanHangs.Find(id);
            if (chiTietPhieuBanHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuBanHang.MaHangHoa);
            ViewBag.SoPhieuBanHang = new SelectList(db.PhieuBanHangs, "SoPhieuBanHang", "TenKhachHang", chiTietPhieuBanHang.SoPhieuBanHang);
            return View(chiTietPhieuBanHang);
        }

        // POST: ChiTietPhieuBanHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuBanHang,MaHangHoa,SoLuong,Gia,ThanhTien")] ChiTietPhieuBanHang chiTietPhieuBanHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietPhieuBanHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuBanHang.MaHangHoa);
            ViewBag.SoPhieuBanHang = new SelectList(db.PhieuBanHangs, "SoPhieuBanHang", "TenKhachHang", chiTietPhieuBanHang.SoPhieuBanHang);
            return View(chiTietPhieuBanHang);
        }

        // GET: ChiTietPhieuBanHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuBanHang chiTietPhieuBanHang = db.ChiTietPhieuBanHangs.Find(id);
            if (chiTietPhieuBanHang == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuBanHang);
        }

        // POST: ChiTietPhieuBanHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietPhieuBanHang chiTietPhieuBanHang = db.ChiTietPhieuBanHangs.Find(id);
            db.ChiTietPhieuBanHangs.Remove(chiTietPhieuBanHang);
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
