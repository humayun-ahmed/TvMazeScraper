namespace Infrastructure.Scheduler.Config
{
    public class ScheduleConfig<T> : IScheduleConfig<T> where T: SchedulerHostedService
    {
        public string Expression { get; set; }
    }
}
