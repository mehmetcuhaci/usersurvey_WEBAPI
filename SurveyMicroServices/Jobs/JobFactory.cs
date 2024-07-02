using Quartz;
using Quartz.Spi;
using System;

namespace SurveyMicroServices.Jobs
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            // Get job type from bundle
            var jobDetail = bundle.JobDetail;
            Type jobType = jobDetail.JobType;

            // Resolve the job instance from DI container
            return _serviceProvider.GetService(jobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            // Dispose or clean up job if necessary
        }
    }
}
