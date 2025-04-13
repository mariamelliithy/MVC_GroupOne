using Demo.BLL.Dtos.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Departments
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentToReturnDto>> GetAllDepartments();
        Task<DepartmentDetailsToReturnDto?> GetDepartmentById(int id);
        Task<int> CreateDepartment(DepartmentToCreateDto department);
        Task<int> UpdateDepartment(DepartmentToUpdateDto department);
        Task<bool> DeleteDepartment(int id);
    }
}
