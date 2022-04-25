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
    public abstract class SqlRepository<T> : IDbRepository<T> where T : Identity
    {
        protected abstract IDbConnectionFactory DbFactory { get; }

        protected abstract string MergeInsertData { get; }

        protected abstract string MergeUpdateData { get; }

        protected abstract string TableName { get; }

        protected abstract string TempTable { get; }

        public void Delete(long? id)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> GetAll(int offset, int pageSize)
        {
            throw new NotImplementedException();
        }

        public virtual T GetById(long? id)
        {
            throw new NotImplementedException();
        }

        public void Insert(T data)
        {
            throw new NotImplementedException();
        }

        public long Merge(DataTable data)
        {
            long id = 0;

            string sql = @$"
                MERGE INTO {TableName} AS Target
                USING dbo.#tmpTable As Source
                ON Source.Id = Target.Id
                WHEN NOT MATCHED BY Target THEN
                    {MergeInsertData}
                WHEN MATCHED THEN
                    {MergeUpdateData}
                OUTPUT
                    inserted.Id;
            ";

            using (var con = DbFactory.GetConnection(DatabaseType.TvMaze))
            {
                con.Open();

                using (var comm = con.CreateCommand())
                {
                    try
                    {
                        comm.CommandText = TempTable;

                        comm.ExecuteNonQuery();

                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)con))
                        {
                            bulkCopy.BulkCopyTimeout = 660;
                            bulkCopy.DestinationTableName = "#tmpTable";
                            bulkCopy.WriteToServer(data);
                            bulkCopy.Close();
                        }

                        comm.CommandText = sql;
                        comm.CommandType = CommandType.Text;
                        id = (long)comm.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                con.Close();
            }

            return id;
        }

        public void Update(T data)
        {
            throw new NotImplementedException();
        }
    }
}
