using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using ToDoApp.Entity.Context;

namespace ToDoApp.Entity.Migrations
{
    

    internal sealed class Configuration : DbMigrationsConfiguration<ToDoAppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            TargetDatabase = new DbConnectionInfo("ToDoAppContext");
        }

        protected override void Seed(ToDoAppContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
