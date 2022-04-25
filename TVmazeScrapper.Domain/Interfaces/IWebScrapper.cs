using System.Threading.Tasks;

namespace TVmazeScrapper.Domain.Interfaces
{
    public interface IWebScrapper
    {
        Task ScrapIt(long? showId);
    }
}
