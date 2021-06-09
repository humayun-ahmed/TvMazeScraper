using System;
using Infrastructure.Scheduler.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Scheduler
{
    public static class ScheduledServiceExtensions
    {
        public static void AddScheduler<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : SchedulerHostedService
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");

            var config = new ScheduleConfig<T>();
            options.Invoke(config);

            if (string.IsNullOrWhiteSpace(config.Expression))
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.Expression), @"Empty Expression is not allowed.");

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<T>();
        }
    }
}
