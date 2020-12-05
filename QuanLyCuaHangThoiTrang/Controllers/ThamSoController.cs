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
    public class ThamSoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: ThamSo
        public ActionResult Index()
        {
            return View(db.ThamSoes.ToList());
        }

        // GET: ThamSo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThamSo thamSo = db.ThamSoes.Find(id);
            if (thamSo == null)
            {
                return HttpNotFound();
            }
            return View(thamSo);
        }

        // GET: ThamSo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ThamSo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaThamSo,TenThamSo,GiaTri,NgayApDung")] ThamSo thamSo)
        {
            if (ModelState.IsValid)
            {
                db.ThamSoes.Add(thamSo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thamSo);
        }

        // GET: ThamSo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThamSo thamSo = db.ThamSoes.Find(id);
            if (thamSo == null)
            {
                return HttpNotFound();
            }
            return View(thamSo);
        }

        // POST: ThamSo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaThamSo,TenThamSo,GiaTri,NgayApDung")] ThamSo thamSo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thamSo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thamSo);
        }

        // GET: ThamSo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThamSo thamSo = db.ThamSoes.Find(id);
            if (thamSo == null)
            {
                return HttpNotFound();
            }
            return View(thamSo);
        }

        // POST: ThamSo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ThamSo thamSo = db.ThamSoes.Find(id);
            db.ThamSoes.Remove(thamSo);
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
