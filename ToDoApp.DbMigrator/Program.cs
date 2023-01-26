using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Reflection;
using ToDoApp.Entity.Context;

namespace ToDoApp.DbMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new DbMigrationsConfiguration
            {
                AutomaticMigrationsEnabled = true,
                AutomaticMigrationDataLossAllowed = true,
                ContextType = typeof(ToDoAppContext),
                MigrationsAssembly = Assembly.GetExecutingAssembly(),
                MigrationsNamespace = "ToDoApp.Entity.Context",
                TargetDatabase = new DbConnectionInfo("ToDoAppContext")
            };

            try
            {
                var migrator = new System.Data.Entity.Migrations.DbMigrator(configuration);
                migrator.Update();
                Console.WriteLine("All databases have been migrated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured during migration of host database.");
                Console.WriteLine("Exception Message: " + ex.Message);
            }
            Console.ReadKey();
        }
    }
}
