using System;
using System.Configuration;
using System.IO;
using Hangfire;
using log4net.Config;
using ToDoApp.WebApi.Tasks;

namespace ToDoApp.WebApi
{
    public class GlobalInitializer
    {
        private static readonly object Lock = new object();
        private static GlobalInitializer _initalizer;

        public static void Initialize()
        {
            if (_initalizer != null) return;
            lock (Lock)
            {
                if (_initalizer == null)
                    _initalizer = new GlobalInitializer();
            }
        }

        public GlobalInitializer()
        {
            XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + @"\_config\log4net.config"));

            GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["ToDoAppContext"].ConnectionString,
               new Hangfire.SqlServer.SqlServerStorageOptions()
               {
                   PrepareSchemaIfNecessary = false,
                   QueuePollInterval = TimeSpan.FromSeconds(15)
               }).UseLog4NetLogProvider();

#if DEBUG

            RecurringJob.RemoveIfExists("NotificationSender");
            BackgroundJob.Enqueue(() => new NotificationSender().Execute());

#else
            int notificationSenderIntervalMin;
            int.TryParse(ConfigurationManager.AppSettings["NotificationSenderIntervalMin"], out notificationSenderIntervalMin);

            RecurringJob.AddOrUpdate("NotificationSender", () => new NotificationSender().Execute(), Cron.MinuteInterval(notificationSenderIntervalMin != 0 ? notificationSenderIntervalMin : 5), queue: "NotificationSender");
#endif
        }
    }
}