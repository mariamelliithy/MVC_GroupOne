using Demo.BLL.Dtos.Employees;
using Demo.DAL.Entities.Employess;
using Demo.DAL.Presistance.Repostories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository) 
        {
            _employeeRepository = employeeRepository;
        }
        public int CreateEmployee(EmployeeToCreateDto EmployeeDto)
        {
            Employee employee = new Employee()
            {
                Name = EmployeeDto.Name,
                Age = EmployeeDto.Age,
                Address = EmployeeDto.Address,
                IsActive = EmployeeDto.IsActive,
                Salary = EmployeeDto.Salary,
                Email = EmployeeDto.Email,
                PhoneNumber = EmployeeDto.PhoneNumber,
                HiringDate = EmployeeDto.HiringDate,
                Gender  = EmployeeDto.Gender,
                EmpolyeeType = EmployeeDto.EmpolyeeType,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow
            };
            return _employeeRepository.AddT(employee);
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if (employee != null)
                return _employeeRepository.DeleteT(employee) > 0;
            return false;
        }

        public IEnumerable<EmployeeToReturnDto> GetAllEmployees()
        {
            return _employeeRepository.GetAllQueryable().Where(E => !E.IsDeleted).Select(employee => new EmployeeToReturnDto(){
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Email = employee.Email,
                Salary = employee.Salary,
                Gender = employee.Gender.ToString(),
                EmpolyeeType = employee.EmpolyeeType.ToString(),
                IsActive = employee.IsActive
            });
        }

        public EmployeeDetailsToReturnDto? GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetById(id);
            if(employee != null)
            {
                return new EmployeeDetailsToReturnDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Address = employee.Address,
                    HiringDate = employee.HiringDate,
                    PhoneNumber = employee.PhoneNumber,
                    Gender = employee.Gender.ToString(),
                    EmpolyeeType = employee.EmpolyeeType.ToString(),
                    IsActive = employee.IsActive,
                    CreatedBy = employee.CreatedBy,
                    CreatedOn = employee.CreatedOn,
                };
            }
             return null!;
        }

        public int UpdateEmployee(EmployeeToUpdateDto EmployeeDto)
        {
            Employee employee = new Employee() 
            {
                Id = EmployeeDto.Id,
                Name = EmployeeDto.Name,
                Age = EmployeeDto.Age,
                Address = EmployeeDto.Address,
                IsActive = EmployeeDto.IsActive,
                Salary = EmployeeDto.Salary,
                Email = EmployeeDto.Email,
                PhoneNumber = EmployeeDto.PhoneNumber,
                HiringDate = EmployeeDto.HiringDate,
                Gender = EmployeeDto.Gender,
                EmpolyeeType = EmployeeDto.EmpolyeeType,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow
            };
            return _employeeRepository.UpdateT(employee);
        }
    }
}
