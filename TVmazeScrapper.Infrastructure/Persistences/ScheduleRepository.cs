using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class ScheduleRepository : SqlRepository<Show>
    {

        protected override IDbConnectionFactory DbFactory { get; }

        public ScheduleRepository(IDbConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }
        protected override string TableName => "dbo.Schedule";

        protected override string MergeInsertData => @"
            INSERT (Time, Days, ShowId)
            VALUES (Source.Time, Source.Days, Source.ShowId)
        ";

        protected override string MergeUpdateData => "UPDATE SET Target.Time = Source.Time, Target.Days = Source.Days";

        protected override string TempTable => @"
            CREATE TABLE #tmpTable(
            Id int,
            Time varchar(255),
            Days varchar(255),
            ShowId int)
        ";
    }
}
