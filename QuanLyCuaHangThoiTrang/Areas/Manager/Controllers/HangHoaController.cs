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
    public class HangHoaController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: HangHoa
        public ActionResult Index()
        {
            var hangHoas = db.HangHoas.Include(h => h.LoaiHangHoa);
            return View(hangHoas.ToList());
        }

        // GET: HangHoa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangHoa hangHoa = db.HangHoas.Find(id);
            if (hangHoa == null)
            {
                return HttpNotFound();
            }
            return View(hangHoa);
        }

        // GET: HangHoa/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiHangHoa = new SelectList(db.LoaiHangHoas, "MaLoaiHangHoa", "TenLoaiHangHoa");
            return View();
        }

        // POST: HangHoa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHangHoa,TenHangHoa,GiamGia,GiaBan,SoLuong,Size,DonViTinh,MoTa,ThoiGianBaoHanh,HinhAnh,ThuongHieu,MaLoaiHangHoa,IsDeleted")] HangHoa hangHoa)
        {
            if (ModelState.IsValid)
            {
                db.HangHoas.Add(hangHoa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiHangHoa = new SelectList(db.LoaiHangHoas, "MaLoaiHangHoa", "TenLoaiHangHoa", hangHoa.MaLoaiHangHoa);
            return View(hangHoa);
        }

        // GET: HangHoa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangHoa hangHoa = db.HangHoas.Find(id);
            if (hangHoa == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiHangHoa = new SelectList(db.LoaiHangHoas, "MaLoaiHangHoa", "TenLoaiHangHoa", hangHoa.MaLoaiHangHoa);
            return View(hangHoa);
        }

        // POST: HangHoa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHangHoa,TenHangHoa,GiamGia,GiaBan,SoLuong,Size,DonViTinh,MoTa,ThoiGianBaoHanh,HinhAnh,ThuongHieu,MaLoaiHangHoa,IsDeleted")] HangHoa hangHoa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hangHoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiHangHoa = new SelectList(db.LoaiHangHoas, "MaLoaiHangHoa", "TenLoaiHangHoa", hangHoa.MaLoaiHangHoa);
            return View(hangHoa);
        }

        // GET: HangHoa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangHoa hangHoa = db.HangHoas.Find(id);
            if (hangHoa == null)
            {
                return HttpNotFound();
            }
            return View(hangHoa);
        }

        // POST: HangHoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HangHoa hangHoa = db.HangHoas.Find(id);
            db.HangHoas.Remove(hangHoa);
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
