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
    public class PhieuKiemKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuKiemKho
        public ActionResult Index()
        {
            var phieuKiemKhoes = db.PhieuKiemKhoes.Include(p => p.NguoiDung);
            var dt = phieuKiemKhoes.OrderByDescending(pnk => pnk.NgayKiemKho).FirstOrDefault();
            var df = phieuKiemKhoes.OrderByDescending(pnk => pnk.NgayKiemKho).ToArray().LastOrDefault();
            if (dt != null && df != null)
            {
                ViewBag.dateTo = dt.NgayKiemKho.ToString("MM/dd/yyyy");
                ViewBag.dateFrom = df.NgayKiemKho.ToString("MM/dd/yyyy");
            }
            return View(phieuKiemKhoes.ToList());
        }
        public ActionResult DanhSachPhieuKiemKho(string searchString, string dateFrom, string dateTo, int page = 1, int pageSize = 10)
        {
            IList<PhieuKiemKho> pkk = db.PhieuKiemKhoes.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                pkk = db.PhieuKiemKhoes.Where(
                phieukiemkho => phieukiemkho.NguoiDung.TenNguoiDung.Contains(searchString)).ToList();
            }
            DateTime tungay = Convert.ToDateTime(dateFrom);
            DateTime denngay = Convert.ToDateTime(dateTo);

            if ((!(tungay == default(DateTime))) && (!(denngay == default(DateTime))))
            {
                pkk = pkk.Where(phieukiemkho => phieukiemkho.NgayKiemKho >= tungay && phieukiemkho.NgayKiemKho <= denngay).ToList();
            }
            //Add search later
            return View(pkk.ToPagedList(page, pageSize));
        }
        public ActionResult LoadThongTinHangHoa(int id)
        {
            var result = db.HangHoas.Where(hh => hh.MaHangHoa == id).FirstOrDefault();
            return Json(new
            {
                TenHangHoa = result.TenHangHoa,
                DonViTinh = result.DonViTinh,
                Size = result.Size,
                SoLuongHienTai = result.SoLuong
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadChiTietPhieuKiemKho(int id)
        {
            var result = db.ChiTietPhieuKiemKhoes.Where(ct => ct.SoPhieuKiemKho == id)
                .Select(ct => new {
                    MaHangHoa = ct.MaHangHoa,
                    TenHangHoa = ct.HangHoa.TenHangHoa,
                    DonViTinh = ct.HangHoa.DonViTinh,
                    Size = ct.HangHoa.Size,
                    SoLuongHienTai = ct.SoLuongHienTai,
                    SoLuongKiemTra = ct.SoLuongKiemTra,
                    TinhTrangHangHoa = ct.TinhTrangHangHoa
                }).ToList();
            var json = JsonConvert.SerializeObject(result);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public void DeleteAllCTPKK(int id)
        {
            var result = db.ChiTietPhieuKiemKhoes.Where(ct => ct.SoPhieuKiemKho == id).ToList();
            foreach (var item in result)
            {
                db.ChiTietPhieuKiemKhoes.Remove(item);
                db.SaveChanges();
            }    
        }
        public void SaveAllCTPKK(ICollection<ChiTietPhieuKiemKho> chiTietPhieuKiemKhoes, int id)
        {
            foreach (var i in chiTietPhieuKiemKhoes)
            {
                i.SoPhieuKiemKho = id;
                db.ChiTietPhieuKiemKhoes.Add(i);
                db.SaveChanges();
            }
        }

        [HttpPost]
        public ActionResult LuuPhieuKiemKho(KiemKho phieuKiemKho)
        {
            PhieuKiemKho pkk = new PhieuKiemKho
            {
                NgayKiemKho = phieuKiemKho.NgayKiemKho,
                MaNguoiDung = phieuKiemKho.MaNguoiDung,
                GhiChu = phieuKiemKho.GhiChu,
                IsDeleted = phieuKiemKho.IsDeleted,
                NgayChinhSua = DateTime.Now.Date
            };
            bool status = false;
            try
            {
                db.PhieuKiemKhoes.Add(pkk);
                db.SaveChanges();
                SaveAllCTPKK(phieuKiemKho.chiTietPhieuKiemKhoes, pkk.SoPhieuKiemKho);
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
        public ActionResult SuaPhieuKiemKho(KiemKho phieuKiemKho)
        {
            bool status = false;
            try
            {
                var phieukiemkho = db.PhieuKiemKhoes.SingleOrDefault(pkk => pkk.SoPhieuKiemKho == phieuKiemKho.SoPhieuKiemKho);
                if (phieukiemkho != null)
                {
                    phieukiemkho.NgayKiemKho = phieuKiemKho.NgayKiemKho;
                    phieukiemkho.MaNguoiDung = phieuKiemKho.MaNguoiDung;
                    phieukiemkho.GhiChu = phieuKiemKho.GhiChu;
                    phieukiemkho.NgayChinhSua = DateTime.Now.Date;
                    DeleteAllCTPKK(phieukiemkho.SoPhieuKiemKho);
                    SaveAllCTPKK(phieuKiemKho.chiTietPhieuKiemKhoes, phieukiemkho.SoPhieuKiemKho);
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
        // GET: PhieuKiemKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            if (phieuKiemKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuKiemKho);
        }

        // GET: PhieuKiemKho/Create
        public ActionResult Create()
        {
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            return View();
        }

        // POST: PhieuKiemKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuKiemKho,NgayKiemKho,MaNguoiDung,GhiChu,IsDeleted,NgayChinhSua")] PhieuKiemKho phieuKiemKho)
        {
            if (ModelState.IsValid)
            {
                db.PhieuKiemKhoes.Add(phieuKiemKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung");
            return View(phieuKiemKho);
        }

        // GET: PhieuKiemKho/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            if (phieuKiemKho == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung");
            return View(phieuKiemKho);
        }

        // POST: PhieuKiemKho/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuKiemKho,NgayKiemKho,MaNguoiDung,GhiChu,IsDeleted,NgayChinhSua")] PhieuKiemKho phieuKiemKho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuKiemKho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung");
            return View(phieuKiemKho);
        }

        // GET: PhieuKiemKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            if (phieuKiemKho == null)
            {
                return HttpNotFound();
            }
            return View(phieuKiemKho);
        }

        // POST: PhieuKiemKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = (NguoiDung)Session["Account"];
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs.Where(nd => nd.MaNguoiDung == user.MaNguoiDung), "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            PhieuKiemKho phieuKiemKho = db.PhieuKiemKhoes.Find(id);
            DeleteAllCTPKK(id);
            db.PhieuKiemKhoes.Remove(phieuKiemKho);
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
