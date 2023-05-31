using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Models;
using ElderEatsAPI.ViewModels;

namespace ElderEatsAPI.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Account, AccountViewModel>();
        CreateMap<AccountUser, AccountUserViewModel>();
        CreateMap<AccountProduct, AccountProductViewModel>();
        CreateMap<Product, ProductViewModel>();
        CreateMap<User, UserViewModel>();
        
        CreateMap<AccountPostDto, Account>();
        CreateMap<ProductViewModel, Product>();
        CreateMap<UserRegistrationPostDto, User>();
        CreateMap<UserLoginPostDto, User>();
        CreateMap<User, UserRegistrationDto>();
        CreateMap<AccountUserDto, AccountUser>();
        CreateMap<ProductPostDto, Product>();
        CreateMap<AccountProductDto, AccountProduct>();
    }
}
