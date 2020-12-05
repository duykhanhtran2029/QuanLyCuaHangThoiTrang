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
    public class ChiTietBaoCaoTonKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: ChiTietBaoCaoTonKho
        public ActionResult Index()
        {
            var chiTietBaoCaoTonKhoes = db.ChiTietBaoCaoTonKhoes.Include(c => c.BaoCaoTonKho).Include(c => c.HangHoa);
            return View(chiTietBaoCaoTonKhoes.ToList());
        }

        // GET: ChiTietBaoCaoTonKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietBaoCaoTonKho chiTietBaoCaoTonKho = db.ChiTietBaoCaoTonKhoes.Find(id);
            if (chiTietBaoCaoTonKho == null)
            {
                return HttpNotFound();
            }
            return View(chiTietBaoCaoTonKho);
        }

        // GET: ChiTietBaoCaoTonKho/Create
        public ActionResult Create()
        {
            ViewBag.MaBaoCaoTonKho = new SelectList(db.BaoCaoTonKhoes, "MaBaoCaoTonKho", "MaBaoCaoTonKho");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            return View();
        }

        // POST: ChiTietBaoCaoTonKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBaoCaoTonKho,MaHangHoa,SoLuongTonDau,SoLuongNhap,SoLuongXuat,SoLuongTonCuoi,TinhTrangHangHoa")] ChiTietBaoCaoTonKho chiTietBaoCaoTonKho)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietBaoCaoTonKhoes.Add(chiTietBaoCaoTonKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaBaoCaoTonKho = new SelectList(db.BaoCaoTonKhoes, "MaBaoCaoTonKho", "MaBaoCaoTonKho", chiTietBaoCaoTonKho.MaBaoCaoTonKho);
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietBaoCaoTonKho.MaHangHoa);
            return View(chiTietBaoCaoTonKho);
        }

        // GET: ChiTietBaoCaoTonKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietBaoCaoTonKho chiTietBaoCaoTonKho = db.ChiTietBaoCaoTonKhoes.Find(id);
            if (chiTietBaoCaoTonKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaBaoCaoTonKho = new SelectList(db.BaoCaoTonKhoes, "MaBaoCaoTonKho", "MaBaoCaoTonKho", chiTietBaoCaoTonKho.MaBaoCaoTonKho);
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietBaoCaoTonKho.MaHangHoa);
            return View(chiTietBaoCaoTonKho);
        }

        // POST: ChiTietBaoCaoTonKho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBaoCaoTonKho,MaHangHoa,SoLuongTonDau,SoLuongNhap,SoLuongXuat,SoLuongTonCuoi,TinhTrangHangHoa")] ChiTietBaoCaoTonKho chiTietBaoCaoTonKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietBaoCaoTonKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaBaoCaoTonKho = new SelectList(db.BaoCaoTonKhoes, "MaBaoCaoTonKho", "MaBaoCaoTonKho", chiTietBaoCaoTonKho.MaBaoCaoTonKho);
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietBaoCaoTonKho.MaHangHoa);
            return View(chiTietBaoCaoTonKho);
        }

        // GET: ChiTietBaoCaoTonKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietBaoCaoTonKho chiTietBaoCaoTonKho = db.ChiTietBaoCaoTonKhoes.Find(id);
            if (chiTietBaoCaoTonKho == null)
            {
                return HttpNotFound();
            }
            return View(chiTietBaoCaoTonKho);
        }

        // POST: ChiTietBaoCaoTonKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietBaoCaoTonKho chiTietBaoCaoTonKho = db.ChiTietBaoCaoTonKhoes.Find(id);
            db.ChiTietBaoCaoTonKhoes.Remove(chiTietBaoCaoTonKho);
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
