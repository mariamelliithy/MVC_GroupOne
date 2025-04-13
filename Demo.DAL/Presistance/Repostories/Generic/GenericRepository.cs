using Demo.DAL.Entities;
using Demo.DAL.Presistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Repostories.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<T>> GetAll(bool AsNoTracking = true)
        {
            if (AsNoTracking)
            {
                return await _dbContext.Set<T>().Where(X => !X.IsDeleted).AsNoTracking().ToListAsync();//ditached
            }
            else
            {
                return await _dbContext.Set<T>().Where(X => !X.IsDeleted).ToListAsync(); //unchanged
            }
        }
        public async Task<T?> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id); //Search locally, in case found ==> return , else ==> send request to database
        }
        public void AddT(T entity)
        {
            _dbContext.Set<T>().Add(entity); //saved locally
        }
        public void UpdateT(T entity)
        {
            _dbContext.Set<T>().Update(entity); //Modifed
        }
        public void DeleteT(T entity)
        {
            //_dbContext.Set<T>().Remove(entity);
            //return _dbContext.SaveChanges();
            entity.IsDeleted = true;
            _dbContext.Set<T>().Update(entity);
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _dbContext.Set<T>();
        }
    }
}
