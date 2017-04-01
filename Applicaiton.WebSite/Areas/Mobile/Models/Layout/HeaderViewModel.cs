using Application.Configuration.Host.Dto;
using Application.Sessions.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.WebSite.Areas.Mobile.Models.Layout
{
    public class HeaderViewModel
    {
        public CurrentLoginInformationsOutput CurrentLoginInformations { get; set; }

        public AppSettingsOutput AppSettings { get; set; }
    }
}