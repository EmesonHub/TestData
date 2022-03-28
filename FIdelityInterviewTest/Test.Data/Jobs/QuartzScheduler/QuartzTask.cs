using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Test.Data.Jobs.QuartzScheduler
{
    public class QuartzTask: IJob
    {
        private readonly IServiceProvider _provider;
        //private readonly IEmailSender _emailSender;
        public QuartzTask(IServiceProvider provider) //, IEmailSender emailSender
        {
            _provider = provider;
            //_emailSender = emailSender;
        }
        public Task Execute(IJobExecutionContext context)
        {
            Logs($"{DateTime.Now} [Reminders Service called]" + Environment.NewLine);

            return Task.CompletedTask;
        }
        public void Logs(string message)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Quartz");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, "Logs.txt");
            using FileStream fstream = new FileStream(path, FileMode.Create);
            using TextWriter writer = new StreamWriter(fstream);
            writer.WriteLine(message);
        }
    }
}
