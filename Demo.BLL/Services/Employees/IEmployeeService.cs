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
        Task<IEnumerable<EmployeeToReturnDto>> GetAllEmployees(string SearchValue);
        Task<EmployeeDetailsToReturnDto?> GetEmployeeById(int id);
        Task<int> CreateEmployee(EmployeeToCreateDto Employee);
        Task<int> UpdateEmployee(EmployeeToUpdateDto Employee);
        Task<bool> DeleteEmployee(int id);
    }
}
