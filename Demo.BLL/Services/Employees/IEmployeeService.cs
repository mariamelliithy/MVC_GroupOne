using Demo.BLL.Dtos.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Employees
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeToReturnDto> GetAllEmployees();
        EmployeeDetailsToReturnDto? GetEmployeeById(int id);
        int CreateEmployee(EmployeeToCreateDto Employee);
        int UpdateEmployee(EmployeeToUpdateDto Employee);
        bool DeleteEmployee(int id);
    }
}
