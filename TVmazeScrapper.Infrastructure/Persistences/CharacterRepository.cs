
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class CharacterRepository : SqlRepository<Character>
    {

        protected override IDbConnectionFactory DbFactory { get; }

        public CharacterRepository(IDbConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }
        protected override string TableName => "dbo.Character";

        protected override string MergeInsertData => @"
            INSERT (
                Id,
                Url,
                Name,
                ImageId)
            VALUES (
                Source.Id,
                Source.Url,
                Source.Name,
                Source.ImageId)
        ";

        protected override string MergeUpdateData => "UPDATE SET Target.Url = Source.Url, Target.Name = Source.Name";

        protected override string TempTable => @"
            CREATE TABLE #tmpTable(
            Id int,
            Url varchar(255),
            Name varchar(255),
            ImageId bigint)
        ";
    }
}
