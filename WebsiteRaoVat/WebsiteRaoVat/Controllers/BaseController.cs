using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteRaoVat.Models;

namespace WebsiteRaoVat.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            TaiKhoan session = (TaiKhoan)Session["TaiKhoan"];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "Index" }));
            }
            else
            {
                if (session.Quyen != 1)
                {
                    
                    filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "NotAuthorize" }));
                    }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}