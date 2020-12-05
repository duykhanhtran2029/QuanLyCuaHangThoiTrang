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
    public class ChiTietPhieuXuatKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: ChiTietPhieuXuatKho
        public ActionResult Index()
        {
            var chiTietPhieuXuatKhoes = db.ChiTietPhieuXuatKhoes.Include(c => c.HangHoa).Include(c => c.PhieuXuatKho);
            return View(chiTietPhieuXuatKhoes.ToList());
        }

        // GET: ChiTietPhieuXuatKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuXuatKho chiTietPhieuXuatKho = db.ChiTietPhieuXuatKhoes.Find(id);
            if (chiTietPhieuXuatKho == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuXuatKho);
        }

        // GET: ChiTietPhieuXuatKho/Create
        public ActionResult Create()
        {
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.SoPhieuXuatKho = new SelectList(db.PhieuXuatKhoes, "SoPhieuXuatKho", "LyDoXuat");
            return View();
        }

        // POST: ChiTietPhieuXuatKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuXuatKho,MaHangHoa,SoLuong,Gia,ThanhTien")] ChiTietPhieuXuatKho chiTietPhieuXuatKho)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietPhieuXuatKhoes.Add(chiTietPhieuXuatKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuXuatKho.MaHangHoa);
            ViewBag.SoPhieuXuatKho = new SelectList(db.PhieuXuatKhoes, "SoPhieuXuatKho", "LyDoXuat", chiTietPhieuXuatKho.SoPhieuXuatKho);
            return View(chiTietPhieuXuatKho);
        }

        // GET: ChiTietPhieuXuatKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuXuatKho chiTietPhieuXuatKho = db.ChiTietPhieuXuatKhoes.Find(id);
            if (chiTietPhieuXuatKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuXuatKho.MaHangHoa);
            ViewBag.SoPhieuXuatKho = new SelectList(db.PhieuXuatKhoes, "SoPhieuXuatKho", "LyDoXuat", chiTietPhieuXuatKho.SoPhieuXuatKho);
            return View(chiTietPhieuXuatKho);
        }

        // POST: ChiTietPhieuXuatKho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuXuatKho,MaHangHoa,SoLuong,Gia,ThanhTien")] ChiTietPhieuXuatKho chiTietPhieuXuatKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietPhieuXuatKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuXuatKho.MaHangHoa);
            ViewBag.SoPhieuXuatKho = new SelectList(db.PhieuXuatKhoes, "SoPhieuXuatKho", "LyDoXuat", chiTietPhieuXuatKho.SoPhieuXuatKho);
            return View(chiTietPhieuXuatKho);
        }

        // GET: ChiTietPhieuXuatKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuXuatKho chiTietPhieuXuatKho = db.ChiTietPhieuXuatKhoes.Find(id);
            if (chiTietPhieuXuatKho == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuXuatKho);
        }

        // POST: ChiTietPhieuXuatKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietPhieuXuatKho chiTietPhieuXuatKho = db.ChiTietPhieuXuatKhoes.Find(id);
            db.ChiTietPhieuXuatKhoes.Remove(chiTietPhieuXuatKho);
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
