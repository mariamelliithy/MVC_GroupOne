using Demo.DAL.Presistance.Data;
using Demo.DAL.Presistance.Repostories.Departments;
using Demo.DAL.Presistance.Repostories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
            //EmployeeRepository = new EmployeeRepository(_dbContext);
            //DepartmentRepository = new DepartmentRepository(_dbContext);
        }
        public IEmployeeRepository EmployeeRepository => new EmployeeRepository(_dbContext);
        public IDepartmentRepository DepartmentRepository => new DepartmentRepository(_dbContext);

        public async Task<int> Complete()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
