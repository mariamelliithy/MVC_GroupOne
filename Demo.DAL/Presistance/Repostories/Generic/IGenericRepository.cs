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
        Task<IEnumerable<T>> GetAll(bool AsNoTracking = true);
        IQueryable<T> GetAllQueryable();
        Task<T?> GetById(int id);
        void AddT(T entity);
        void UpdateT(T entity);
        void DeleteT(T entity);
    }
}
