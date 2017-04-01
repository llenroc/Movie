using Infrastructure.Domain.Entities;
using Infrastructure.Domain.Entities.Auditing;
using System;

namespace Application.Movies
{
    public class MovieHint : Entity, IHasCreationTime
    {
        public long? UserId { get; set; }

        public int MovieId { get; set; }

        public virtual Movie Movie{get;set;}

        public DateTime CreationTime { get; set; }
    }
}
