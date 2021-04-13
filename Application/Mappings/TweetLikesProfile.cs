namespace Application.Mappings
{
    using Application.Dtos.TweetLikes;
    using AutoMapper;
    using Domain.Entities;

    public class TweetLikesProfile : Profile
    {
        public TweetLikesProfile()
        {
                     CreateMap<TweetLikes, TweetLikesDto>()
                .ReverseMap();
            CreateMap<TweetLikesForCreationDto, TweetLikes>();
            CreateMap<TweetLikesForUpdateDto, TweetLikes>()
                .ReverseMap();
        }
    }
}