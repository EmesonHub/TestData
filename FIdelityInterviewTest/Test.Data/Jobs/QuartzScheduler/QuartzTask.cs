using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Test.Data.Interfaces;

namespace Test.Data.Jobs.QuartzScheduler
{
    public class QuartzTask: IJob
    {
        //private readonly IServiceProvider _provider;
        //private readonly IEmailSender _emailSender;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public QuartzTask(IServiceScopeFactory serviceScopeFactory) //IServiceProvider provider, IEmailSender emailSender
        {
            //_provider = provider;
            //_emailSender = emailSender;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public Task Execute(IJobExecutionContext context)
        {
            TaskJobs();
            return Task.CompletedTask;
        }
        public void TaskJobs()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var user_interface = scope.ServiceProvider.GetRequiredService<IUserData>();
            user_interface.GetUsersById(1);

        }
    }
}
