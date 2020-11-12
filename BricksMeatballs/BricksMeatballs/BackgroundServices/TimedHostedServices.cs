using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using BricksMeatballs.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BricksMeatballs.BackgroundServices
{
    /// <summary>
    /// Author: Huang Chaoshan, Lim Pei Yan
    /// The TimeHostedService class is a background services which will automatically call the URA API after 7 days
    /// </summary>
    public class TimedHostedService : IHostedService
    {
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly UraService _uraService;

        /// <summary>
        /// The TimeHostedService method intialise the logger, serviceProvider and uraService
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="uraService"></param>
        public TimedHostedService(ILogger<TimedHostedService> logger, IServiceProvider serviceProvider, UraService uraService)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _uraService = uraService;

        }

        /// <summary>
        /// The StartAsync method will do the task of calling api method and reset the counter 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                   // bool chk = await TruncateTable();
                   //if (chk == true){
                        _logger.LogInformation("Call and Update Database");
                        await CallApiAndUpdateDb();
                   // }

                    await Task.Delay(TimeSpan.FromDays(7));
                }
            }, TaskCreationOptions.LongRunning);

            return Task.CompletedTask;
        }

        /// <summary>
        /// The method CallApiAndUpdateDb will call the API services and fetch the transaction result
        /// </summary>
        /// <returns></returns>
        private async Task CallApiAndUpdateDb()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedServiceProvider = scope.ServiceProvider;
                var dbContext = scopedServiceProvider.GetRequiredService<AppDBContext>();
                for (int i = 1; i <= 4; i++)
                {
                    _logger.LogInformation("Retrieving Batch " + i);
                    var xxresult = await _uraService.GetPMI_Resi_Transaction(i);
                }

            }

        }

        /// <summary>
        /// Use to trucate the table in database
        /// </summary>
        /// <returns>Returns if the table has been truncated sucessfully</returns>
        private async Task<bool> TruncateTable()
        {
            _logger.LogInformation("Truncate table");
            MySqlConnection conn = new MySqlConnection();
            MySqlCommand cmd = null;
            conn.ConnectionString = "Persist Security Info=False; server = localhost; port = 3306; database = bricks; user = root; password = 2012";

            try
            {
                string cmdString = "TRUNCATE TABLE bricks.pmiresidenceresult";
                conn.Open();
                cmd = new MySqlCommand(cmdString, conn);
                int result = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to Truncate" + e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// The StopAsync method is to track the log when task finished.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            return Task.CompletedTask;
        }
    }
}
