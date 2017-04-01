using Infrastructure.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Movies.Dto
{
    [AutoMapTo(typeof(MovieHint))]
    public class MovieHintInput
    {
        public int MovieId { get; set; }
    }
}
