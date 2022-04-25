using Microsoft.Extensions.DependencyInjection;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Infrastructure.Persistences;

namespace TVmazeScrapper.API.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IServiceCollection AddDbRepository(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactory, DbFactory>();
            services.AddSingleton<ShowRepository>();
            services.AddSingleton<ScheduleRepository>();
            services.AddSingleton<CountryRepository>();
            services.AddSingleton<NetworkRepository>();
            services.AddSingleton<PersonRepository>();
            services.AddSingleton<CharacterRepository>();
            services.AddSingleton<CastRepository>();
            services.AddSingleton<ImageRepository>();
            services.AddSingleton<ExternalRepository>();
            services.AddSingleton<UnitOfWork>();

            return services;
        }
    }
}
