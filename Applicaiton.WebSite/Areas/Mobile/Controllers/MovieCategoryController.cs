using Application.MovieCategorys.Dto;
using Application.Movies;
using Application.Movies.Dto;
using Infrastructure.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.WebSite.Areas.Mobile.Controllers
{
    public class MovieCategoryController : AnonymousMobileControllerBase
    {
        public MovieAppService movieAppService { get; set; }

        // GET: Mobile/MovieCategory
        public ActionResult Index(MovieGetAllInput input)
        {
            PagedResultDto<MovieDto> moviePagedResult = movieAppService.GetAllOfPage(input);
            return View(moviePagedResult);
        }
    }
}