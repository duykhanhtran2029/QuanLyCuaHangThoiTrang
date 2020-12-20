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
    public class ChiTietPhieuNhapKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: ChiTietPhieuNhapKho
        public ActionResult Index()
        {
            var chiTietPhieuNhapKhoes = db.ChiTietPhieuNhapKhoes.Include(c => c.HangHoa).Include(c => c.PhieuNhapKho);
            return View(chiTietPhieuNhapKhoes.ToList());
        }

        // GET: ChiTietPhieuNhapKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuNhapKho chiTietPhieuNhapKho = db.ChiTietPhieuNhapKhoes.Find(id);
            if (chiTietPhieuNhapKho == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuNhapKho);
        }

        // GET: ChiTietPhieuNhapKho/Create
        public ActionResult Create()
        {
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.SoPhieuNhapKho = new SelectList(db.PhieuNhapKhoes, "SoPhieuNhapKho", "SoPhieuNhapKho");
            return View();
        }

        // POST: ChiTietPhieuNhapKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuNhapKho,MaHangHoa,SoLuong,GiaNhap,ThanhTien")] ChiTietPhieuNhapKho chiTietPhieuNhapKho)
        {
            if (ModelState.IsValid)
            {
                db.ChiTietPhieuNhapKhoes.Add(chiTietPhieuNhapKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuNhapKho.MaHangHoa);
            ViewBag.SoPhieuNhapKho = new SelectList(db.PhieuNhapKhoes, "SoPhieuNhapKho", "Ghichu", chiTietPhieuNhapKho.SoPhieuNhapKho);
            return View(chiTietPhieuNhapKho);
        }

        // GET: ChiTietPhieuNhapKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuNhapKho chiTietPhieuNhapKho = db.ChiTietPhieuNhapKhoes.Find(id);
            if (chiTietPhieuNhapKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuNhapKho.MaHangHoa);
            ViewBag.SoPhieuNhapKho = new SelectList(db.PhieuNhapKhoes, "SoPhieuNhapKho", "Ghichu", chiTietPhieuNhapKho.SoPhieuNhapKho);
            return View(chiTietPhieuNhapKho);
        }

        // POST: ChiTietPhieuNhapKho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuNhapKho,MaHangHoa,SoLuong,GiaNhap,MaNhaCungCap,ThanhTien")] ChiTietPhieuNhapKho chiTietPhieuNhapKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiTietPhieuNhapKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa", chiTietPhieuNhapKho.MaHangHoa);
            ViewBag.SoPhieuNhapKho = new SelectList(db.PhieuNhapKhoes, "SoPhieuNhapKho", "Ghichu", chiTietPhieuNhapKho.SoPhieuNhapKho);
            return View(chiTietPhieuNhapKho);
        }

        // GET: ChiTietPhieuNhapKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiTietPhieuNhapKho chiTietPhieuNhapKho = db.ChiTietPhieuNhapKhoes.Find(id);
            if (chiTietPhieuNhapKho == null)
            {
                return HttpNotFound();
            }
            return View(chiTietPhieuNhapKho);
        }

        // POST: ChiTietPhieuNhapKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiTietPhieuNhapKho chiTietPhieuNhapKho = db.ChiTietPhieuNhapKhoes.Find(id);
            db.ChiTietPhieuNhapKhoes.Remove(chiTietPhieuNhapKho);
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
