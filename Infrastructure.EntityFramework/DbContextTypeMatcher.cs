using Infrastructure.Domain.UnitOfWork;

namespace Infrastructure.EntityFramework
{
    public class DbContextTypeMatcher : DbContextTypeMatcher<InfrastructureDbContext>
    {
        public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider) : base(currentUnitOfWorkProvider)
        {
        }
    }
}
