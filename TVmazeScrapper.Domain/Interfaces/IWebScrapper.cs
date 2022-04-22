using System.Threading.Tasks;

namespace TVmazeScrapper.Domain.Interfaces
{
    public interface IWebScrapper
    {
        Task<T> FetchData<T>(string endpoint) where T : class;

        Task StoreData<T>(T data) where T : class;
    }
}
