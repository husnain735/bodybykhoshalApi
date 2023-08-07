using AutoMapper;
using bodybykhoshalApi.Models.Entities;
using bodybykhoshalApi.Models.ViewModel;
using static bodybykhoshalApi.Models.ViewModel.HttpRequest;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Users, UserViewModel>();
        CreateMap<SaveChatRequestHandler,Chats>();
    }
}