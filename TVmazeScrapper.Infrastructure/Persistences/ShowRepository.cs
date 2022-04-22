using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVmazeScrapper.Domain.Enums;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class ShowRepository : SqlRepository<Show>
    {

        protected override IDbConnectionFactory DbFactory { get; }

        public ShowRepository(IDbConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }
        protected override string TableName => "dbo.Show";

        protected override string MergeInsertData => @"
            INSERT (
                Id,
                Url,
                Name,
                Type,
                Language,
                Status,
                Runtime,
                AverageRuntime,
                Premiered,
                Ended,
                OfficialSite,
                Weight,
                WebChannel, 
                DvdCountry,
                Summary,
                Updated)
            VALUES (
                Source.Id,
                Source.Url,
                Source.Name,
                Source.Type,
                Source.Language,
                Source.Status,
                Source.Runtime,
                Source.AverageRuntime,
                Source.Premiered,
                Source.Ended,
                Source.OfficialSite,
                Source.Weight,
                Source.WebChannel,
                Source.DvdCountry,
                Source.Summary,
                Source.Updated)
        ";

        protected override string MergeUpdateData => "UPDATE SET Target.Url = Source.Url, Target.Name = Source.Name, Target.Status = Source.Status";

        protected override string TempTable => @"
            CREATE TABLE #tmpTable(
            Id int,
            Url varchar(255),
            Name varchar(255),
            Type varchar(255),
            Language varchar(255),
            Status varchar(255),
            Runtime int,
            AverageRuntime int,
            Premiered Date,
            Ended Date,
            OfficialSite varchar(255),
            Weight int,
            WebChannel varchar(255),
            DvdCountry varchar(255),
            Summary text,
            Updated bigint)
        ";

        public override IEnumerable<Show> GetAll()
        {
            List<Show> result = new();
            using (var con = DbFactory.GetConnection(DatabaseType.TvMaze))
            {
                con.Open();

                using (var comm = new SqlCommand())
                {
                    comm.Connection = (SqlConnection)con;
                    try
                    {
                        string sql = @"
                            SELECT 
                                t1.Id,
                                t1.Url,
                                t1.Name,
                                t1.Type,
                                t1.Language,
                                t1.Status,
                                t1.Runtime,
                                t1.AverageRuntime,
                                t1.Premiered,
                                t1.Ended,
                                t1.OfficialSite,
                                t4.Time,
                                t4.Days,
                                t1.Weight,
                                t2.Id as NetworkId,
                                t2.OfficialSite as NetworkOfficialSite,
                                t2.Name as NetworkName,
                                t3.Name as CountryName,
                                t3.Code as CountryCode,
                                t3.Timezone as CountryTimezone,
                                t1.WebChannel,
                                t1.DvdCountry,
                                t1.Summary,
                                t1.Updated
                            FROM [master].[dbo].[Show] t1
                                left join [master].[dbo].[Network] t2 on t1.Id = t2.ShowId
                                left join [master].[dbo].[Country] t3 on t3.id = t2.CountryId
                                left join [master].[dbo].[Schedule] t4 on t4.ShowId = t1.Id;
                            
                            SELECT
                                t3.Name,
                                t3.Birthday,
                                t5.Code as CountryCode,
                                t5.Name as CountryName,
                                t5.Timezone as CountryTimezone,
                                t3.Gender,
                                t3.Deadday,
                                t3.Updated,
                                t3.Url,
                                t3.Id,
                                t4.Url as CharacterUrl,
                                t4.Name as CharacterName,
                                t4.Id as CharacterId
                             FROM [master].[dbo].[Show] t1
	                            left join [master].[dbo].[Cast] t2 on t2.ShowId = t1.Id
	                            left join [master].[dbo].[Person] t3 on t3.Id = t2.PersonId
	                            left join [master].[dbo].[Character] t4 on t4.Id = t2.CharacterId
                                left join [master].[dbo].[Country] t5 on t5.id = t3.CountryId;
                        ";

                        comm.CommandText = sql;
                        comm.CommandType = CommandType.Text;
                        using (var reader = comm.ExecuteReader())
                        {
                            while (reader.HasRows)
                            {
                                Show show = new();
                                while (reader.Read())
                                {
                                    show = show with
                                    {
                                        Id = (int)reader.GetInt64("Id"),
                                        Url = reader.GetString("Url"),
                                        Name = reader.GetString("Name"),
                                        Type = reader.GetString("Type"),
                                        Language = reader.GetString("Language"),
                                        Status = reader.GetString("Status"),
                                        Runtime = (int)reader.GetInt64("Runtime"),
                                        AverageRuntime = (int)reader.GetInt64("AverageRuntime"),
                                        Premiered = reader.GetDateTime("Premiered"),
                                        Ended = reader.GetDateTime("Ended"),
                                        OfficialSite = reader.GetString("OfficialSite"),
                                        Schedule = new Schedule
                                        {
                                            Time = reader.GetString("Time"),
                                            Days = reader.GetString("Days").Split(','),
                                        },
                                        Weight = reader.GetInt32("Weight"),
                                        WebChannel = reader["WebChannel"] as string,
                                        Network = new Network
                                        {
                                            OfficialSite = reader.GetString("NetworkOfficialSite"),
                                            Country = new Country
                                            {
                                                Code = reader.GetString("CountryCode"),
                                                Name = reader.GetString("CountryName"),
                                                Timezone = reader.GetString("CountryTimezone"),
                                            },
                                            Id = (int)reader.GetInt64("NetworkId"),
                                            Name = reader.GetString("NetworkName")
                                        },
                                        DvdCountry = reader["DvdCountry"] as string,
                                        Summary = reader.GetString("Summary"),
                                        Updated = reader.GetInt64("Updated")
                                    };
                                }

                                reader.NextResult();

                                while (reader.Read())
                                {
                                    show.Cast.Add(new Cast
                                    {
                                        Person = new Person
                                        {
                                            Id = reader.GetInt64("Id"),
                                            Name = reader.GetString("Name"),
                                            Birthday = reader.GetDateTime("Birthday"),
                                            Country = new Country
                                            {
                                                Code = reader.GetString("CountryCode"),
                                                Name = reader.GetString("CountryName"),
                                                Timezone = reader.GetString("CountryTimezone"),
                                            },
                                            Gender = (Gender)Enum.Parse(typeof(Gender), reader.GetString("Gender")),
                                            Deadday = reader.GetDateTime("Deadday"),
                                            Updated = reader.GetInt64("Updated"),
                                            Url = reader.GetString("Url"),
                                        },
                                        Character = new Character
                                        {
                                            Id = reader.GetInt64("CharacterId"),
                                            Name = reader.GetString("CharacterName"),
                                            Url = reader.GetString("CharacterUrl"),
                                        }
                                    });
                                }

                                result.Add(show);

                                reader.NextResult();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                con.Close();
            }

            return result;
        }
    }
}
