using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using QuartzConsoleDemo;

// NOTE: You need to set up the log provider before creating the job scheduler
LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

var factory = new StdSchedulerFactory();
var scheduler = await factory.GetScheduler();

var helloJob = JobBuilder.Create<HelloJob>()
    .WithIdentity("hello-world-job", "sample-jobs-group")
    .Build();

// Trigger the job now, and then repeat every 10 seconds
var helloTrigger = TriggerBuilder.Create()
    .WithIdentity("hello-world-trigger", "sample-triggers-group")
    .StartNow()
    .WithSimpleSchedule(builder => builder
        .WithIntervalInSeconds(10)
        .RepeatForever())
    .Build();

await scheduler.ScheduleJob(helloJob, helloTrigger);
await scheduler.Start();

await Task.Delay(TimeSpan.FromSeconds(60));

await scheduler.Shutdown();

Console.WriteLine("Press any key to close the application");
Console.ReadKey();
