using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Models;
using ElderEatsAPI.Requests;
using ElderEatsAPI.ViewModels;

namespace ElderEatsAPI.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Account, AccountViewModel>();
        CreateMap<AccountUser, AccountUserViewModel>();
        CreateMap<AccountProduct, AccountProductViewModel>();
        CreateMap<AccountProductDto, AccountProduct>();
        CreateMap<Product, ProductViewModel>();
        CreateMap<User, UserViewModel>();
        CreateMap<ProductGroupedDto, ProductGroupedViewModel>();
        CreateMap<AccountStoreRequest, Account>();
        CreateMap<UserRegistrationRequest, User>();
        CreateMap<UserLoginRequest, User>();
        CreateMap<User, UserRegistrationViewModel>();
        CreateMap<AccountUserUpdateRequest, AccountUser>();
        CreateMap<ProductStoreRequest, Product>();
        CreateMap<ProductImageRequest,Product>();
    }
}
