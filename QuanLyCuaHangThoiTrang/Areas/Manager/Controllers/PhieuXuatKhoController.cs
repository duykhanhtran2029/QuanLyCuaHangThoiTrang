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
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace QuanLyCuaHangThoiTrang.Areas.Manager.Controllers
{
    public class PhieuXuatKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuXuatKho
        public ActionResult Index()
        {
            var phieuXuatKhoes = db.PhieuXuatKhoes.Include(p => p.NguoiDung);
            var dt = phieuXuatKhoes.OrderByDescending(pnk => pnk.NgayXuatKho).FirstOrDefault();
            var df = phieuXuatKhoes.OrderByDescending(pnk => pnk.NgayXuatKho).ToArray().LastOrDefault();
            if (dt != null && df != null)
            {
                ViewBag.dateTo = dt.NgayXuatKho.ToString("MM/dd/yyyy");
                ViewBag.dateFrom = df.NgayXuatKho.ToString("MM/dd/yyyy");
            }
            return View(phieuXuatKhoes.ToList());
        }
        public ActionResult DanhSachPhieuXuatKho(string searchString, string dateFrom, string dateTo, int page = 1, int pageSize = 10)
        {
            IList<PhieuXuatKho> pxk = db.PhieuXuatKhoes.Where(nc => nc.IsDeleted != true).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                pxk = db.PhieuXuatKhoes.Where(
                phieuxuatkho => phieuxuatkho.NguoiDung.TenNguoiDung.Contains(searchString) ||
                phieuxuatkho.LyDoXuat.Contains(searchString)).ToList();
            }
            DateTime tungay = Convert.ToDateTime(dateFrom);
            DateTime denngay = Convert.ToDateTime(dateTo);

            if ((!(tungay == default(DateTime))) && (!(denngay == default(DateTime))))
            {
                pxk = pxk.Where(phieuxuatkho => phieuxuatkho.NgayXuatKho >= tungay && phieuxuatkho.NgayXuatKho <= denngay).ToList();
            }
            //Add search later
            return View(pxk.ToPagedList(page, pageSize));
        }
        public ActionResult LoadThongTinHangHoa(int id)
        {
            var result = db.HangHoas.Where(hh => hh.MaHangHoa == id).FirstOrDefault();
            return Json(new
            {
                TenHangHoa = result.TenHangHoa,
                DonViTinh = result.DonViTinh,
                Size = result.Size,
                SoLuongTon =  result.SoLuong,
                Gia = result.GiaBan,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadChiTietPhieuXuatKho(int id)
        {
            var result = db.ChiTietPhieuXuatKhoes.Where(ct => ct.SoPhieuXuatKho == id)
                .Select(ct => new {
                    MaHangHoa = ct.MaHangHoa,
                    TenHangHoa = ct.HangHoa.TenHangHoa,
                    Size = ct.HangHoa.Size,
                    SoLuongTon = ct.HangHoa.SoLuong,
                    SoLuong = ct.SoLuong,
                    Gia= ct.Gia,
                    ThanhTien = ct.ThanhTien
                }).ToList();
            var json = JsonConvert.SerializeObject(result);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public void DeleteAllCTPXK(int id)
        {
            var result = db.ChiTietPhieuXuatKhoes.Where(ct => ct.SoPhieuXuatKho == id).ToList();
            foreach (var item in result)
            {
                var hanghoa = db.HangHoas.Where(hh => hh.MaHangHoa == item.MaHangHoa).FirstOrDefault();
                hanghoa.SoLuong += item.SoLuong;
                db.ChiTietPhieuXuatKhoes.Remove(item);
            }
            db.SaveChanges();
        }
        public void SaveAllCTPXK(ICollection<ChiTietPhieuXuatKho> chiTietPhieuXuatKhoes, int id)
        {
            foreach (var i in chiTietPhieuXuatKhoes)
            {
                i.SoPhieuXuatKho = id;
                db.ChiTietPhieuXuatKhoes.Add(i);
                var hanghoa = db.HangHoas.Where(hh => hh.MaHangHoa == i.MaHangHoa).FirstOrDefault();
                hanghoa.SoLuong -= i.SoLuong;
                db.SaveChanges();
            }
        }

        [HttpPost]
        public ActionResult LuuPhieuXuatKho(PhieuXuatKho phieuXuatKho)
        {
            PhieuXuatKho pxk = new PhieuXuatKho
            {
                NgayXuatKho = phieuXuatKho.NgayXuatKho,
                MaNguoiDung = phieuXuatKho.MaNguoiDung,
                LyDoXuat = phieuXuatKho.LyDoXuat,
                TongTien = phieuXuatKho.TongTien,
                IsDeleted = phieuXuatKho.IsDeleted,
                NgayChinhSua = DateTime.Now.Date
            };
            bool status = false;
            try
            {
                db.PhieuXuatKhoes.Add(pxk);
                db.SaveChanges();
                SaveAllCTPXK(phieuXuatKho.ChiTietPhieuXuatKhoes, pxk.SoPhieuXuatKho);
                status = true;
            }
            catch
            {
                status = false;
                throw;
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public ActionResult SuaPhieuXuatKho(PhieuXuatKho phieuXuatKho)
        {
            bool status = false;
            try
            {
                var phieunhapkho = db.PhieuXuatKhoes.SingleOrDefault(pxk => pxk.SoPhieuXuatKho == phieuXuatKho.SoPhieuXuatKho);
                if (phieunhapkho != null)
                {
                    phieunhapkho.NgayXuatKho = phieuXuatKho.NgayXuatKho;
                    phieunhapkho.MaNguoiDung = phieuXuatKho.MaNguoiDung;
                    phieunhapkho.LyDoXuat = phieuXuatKho.LyDoXuat;
                    phieunhapkho.TongTien = phieuXuatKho.TongTien;
                    phieunhapkho.NgayChinhSua = DateTime.Now.Date;
                    DeleteAllCTPXK(phieunhapkho.SoPhieuXuatKho);
                    SaveAllCTPXK(phieuXuatKho.ChiTietPhieuXuatKhoes, phieunhapkho.SoPhieuXuatKho);
                    db.SaveChanges();
                    status = true;
                }
                else
                    status = false;

            }
            catch
            {
                status = false;
                throw;
            }
            return new JsonResult { Data = new { status = status } };
        }
        // GET: PhieuXuatKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuXuatKho phieuXuatKho = db.PhieuXuatKhoes.Find(id);
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung"); 
            if (phieuXuatKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuXuatKho);
        }

        // GET: PhieuXuatKho/Create
        public ActionResult Create()
        {
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung"); 
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(i => i.IsDeleted != true), "MaHangHoa", "TenHangHoa");
            return View();
        }

        // POST: PhieuXuatKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuXuatKho,NgayXuatKho,MaNguoiDung,LyDoXuat,TongTien,IsDeleted,NgayChinhSua")] PhieuXuatKho phieuXuatKho)
        {
            if (ModelState.IsValid)
            {
                db.PhieuXuatKhoes.Add(phieuXuatKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung"); 
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(i => i.IsDeleted != true), "MaHangHoa", "TenHangHoa");
            return View(phieuXuatKho);
        }

        // GET: PhieuXuatKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuXuatKho phieuXuatKho = db.PhieuXuatKhoes.Find(id);
            if (phieuXuatKho == null)
            {
                return HttpNotFound();
            }
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung"); 
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(i => i.IsDeleted != true), "MaHangHoa", "TenHangHoa");
            return View(phieuXuatKho);
        }

        // POST: PhieuXuatKho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuXuatKho,NgayXuatKho,MaNguoiDung,LyDoXuat,TongTien,IsDeleted,NgayChinhSua")] PhieuXuatKho phieuXuatKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuXuatKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung"); 
            ViewBag.MaHangHoa = new SelectList(db.HangHoas.Where(i => i.IsDeleted != true), "MaHangHoa", "TenHangHoa"); 
            return View(phieuXuatKho);
        }

        // GET: PhieuXuatKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuXuatKho phieuXuatKho = db.PhieuXuatKhoes.Find(id);
            if (phieuXuatKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuXuatKho);
        }

        // POST: PhieuXuatKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuXuatKho phieuXuatKho = db.PhieuXuatKhoes.Find(id);
            db.PhieuXuatKhoes.Remove(phieuXuatKho);
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
