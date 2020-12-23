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
    public class BaoCaoBanHangController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: BaoCaoBanHang
        public ActionResult Index()
        {
            var baoCaoBanHangs = db.BaoCaoBanHangs.Include(b => b.NguoiDung);
            return View(baoCaoBanHangs.ToList());
        }

        // GET: BaoCaoBanHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            if (baoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoBanHang);
        }

        // GET: BaoCaoBanHang/Create
        public ActionResult Create()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung");
            return View();
        }

        // POST: BaoCaoBanHang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NgayBatDau,NgayKetThuc")] BaoCaoBanHang baoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                if(baoCaoBanHang.NgayBatDau > baoCaoBanHang.NgayKetThuc)
                {
                    SetAlert("Ngày kết thúc phải sau ngày bắt đầu!", "error");
                    return View(baoCaoBanHang);
                }
                var baocao1 = db.PhieuBanHangs.Where(n => n.NgayBan >= baoCaoBanHang.NgayBatDau && n.NgayBan <= baoCaoBanHang.NgayKetThuc);

                var tongpbh = baocao1.Count();

                if(tongpbh == 0)
                {
                    SetAlert("Không có thông tin để lập báo cáo!", "warning");
                    return View(baoCaoBanHang);
                }

                var tongtien = baocao1.Sum(n => n.TongTien);


                var baocao2 = db.PhieuNhapKhoes.Where(n => n.NgayNhapKho >= baoCaoBanHang.NgayBatDau && n.NgayNhapKho <= baoCaoBanHang.NgayKetThuc);
                decimal tongtiennhap = 0;
                if (baocao2.Count() == 0)
                {
                    tongtiennhap = 0;
                }
                else
                {
                    tongtiennhap = baocao2.Sum(n => n.TongTien);
                }
                
                var user = (NguoiDung)Session["Account"];

                var res = (from pbh in db.PhieuBanHangs
                           where (pbh.NgayBan >= baoCaoBanHang.NgayBatDau && pbh.NgayBan <= baoCaoBanHang.NgayKetThuc)
                           group pbh by pbh.NgayBan into g
                           select new
                           {
                               NgayBan = g.Key,
                               SoDonHang = g.Count(),
                               DoanhThu = g.Sum(n => n.TongTien)
                           }).ToList();
                decimal TongDT = 0;

                foreach (var i in res)
                {
                    TongDT += i.DoanhThu;
                }
                if (TongDT == 0)
                {
                    SetAlert("Tạo báo cáo bán hàng không thành công!", "error");
                    return View(baoCaoBanHang);
                }
                   
                var record = new BaoCaoBanHang
                {
                    NgayBatDau = baoCaoBanHang.NgayBatDau,
                    NgayKetThuc = baoCaoBanHang.NgayKetThuc,
                    SoLuongPhieuBanHang = tongpbh,
                    TongTienBanHang = tongtien,
                    TongTienNhapHang = tongtiennhap,
                    TongDoanhThu = tongtien - tongtiennhap,
                    MaNguoiDung = user.MaNguoiDung,
                    IsDeleted = false
                };
                db.BaoCaoBanHangs.Add(record);
                db.SaveChanges();            

                foreach (var i in res)
                {
                    db.ChiTietBaoCaoBanHangs.Add(new ChiTietBaoCaoBanHang
                    {
                        MaBaoCaoBanHang = record.MaBaoCaoBanHang,
                        Ngay = i.NgayBan,
                        SoLuongPhieuBanHang = i.SoDonHang,
                        DoanhThu = i.DoanhThu,
                        TiLe = Convert.ToDouble(i.DoanhThu / TongDT)
                    });
                    db.SaveChanges();
                }
                

                return RedirectToAction("Index");
                //Done
            }

            //if (ModelState.IsValid) MaBaoCaoBanHang,,SoLuongPhieuBanHang,TongTienBanHang,TongTienNhapHang,TongDoanhThu,MaNguoiDung,IsDeleted
            //{
            //    db.BaoCaoBanHangs.Add(baoCaoBanHang);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoBanHang.MaNguoiDung);
            return View(baoCaoBanHang);
        }

        // GET: BaoCaoBanHang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            if (baoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoBanHang.MaNguoiDung);
            return View(baoCaoBanHang);
        }

        // POST: BaoCaoBanHang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBaoCaoBanHang,NgayBatDau,NgayKetThuc,SoLuongPhieuBanHang,TongTienBanHang,TongTienNhapHang,TongDoanhThu,MaNguoiDung,IsDeleted")] BaoCaoBanHang baoCaoBanHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(baoCaoBanHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", baoCaoBanHang.MaNguoiDung);
            return View(baoCaoBanHang);
        }

        // GET: BaoCaoBanHang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            if (baoCaoBanHang == null)
            {
                return HttpNotFound();
            }
            return View(baoCaoBanHang);
        }

        // POST: BaoCaoBanHang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BaoCaoBanHang baoCaoBanHang = db.BaoCaoBanHangs.Find(id);
            db.BaoCaoBanHangs.Remove(baoCaoBanHang);
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
