using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Configs;
using TVmazeScrapper.Domain.Models.Entities;
using TVmazeScrapper.Infrastructure.Persistences;
using Xunit;

namespace TVmazeScrapper.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            try
            {
                var x = JsonConvert.DeserializeObject<Show>(File.ReadAllText(@"Data\Show.json"));
                var y = JsonConvert.DeserializeObject<IEnumerable<Cast>>(File.ReadAllText(@"Data\Cast.json"));
                var d = new AppConfig
                {
                    DbConnStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
                };

                IDbConnectionFactory dbConnectionFactory = new DbFactory(d);
                var u = new UnitOfWork(new ShowRepository(dbConnectionFactory),
                    new ScheduleRepository(dbConnectionFactory),
                    new CountryRepository(dbConnectionFactory),
                    new NetworkRepository(dbConnectionFactory),
                    new PersonRepository(dbConnectionFactory),
                    new CharacterRepository(dbConnectionFactory),
                    new CastRepository(dbConnectionFactory));

                u.Dump(x);
                u.Dump(x.Id, y);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
