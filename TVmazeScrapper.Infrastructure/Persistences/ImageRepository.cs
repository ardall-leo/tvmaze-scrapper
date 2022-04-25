using System;
using System.Data;
using System.Data.SqlClient;
using TVmazeScrapper.Domain.Enums;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class ImageRepository : SqlRepository<Image>
    {
        protected override IDbConnectionFactory DbFactory { get; }

        public ImageRepository(IDbConnectionFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        protected override string MergeInsertData => @"
            INSERT (Medium, Original, Type, OwnerId)
            VALUES (Source.Medium, Source.Original, Source.Type, Source.OwnerId)
        ";

        protected override string MergeUpdateData => "UPDATE SET Target.Medium = Source.Medium, Target.Original = Source.Original, Target.Type = Source.Type";

        protected override string TableName => "dbo.Images";

        protected override string TempTable => @"
            CREATE TABLE #tmpTable(
            Id bigint,
            Medium varchar(255),
            Original varchar(255),
            Type varchar(255),
            OwnerId bigint)
        ";

        public Image FindImageById(long? Id, ImageType type)
        {
            string sql = $@"SELECT TOP 1 * FROM {TableName} WHERE Type = '{type}' AND OwnerId = '{Id}'";
            Image result = null;

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
                            result = new Image
                            {
                                Medium = reader["Medium"] as string,
                                Original = reader["Original"] as string,
                                Id = reader.GetInt64("Id")
                            };
                        }
                    }
                }
            }

            return result;
        }
    }
}
