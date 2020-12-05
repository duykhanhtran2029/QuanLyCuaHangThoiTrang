using System.Web.Mvc;

namespace QuanLyCuaHangThoiTrang.Areas.Manager
{
    public class ManagerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Manager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Manager_default",
                "Manager/{controller}/{action}/{id}",
                new { action = "Index",  id = UrlParameter.Optional },
                namespaces: new string[] { "QuanLyCuaHangThoiTrang.Areas.Manager.Controllers" }
            );
        }
    }
}