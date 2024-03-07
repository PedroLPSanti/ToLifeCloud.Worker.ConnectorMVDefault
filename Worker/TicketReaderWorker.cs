using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ToLifeCloud.Worker.ConnectorMVDefault.Worker
{
    public class TicketReaderWorker : BackgroundService
    {
        private readonly ILogger<TicketReaderWorker> _logger;
        private IServiceProvider Services { get; }
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _enviornment;

        public TicketReaderWorker(ILogger<TicketReaderWorker> logger, IOptions<AppSettings> appSettings, IWebHostEnvironment environment, IServiceProvider services)
        {
            Services = services;
            _logger = logger;
            _appSettings = appSettings.Value;
            _enviornment = environment;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            UtilLogs.PrintLog(_logger, _appSettings, "Worker TicketReader running {time}", _appSettings.ticketReaderWorkerRun);
            if (_appSettings.ticketReaderWorkerRun)
            {
                TicketReaderService passwordReaderService = new TicketReaderService(_appSettings, Services);
                while (!stoppingToken.IsCancellationRequested)
                {
                    UtilLogs.PrintLog(_logger, _appSettings, "Worker TicketReader running at: {time}", DateTimeOffset.Now);                    
                    if (passwordReaderService.Process())
                        await Task.Delay(_appSettings.workersDelays.ticketReader, stoppingToken);
                    else
                        await Task.Delay(1000, stoppingToken);

                }
            }
        });
    }
}
