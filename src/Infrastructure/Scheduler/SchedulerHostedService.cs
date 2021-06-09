using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NCrontab;

namespace Infrastructure.Scheduler
{
    public abstract class SchedulerHostedService : IHostedService, IDisposable
    {
        private System.Timers.Timer m_timer;
        private readonly CrontabSchedule m_expression;
        protected SchedulerHostedService(string cronExpression)
        {
            m_expression = CrontabSchedule.Parse(cronExpression);
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = m_expression.GetNextOccurrence(DateTime.UtcNow);
            var delay = next - DateTimeOffset.Now;

            if (delay.TotalMilliseconds <= 0)
                await ScheduleJob(cancellationToken);

            m_timer = new System.Timers.Timer(delay.TotalMilliseconds);
            m_timer.Elapsed += async (sender, args) =>
            {
                m_timer.Dispose();
                m_timer = null;

                if (!cancellationToken.IsCancellationRequested)
                {
                    await ExecuteAsync(cancellationToken);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    await ScheduleJob(cancellationToken);
                }
            };

            m_timer.Start();
            await Task.CompletedTask;
        }

        public abstract Task ExecuteAsync(CancellationToken cancellationToken);

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            m_timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            m_timer?.Dispose();
        }
    }
}
