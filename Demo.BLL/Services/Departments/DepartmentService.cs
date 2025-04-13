using Demo.BLL.Dtos.Departments;
using Demo.DAL.Entities.Departments;
using Demo.DAL.Presistance.Repostories.Departments;
using Demo.DAL.Presistance.UnitOfWork;
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
        //private readonly IDepartmentRepository _departmentRepository;
        //public DepartmentService(IDepartmentRepository departmentRepository) 
        //{
        //    _departmentRepository = departmentRepository;
        //}

        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DepartmentToReturnDto>> GetAllDepartments()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllQueryable().Where(D => !D.IsDeleted).Select(department => new DepartmentToReturnDto
            {
                //Description = department.Description,
                CreationDate = department.CreationDate,
                Code = department.Code,
                Id = department.Id,
                Name = department.Name
            }).AsNoTracking().ToListAsync();
            return departments;
        }

        public async Task<DepartmentDetailsToReturnDto?> GetDepartmentById(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetById(id);
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

        public async Task<int> CreateDepartment(DepartmentToCreateDto department)
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
            _unitOfWork.DepartmentRepository.AddT(depatmentCreated);
            return await _unitOfWork.Complete();
        }

        public async Task<int> UpdateDepartment(DepartmentToUpdateDto department)
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
            _unitOfWork.DepartmentRepository.UpdateT(depatmentUpdated);
            return await _unitOfWork.Complete();
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var departmentRepo = _unitOfWork.DepartmentRepository;
            var department = await departmentRepo.GetById(id);
            if (department is not null) 
                departmentRepo.DeleteT(department);
            return await _unitOfWork.Complete() > 0;
        }
    }
}
