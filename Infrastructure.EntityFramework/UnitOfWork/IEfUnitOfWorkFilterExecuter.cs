using System.Data.Entity;
using Infrastructure.Domain.UnitOfWork;

namespace Infrastructure.EntityFramework.UnitOfWork
{
    public interface IEfUnitOfWorkFilterExecuter : IUnitOfWorkFilterExecuter
    {
        void ApplyCurrentFilters(IUnitOfWork unitOfWork, DbContext dbContext);
    }
}