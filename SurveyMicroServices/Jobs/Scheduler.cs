using Quartz;
using Quartz.Impl;

namespace SurveyMicroServices.Jobs
{
    public static class Scheduler
    {
        public static async Task StartAsync(IServiceProvider serviceProvider)
        {
            ISchedulerFactory schedulerFactory= new StdSchedulerFactory();
            IScheduler scheduler=await schedulerFactory.GetScheduler();
            scheduler.JobFactory=serviceProvider.GetRequiredService<JobFactory>();

            IJobDetail job=JobBuilder.Create<UpdateSurveyStatusJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithSimpleSchedule(x => x.WithIntervalInHours(1).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
    }
}
