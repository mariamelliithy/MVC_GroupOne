using Demo.DAL.Entities.Departments;
using Demo.DAL.Presistance.Repostories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Presistance.Repostories.Departments
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
    }
}
