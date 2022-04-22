using System;
using System.Data;
using System.Data.SqlClient;
using TVmazeScrapper.Domain.Enums;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class CountryRepository : SqlRepository<Country>
    {
        protected override IDbConnectionFactory DbFactory { get; }

        public CountryRepository(IDbConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        protected override string MergeInsertData => @"
            INSERT (Name, Code, Timezone)
            VALUES (Source.Name, Source.Code, Source.Timezone)
        ";

        protected override string MergeUpdateData => "UPDATE SET Target.Name = Source.Name, Target.Code = Source.Code, Target.Timezone = Source.Timezone";

        protected override string TableName => "dbo.Country";

        protected override string TempTable => @"
            CREATE TABLE #tmpTable(
            Id int,
            Name varchar(255),
            Code varchar(255),
            Timezone varchar(255))
        ";

        public Country FindByCountryCode(string countryCode)
        {
            string sql = $@"SELECT TOP 1 * FROM {TableName} WHERE Code = '{countryCode}'";
            Country result = null;

            using (var con = DbFactory.GetConnection(DatabaseType.TvMaze))
            {
                con.Open();

                using (var comm = new SqlCommand())
                {
                    comm.Connection = (SqlConnection)con;
                    comm.CommandText = sql;

                    using (var reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = new Country
                            {
                                Code = reader["Code"] as string,
                                Name = reader["Name"] as string,
                                Id = reader.GetInt64("Id"),
                                Timezone = reader["Timezone"] as string
                            };
                        }
                    }
                }
            }

            return result;
        }
    }
}
