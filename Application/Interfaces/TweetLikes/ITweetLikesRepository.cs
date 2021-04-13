namespace Application.Interfaces.TweetLikes
{
    using System;
    using Application.Dtos.TweetLikes;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface ITweetLikesRepository
    {
        Task<PagedList<TweetLikes>> GetTweetLikessAsync(TweetLikesParametersDto TweetLikesParameters);
        Task<TweetLikes> GetTweetLikesAsync(int TweetLikesId);
        TweetLikes GetTweetLikes(int TweetLikesId);
        Task AddTweetLikes(TweetLikes tweetLikes);
        void DeleteTweetLikes(TweetLikes tweetLikes);
        void UpdateTweetLikes(TweetLikes tweetLikes);
        bool Save();
        Task<bool> SaveAsync();
    }
}