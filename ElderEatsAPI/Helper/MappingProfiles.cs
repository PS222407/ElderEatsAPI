using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Models;
using ElderEatsAPI.ViewModels;

namespace ElderEatsAPI.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Account, AccountDto>();
        CreateMap<AccountPostDto, Account>();
        CreateMap<Product, ProductViewModel>();
        CreateMap<ProductViewModel, Product>();
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<UserRegistrationPostDto, User>();
        CreateMap<UserLoginPostDto, User>();
        CreateMap<User, UserRegistrationDto>();
        CreateMap<AccountUserDto, AccountUser>();
        CreateMap<ProductPostDto, Product>();
        CreateMap<AccountProductDto, AccountProduct>();
    }
}
