using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVmazeScrapper.Domain.Enums;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class UnitOfWork
    {
        private readonly ShowRepository _showRepository;
        private readonly ScheduleRepository _scheduleRepository;
        private readonly CountryRepository _countryRepository;
        private readonly NetworkRepository _networkRepository;
        private readonly PersonRepository _personRepository;
        private readonly CharacterRepository _characterRepository;
        private readonly CastRepository _castRepository;
        private readonly ImageRepository _imageRepository;
        private readonly ExternalRepository _externalRepository;

        public UnitOfWork(ShowRepository showRepository,
            ScheduleRepository scheduleRepository,
            CountryRepository countryRepository,
            NetworkRepository networkRepository,
            PersonRepository personRepository,
            CharacterRepository characterRepository,
            CastRepository castRepository,
            ImageRepository imageRepository,
            ExternalRepository externalRepository)
        {
            _showRepository = showRepository;
            _scheduleRepository = scheduleRepository;
            _countryRepository = countryRepository;
            _networkRepository = networkRepository;
            _personRepository = personRepository;
            _characterRepository = characterRepository;
            _castRepository = castRepository;
            _imageRepository = imageRepository;
            _externalRepository = externalRepository;
        }
        public IEnumerable<Show> GetShows(int offset, int pageSize)
        {
            var shows = _showRepository.GetAll(offset, pageSize);
            foreach (var item in shows)
            {
                item.Cast = _castRepository.GetAll(item.Id);
            }

            return shows;
        }

        public Show GetShow(long? id)
        {
            var show = _showRepository.GetById(id);
            show.Cast = _castRepository.GetAll(show.Id);

            return show;
        }

        public void Dump(Show data)
        {
            DumpShow(data);
            DumpSchedule(data.Schedule, data.Id);
        }

        public void Dump(long? ShowId, IEnumerable<Cast> data)
        {
            foreach(var d in data)
            {
                DumpPerson(d.Person);
                DumpCharacter(d.Character);
                DumpCast(ShowId, d.Person.Id, d.Character.Id);
            }
        }

        private void DumpPerson(Person data)
        {
            long? countryId;
            var country = _countryRepository.FindByCountryCode(data.Country?.Code);
            if (country is null && data.Country is not null)
            {
                countryId = DumpCountry(data.Country);
            }
            else
            {
                countryId = country?.Id;
            }

            var imageId = DumpImages(data.Image, ImageType.Person, data.Id);

            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Url");
            dt.Columns.Add("Name");
            dt.Columns.Add("CountryId");
            dt.Columns.Add("Birthday");
            dt.Columns.Add("Deadday");
            dt.Columns.Add("Gender");
            dt.Columns.Add("ImageId");
            dt.Columns.Add("Updated");
            dt.Rows.Add(data.Id, data.Url, data.Name, countryId, data.Birthday, data.Deadday, data.Gender, imageId, data.Updated);

            _personRepository.Merge(dt);
        }

        private void DumpCast(long? ShowId, long? PersonId, long? CharacterId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("ShowId");
            dt.Columns.Add("PersonId");
            dt.Columns.Add("CharacterId");
            dt.Rows.Add(null, ShowId, PersonId, CharacterId);

            _castRepository.Merge(dt);
        }

        private void DumpCharacter(Character data)
        {
            var imageId = DumpImages(data.Image, ImageType.Character, data.Id);

            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Url");
            dt.Columns.Add("Name");
            dt.Columns.Add("ImageId");
            dt.Rows.Add(data.Id, data.Url, data.Name, imageId);

            _characterRepository.Merge(dt);
        }

        private void DumpNetwork(Network data, long? countryId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("CountryId");
            dt.Columns.Add("OfficialSite");
            dt.Rows.Add(data.Id, data.Name, countryId, data.OfficialSite);

            _networkRepository.Merge(dt);
        }

        private void DumpSchedule(Schedule data, long? ShowId)
        {
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Id");
            dt2.Columns.Add("Time");
            dt2.Columns.Add("Days");
            dt2.Columns.Add("ShowId");
            dt2.Rows.Add(null, data.Time, string.Join(',', data.Days), ShowId);

            _scheduleRepository.Merge(dt2);
        }

        private long DumpCountry(Country data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Code");
            dt.Columns.Add("Timezone");
            dt.Rows.Add(null, data.Name, data.Code, data.Timezone);

            return _countryRepository.Merge(dt);
        }

        private long DumpExternal(External data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Tvrage");
            dt.Columns.Add("Thetvdb");
            dt.Columns.Add("Imdb");
            dt.Rows.Add(null, data.Tvrage, data.Thetvdb, data.Imdb);

            return _externalRepository.Merge(dt);
        }

        private void DumpShow(Show data)
        {
            var imageId = DumpImages(data.Image, ImageType.Show, data.Id);
            var externalId = DumpExternal(data.Externals);

            long? countryId;
            var country = _countryRepository.FindByCountryCode(data.Network.Country.Code);
            if (country is null)
            {
                countryId = DumpCountry(data.Network.Country);
            }
            else
            {
                countryId = country.Id;
            }

            DumpNetwork(data.Network, countryId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Url");
            dt.Columns.Add("Name");
            dt.Columns.Add("Type");
            dt.Columns.Add("Language");
            dt.Columns.Add("Genres");
            dt.Columns.Add("Status");
            dt.Columns.Add("Runtime");
            dt.Columns.Add("AverageRuntime");
            dt.Columns.Add("Premiered");
            dt.Columns.Add("Ended");
            dt.Columns.Add("OfficialSite");
            dt.Columns.Add("Rating.Average");
            dt.Columns.Add("Weight");
            dt.Columns.Add("WebChannel");
            dt.Columns.Add("DvdCountry");
            dt.Columns.Add("ImageId");
            dt.Columns.Add("ExternalId");
            dt.Columns.Add("NetworkId");
            dt.Columns.Add("Summary", typeof(SqlString));
            dt.Columns.Add("Updated");

            dt.Rows.Add(data.Id,
                data.Url,
                data.Name,
                data.Type,
                data.Language,
                string.Join(',', data.Genres),
                data.Status,
                data.Runtime,
                data.AverageRuntime,
                data.Premiered,
                data.Ended,
                data.OfficialSite,
                data.Rating.Average,
                data.Weight,
                data.WebChannel,
                data.DvdCountry,
                imageId,
                externalId,
                data.Network.Id,
                data.Summary,
                data.Updated);

            _showRepository.Merge(dt);
        }

        private long? DumpImages(Image data, ImageType type, long? ownerId)
        {
            if (data is null)
            {
                return default;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Medium");
            dt.Columns.Add("Original");
            dt.Columns.Add("Type");
            dt.Columns.Add("OwnerId");
            dt.Rows.Add(null, data.Medium, data.Original, type, ownerId);

            var res = _imageRepository.Merge(dt);
            return res == 0 ? _imageRepository.FindImageById(ownerId, type).Id : res;
        }
    }
}
