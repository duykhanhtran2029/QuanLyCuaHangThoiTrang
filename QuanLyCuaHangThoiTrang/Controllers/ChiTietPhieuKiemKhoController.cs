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
    public class ChiTietPhieuKiemKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: ChiTietPhieuKiemKho
        public ActionResult Index()
        {
            var chiTietPhieuKiemKhoes = db.ChiTietPhieuKiemKhoes.Include(c => c.HangHoa).Include(c => c.PhieuKiemKho);
            return View(chiTietPhieuKiemKhoes.ToList());
        }

        // GET: ChiTietPhieuKiemKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuKiemKho chiTietPhieuKiemKho = db.ChiTietPhieuKiemKhoes.Find(id);
            if (chiTietPhieuKiemKho == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuKiemKho);
        }

        // GET: ChiTietPhieuKiemKho/Create
        public ActionResult Create()
        {
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.SoPhieuKiemKho = new SelectList(db.PhieuKiemKhoes, "SoPhieuKiemKho", "GhiChu");
            return View();
        }

        // POST: ChiTietPhieuKiemKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuKiemKho,MaHangHoa,SoLuongHienTai,SoLuongKiemTra,TinhTrangHangHoa")] ChiTietPhieuKiemKho chiTietPhieuKiemKho)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietPhieuKiemKhoes.Add(chiTietPhieuKiemKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuKiemKho.MaHangHoa);
            ViewBag.SoPhieuKiemKho = new SelectList(db.PhieuKiemKhoes, "SoPhieuKiemKho", "GhiChu", chiTietPhieuKiemKho.SoPhieuKiemKho);
            return View(chiTietPhieuKiemKho);
        }

        // GET: ChiTietPhieuKiemKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuKiemKho chiTietPhieuKiemKho = db.ChiTietPhieuKiemKhoes.Find(id);
            if (chiTietPhieuKiemKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuKiemKho.MaHangHoa);
            ViewBag.SoPhieuKiemKho = new SelectList(db.PhieuKiemKhoes, "SoPhieuKiemKho", "GhiChu", chiTietPhieuKiemKho.SoPhieuKiemKho);
            return View(chiTietPhieuKiemKho);
        }

        // POST: ChiTietPhieuKiemKho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuKiemKho,MaHangHoa,SoLuongHienTai,SoLuongKiemTra,TinhTrangHangHoa")] ChiTietPhieuKiemKho chiTietPhieuKiemKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietPhieuKiemKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuKiemKho.MaHangHoa);
            ViewBag.SoPhieuKiemKho = new SelectList(db.PhieuKiemKhoes, "SoPhieuKiemKho", "GhiChu", chiTietPhieuKiemKho.SoPhieuKiemKho);
            return View(chiTietPhieuKiemKho);
        }

        // GET: ChiTietPhieuKiemKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuKiemKho chiTietPhieuKiemKho = db.ChiTietPhieuKiemKhoes.Find(id);
            if (chiTietPhieuKiemKho == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuKiemKho);
        }

        // POST: ChiTietPhieuKiemKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietPhieuKiemKho chiTietPhieuKiemKho = db.ChiTietPhieuKiemKhoes.Find(id);
            db.ChiTietPhieuKiemKhoes.Remove(chiTietPhieuKiemKho);
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
