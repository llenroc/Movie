﻿using Application.WebSite.Filters;
using System.Web;
using System.Web.Mvc;

namespace Application.WebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
