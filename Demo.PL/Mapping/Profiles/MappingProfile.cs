using AutoMapper;
using Demo.BLL.Dtos.Departments;
using Demo.PL.ViewModels.Departments;

namespace Demo.PL.Mapping.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
        #region Employee module

        #endregion

        #region Department module
            CreateMap<DepartmentViewModel, DepartmentToCreateDto>();
            CreateMap<DepartmentDetailsToReturnDto, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, DepartmentToUpdateDto>();
        #endregion
        }
    }
}
