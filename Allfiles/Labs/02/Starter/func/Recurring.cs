using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace func;

public class Recurring
{
    private readonly ILogger _logger;

    public Recurring(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Recurring>();
    }

    [Function("Recurring")]
    public void Run([TimerTrigger("* */5 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}