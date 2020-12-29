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
    public class HangHoaController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: HangHoa
        public ActionResult Index()
        {
            var hangHoas = db.HangHoas.Include(h => h.LoaiHangHoa);
            return View(hangHoas.ToList());
        }
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
                TempData["AlertType"] = "alert-success";
            else if (type == "warning")
                TempData["AlertType"] = "alert-warning";
            else if (type == "error")
                TempData["AlertType"] = "alert-danger";
        }
        public ActionResult DanhSachHangHoa(string searchString, int page = 1, int pageSize = 10)
        {
            IList<HangHoa> hh = db.HangHoas.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                hh = db.HangHoas.Where(
                hanghoa => hanghoa.TenHangHoa.Contains(searchString) ||
                hanghoa.ThuongHieu.Contains(searchString)).ToList();
            }
            //Add search later
            return View(hh.ToPagedList(page, pageSize));
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
            ViewBag.MaLoaiHangHoa = new SelectList(db.LoaiHangHoas.Where(i => i.IsDeleted != true), "MaLoaiHangHoa", "TenLoaiHangHoa");
            return View();
        }

        // POST: HangHoa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaHangHoa,TenHangHoa,GiaNhap,GiamGia,GiaBan,SoLuong,Size,DonViTinh,MoTa,ThoiGianBaoHanh,HinhAnh,ThuongHieu,MaLoaiHangHoa,IsDeleted")] HangHoa hangHoa)
        {
            if (ModelState.IsValid)
            {
                db.HangHoas.Add(hangHoa);
                db.SaveChanges();
                SetAlert("Thêm hàng hóa thành công!", "success");
                return RedirectToAction("Index");
            }

            ViewBag.MaLoaiHangHoa = new SelectList(db.LoaiHangHoas.Where(i => i.IsDeleted != true), "MaLoaiHangHoa", "TenLoaiHangHoa", hangHoa.MaLoaiHangHoa);
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
            ViewBag.MaLoaiHangHoa = new SelectList(db.LoaiHangHoas.Where(i => i.IsDeleted != true), "MaLoaiHangHoa", "TenLoaiHangHoa", hangHoa.MaLoaiHangHoa);
            return View(hangHoa);
        }

        // POST: HangHoa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaHangHoa,TenHangHoa,GiaNhap,GiamGia,GiaBan,SoLuong,Size,DonViTinh,MoTa,ThoiGianBaoHanh,HinhAnh,ThuongHieu,MaLoaiHangHoa,IsDeleted")] HangHoa hangHoa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hangHoa).State = EntityState.Modified;
                db.SaveChanges();
                SetAlert("Sửa hàng hóa thành công!", "success");
                return RedirectToAction("Index");
            }
            ViewBag.MaLoaiHangHoa = new SelectList(db.LoaiHangHoas.Where(i => i.IsDeleted != true), "MaLoaiHangHoa", "TenLoaiHangHoa", hangHoa.MaLoaiHangHoa);
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
            hangHoa.IsDeleted = true;
            db.SaveChanges();
            SetAlert("Ngừng kinh doanh hàng hóa thành công!", "success");
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
