using System;
using System.Data.Entity;
using System.Threading.Tasks;
using ToDoApp.Core.Persistence;

namespace ToDoApp.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork<DbContext>
    {
        private bool _disposed;

        public UnitOfWork(IDatabaseContextFactory<DbContext> dbContextfactory)
        {
            if (dbContextfactory == null)
            {
                throw new ArgumentNullException(nameof(dbContextfactory));
            }

            var masterDbContext = dbContextfactory.MasterDbContext();

            if (masterDbContext == null)
            {
                throw new ArgumentNullException(nameof(masterDbContext), @"Master database context cannot be null");
            }

            DatabaseContext = masterDbContext;
        }

        public DbContext DatabaseContext { get; }

        public void Commit()
        {
            DatabaseContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await DatabaseContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposning)
        {
            if (_disposed)
                return;

            if (disposning)
            {
                DatabaseContext.Dispose();
            }
            _disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
