using AutoMapper;
using TMS.Domain.Models;
using TMS.Domain.Entities;

namespace src.TMS.API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SaveTestRequest, Test>();
            CreateMap<SaveTestDetailRequest, TestDetail>();
            CreateMap<RegisterRequest, User>();
            CreateMap<User, AuthenticateResponse>();
        }
    }
}