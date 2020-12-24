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
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace QuanLyCuaHangThoiTrang.Areas.Manager.Controllers
{
    public class PhieuNhapKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuNhapKho
        public ActionResult Index()
        {
            var phieuNhapKhoes = db.PhieuNhapKhoes.Include(p => p.NguoiDung).Include(p => p.NhaCungCap);
            var dt = phieuNhapKhoes.OrderByDescending(pnk => pnk.NgayNhapKho).FirstOrDefault();
            var df = phieuNhapKhoes.OrderByDescending(pnk => pnk.NgayNhapKho).ToArray().LastOrDefault();
            if(dt != null && df != null)
            {
                ViewBag.dateTo = dt.NgayNhapKho.ToString("MM/dd/yyyy");
                ViewBag.dateFrom = df.NgayNhapKho.ToString("MM/dd/yyyy");
            }
            return View(phieuNhapKhoes.ToList());
        }
        public ActionResult DanhSachPhieuNhapKho(string searchString,string dateFrom, string dateTo, int page = 1, int pageSize = 10)
        {
            IList<PhieuNhapKho> pnk = db.PhieuNhapKhoes.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                pnk = db.PhieuNhapKhoes.Where(
                phieunhapkho => phieunhapkho.NguoiDung.TenNguoiDung.Contains(searchString) ||
                phieunhapkho.NhaCungCap.TenNhaCungCap.Contains(searchString)).ToList();
            }
            DateTime tungay = Convert.ToDateTime(dateFrom); 
            DateTime denngay = Convert.ToDateTime(dateTo);

            if ((!(tungay == default(DateTime))) && (!(denngay == default(DateTime))))
            {
                pnk = pnk.Where(phieunhapkho => phieunhapkho.NgayNhapKho >= tungay && phieunhapkho.NgayNhapKho <= denngay).ToList();
            }
            //Add search later
            return View(pnk.ToPagedList(page, pageSize));
        }
        public ActionResult LoadThongTinHangHoa(int id)
        {
            var result = db.HangHoas.Where(hh => hh.MaHangHoa == id).FirstOrDefault();
            return Json(new
            {
                TenHangHoa = result.TenHangHoa,
                DonViTinh = result.DonViTinh,
                Size = result.Size
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadChiTietPhieuNhapKho(int id)
        {
            var result = db.ChiTietPhieuNhapKhoes.Where(ct => ct.SoPhieuNhapKho == id)
                .Select(ct => new {
                    MaHangHoa = ct.MaHangHoa,
                    TenHangHoa = ct.HangHoa.TenHangHoa,
                    DonViTinh = ct.HangHoa.DonViTinh,
                    Size = ct.HangHoa.Size,
                    SoLuong = ct.SoLuong,
                    GiaNhap = ct.GiaNhap,
                    ThanhTien = ct.ThanhTien
                }).ToList();
            var json = JsonConvert.SerializeObject(result);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public void DeleteAllCTPNK(int id)
        {
            var result = db.ChiTietPhieuNhapKhoes.Where(ct => ct.SoPhieuNhapKho == id).ToList();
            foreach (var item in result)
            {
                var hanghoa = db.HangHoas.Where(hh => hh.MaHangHoa == item.MaHangHoa).FirstOrDefault();
                hanghoa.SoLuong -= item.SoLuong;
                db.ChiTietPhieuNhapKhoes.Remove(item);
            }
            db.SaveChanges();
        }
        public void SaveAllCTPNK(ICollection<ChiTietPhieuNhapKho> chiTietPhieuNhapKhoes, int id)
        {
            foreach (var i in chiTietPhieuNhapKhoes)
            {
                i.SoPhieuNhapKho = id;
                db.ChiTietPhieuNhapKhoes.Add(i);
                db.SaveChanges();//cho nay k loi
                var hanghoa = db.HangHoas.Where(hh => hh.MaHangHoa == i.MaHangHoa).FirstOrDefault();
                hanghoa.SoLuong += i.SoLuong;
                db.SaveChanges();
            }
        }

        [HttpPost]
        public ActionResult LuuPhieuNhapKho(PhieuNhapKho phieuNhapKho)
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
                SaveAllCTPNK(phieuNhapKho.ChiTietPhieuNhapKhoes, pnk.SoPhieuNhapKho);
                status = true;
            }
            catch (DbEntityValidationException dbEx)
            {
                status = false;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public ActionResult SuaPhieuNhapKho(PhieuNhapKho phieuNhapKho)
        {
            bool status = false;
            try
            {
                var phieunhapkho = db.PhieuNhapKhoes.SingleOrDefault(pnk => pnk.SoPhieuNhapKho == phieuNhapKho.SoPhieuNhapKho);
                if (phieunhapkho != null)
                {
                    phieunhapkho.NgayNhapKho = phieuNhapKho.NgayNhapKho;
                    phieunhapkho.MaNguoiDung = phieuNhapKho.MaNguoiDung;
                    phieunhapkho.MaNhaCungCap = phieuNhapKho.MaNhaCungCap;
                    phieunhapkho.TongTien = phieuNhapKho.TongTien;
                    phieunhapkho.GhiChu = phieuNhapKho.GhiChu;
                    phieunhapkho.NgayChinhSua = DateTime.Now.Date;
                    DeleteAllCTPNK(phieunhapkho.SoPhieuNhapKho);
                    SaveAllCTPNK(phieuNhapKho.ChiTietPhieuNhapKhoes, phieunhapkho.SoPhieuNhapKho);
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
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.ChiTietPhieuNhapKho = db.ChiTietPhieuNhapKhoes.ToList();
            return View(phieuNhapKho);
        }
        // GET: PhieuNhapKho/Create
        public ActionResult Create()
        {
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaNhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap");
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

            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung"); ViewBag.MaNhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", phieuNhapKho.MaNhaCungCap);
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
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung"); ViewBag.MaNhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", phieuNhapKho.MaNhaCungCap);
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
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung"); ViewBag.MaNhaCungCap = new SelectList(db.NhaCungCaps, "MaNhaCungCap", "TenNhaCungCap", phieuNhapKho.MaNhaCungCap);
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
            DeleteAllCTPNK(id);
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
