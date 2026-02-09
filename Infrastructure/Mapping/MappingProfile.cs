using AutoMapper;
using MyPortfolio.DTOs;
using MyPortfolio.Models;

namespace MyPortfolio.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Photo mappings
            CreateMap<Photo, PhotoResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore()); // Set separately in handler

            // Category mappings
            CreateMap<Category, CategoryResponseDto>();
        }
    }
}
