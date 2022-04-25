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
                Genres,
                Status,
                Runtime,
                AverageRuntime,
                Premiered,
                Ended,
                OfficialSite,
                [Rating.Average],
                Weight,
                WebChannel, 
                DvdCountry,
                ImageId,
                ExternalId,
                NetworkId,
                Summary,
                Updated)
            VALUES (
                Source.Id,
                Source.Url,
                Source.Name,
                Source.Type,
                Source.Language,
                Source.Genres,
                Source.Status,
                Source.Runtime,
                Source.AverageRuntime,
                Source.Premiered,
                Source.Ended,
                Source.OfficialSite,
                Source.[Rating.Average],
                Source.Weight,
                Source.WebChannel,
                Source.DvdCountry,
                Source.ImageId,
                Source.ExternalId,
                Source.NetworkId,
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
            Genres varchar(255),
            Status varchar(255),
            Runtime int,
            AverageRuntime int,
            Premiered Date,
            Ended Date,
            OfficialSite varchar(255),
            [Rating.Average] decimal,
            Weight int,
            WebChannel varchar(255),
            DvdCountry varchar(255),
            ImageId bigint,
            ExternalId bigint,
            NetworkId bigint,
            Summary text,
            Updated bigint)
        ";

        protected const string GetQuery = @"
            SELECT 
                t1.Id,
                t1.Url,
                t1.Name,
                t1.Type,
                t1.Language,
                t1.Genres,
                t1.Status,
                t1.Runtime,
                t1.AverageRuntime,
                t1.Premiered,
                t1.Ended,
                t1.OfficialSite,
                t4.Time,
                t4.Days,
                t1.[Rating.Average],
                t1.Weight,
                t1.NetworkId,
                t2.OfficialSite as NetworkOfficialSite,
                t2.Name as NetworkName,
                t3.Name as CountryName,
                t3.Code as CountryCode,
                t3.Timezone as CountryTimezone,
                t1.WebChannel,
                t1.DvdCountry,
                t1.Summary,
                t5.Medium as ImageMedium,
                t5.Original as ImageOriginal,
                t6.Tvrage,
                t6.Thetvdb,
                t6.Imdb,                                
                t1.Updated
            FROM [master].[dbo].[Show] t1
                left join [master].[dbo].[Network] t2 on t2.Id = t1.NetworkId
                left join [master].[dbo].[Country] t3 on t3.id = t2.CountryId
                left join [master].[dbo].[Schedule] t4 on t4.ShowId = t1.Id
                left join [master].[dbo].[Images] t5 on t5.Id = t1.ImageId
                left join [master].[dbo].[Externals] t6 on t6.Id = t1.ExternalId";

        public long? GetLastId()
        {
            long? result = default;
            using (var con = DbFactory.GetConnection(DatabaseType.TvMaze))
            {
                con.Open();

                using (var comm = new SqlCommand())
                {
                    comm.Connection = (SqlConnection)con;
                    try
                    {
                        string sql = $"SELECT TOP 1 Id FROM {TableName} ORDER BY ID DESC";
                        comm.CommandText = sql;
                        comm.CommandType = CommandType.Text;
                        result = (long?)comm.ExecuteScalar();
                    }
                    catch
                    {

                    }
                }
            }

            return result;
        }

        public override Show GetById(long? id)
        {
            Show result = new();
            string sql = @$"
                {GetQuery}
                WHERE t1.Id = {id}
            ";

            using (var con = DbFactory.GetConnection(DatabaseType.TvMaze))
            {
                con.Open();

                using (var comm = new SqlCommand())
                {
                    comm.Connection = (SqlConnection)con;
                    try
                    {
                        comm.CommandText = sql;
                        comm.CommandType = CommandType.Text;
                        using (var reader = comm.ExecuteReader())
                        {
                            while (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    result = MapShow(reader);
                                }

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

        public override IEnumerable<Show> GetAll(int offset, int pageSize)
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
                        string sql = $@"
                            {GetQuery}
                            ORDER BY ID
						    OFFSET {pageSize * (offset - 1)} ROWS FETCH NEXT {pageSize} ROWS ONLY
                        ";

                        comm.CommandText = sql;
                        comm.CommandType = CommandType.Text;
                        using (var reader = comm.ExecuteReader())
                        {
                            while (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Show show = MapShow(reader);

                                    result.Add(show);
                                }


                                reader.NextResult();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                con.Close();
            }

            return result;
        }

        private static Show MapShow(SqlDataReader reader)
        {
            return new Show()
            {
                Id = (int)reader.GetInt64("Id"),
                Url = reader.GetString("Url"),
                Name = reader.GetString("Name"),
                Type = reader.GetString("Type"),
                Language = reader.GetString("Language"),
                Genres = reader.GetString("Genres").Split(','),
                Status = reader.GetString("Status"),
                Runtime = (int)reader.GetInt64("Runtime"),
                AverageRuntime = (int)reader.GetInt64("AverageRuntime"),
                Premiered = reader.GetDateTime("Premiered"),
                Ended = reader["Ended"] as DateTime?,
                Rating = new Rating
                {
                    Average = reader.GetDecimal("Rating.Average"),
                },
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
                    OfficialSite = reader["NetworkOfficialSite"] as string,
                    Country = new Country
                    {
                        Code = reader["CountryCode"] as string,
                        Name = reader["CountryName"] as string,
                        Timezone = reader["CountryTimezone"] as string
                    },
                    Id = reader["NetworkId"] as long?,
                    Name = reader["NetworkName"] as string
                },
                DvdCountry = reader["DvdCountry"] as string,
                Summary = reader.GetString("Summary"),
                Image = new Image
                {
                    Medium = reader["ImageMedium"] as string,
                    Original = reader["ImageOriginal"] as string
                },
                Externals = new External
                {
                    Imdb = reader.GetString("Imdb"),
                    Thetvdb = reader.GetInt64("Thetvdb"),
                    Tvrage = reader.GetInt64("Tvrage")
                },
                Updated = reader.GetInt64("Updated")
            };
        }
    }
}
