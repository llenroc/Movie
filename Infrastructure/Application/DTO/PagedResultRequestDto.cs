using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Application.DTO
{
    /// <summary>
    /// Simply implements <see cref="IPagedResultRequest"/>.
    /// </summary>
    [Serializable]
    public class PagedResultRequestDto : LimitedResultRequestDto,IPagedResultRequest
    {
        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; } = 1;

        public int SkipCount
        {
            get
            {
                return (PageIndex - 1) * PageSize;
            }
        }
    }
}
