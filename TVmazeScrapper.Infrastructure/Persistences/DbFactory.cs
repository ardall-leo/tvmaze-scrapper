using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVmazeScrapper.Domain.Enums;
using TVmazeScrapper.Domain.Interfaces;
using TVmazeScrapper.Domain.Models.Configs;

namespace TVmazeScrapper.Infrastructure.Persistences
{
    public class DbFactory : IDbConnectionFactory
    {
        private readonly AppConfig _config;

        public DbFactory(AppConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// It returns new SQL connection, but under the hood ADO.NET will return the existing open connection
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public IDbConnection GetConnection(DatabaseType dbType)
        {
            return dbType switch
            {
                DatabaseType.TvMaze => new SqlConnection(_config.DbConnStr),
                _ => throw new NotSupportedException("Repository not supported")
            };
        }
    }
}
