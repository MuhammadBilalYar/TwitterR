namespace Application.Mappings
{
    using Application.Dtos.TwitterUser;
    using AutoMapper;
    using Domain.Entities;

    public class TwitterUserProfile : Profile
    {
        public TwitterUserProfile()
        {
                     CreateMap<TwitterUser, TwitterUserDto>()
                .ReverseMap();
            CreateMap<TwitterUserForCreationDto, TwitterUser>();
            CreateMap<TwitterUserForUpdateDto, TwitterUser>()
                .ReverseMap();
        }
    }
}