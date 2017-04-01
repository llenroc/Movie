using Application.Movies;
using Application.Movies.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.WebSite.Areas.Mobile.Controllers
{
    public class MovieController : AnonymousMobileControllerBase
    {
        public IMovieAppService movieAppService { get; set; }

        // GET: Mobile/Movie
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Play(MovieGetInput input)
        {
            MovieForUserDto movie = movieAppService.GetMovieForUser(input);
            return View(movie);
        }

        public JsonResult GetPlayPath(MovieGetInput input)
        {
            string path = movieAppService.GetMoviePlayPathForUser(input).Result.ToString();
            return Json(path);
        }
    }
}