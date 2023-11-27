
using AutoMapper;
using MyAPI.DTOs.User;
using MyAPI.Models;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<UserDTO, UserModel>();
        CreateMap<UserModel, UserDTO>();
    }
}