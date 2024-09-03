using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Services.Interfaces;
using System.Data.Common;
using System.Data;

namespace ShoppingAppAPI.Services.Classes
{
    public class UnitOfWorkServices : IUnitOfWork
    {
        private readonly ShoppingAppContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed;

        /// <summary>
        /// Constructor for UnitOfWorkServices class.
        /// </summary>
        /// <param name="context">Instance of the ShoppingAppContext.</param>
        public UnitOfWorkServices(ShoppingAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <returns>Returns the created transaction.</returns>
        public IDbContextTransaction BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
            return _transaction;
        }

        /// <summary>
        /// Rollbacks the current transaction asynchronously.
        /// </summary>
        public async Task Rollback()
        {
            await _transaction.RollbackAsync();
        }

        /// <summary>
        /// Disposes the current transaction.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction?.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// </summary>
        public async Task Commit()
        {
            await _transaction.CommitAsync();
        }
    }
}
