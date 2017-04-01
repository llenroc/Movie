using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Application.DTO
{

    /// <summary>
    /// Simply implements <see cref="ILimitedResultRequest"/>.
    /// </summary>
    public class LimitedResultRequestDto : ILimitedResultRequest
    {
        [Range(1, int.MaxValue)]
        public virtual int PageSize { get; set; } = 10;
    }
}