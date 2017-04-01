using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.WebSite.Controllers;
using Infrastructure.Authorization;

namespace Application.WebSite.Controllers
{
    [AllowAnonymous]
    public abstract class AnonymousHomeControllerBase : HomeControllerBase
    {
    }
}