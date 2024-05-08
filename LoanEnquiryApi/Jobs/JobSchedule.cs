using Quartz;

namespace LoanEnquiryApi.Jobs
{
    public class JobSchedule
    {
        internal static async Task ScheduleJobs(IServiceProvider serviceProvider)
        {
            var scheduler = serviceProvider.GetRequiredService<IScheduler>();

            // Define and schedule your jobs here
            IJobDetail job = JobBuilder.Create<ExecuteJob>()
                .WithIdentity("yourJob", "group1")
                .Build();
            Console.WriteLine($"Set job - ExecuteJob : {job}");
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("yourTrigger", "group1")
                .StartNow()
                .WithCronSchedule("0 5 9 ? * * *") // Schedule at 9:05 AM every day
                .Build();
            Console.WriteLine($"Set time trigger - ExecuteJob : {trigger}");
            await scheduler.ScheduleJob(job, trigger);

            // Start the scheduler
            await scheduler.Start();
        }
    }
}
