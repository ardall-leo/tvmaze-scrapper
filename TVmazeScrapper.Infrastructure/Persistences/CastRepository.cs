using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class CastRepository : SqlRepository<Cast>
    {

        protected override IDbConnectionFactory DbFactory { get; }

        public CastRepository(IDbConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }
        protected override string TableName => "dbo.Cast";

        protected override string MergeInsertData => @"
            INSERT (
                ShowId,
                PersonId,
                CharacterId)
            VALUES (
                Source.ShowId,
                Source.PersonId,
                Source.CharacterId)
        ";

        protected override string MergeUpdateData => "UPDATE SET Target.ShowId = Source.ShowId, Target.PersonId = Source.PersonId, Target.CharacterId = Source.CharacterId";

        protected override string TempTable => @"
            CREATE TABLE #tmpTable(
            Id bigint,
            ShowId bigint,
            PersonId bigint,
            CharacterId bigint)
        ";
    }
}
