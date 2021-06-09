namespace Infrastructure.Scheduler.Config
{
    public interface IScheduleConfig<T> where T: SchedulerHostedService
    {
        string Expression { get; set; }
    }
}
