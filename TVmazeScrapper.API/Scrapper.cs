using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TVmazeScrapper.API
{
    public class Scrapper : BackgroundService
    {
        private readonly ILogger<Scrapper> _logger;

        public Scrapper(ILogger<Scrapper> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000);
            }
        }
    }
}
