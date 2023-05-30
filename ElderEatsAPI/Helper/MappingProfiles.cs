using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Models;

namespace ElderEatsAPI.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Account, AccountDto>();
        CreateMap<AccountPostDto, Account>();
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<AccountUserDto, AccountUser>();
        CreateMap<ProductPostDto, Product>();
        CreateMap<AccountProductDto, AccountProduct>();
    }
}
