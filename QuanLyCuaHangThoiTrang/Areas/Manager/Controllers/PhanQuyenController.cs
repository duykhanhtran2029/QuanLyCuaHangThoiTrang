using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager.Controllers
{
    //[Authorize(Roles = "Dev")]
    public class PhanQuyenController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhanQuyen
        public ActionResult Index()
        {
            var phanQuyens = db.PhanQuyens.Include(p => p.ChucVu).Include(p => p.Quyen);
            return View(phanQuyens.ToList());
        }

        // GET: PhanQuyen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanQuyen phanQuyen = db.PhanQuyens.Find(id);
            if (phanQuyen == null)
            {
                return HttpNotFound();
            }
            return View(phanQuyen);
        }

        public ActionResult DanhSachPhanQuyen(string searchString, int page = 1, int pageSize = 10)
        {

            IList<PhanQuyen> phanQuyens = db.PhanQuyens.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                phanQuyens = db.PhanQuyens.Where(n => n.Quyen.TenQuyen.Contains(searchString)
                || n.ChucVu.TenChucVu.Contains(searchString)).ToList();
            }
            return View(phanQuyens.ToPagedList(page, pageSize));

        }

        // GET: PhanQuyen/Create
        public ActionResult Create()
        {
            ViewBag.MaChucVu = new SelectList(db.ChucVus, "MaChucVu", "TenChucVu");
            ViewBag.MaQuyen = new SelectList(db.Quyens, "MaQuyen", "TenQuyen");
            return View();
        }

        // POST: PhanQuyen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaChucVu,MaQuyen,ChuThich")] PhanQuyen phanQuyen)
        {
            if (ModelState.IsValid)
            {
                db.PhanQuyens.Add(phanQuyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaChucVu = new SelectList(db.ChucVus, "MaChucVu", "TenChucVu", phanQuyen.MaChucVu);
            ViewBag.MaQuyen = new SelectList(db.Quyens, "MaQuyen", "TenQuyen", phanQuyen.MaQuyen);
            return View(phanQuyen);
        }

        // GET: PhanQuyen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhanQuyen phanQuyen = db.PhanQuyens.Find(id);
            if (phanQuyen == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaChucVu = new SelectList(db.ChucVus, "MaChucVu", "TenChucVu", phanQuyen.MaChucVu);
            ViewBag.MaQuyen = new SelectList(db.Quyens, "MaQuyen", "TenQuyen", phanQuyen.MaQuyen);
            return View(phanQuyen);
        }

        // POST: PhanQuyen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaChucVu,MaQuyen,ChuThich")] PhanQuyen phanQuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phanQuyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaChucVu = new SelectList(db.ChucVus, "MaChucVu", "TenChucVu", phanQuyen.MaChucVu);
            ViewBag.MaQuyen = new SelectList(db.Quyens, "MaQuyen", "TenQuyen", phanQuyen.MaQuyen);
            return View(phanQuyen);
        }

        // GET: PhanQuyen/Delete/5
        public ActionResult Delete(int? id1, int?id2)
        {
            if (id1 == null || id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var phanQuyen = db.PhanQuyens.SingleOrDefault(n => n.MaChucVu == id1 && n.MaQuyen == id2);
            if (phanQuyen == null)
            {
                return HttpNotFound();
            }
            return View(phanQuyen);
        }

        // POST: PhanQuyen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id1, int? id2)
        {
            var phanQuyen = db.PhanQuyens.SingleOrDefault(n => n.MaChucVu == id1 && n.MaQuyen == id2);
            db.PhanQuyens.Remove(phanQuyen);
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
