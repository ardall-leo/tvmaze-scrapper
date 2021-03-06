using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Configs;
using TVmazeScrapper.Domain.Models.Exceptions;
using TVmazeScrapper.Infrastructure.Persistences;

namespace TVmazeScrapper.API
{
    public class Scrapper : BackgroundService
    {
        private readonly ILogger<Scrapper> _logger;
        private readonly IWebScrapper _webScrapper;
        private readonly ShowRepository _showRepository;
        private readonly AppConfig _config;
        private long? showId = 1;

        public Scrapper(ILogger<Scrapper> logger, IWebScrapper webScrapper, ShowRepository showRepository, AppConfig config)
        {
            _logger = logger;
            _webScrapper = webScrapper;
            _showRepository = showRepository;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            long? lastId = _showRepository.GetLastId();
            showId = lastId == null ? showId : lastId;
            int retryAttempt = 1;
            bool continueScrapping = true;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"Scrapping showID {showId}");
                    await _webScrapper.ScrapIt(showId);
                    await Task.Delay(_config.ScrappingInterval * 1000);

                    showId++;
                }
                catch(ScrapException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // this could be the end of the data source, so just stop scrapping
                    continueScrapping = false;
                }
                catch (SocketException ex)
                {
                    var retryInSeconds = (int)Math.Pow(2, retryAttempt);
                    _logger.LogError($"{ex.Message}. Retry in {retryInSeconds} seconds.");
                    await Task.Delay(retryInSeconds * 1000);
                    continue;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }
}
