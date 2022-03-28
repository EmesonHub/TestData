using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using NCrontab;
using System.Threading.Tasks;
using System.Threading;
using Test.Data.Classes;

namespace Test.Data.Jobs
{
    public class NCronTabService: BackgroundService
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        private AccountData AccData = new AccountData();

        private int ScheduleDelay => 1000 * 60 * 1; //after a minute
        private string Schedule => "* */31 12 * * *";  //Runs every 12:18 pm 

        //"* * * */1 1 *" //Runs every 1st of January month
        //"* * * * */ *" //Runs every 1st of the month

        //"*/10 * * * * *"; //Runs every 10 seconds

        public NCronTabService()
        {
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                var now = DateTime.Now;
                var nextrun = _schedule.GetNextOccurrence(now);
                if (now > _nextRun)
                {
                    var data = AccData.GetAllAccounts();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
                await Task.Delay(ScheduleDelay, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }

    }
}
