namespace Application.Mappings
{
    using Application.Dtos.TwitterUserFollowsTwitterUser;
    using AutoMapper;
    using Domain.Entities;

    public class TwitterUserFollowsTwitterUserProfile : Profile
    {
        public TwitterUserFollowsTwitterUserProfile()
        {
                     CreateMap<TwitterUserFollowsTwitterUser, TwitterUserFollowsTwitterUserDto>()
                .ReverseMap();
            CreateMap<TwitterUserFollowsTwitterUserForCreationDto, TwitterUserFollowsTwitterUser>();
            CreateMap<TwitterUserFollowsTwitterUserForUpdateDto, TwitterUserFollowsTwitterUser>()
                .ReverseMap();
        }
    }
}