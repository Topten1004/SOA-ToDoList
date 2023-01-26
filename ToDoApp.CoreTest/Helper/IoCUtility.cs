using System.Configuration;
using System.Data.Entity;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ToDoApp.Core.Persistence;
using ToDoApp.Core.Service.Log;
using ToDoApp.Infrastructure.Persistence;
using ToDoApp.MongoLogger.Service;

namespace ToDoApp.Test.Helper
{
    public static class IoCUtility
    {
        private static IWindsorContainer _container;
        private static IWindsorContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = BootstrapContainer();
                }
                return _container;
            }
        }

        private static IWindsorContainer BootstrapContainer()
        {
            return new WindsorContainer().Register(
                Component.For<IDatabaseContextFactory<DbContext>>()
                    .ImplementedBy<DatabaseContextFactory>()
                    .LifestyleSingleton()
                    .UsingFactoryMethod(x => DatabaseContextFactory.Instance),
                Component.For<IUnitOfWork<DbContext>>().ImplementedBy<UnitOfWork>().LifestylePerThread(),
                Component.For<IUserRepository>()
                    .ImplementedBy<UserRepository>()
                    .LifestylePerThread()
                    .DynamicParameters((kernel, parameters) => { parameters["unitOfWork"] = Resolve<IUnitOfWork<DbContext>>(); }),
                Component.For<IAuditLogService>().ImplementedBy<AuditLogService>().LifestyleSingleton().DynamicParameters((kernel, parameters) => { parameters["connectionString"] = ConfigurationManager.AppSettings["MongoDbConnectionString"]; })
        );

    }
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
