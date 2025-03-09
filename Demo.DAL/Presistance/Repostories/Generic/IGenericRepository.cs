using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Repostories.Generic
{
    public interface IGenericRepository<T> where T : ModelBase
    {
        IEnumerable<T> GetAll(bool AsNoTracking = true);
        IQueryable<T> GetAllQueryable();
        T? GetById(int id);
        int AddT(T entity);
        int UpdateT(T entity);
        int DeleteT(T entity);
    }
}
