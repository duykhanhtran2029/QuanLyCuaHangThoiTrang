using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyCuaHangThoiTrang.Model;

namespace QuanLyCuaHangThoiTrang.Areas.Manager.Controllers
{
    public class HomeController : Controller
    {
        // GET: Manager/Home
        public ActionResult Index()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "/Home");
            }
            return View();
        }
    }
}