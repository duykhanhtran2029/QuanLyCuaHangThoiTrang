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
    public class BaoCaoTonKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: BaoCaoTonKho
        public ActionResult Index()
        {
            var baoCaoTonKhoes = db.BaoCaoTonKhoes.Include(b => b.NguoiDung);
            return View(baoCaoTonKhoes.ToList());
        }

        // GET: BaoCaoTonKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoTonKho baoCaoTonKho = db.BaoCaoTonKhoes.Find(id);
            if (baoCaoTonKho == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoTonKho);
        }

        // GET: BaoCaoTonKho/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            return View();
        }

        // POST: BaoCaoTonKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBaoCaoTonKho,Thang,Nam,MaNguoiDung,NgayLap,TongSoHangHoa,IsDeleted")] BaoCaoTonKho baoCaoTonKho)
        {
            if (ModelState.IsValid)
            {
                db.BaoCaoTonKhoes.Add(baoCaoTonKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoTonKho.MaNguoiDung);
            return View(baoCaoTonKho);
        }

        // GET: BaoCaoTonKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoTonKho baoCaoTonKho = db.BaoCaoTonKhoes.Find(id);
            if (baoCaoTonKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoTonKho.MaNguoiDung);
            return View(baoCaoTonKho);
        }

        // POST: BaoCaoTonKho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBaoCaoTonKho,Thang,Nam,MaNguoiDung,NgayLap,TongSoHangHoa,IsDeleted")] BaoCaoTonKho baoCaoTonKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(baoCaoTonKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoTonKho.MaNguoiDung);
            return View(baoCaoTonKho);
        }

        // GET: BaoCaoTonKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoTonKho baoCaoTonKho = db.BaoCaoTonKhoes.Find(id);
            if (baoCaoTonKho == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoTonKho);
        }

        // POST: BaoCaoTonKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BaoCaoTonKho baoCaoTonKho = db.BaoCaoTonKhoes.Find(id);
            db.BaoCaoTonKhoes.Remove(baoCaoTonKho);
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
