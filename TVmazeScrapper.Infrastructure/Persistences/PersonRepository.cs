
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class PersonRepository : SqlRepository<Person>
    {

        protected override IDbConnectionFactory DbFactory { get; }

        public PersonRepository(IDbConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }
        protected override string TableName => "dbo.Person";

        protected override string MergeInsertData => @"
            INSERT (
                Id,
                Url,
                Name,
                CountryId,
                Birthday,
                Deadday,
                Gender,
                ImageId,
                Updated)
            VALUES (
                Source.Id,
                Source.Url,
                Source.Name,
                Source.CountryId,
                Source.Birthday,
                Source.Deadday,
                Source.Gender,
                Source.ImageId,
                Source.Updated)
        ";

        protected override string MergeUpdateData => "UPDATE SET Target.Url = Source.Url, Target.Name = Source.Name";

        protected override string TempTable => @"
            CREATE TABLE #tmpTable(
            Id int,
            Url varchar(255),
            Name varchar(255),
            CountryId int,
            Birthday Date,
            Deadday Date,
            Gender varchar(255),
            ImageId bigint,
            Updated bigint)
        ";
    }
}
