namespace Application.Mappings
{
    using Application.Dtos.TweetType;
    using AutoMapper;
    using Domain.Entities;

    public class TweetTypeProfile : Profile
    {
        public TweetTypeProfile()
        {
                     CreateMap<TweetType, TweetTypeDto>()
                .ReverseMap();
            CreateMap<TweetTypeForCreationDto, TweetType>();
            CreateMap<TweetTypeForUpdateDto, TweetType>()
                .ReverseMap();
        }
    }
}