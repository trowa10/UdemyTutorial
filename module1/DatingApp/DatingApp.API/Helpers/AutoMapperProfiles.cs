using System.Linq;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                //For Member to manaully map the custom field from your DTO
                .ForMember(dest => dest.PhotoUrl, 
                opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, 
                opt => {
                    opt.ResolveUsing(x=>x.DateOfBirth.CalculateAge());
                });

            CreateMap<User, UserForDetailedDto>()
            //For Member to manaully map the custom field from your DTO
                .ForMember(dest => dest.PhotoUrl, 
                opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, 
                opt => {
                    opt.ResolveUsing(x=>x.DateOfBirth.CalculateAge());
                });
                
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<UserForUpdateDTO, User>();
        }
    }
}