using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;
using FarmworkersWebAPI.Models;
using FarmworkersWebAPI.Controllers;

[assembly: OwinStartup(typeof(FarmworkersWebAPI.Startup))]

namespace FarmworkersWebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Hangfire.GlobalConfiguration.Configuration.UseSqlServerStorage("FarmWorkerAppDB");

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            NotificationsController _notificationsInstance = new NotificationsController();

            RecurringJob.AddOrUpdate(() => _notificationsInstance.ScheduledWeatherForeCastNotifications(), Cron.Yearly);
            RecurringJob.AddOrUpdate(() => _notificationsInstance.ScheduledEducationalContentNotifications(), Cron.Yearly);
            RecurringJob.AddOrUpdate(() => _notificationsInstance.ScheduledCurrentWeatherWarningNotifications(), Cron.Yearly); //"*/10 * * * *"


        }
    }
}
