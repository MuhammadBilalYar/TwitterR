namespace Application.Mappings
{
    using Application.Dtos.TweetRetweets;
    using AutoMapper;
    using Domain.Entities;

    public class TweetRetweetsProfile : Profile
    {
        public TweetRetweetsProfile()
        {
                     CreateMap<TweetRetweets, TweetRetweetsDto>()
                .ReverseMap();
            CreateMap<TweetRetweetsForCreationDto, TweetRetweets>();
            CreateMap<TweetRetweetsForUpdateDto, TweetRetweets>()
                .ReverseMap();
        }
    }
}