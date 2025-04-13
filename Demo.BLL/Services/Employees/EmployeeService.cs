using Demo.BLL.Common.Services.AttachmentService;
using Demo.BLL.Dtos.Employees;
using Demo.DAL.Entities.Employess;
using Demo.DAL.Presistance.Repostories.Employees;
using Demo.DAL.Presistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        //private readonly IEmployeeRepository _employeeRepository;
        //public EmployeeService(IEmployeeRepository employeeRepository) 
        //{
        //    _employeeRepository = employeeRepository;
        //}
        public EmployeeService(IUnitOfWork unitOfWork, IAttachmentService attachmentService) 
        {
           _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }
        public async Task<int> CreateEmployee(EmployeeToCreateDto EmployeeDto)
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
                LastModifiedOn = DateTime.UtcNow,
                DepartmentId = EmployeeDto.DepartmentId,
            };
            if(EmployeeDto.Image is not null ) 
                employee.Image = await _attachmentService.Upload(EmployeeDto.Image, "images" );
            _unitOfWork.EmployeeRepository.AddT(employee);
            return await _unitOfWork.Complete();
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            var employeeRepo = _unitOfWork.EmployeeRepository;
            var employee = await employeeRepo.GetById(id);
            if (employee != null)
                employeeRepo.DeleteT(employee);
            return await _unitOfWork.Complete() > 0;
        }

        public async Task<IEnumerable<EmployeeToReturnDto>> GetAllEmployees(string SearchValue)
        {
            return await _unitOfWork.EmployeeRepository.GetAllQueryable()
                .Include(E => E.Department)
                .Where(E => !E.IsDeleted && (string.IsNullOrEmpty(SearchValue) || E.Name.ToLower().Contains(SearchValue.ToLower())))
                .Select(employee => new EmployeeToReturnDto(){
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Gender = employee.Gender.ToString(),
                    EmpolyeeType = employee.EmpolyeeType.ToString(),
                    IsActive = employee.IsActive,
                    Image = employee.Image,
                    Department = employee.Department.Name,
                }).ToListAsync();
        }

        public async Task<EmployeeDetailsToReturnDto?> GetEmployeeById(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetById(id);
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
                    Image = employee.Image,
                    Department = employee.Department?.Name ?? "No department",
                };
            }
             return null!;
        }

        public async Task<int> UpdateEmployee(EmployeeToUpdateDto EmployeeDto)
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
                LastModifiedOn = DateTime.UtcNow,
                DepartmentId = EmployeeDto.DepartmentId,
            };
            if (EmployeeDto.Image is not null)
                employee.Image = await _attachmentService.Upload(EmployeeDto.Image, "images");
            _unitOfWork.EmployeeRepository.AddT(employee);
            _unitOfWork.EmployeeRepository.UpdateT(employee);
            return await _unitOfWork.Complete();
        }
    }
}
