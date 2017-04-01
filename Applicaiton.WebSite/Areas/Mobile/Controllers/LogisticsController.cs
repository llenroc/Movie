using Application.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.WebSite.Areas.Mobile.Controllers
{
    public class LogisticsController : AnonymousMobileControllerBase
    {
        // GET: Mobile/Logistics
        public ActionResult Detail(ExpressInfo expressInfo)
        {
            return View(expressInfo);
        }
    }
}