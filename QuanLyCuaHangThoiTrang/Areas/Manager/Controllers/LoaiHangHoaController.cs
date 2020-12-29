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
    public class LoaiHangHoaController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: LoaiHangHoa
        public ActionResult Index()
        {
            return View(db.LoaiHangHoas.ToList());
        }
        public ActionResult DanhSachLoaiHangHoa(string searchString, int page = 1, int pageSize = 10)
        {
            IList<LoaiHangHoa> lhh  = db.LoaiHangHoas.Where(nc => nc.IsDeleted != true).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                lhh = db.LoaiHangHoas.Where(
                loaihanghoa => loaihanghoa.TenLoaiHangHoa.Contains(searchString) ||
                loaihanghoa.GioiTinh.Contains(searchString)).ToList();
            }
            //Add search later
            return View(lhh.ToPagedList(page, pageSize));
        }
        // GET: LoaiHangHoa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiHangHoa loaiHangHoa = db.LoaiHangHoas.Find(id);
            if (loaiHangHoa == null)
            {
                return HttpNotFound();
            }
            return View(loaiHangHoa);
        }

        // GET: LoaiHangHoa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoaiHangHoa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoaiHangHoa,TenLoaiHangHoa,GioiTinh,IsDeleted")] LoaiHangHoa loaiHangHoa)
        {

            if (ModelState.IsValid)
            {
                db.LoaiHangHoas.Add(loaiHangHoa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(loaiHangHoa);
        }

        // GET: LoaiHangHoa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiHangHoa loaiHangHoa = db.LoaiHangHoas.Find(id);
            if (loaiHangHoa == null)
            {
                return HttpNotFound();
            }
            return View(loaiHangHoa);
        }

        // POST: LoaiHangHoa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoaiHangHoa,TenLoaiHangHoa,GioiTinh,IsDeleted")] LoaiHangHoa loaiHangHoa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiHangHoa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(loaiHangHoa);
        }

        // GET: LoaiHangHoa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiHangHoa loaiHangHoa = db.LoaiHangHoas.Find(id);
            if (loaiHangHoa == null)
            {
                return HttpNotFound();
            }
            return View(loaiHangHoa);
        }

        // POST: LoaiHangHoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoaiHangHoa loaiHangHoa = db.LoaiHangHoas.Find(id);
            loaiHangHoa.IsDeleted = true;
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
