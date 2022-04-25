using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TVmazeScrapper.API;
using TVmazeScrapper.API.Extensions;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Configs;
using TVmazeScrapper.Infrastructure.Persistences;
using TVmazeScrapper.Infrastructure.Services;

try
{
    using IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        })
        .ConfigureServices((hostContext, services) =>
        {
            var config = hostContext.Configuration;
            var rootConfig = new AppConfig();
            config.Bind("AppConfig", rootConfig);
            services.AddSingleton(rootConfig);
            services.AddDbRepository();
            services.AddSingleton<IWebScrapper, TzmazeScrapper>();
            services.AddHostedService<Scrapper>(); 
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    throw;
}