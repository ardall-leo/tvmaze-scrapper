using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public UnitOfWork(ShowRepository showRepository,
            ScheduleRepository scheduleRepository,
            CountryRepository countryRepository,
            NetworkRepository networkRepository,
            PersonRepository personRepository,
            CharacterRepository characterRepository,
            CastRepository castRepository)
        {
            _showRepository = showRepository;
            _scheduleRepository = scheduleRepository;
            _countryRepository = countryRepository;
            _networkRepository = networkRepository;
            _personRepository = personRepository;
            _characterRepository = characterRepository;
            _castRepository = castRepository;
        }

        public void Dump(Show data)
        {
            DumpShow(data);
            DumpSchedule(data.Schedule, data.Id);
            long countryId;
            var country = _countryRepository.FindByCountryCode(data.Network.Country.Code);
            if (country is null)
            {
                countryId = DumpCountry(data.Network.Country);
            }
            else
            {
                countryId = country.Id;
            }

            if (countryId > -1)
            {
                DumpNetwork(data.Network, countryId, data.Id);
            }
        }

        public void Dump(long ShowId, IEnumerable<Cast> data)
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
            long countryId;
            var country = _countryRepository.FindByCountryCode(data.Country.Code);
            if (country is null)
            {
                countryId = DumpCountry(data.Country);
            }
            else
            {
                countryId = country.Id;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Url");
            dt.Columns.Add("Name");
            dt.Columns.Add("CountryId");
            dt.Columns.Add("Birthday");
            dt.Columns.Add("Deadday");
            dt.Columns.Add("Gender");
            dt.Columns.Add("Image");
            dt.Columns.Add("Updated");
            dt.Rows.Add(data.Id, data.Url, data.Name, countryId, data.Birthday, data.Deadday, data.Gender, 0, data.Updated);

            _personRepository.Merge(dt);
        }

        private void DumpCast(long ShowId, long PersonId, long CharacterId)
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
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Url");
            dt.Columns.Add("Name");
            dt.Rows.Add(data.Id, data.Url, data.Name);

            _characterRepository.Merge(dt);
        }

        private void DumpNetwork(Network data, long countryId, long ShowId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("CountryId");
            dt.Columns.Add("OfficialSite");
            dt.Columns.Add("ShowId");
            dt.Rows.Add(data.Id, data.Name, countryId, data.OfficialSite, ShowId);

            _networkRepository.Merge(dt);
        }

        private void DumpSchedule(Schedule data, long ShowId)
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

        private void DumpShow(Show data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Url");
            dt.Columns.Add("Name");
            dt.Columns.Add("Type");
            dt.Columns.Add("Language");
            dt.Columns.Add("Status");
            dt.Columns.Add("Runtime");
            dt.Columns.Add("AverageRuntime");
            dt.Columns.Add("Premiered");
            dt.Columns.Add("Ended");
            dt.Columns.Add("OfficialSite");
            dt.Columns.Add("Weight");
            dt.Columns.Add("WebChannel");
            dt.Columns.Add("DvdCountry");
            dt.Columns.Add("Summary", typeof(SqlString));
            dt.Columns.Add("Updated");

            dt.Rows.Add(data.Id,
                data.Url,
                data.Name,
                data.Type,
                data.Language,
                data.Status,
                data.Runtime,
                data.AverageRuntime,
                data.Premiered,
                data.Ended,
                data.OfficialSite,
                data.Weight,
                data.WebChannel,
                data.DvdCountry,
                data.Summary,
                data.Updated);

            _showRepository.Merge(dt);
        }
    }
}
