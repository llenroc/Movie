using Infrastructure.Domain.Entities.Auditing;

namespace Application.MovieCategorys
{
    public class MovieCategory:AuditedEntity
    {
        public string Title { get; set; }
    }
}
