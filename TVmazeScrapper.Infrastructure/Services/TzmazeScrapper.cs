using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;
using TVmazeScrapper.Infrastructure.Persistences;

namespace TVmazeScrapper.Infrastructure.Services
{
    public class TzmazeScrapper : ApiClient, IWebScrapper
    {
        private UnitOfWork _unitOfWork;

        public TzmazeScrapper(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task ScrapIt(long? showId)
        {
            var show = await this.SendRequest<Show>($@"https://api.tvmaze.com/shows/{showId}", HttpMethod.Get, null);
            _unitOfWork.Dump(show);
            var cast = await this.SendRequest<IEnumerable<Cast>>($@"https://api.tvmaze.com/shows/{showId}/cast", HttpMethod.Get, null);
            _unitOfWork.Dump(show.Id, cast);
        }
    }
}
