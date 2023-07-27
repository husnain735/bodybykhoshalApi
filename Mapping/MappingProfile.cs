using AutoMapper;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Models.ViewModel;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Users, UserViewModel>();
    }
}