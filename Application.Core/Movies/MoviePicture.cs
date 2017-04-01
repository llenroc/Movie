using Infrastructure.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Movies
{
    public class MoviePicture:Entity
    {
        [Required]
        public string Path { get; set; }
    }
}
