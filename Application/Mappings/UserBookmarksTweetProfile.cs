namespace Application.Mappings
{
    using Application.Dtos.UserBookmarksTweet;
    using AutoMapper;
    using Domain.Entities;

    public class UserBookmarksTweetProfile : Profile
    {
        public UserBookmarksTweetProfile()
        {
                     CreateMap<UserBookmarksTweet, UserBookmarksTweetDto>()
                .ReverseMap();
            CreateMap<UserBookmarksTweetForCreationDto, UserBookmarksTweet>();
            CreateMap<UserBookmarksTweetForUpdateDto, UserBookmarksTweet>()
                .ReverseMap();
        }
    }
}