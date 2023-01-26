using System.Configuration;
using System.Data.Entity;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ToDoApp.Core.Persistence;
using ToDoApp.Core.Service;
using ToDoApp.Core.Service.Log;
using ToDoApp.Infrastructure.Persistence;
using ToDoApp.Infrastructure.Service;
using ToDoApp.MongoLogger.Service;

namespace ToDoApp.WebApi.Utility
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
                Component.For<IUnitOfWork<DbContext>>()
                    .ImplementedBy<UnitOfWork>()
                    .LifestylePerWebRequest(),
                Component.For<IRepositoryFactory>()
                    .ImplementedBy<RepositoryFactory>()
                    .LifestylePerWebRequest()
                    .DynamicParameters((kernel, parameters) => { parameters["unitOfWork"] = Resolve<IUnitOfWork<DbContext>>(); }),

                #region Isolated Factories
                Component.For<IUnitOfWork<DbContext>>()
                    .ImplementedBy<UnitOfWork>()
                    .Named("isolatedUnitOfWork"),
                Component.For<IRepositoryFactory>()
                    .ImplementedBy<RepositoryFactory>()
                    .Named("isolatedRepositoryFactory")
                    .DynamicParameters((kernel, parameters) => { parameters["unitOfWork"] = Resolve<IUnitOfWork<DbContext>>("isolatedUnitOfWork"); }),
                #endregion

                Component.For<IAuditLogService>()
                    .ImplementedBy<AuditLogService>()
                    .LifestyleSingleton()
                    .DynamicParameters((kernel, parameters) => { parameters["connectionString"] = ConfigurationManager.AppSettings["MongoDbConnectionString"]; }),
                Component.For<INotificationService>()
                    .ImplementedBy<MailNotificationService>()
                    .DynamicParameters((kernel, parameters) =>
                    {
                        parameters["smtpHost"] = ConfigurationManager.AppSettings["NotificationSmtpHost"];
                        parameters["smtpPort"] = ConfigurationManager.AppSettings["NotificationSmtpPort"];
                        parameters["smtpUsername"] = ConfigurationManager.AppSettings["NotificationSmtpUsername"];
                        parameters["smtpPassword"] = ConfigurationManager.AppSettings["NotificationSmtpPassword"];
                    })
        );

        }
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return Container.Resolve<T>(name);
        }
    }
}
