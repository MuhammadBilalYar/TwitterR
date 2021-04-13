namespace Application.Interfaces.TweetType
{
    using System;
    using Application.Dtos.TweetType;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface ITweetTypeRepository
    {
        Task<PagedList<TweetType>> GetTweetTypesAsync(TweetTypeParametersDto TweetTypeParameters);
        Task<TweetType> GetTweetTypeAsync(int TweetTypeId);
        TweetType GetTweetType(int TweetTypeId);
        Task AddTweetType(TweetType tweetType);
        void DeleteTweetType(TweetType tweetType);
        void UpdateTweetType(TweetType tweetType);
        bool Save();
        Task<bool> SaveAsync();
    }
}