using System;
using System.Data;
using System.Data.SqlClient;
using TVmazeScrapper.Domain.Enums;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class ExternalRepository : SqlRepository<External>
    {
        protected override IDbConnectionFactory DbFactory { get; }

        public ExternalRepository(IDbConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        protected override string MergeInsertData => @"
            INSERT (Tvrage, Thetvdb, Imdb)
            VALUES (Source.Tvrage, Source.Thetvdb, Source.Imdb)
        ";

        protected override string MergeUpdateData => "UPDATE SET Target.Tvrage = Source.Tvrage, Target.Thetvdb = Source.Thetvdb, Target.Imdb = Source.Imdb";

        protected override string TableName => "dbo.Externals";

        protected override string TempTable => @"
            CREATE TABLE #tmpTable(
            Id int,
            Tvrage bigint,
            Thetvdb bigint,
            Imdb varchar(255))
        ";
    }
}
