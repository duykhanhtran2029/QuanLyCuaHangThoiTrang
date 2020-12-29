﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager.Controllers
{
    public class PhieuChiController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();

        // GET: PhieuChi
        public ActionResult Index()
        {
            var phieuChis = db.PhieuChis;//.Include(p => p.NguoiDung).Include(p => p.PhieuNhapKho);
    
            return View(phieuChis.ToList());
        }

        // GET: PhieuChi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            if (phieuChi == null)
            {
                return HttpNotFound();
            }
            return View(phieuChi);
        }

        // GET: PhieuChi/Create
        
        public ActionResult Create()
        {

          
            ViewBag.SoPhieuNhapKho = ListPhieuNhapKho();

            return View();
        }

        public SelectList ListPhieuNhapKho()
        {
            var SoPNKinPC = (from pc in db.PhieuChis
                             select new
                             {
                                 pc.SoPhieuNhapKho
                             }.SoPhieuNhapKho);
            var ConLai = (from pn in db.PhieuNhapKhoes
                          where !SoPNKinPC.Contains(pn.SoPhieuNhapKho) && pn.IsDeleted == false
                          select new SelectListItem
                          {
                              Text = pn.SoPhieuNhapKho.ToString(),
                              Value = pn.SoPhieuNhapKho.ToString()
                          });
            return new SelectList(ConLai, "Value", "Text");
        }

        public ActionResult DanhSachPhieuChi(string searchString, int page = 1, int pageSize = 10)
        {
            IList<PhieuChi> phieuchi = db.PhieuChis.Where(nc => nc.IsDeleted != true).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                phieuchi = db.PhieuChis.Where(n => n.NguoiDung.TenNguoiDung.Contains(searchString) 
                || n.SoPhieuNhapKho == Int32.Parse(searchString) || n.SoPhieuChi == Int32.Parse(searchString)).ToList();

            }
                
            return View(phieuchi.ToPagedList(page, pageSize));
        }


        public JsonResult TongTienChi(int id)
        {
            var data = db.PhieuNhapKhoes.Where(n => n.SoPhieuNhapKho == id && n.IsDeleted == false).ToList();
            if (data.Count() != 0)
            {
                Dictionary<string, string> dt = new Dictionary<string, string>();
                dt.Add("TongTien", data.First().TongTien.ToString());
                return Json(JsonConvert.SerializeObject(dt), JsonRequestBehavior.AllowGet);
            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }


        // POST: PhieuChi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhieuNhapKho")] PhieuChi phieuChi, FormCollection form)
        {

            string ngaychi = form["NgayChi"].ToString();
            var user = (NguoiDung)Session["Account"];
            string tien = form["Tongtienchi"].ToString();
            string sophieunhap = form["Sophieunhapkho"].ToString();
            string ghichu = form["Ghichu"].ToString();
            if (ModelState.IsValid)
            {
                var phieu = Int32.Parse(sophieunhap);
                var phieuChidb = db.PhieuChis.Where(n => n.SoPhieuNhapKho == phieu).ToList();
                if (phieuChidb.Count() == 0)
                {

                        db.PhieuChis.Add(new PhieuChi
                        {
                            NgayChi = DateTime.ParseExact(ngaychi, "dd/MM/yyyy", null),
                            MaNguoiDung = user.MaNguoiDung,
                            SoPhieuNhapKho = Int32.Parse(sophieunhap),
                            TongTienChi = Decimal.Parse(tien),
                            GhiChu = ghichu,
                            NgayChinhSua = DateTime.Now,
                            IsDeleted = false
                        });
                        db.SaveChanges();
                        SetAlert("Tạo phiếu chi thành công!", "success");
                        return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Tạo phiếu chi thất bại, vui lòng kiểm tra lại!", "error");
                }
            }
            ViewBag.SoPhieuNhapKho = ListPhieuNhapKho();
            return View(phieuChi);
        }

        // GET: PhieuChi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            if (phieuChi == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuChi.MaNguoiDung);
            ViewBag.SoPhieuNhapKho = new SelectList(db.PhieuNhapKhoes, "SoPhieuNhapKho", "GhiChu", phieuChi.SoPhieuNhapKho);
            return View(phieuChi);
        }

        // POST: PhieuChi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SoPhieuChi,NgayChi,MaNguoiDung,SoPhieuNhapKho,TongTienChi,GhiChu,NgayChinhSua,IsDeleted")] PhieuChi phieuChi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phieuChi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDungs, "MaNguoiDung", "TenNguoiDung", phieuChi.MaNguoiDung);
            ViewBag.SoPhieuNhapKho = new SelectList(db.PhieuNhapKhoes, "SoPhieuNhapKho", "GhiChu", phieuChi.SoPhieuNhapKho);
            return View(phieuChi);
        }

        // GET: PhieuChi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            if (phieuChi == null)
            {
                return HttpNotFound();
            }
            return View(phieuChi);
        }

        // POST: PhieuChi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhieuChi phieuChi = db.PhieuChis.Find(id);
            db.PhieuChis.Remove(phieuChi);
            db.SaveChanges();
            return RedirectToAction("Index");
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

        [ValidateAntiForgeryToken]
        public ActionResult TaoPhieuChi(FormCollection form)
        {
            string ngaychi = form["NgayChi"].ToString();
            var user = (NguoiDung)Session["Account"];
            string tien = form["Tongtienchi"].ToString();
            string sophieunhap = form["Sophieunhapkho"].ToString();
            string ghichu = form["GhiChu"].ToString();
            var phieuChi = db.PhieuChis.SingleOrDefault();
            if (phieuChi == null)
            {
                if (ModelState.IsValid)
                {
                    db.PhieuChis.Add(new PhieuChi
                    {
                        NgayChi = DateTime.ParseExact(ngaychi, "dd/MM/yyyy", null),
                        MaNguoiDung = user.MaNguoiDung,
                        SoPhieuNhapKho = Int32.Parse(sophieunhap),
                        TongTienChi = Decimal.Parse(tien),
                        GhiChu = ghichu,
                        NgayChinhSua = DateTime.Now,
                        IsDeleted = false
                    });
                    db.SaveChanges();
                    SetAlert("Tạo phiếu chi thành công!", "success");
                } 
            }
            else
            {
                SetAlert("Tạo phiếu chi thất bại, vui lòng kiểm tra lại!", "error");
                return View(phieuChi);
            }
            //return RedirectToAction("Create");
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
