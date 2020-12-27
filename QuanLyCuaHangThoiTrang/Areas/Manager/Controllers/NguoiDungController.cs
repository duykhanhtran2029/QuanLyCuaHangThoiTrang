using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using QuanLyCuaHangThoiTrang.Extension;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager.Controllers
{
    public class NguoiDungController : Controller
    {
        private QuanLyCuaHangThoiTrangDbContext db = new QuanLyCuaHangThoiTrangDbContext();
        private static string current_avatar = "";

        // GET: NguoiDung
        public ActionResult Index()
        {
            var nguoiDungs = db.NguoiDungs.Include(n => n.ChucVu);
            return View(nguoiDungs.ToList());
        }

        // GET: NguoiDung/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // GET: NguoiDung/Create
        public ActionResult Create()
        {
            
            ViewBag.MaChucVu = new SelectList(db.ChucVus.Where(n=>n.TenChucVu != "Admin" && n.TenChucVu != "ChuCuaHang"), "MaChucVu", "TenChucVu");
            return View();
        }

        // Get
        public ActionResult DanhSachNguoiDung(string searchString, int page = 1, int pageSize = 10)
        {

            IList<NguoiDung> nguoiDung = db.NguoiDungs.ToList();
            if(!String.IsNullOrEmpty(searchString))
            {
                nguoiDung = db.NguoiDungs.Where(n => n.TenNguoiDung.Contains(searchString) 
                || n.ChucVu.TenChucVu.Contains(searchString)).ToList();
            }
            return View(nguoiDung.ToPagedList(page, pageSize));

        }

        // POST: NguoiDung/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNguoiDung,TenNguoiDung,DiaChi,SoDienThoai,Email,CMND,UserName,PassWord,MaChucVu,Avatar")] NguoiDung nguoiDung, HttpPostedFileBase avatar)
        {                                                                                                           //,IsDeleted
            if (ModelState.IsValid)
            {

                if (avatar != null && avatar.ContentLength > 0)
                {
                    try
                    {
                        Random random = new Random();
                        string avatarfile = nguoiDung.UserName + "_" +  random.Next(10000).ToString() + "_"+ Path.GetFileName(avatar.FileName);
                        string path = Path.Combine(Server.MapPath("~/images/avatar/"), avatarfile);
                        avatar.SaveAs(path);
                        nguoiDung.Avatar = avatarfile;
                    }
                    catch (Exception)
                    {
                       //
                    }
                }
                else
                {
                    nguoiDung.Avatar = "default.png";
                }
                nguoiDung.PassWord = MD5Encode.CreateMD5(nguoiDung.PassWord);
                db.NguoiDungs.Add(nguoiDung);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaChucVu = new SelectList(db.ChucVus, "MaChucVu", "TenChucVu", nguoiDung.MaChucVu);
            return View(nguoiDung);
        }

        // GET: NguoiDung/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);            
            current_avatar = nguoiDung.Avatar;
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            var user = Session["Account"] as NguoiDung;

            if(user.MaNguoiDung == id)
            {
                ViewBag.MaChucVu = new SelectList(db.ChucVus.Where(n => n.TenChucVu == "Admin"), "MaChucVu", "TenChucVu", nguoiDung.MaChucVu);
            }
           else
            {
                ViewBag.MaChucVu = new SelectList(db.ChucVus.Where(n => n.TenChucVu != "Admin"), "MaChucVu", "TenChucVu", nguoiDung.MaChucVu);
            }
            
            return View(nguoiDung);
        }

        // POST: NguoiDung/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNguoiDung,TenNguoiDung,DiaChi,SoDienThoai,Email,CMND,UserName,PassWord,IsDeleted,MaChucVu,Avatar")] NguoiDung nguoiDung, HttpPostedFileBase avatar)
        {
            if (ModelState.IsValid)
            {
                if (avatar != null && avatar.ContentLength > 0)
                {
                    try
                    {
                        Random random = new Random();
                        string avatarfile = nguoiDung.UserName + "_" + random.Next(10000).ToString() + "_" + Path.GetFileName(avatar.FileName);
                        string path = Path.Combine(Server.MapPath("~/images/avatar/"), avatarfile);
                        avatar.SaveAs(path);
                        nguoiDung.Avatar = avatarfile;
                    }
                    catch (Exception)
                    {
                       
                    }
                }
                else
                {
                    nguoiDung.Avatar = current_avatar;
                }
                nguoiDung.PassWord = MD5Encode.CreateMD5(nguoiDung.PassWord);

                db.Entry(nguoiDung).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaChucVu = new SelectList(db.ChucVus.Where(n => n.TenChucVu != "Admin" && n.TenChucVu != "ChuCuaHang"), "MaChucVu", "TenChucVu", nguoiDung.MaChucVu);
            return View(nguoiDung);
        }

        // GET: NguoiDung/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // POST: NguoiDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if(nguoiDung.ChucVu.TenChucVu == "Admin")
            {
                return RedirectToAction("Index");
            }
            db.NguoiDungs.Remove(nguoiDung);
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
