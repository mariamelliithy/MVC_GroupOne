using Demo.DAL.Entities.Employess;
using Demo.DAL.Presistance.Repostories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Repostories.Employees
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
    }
}
