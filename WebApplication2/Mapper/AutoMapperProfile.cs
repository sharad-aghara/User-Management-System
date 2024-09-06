using AutoMapper;
using UMS.DAL.DTO;
using UMS.DAL.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Entity to DTO and vice versa
            CreateMap<District, DistrictDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<State, StateDTO>().ReverseMap();
            CreateMap<Taluka, TalukaDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();

            // Entity to ViewModel and vice versa
            CreateMap<District, DistrictViewModel>().ReverseMap();
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<State, StateViewModel>().ReverseMap();
            CreateMap<Taluka, TalukaViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
        }
    }
}
