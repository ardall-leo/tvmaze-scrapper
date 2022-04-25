using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TVmazeScrapper.Domain.Enums;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class CastRepository : SqlRepository<Cast>
    {

        protected override IDbConnectionFactory DbFactory { get; }

        private readonly ILogger<CastRepository> _logger;

        public CastRepository(ILogger<CastRepository> logger, IDbConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
            _logger = logger;
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

        public List<Cast> GetAll(long? ShowId)
        {
            List<Cast> result = new();
            using (var con = DbFactory.GetConnection(DatabaseType.TvMaze))
            {
                con.Open();

                using (var comm = new SqlCommand())
                {
                    comm.Connection = (SqlConnection)con;
                    try
                    {
                        string sql = $@"
                             SELECT
                                t3.Name as PersonName,
                                t3.Birthday,
                                t5.Code as CountryCode,
                                t5.Name as CountryName,
                                t5.Timezone as CountryTimezone,
                                t3.Gender,
                                t3.Deadday,
                                t3.Updated as PersonUpdated,
                                t3.Url as PersonUrl,
                                t3.Id as PersonId,
                                t4.Url as CharacterUrl,
                                t4.Name as CharacterName,
                                t4.Id as CharacterId,
                                t6.Medium as PersonImageMedium,
                                t6.Original as PersonImageOriginal,
                                t7.Medium as CharacterImageMedium,
                                t7.Original as CharacterImageOriginal
                             FROM [master].[dbo].[Cast] t2
	                            left join [master].[dbo].[Person] t3 on t3.Id = t2.PersonId
	                            left join [master].[dbo].[Character] t4 on t4.Id = t2.CharacterId
                                left join [master].[dbo].[Country] t5 on t5.id = t3.CountryId
                                left join [master].[dbo].[Images] t6 on t6.Id = t3.ImageId
                                left join [master].[dbo].[Images] t7 on t7.Id = t4.ImageId
                            WHERE t2.ShowId = '{ShowId}'
                        ";

                        comm.CommandText = sql;
                        comm.CommandType = CommandType.Text;
                        using (var reader = comm.ExecuteReader())
                        {
                            while (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    result.Add(new Cast
                                    {
                                        Person = new Person
                                        {
                                            Id = reader.GetInt64("PersonId"),
                                            Name = reader.GetString("PersonName"),
                                            Birthday = reader["Birthday"] as DateTime?,
                                            Country = new Country
                                            {
                                                Code = reader["CountryCode"] as string,
                                                Name = reader["CountryName"] as string,
                                                Timezone = reader["CountryTimezone"] as string
                                            },
                                            Gender = (Gender)Enum.Parse(typeof(Gender), reader.GetString("Gender")),
                                            Deadday = reader["Deadday"] as DateTime?,
                                            Updated = reader.GetInt64("PersonUpdated"),
                                            Url = reader.GetString("PersonUrl"),
                                            Image = new Image
                                            {
                                                Medium = reader["PersonImageMedium"] as string,
                                                Original = reader["PersonImageOriginal"] as string
                                            },
                                        },
                                        Character = new Character
                                        {
                                            Id = reader.GetInt64("CharacterId"),
                                            Name = reader.GetString("CharacterName"),
                                            Url = reader.GetString("CharacterUrl"),
                                            Image = new Image
                                            {
                                                Medium = reader["CharacterImageMedium"] as string,
                                                Original = reader["CharacterImageOriginal"] as string
                                            },
                                        }
                                    });
                                }

                                reader.NextResult();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }

                con.Close();
            }

            return result;
        }
    }
}
