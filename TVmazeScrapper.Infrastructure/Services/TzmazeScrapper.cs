using System;
using System.Net.Http;
using System.Threading.Tasks;
using TVmazeScrapper.Domain.Interfaces;

namespace TVmazeScrapper.Infrastructure.Services
{
    public class TzmazeScrapper : ApiClient, IWebScrapper
    {
        public Task<T> FetchData<T>(string endpoint) where T : class
        {
            return this.SendRequest<T>(endpoint, HttpMethod.Get, null);
        }

        public Task StoreData<T>(T data) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
