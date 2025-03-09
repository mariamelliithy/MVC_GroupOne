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
        public IEnumerable<T> GetAll(bool AsNoTracking = true)
        {
            if (AsNoTracking)
            {
                return _dbContext.Set<T>().Where(X => !X.IsDeleted).AsNoTracking().ToList();//ditached
            }
            else
            {
                return _dbContext.Set<T>().Where(X => !X.IsDeleted).ToList(); //unchanged
            }
        }
        public T? GetById(int id)
        {
            return _dbContext.Set<T>().Find(id); //Search locally, in case found ==> return , else ==> send request to database
        }
        public int AddT(T entity)
        {
            _dbContext.Set<T>().Add(entity); //saved locally
            return _dbContext.SaveChanges(); //apply remotly
        }
        public int UpdateT(T entity)
        {
            _dbContext.Set<T>().Update(entity); //Modifed
            return _dbContext.SaveChanges(); //Unchanged
        }
        public int DeleteT(T entity)
        {
            //_dbContext.Set<T>().Remove(entity);
            //return _dbContext.SaveChanges();
            entity.IsDeleted = true;
            _dbContext.Set<T>().Update(entity);
            return _dbContext.SaveChanges();
        }

        public IQueryable<T> GetAllQueryable()
        {
            return _dbContext.Set<T>();
        }
    }
}
