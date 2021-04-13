namespace Application.Mappings
{
    using Application.Dtos.TweetReplies;
    using AutoMapper;
    using Domain.Entities;

    public class TweetRepliesProfile : Profile
    {
        public TweetRepliesProfile()
        {
                     CreateMap<TweetReplies, TweetRepliesDto>()
                .ReverseMap();
            CreateMap<TweetRepliesForCreationDto, TweetReplies>();
            CreateMap<TweetRepliesForUpdateDto, TweetReplies>()
                .ReverseMap();
        }
    }
}