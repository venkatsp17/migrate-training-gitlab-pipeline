using Microsoft.EntityFrameworkCore.Storage;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContextTransaction BeginTransaction();
        Task Commit();
        Task Rollback();
    }
}
