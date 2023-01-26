using System.Data.Entity;
using ToDoApp.Core.Persistence;
using ToDoApp.Entity.Context;

namespace ToDoApp.Infrastructure.Persistence
{
    public class DatabaseContextFactory : IDatabaseContextFactory<DbContext>
    {
        private static readonly DatabaseContextFactory instance = new DatabaseContextFactory();

        static DatabaseContextFactory()
        {
        }

        private DatabaseContextFactory()
        {
        }

        public static DatabaseContextFactory Instance => instance;

        public DbContext MasterDbContext()
        {
            return new ToDoAppContext();
        }
    }
}
