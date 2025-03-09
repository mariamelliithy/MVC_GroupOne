using Demo.BLL.Dtos.Departments;
using Demo.DAL.Entities.Departments;
using Demo.DAL.Presistance.Repostories.Departments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository) 
        {
            _departmentRepository = departmentRepository;
        }
        public IEnumerable<DepartmentToReturnDto> GetAllDepartments()
        {
            var departments = _departmentRepository.GetAllQueryable().Where(D => !D.IsDeleted).Select(department => new DepartmentToReturnDto
            {
                //Description = department.Description,
                CreationDate = department.CreationDate,
                Code = department.Code,
                Id = department.Id,
                Name = department.Name
            }).AsNoTracking().ToList();
            return departments;
        }

        public DepartmentDetailsToReturnDto? GetDepartmentById(int id)
        {
            var department = _departmentRepository.GetById(id);
            if (department is not null)  //department != null , department is {}
            {
                return new DepartmentDetailsToReturnDto()
                {
                    Id = department.Id,
                    Name = department.Name,
                    Code = department.Code,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    CreationDate = department.CreationDate,
                    LastModifiedBy = department.LastModifiedBy,
                    LastModifiedOn = department.LastModifiedOn,
                    Description = department.Description,
                    IsDeleted = department.IsDeleted
                };
            }
            return null;
        }

        public int CreateDepartment(DepartmentToCreateDto department)
        {
            var depatmentCreated = new Department()
            {
                Code = department.Code,
                Description = department.Description,
                Name = department.Name,
                CreationDate = department.CreationDate,
                LastModifiedBy = 1,
                CreatedBy = 1,
                LastModifiedOn = DateTime.UtcNow
            };
            return _departmentRepository.AddDepartment(depatmentCreated);
        }

        public int UpdateDepartment(DepartmentToUpdateDto department)
        {
            var depatmentUpdated = new Department()
            {
                Id = department.Id,
                Code = department.Code,
                Description = department.Description,
                Name = department.Name,
                CreationDate = department.CreationDate,
                LastModifiedBy = 1,
                CreatedBy = 1,
                LastModifiedOn = DateTime.UtcNow
            };
            return _departmentRepository.UpdateDepartment(depatmentUpdated);
        }

        public bool DeleteDepartment(int id)
        {
            var department = _departmentRepository.GetById(id);
            if (department is not null) 
            {
                return _departmentRepository.DeleteDepartment(department) > 0;
            }
            return false;
        }
    }
}
