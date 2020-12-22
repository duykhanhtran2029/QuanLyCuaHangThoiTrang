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
    public class BaoCaoTonKhoController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: BaoCaoTonKho
        public ActionResult Index()
        {
            var baoCaoTonKhoes = db.BaoCaoTonKhoes.Include(b => b.NguoiDung);
            var dt = baoCaoTonKhoes.OrderByDescending(pnk => pnk.Nam).ThenBy(pnk => pnk.Thang).FirstOrDefault();
            var df = baoCaoTonKhoes.OrderByDescending(pnk => pnk.Nam).ThenBy(pnk => pnk.Thang).ToArray().LastOrDefault();
            if (dt != null && df != null)
            {
                ViewBag.dateTo = dt.Nam + "-" + dt.Thang;
                ViewBag.dateFrom = df.Nam + "-" + df.Thang;
            }
            return View(baoCaoTonKhoes.ToList());
        }
        public ActionResult DanhSachBaoCaoTonKho(string searchString, string dateFrom, string dateTo, int page = 1, int pageSize = 10)
        {
            IList<BaoCaoTonKho> bctc = db.BaoCaoTonKhoes.ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                bctc = db.BaoCaoTonKhoes.Where(
                baocaotonkho => baocaotonkho.NguoiDung.TenNguoiDung.Contains(searchString)).ToList();
            }

            if (!String.IsNullOrEmpty(dateFrom) && !String.IsNullOrEmpty(dateFrom))
            {
                string[] dateFroms = dateFrom.Split('-');
                string[] dateTos = dateTo.Split('-');

                int tuthang = int.Parse(dateFroms[1]);
                int tunam = int.Parse(dateFroms[0]);

                int denthang = int.Parse(dateTos[1]);
                int dennam = int.Parse(dateTos[0]);

                bctc = bctc.Where(baocaotonkho => (baocaotonkho.Nam > tunam && baocaotonkho.Nam < dennam)
                || (baocaotonkho.Nam == tunam && baocaotonkho.Thang >= tuthang) 
                || (baocaotonkho.Nam == dennam && baocaotonkho.Thang <= denthang)).ToList();
            }
            //Add search later
            return View(bctc.ToPagedList(page, pageSize));
        }
        public ActionResult LoadThongTinHangHoa(int thang, int nam)
        {
            var result = new List<Object>();
            var hangHoas = db.HangHoas.ToList();
            foreach (var item in hangHoas)
            {
                var TongNhap = db.ChiTietPhieuNhapKhoes
                                .Where(ct => ct.MaHangHoa == item.MaHangHoa &&
                                ct.PhieuNhapKho.NgayNhapKho.Year <= nam && ct.PhieuNhapKho.NgayNhapKho.Month < thang
                                && ct.SoLuong > 0)
                                .Sum(ct => (int?)ct.SoLuong) ?? 0;
                var TongXuat = db.ChiTietPhieuXuatKhoes
                                .Where(ct => ct.MaHangHoa == item.MaHangHoa &&
                                ct.PhieuXuatKho.NgayXuatKho.Year <= nam && ct.PhieuXuatKho.NgayXuatKho.Month < thang
                                && ct.SoLuong > 0)
                                .Sum(ct => (int?)ct.SoLuong) ?? 0;
                var SoLuongTonDau = TongNhap - TongXuat;
                var SoLuongNhap = db.ChiTietPhieuNhapKhoes
                                .Where(ct => ct.MaHangHoa == item.MaHangHoa &&
                                ct.PhieuNhapKho.NgayNhapKho.Year == nam && ct.PhieuNhapKho.NgayNhapKho.Month == thang
                                && ct.SoLuong > 0)
                                .Sum(ct => (int?)ct.SoLuong) ?? 0;
                var SoLuongXuat = db.ChiTietPhieuXuatKhoes
                                .Where(ct => ct.MaHangHoa == item.MaHangHoa &&
                                ct.PhieuXuatKho.NgayXuatKho.Year == nam && ct.PhieuXuatKho.NgayXuatKho.Month == thang
                                && ct.SoLuong > 0)
                                .Sum(ct => (int?)ct.SoLuong) ?? 0;
                var SoLuongTonCuoi = SoLuongTonDau + SoLuongNhap - SoLuongXuat;

                string TinhTrangHangHoa;

                if (item.IsDeleted == false)
                    TinhTrangHangHoa = "Đang kinh doanh";
                else
                    TinhTrangHangHoa = "Ngừng kinh doanh";
                var it = new
                {
                    MaHangHoa = item.MaHangHoa,
                    TenHangHoa = item.TenHangHoa,
                    DonViTinh = item.DonViTinh,
                    Size = item.Size,
                    SoLuongTonDau = SoLuongTonDau,
                    SoLuongNhap = SoLuongNhap,
                    SoLuongXuat = SoLuongXuat,
                    SoLuongTonCuoi = SoLuongTonCuoi,
                    TinhTrangHangHoa = TinhTrangHangHoa

                };
                result.Add(it);
            }
            var json = JsonConvert.SerializeObject(result);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadChiTietBaoCaoTonKho(int id)
        {
            var result = db.ChiTietBaoCaoTonKhoes.Where(ct => ct.MaBaoCaoTonKho == id)
                .Select(ct => new
                {
                    MaHangHoa = ct.MaHangHoa,
                    TenHangHoa = ct.HangHoa.TenHangHoa,
                    DonViTinh = ct.HangHoa.DonViTinh,
                    Size = ct.HangHoa.Size,
                    SoLuongTonDau = ct.SoLuongTonDau,
                    SoLuongNhap = ct.SoLuongNhap,
                    SoLuongXuat = ct.SoLuongXuat,
                    SoLuongTonCuoi = ct.SoLuongTonCuoi,
                    TinhTrangHangHoa = ct.TinhTrangHangHoa
                }).ToList();
            var json = JsonConvert.SerializeObject(result);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public void SaveAllCTBCTK(ICollection<ChiTietBaoCaoTonKho> chiTietBaoCaoTonKhoes, int id)
        {

            foreach (var i in chiTietBaoCaoTonKhoes)
            {
                i.MaBaoCaoTonKho = id;
                db.ChiTietBaoCaoTonKhoes.Add(i);
                db.SaveChanges();
            }

        }
        public void DeleteAllCTBCTK(int id)
        {
            var result = db.ChiTietBaoCaoTonKhoes.Where(ct => ct.MaBaoCaoTonKho == id).ToList();
            foreach (var item in result)
            {
                db.ChiTietBaoCaoTonKhoes.Remove(item);
                db.SaveChanges();
            }

        }

        [HttpPost]
        public ActionResult LuuBaoCaoTonKho(TonKho baoCaoTonKho)
        {
            BaoCaoTonKho bctk = new BaoCaoTonKho
            {
                Thang = baoCaoTonKho.Thang,
                Nam = baoCaoTonKho.Nam,
                MaNguoiDung = baoCaoTonKho.MaNguoiDung,
                NgayLap = baoCaoTonKho.NgayLap,
                TongSoHangHoa = baoCaoTonKho.TongSoHangHoa,
                IsDeleted = baoCaoTonKho.IsDeleted,
            };
            bool status = false;
            try
            {
                db.BaoCaoTonKhoes.Add(bctk);
                db.SaveChanges();
                SaveAllCTBCTK(baoCaoTonKho.chiTietBaoCaoTonKhoes, bctk.MaBaoCaoTonKho);
                status = true;
            }
            catch
            {
                status = false;
                throw;
            }
            return new JsonResult { Data = new { status = status } };
        }
        // GET: BaoCaoTonKho/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoTonKho baoCaoTonKho = db.BaoCaoTonKhoes.Find(id);
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            string thang = baoCaoTonKho.Thang.ToString();
            if (baoCaoTonKho.Thang < 10)
                thang = "0" + thang;
            ViewBag.ThangNam = thang + "/" + baoCaoTonKho.Nam;
            if (baoCaoTonKho == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoTonKho);
        }

        // GET: BaoCaoTonKho/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            return View();
        }

        // POST: BaoCaoTonKho/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBaoCaoTonKho,Thang,Nam,MaNguoiDung,NgayLap,TongSoHangHoa,IsDeleted")] BaoCaoTonKho baoCaoTonKho)
        {
            if (ModelState.IsValid)
            {
                db.BaoCaoTonKhoes.Add(baoCaoTonKho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoTonKho.MaNguoiDung);
            return View(baoCaoTonKho);
        }

        // GET: BaoCaoTonKho/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    BaoCaoTonKho baoCaoTonKho = db.BaoCaoTonKhoes.Find(id);
        //    if (baoCaoTonKho == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoTonKho.MaNguoiDung);
        //    return View(baoCaoTonKho);
        //}

        //// POST: BaoCaoTonKho/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "MaBaoCaoTonKho,Thang,Nam,MaNguoiDung,NgayLap,TongSoHangHoa,IsDeleted")] BaoCaoTonKho baoCaoTonKho)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(baoCaoTonKho).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoTonKho.MaNguoiDung);
        //    return View(baoCaoTonKho);
        //}

        // GET: BaoCaoTonKho/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoTonKho baoCaoTonKho = db.BaoCaoTonKhoes.Find(id);
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            string thang = baoCaoTonKho.Thang.ToString();
            if (baoCaoTonKho.Thang < 10)
                thang = "0" + thang;
            ViewBag.ThangNam = thang + "/" + baoCaoTonKho.Nam;
            if (baoCaoTonKho == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoTonKho);
        }

        // POST: BaoCaoTonKho/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            ViewBag.MaHangHoa = new SelectList(db.HangHoas, "MaHangHoa", "TenHangHoa");
            DeleteAllCTBCTK(id);
            BaoCaoTonKho baoCaoTonKho = db.BaoCaoTonKhoes.Find(id);
            db.BaoCaoTonKhoes.Remove(baoCaoTonKho);
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
