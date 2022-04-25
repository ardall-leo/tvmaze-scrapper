using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVmazeScrapper.Domain.Models.Entities;

namespace TVmazeScrapper.Domain.Interfaces
{
    public interface IDbRepository<T> where T: Identity
    {
        IEnumerable<T> GetAll(int offset, int pageSize);

        T GetById(long? id);

        void Insert(T data);

        void Update(T data);

        long Merge(DataTable data);

        void Delete(long? id);
    }
}
