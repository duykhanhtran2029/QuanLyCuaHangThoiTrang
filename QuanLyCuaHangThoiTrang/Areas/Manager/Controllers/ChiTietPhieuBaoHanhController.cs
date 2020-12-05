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
    public class ChiTietPhieuBaoHanhController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: ChiTietPhieuBaoHanh
        public ActionResult Index()
        {
            var chiTietPhieuBaoHanhs = db.ChiTietPhieuBaoHanhs.Include(c => c.HangHoa).Include(c => c.PhieuBaoHanh);
            return View(chiTietPhieuBaoHanhs.ToList());
        }

        // GET: ChiTietPhieuBaoHanh/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuBaoHanh chiTietPhieuBaoHanh = db.ChiTietPhieuBaoHanhs.Find(id);
            if (chiTietPhieuBaoHanh == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuBaoHanh);
        }

        // GET: ChiTietPhieuBaoHanh/Create
        public ActionResult Create()
        {
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.SoPhieuBaoHanh = new SelectList(db.PhieuBaoHanhs, "SoPhieuBaoHanh", "TenKhachHang");
            return View();
        }

        // POST: ChiTietPhieuBaoHanh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuBaoHanh,MaHangHoa,SoLuong,DonGia,ThanhTien,NoiDungBaoHanh,GhiChu")] ChiTietPhieuBaoHanh chiTietPhieuBaoHanh)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietPhieuBaoHanhs.Add(chiTietPhieuBaoHanh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuBaoHanh.MaHangHoa);
            ViewBag.SoPhieuBaoHanh = new SelectList(db.PhieuBaoHanhs, "SoPhieuBaoHanh", "TenKhachHang", chiTietPhieuBaoHanh.SoPhieuBaoHanh);
            return View(chiTietPhieuBaoHanh);
        }

        // GET: ChiTietPhieuBaoHanh/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuBaoHanh chiTietPhieuBaoHanh = db.ChiTietPhieuBaoHanhs.Find(id);
            if (chiTietPhieuBaoHanh == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuBaoHanh.MaHangHoa);
            ViewBag.SoPhieuBaoHanh = new SelectList(db.PhieuBaoHanhs, "SoPhieuBaoHanh", "TenKhachHang", chiTietPhieuBaoHanh.SoPhieuBaoHanh);
            return View(chiTietPhieuBaoHanh);
        }

        // POST: ChiTietPhieuBaoHanh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuBaoHanh,MaHangHoa,SoLuong,DonGia,ThanhTien,NoiDungBaoHanh,GhiChu")] ChiTietPhieuBaoHanh chiTietPhieuBaoHanh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietPhieuBaoHanh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuBaoHanh.MaHangHoa);
            ViewBag.SoPhieuBaoHanh = new SelectList(db.PhieuBaoHanhs, "SoPhieuBaoHanh", "TenKhachHang", chiTietPhieuBaoHanh.SoPhieuBaoHanh);
            return View(chiTietPhieuBaoHanh);
        }

        // GET: ChiTietPhieuBaoHanh/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuBaoHanh chiTietPhieuBaoHanh = db.ChiTietPhieuBaoHanhs.Find(id);
            if (chiTietPhieuBaoHanh == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuBaoHanh);
        }

        // POST: ChiTietPhieuBaoHanh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietPhieuBaoHanh chiTietPhieuBaoHanh = db.ChiTietPhieuBaoHanhs.Find(id);
            db.ChiTietPhieuBaoHanhs.Remove(chiTietPhieuBaoHanh);
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
