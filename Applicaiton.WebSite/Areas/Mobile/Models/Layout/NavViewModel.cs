using Application.MovieCategorys.Dto;
using System.Collections.Generic;

namespace Application.WebSite.Areas.Mobile.Models.Layout
{
    public class NavViewModel
    {
        public List<MovieCategoryDto> MovieCategorys { get; set; }
    }
}