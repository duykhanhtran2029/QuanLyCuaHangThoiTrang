using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager.Controllers
{
    public class PhieuNhapKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuNhapKho
        public ActionResult Index()
        {
            var phieuNhapKhoes = db.PhieuNhapKhoes.Include(p => p.NguoiDung).Include(p => p.NhaCungCap);
            return View(phieuNhapKhoes.ToList());
        }
        public ActionResult DanhSachPhieuNhapKho(string searchString, int page = 1, int pageSize = 10)
        {
            IList<PhieuNhapKho> pnk = db.PhieuNhapKhoes.ToList();
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    ncc = db.NhaCungCaps.Where(
            //    nhacungcap => nhacungcap.TenNhaCungCap.Contains(searchString) ||
            //    nhacungcap.Email.Contains(searchString) ||
            //    nhacungcap.DiaChi.Contains(searchString) ||
            //    nhacungcap.SoDienThoai.Contains(searchString)).ToList();
            //}
            //Add search later
            return View(pnk.ToPagedList(page, pageSize));
        }

        public void LuuChiTietPhieuNhapKho(List<ChiTietPhieuNhapKho> orderItems)
        {
            db.ChiTietPhieuNhapKhoes.Add(orderItems[0]);
        }
        // GET: PhieuNhapKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuNhapKho phieuNhapKho = db.PhieuNhapKhoes.Find(id);
            if (phieuNhapKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuNhapKho);
        }
        public ActionResult LoadThongTinHangHoa(int id)
        {
            var result = db.HangHoas.Where(hh => hh.MaHangHoa == id).FirstOrDefault();
            return Json(new { TenHangHoa = result.TenHangHoa,
                DonViTinh = result.DonViTinh,
                Size = result.Size }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LuuPhieuNhapKho(NhapKho phieuNhapKho)
        {
            PhieuNhapKho pnk = new PhieuNhapKho
            {
                NgayNhapKho = phieuNhapKho.NgayNhapKho,
                MaNguoiDung = phieuNhapKho.MaNguoiDung,
                MaNhaCungCap = phieuNhapKho.MaNhaCungCap,
                TongTien = phieuNhapKho.TongTien,
                GhiChu = phieuNhapKho.GhiChu,
                IsDeleted = phieuNhapKho.IsDeleted,
                NgayChinhSua = DateTime.Now.Date
            };
            bool status = false;
            try
            {
                db.PhieuNhapKhoes.Add(pnk);
                db.SaveChanges();

                foreach (var i in phieuNhapKho.chiTietPhieuNhapKhoes)
                {
                    i.SoPhieuNhapKho = pnk.SoPhieuNhapKho;
                    db.ChiTietPhieuNhapKhoes.Add(i);
                    var hanghoa = db.HangHoas.Where(hh => hh.MaHangHoa == i.MaHangHoa).FirstOrDefault();
                    hanghoa.SoLuong += i.SoLuong;
                    db.SaveChanges();
                }

                status = true;
            }
            catch
            {
                status = false;
                throw;
            }
            return new JsonResult { Data = new { status = status } };
        }
        // GET: PhieuNhapKho/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaNhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap");
            //ViewBag.HangHoas = db.HangHoas.Where(hh => hh.IsDeleted == false).ToList();
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa" , "TenHangHoa");
            return View();
        }

        // POST: PhieuNhapKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuNhapKho,NgayNhapKho,MaNguoiDung,MaNhaCungCap,TongTien,GhiChu,IsDeleted,NgayChinhSua")] PhieuNhapKho phieuNhapKho)
        {
            if (ModelState.IsValid)
            {
                db.PhieuNhapKhoes.Add(phieuNhapKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuNhapKho.MaNguoiDung);
            ViewBag.MaNhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", phieuNhapKho.MaNhaCungCap);
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            return View(phieuNhapKho);
        }

        // GET: PhieuNhapKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuNhapKho phieuNhapKho = db.PhieuNhapKhoes.Find(id);
            if (phieuNhapKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuNhapKho.MaNguoiDung);
            ViewBag.MaNhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", phieuNhapKho.MaNhaCungCap);
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuNhapKho = db.ChiTietPhieuNhapKhoes.ToList();
            return View(phieuNhapKho);
        }

        // POST: PhieuNhapKho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuNhapKho,NgayNhapKho,MaNguoiDung,MaNhaCungCap,TongTien,GhiChu,IsDeleted,NgayChinhSua")] PhieuNhapKho phieuNhapKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuNhapKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuNhapKho.MaNguoiDung);
            ViewBag.MaNhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", phieuNhapKho.MaNhaCungCap);
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuNhapKho = db.ChiTietPhieuNhapKhoes.ToList();
            return View(phieuNhapKho);
        }

        // GET: PhieuNhapKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuNhapKho phieuNhapKho = db.PhieuNhapKhoes.Find(id);
            if (phieuNhapKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuNhapKho);
        }

        // POST: PhieuNhapKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuNhapKho phieuNhapKho = db.PhieuNhapKhoes.Find(id);
            db.PhieuNhapKhoes.Remove(phieuNhapKho);
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
