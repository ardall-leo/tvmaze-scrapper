using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class NetworkRepository : SqlRepository<Network>
    {

        protected override IDbConnectionFactory DbFactory { get; }

        public NetworkRepository(IDbConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }
        protected override string TableName => "dbo.Network";

        protected override string MergeInsertData => @"
            INSERT (Id, Name, CountryId, OfficialSite)
            VALUES (Source.Id, Source.Name, Source.CountryId, Source.OfficialSite)
        ";

        protected override string MergeUpdateData => "UPDATE SET Target.Name = Source.Name, Target.CountryId = Source.CountryId, Target.OfficialSite = Source.OfficialSite";

        protected override string TempTable => @"
            CREATE TABLE #tmpTable(
            Id int,
            Name varchar(255),
            CountryId int,
            OfficialSite varchar(255))
        ";
    }
}
