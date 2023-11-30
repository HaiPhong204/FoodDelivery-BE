
using AutoMapper;
using MyAPI.DTOs.Cart;
using MyAPI.DTOs.Food;
using MyAPI.DTOs.User;
using MyAPI.Models;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<UserDTO, UserModel>();
        CreateMap<UserModel, UserDTO>();
        CreateMap<RegisterUserDTO, UserModel>();
        CreateMap<UserModel, RegisterUserDTO>();

        CreateMap<FoodModel, FoodDTO>();
        CreateMap<FoodDTO, FoodModel>();
        CreateMap<CartModel, CartDTO>();
        CreateMap<CartDTO, CartModel>();

        //        CreateMap<User, UserDto>()
        //.ForMember(u => u.Languages, opt => opt.MapFrom(uSrc => JsonConvert.DeserializeObject<List<Language>>(uSrc.Languages)))
        //.ForMember(u => u.Skills, opt => opt.MapFrom(uSrc => JsonConvert.DeserializeObject<List<string>>(uSrc.Skills)));

        //        CreateMap<UpdateUserDto, User>()
        //        .ForMember(u => u.Languages, opt => opt.MapFrom((uSrc, des) => uSrc.Languages != null ? JsonConvert.SerializeObject(uSrc.Languages) : des.Languages))
        //        .ForMember(u => u.Skills, opt => opt.MapFrom((uSrc, des) => uSrc.Skills != null ? JsonConvert.SerializeObject(uSrc.Skills) : des.Skills))
        //        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}