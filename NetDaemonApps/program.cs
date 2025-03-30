using System.Reflection;
using Microsoft.Extensions.Hosting;
using NetDaemon.Extensions.Logging;
using NetDaemon.Extensions.Scheduler;
using NetDaemon.Extensions.Tts;
using NetDaemon.Runtime;
using HomeAssistantGenerated;
using Microsoft.Extensions.DependencyInjection;
using NetDaemon.Extensions.MqttEntityManager;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Services;

#pragma warning disable CA1812

try
{
    await Host.CreateDefaultBuilder(args)
        .UseNetDaemonAppSettings()
        .UseNetDaemonDefaultLogging()
        .UseNetDaemonRuntime()
        .UseNetDaemonTextToSpeech()
        .UseNetDaemonMqttEntityManagement()
        .ConfigureServices((_, services) =>
            services
                .AddScoped<INotificationService, NotificationService>()
                .AddAppsFromAssembly(Assembly.GetExecutingAssembly())
                .AddNetDaemonStateManager()
                .AddNetDaemonScheduler()
                .AddHomeAssistantGenerated()
                .AddMqttEntities())
        .Build()
        .RunAsync()
        .ConfigureAwait(false);
    
    
}
catch (Exception e)
{
    Console.WriteLine($"Failed to start host... {e}");
    throw;
}