using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User,UserForDetailDto>()
                .ForMember( des => des.PhotoUrl, opt => {
                    opt.MapFrom( src => src.Photos.FirstOrDefault( p => p.IsMain).Url);
                })
                .ForMember( des => des.Age, opt => {
                    opt.ResolveUsing( d => d.DateOfBirth.AgeCalculate());
                });
                
            CreateMap<User,UserForListDto>()
                .ForMember( des => des.PhotoUrl, opt => {
                    opt.MapFrom( src => src.Photos.FirstOrDefault( p => p.IsMain).Url);
                })
                .ForMember( des => des.Age, opt => {
                    opt.ResolveUsing( d => d.DateOfBirth.AgeCalculate());
                });
            CreateMap<Photo,PhotosForDetailDto>();
            CreateMap<UserForUpdateDto,User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
        }
    }
}