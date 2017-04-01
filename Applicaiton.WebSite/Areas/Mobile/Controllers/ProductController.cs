using Application.Products.Fronts.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Products.Fronts;

namespace Application.WebSite.Areas.Mobile.Controllers
{
    public class ProductController : AuthorizationMobileControllerBase
    {
        public ProductForFrontAppService ProductForFrontAppService { get; set; }

        // GET: Mobile/Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(ProductGetInput input)
        {
            ViewBag.PageTitle = L("ProductDetail");
            return View(input);
        }
    }
}