namespace Infrastructure.Application.DTO
{
    /// <summary>
    /// This interface is defined to standardize to request a paged result.
    /// </summary>
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        int PageIndex { get; set; }
    }
}
