using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BricksMeatballs.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BricksMeatballs.BackgroundServices
{
    public class TimedHostedService : IHostedService
    {
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly UraService _uraService;

        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceProvider serviceProvider, UraService uraService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _uraService = uraService;
        
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await CallApiAndUpdateDb();

                    await Task.Delay(TimeSpan.FromDays(7));
                }
            },TaskCreationOptions.LongRunning);

            return Task.CompletedTask;
        }
        private async Task CallApiAndUpdateDb()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var dbContext=scopedServiceProvider.GetRequiredService<AppDBContext>();
                for (int i = 1; i <= 4; i++)
                {
                    _logger.LogError("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
                    var xxresult = await _uraService.GetPMI_Resi_Transaction(i);
                }

            }

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            return Task.CompletedTask;
        }
    }
}
