namespace Application.Interfaces.TweetReplies
{
    using System;
    using Application.Dtos.TweetReplies;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface ITweetRepliesRepository
    {
        Task<PagedList<TweetReplies>> GetTweetRepliessAsync(TweetRepliesParametersDto TweetRepliesParameters);
        Task<TweetReplies> GetTweetRepliesAsync(int TweetRepliesId);
        TweetReplies GetTweetReplies(int TweetRepliesId);
        Task AddTweetReplies(TweetReplies tweetReplies);
        void DeleteTweetReplies(TweetReplies tweetReplies);
        void UpdateTweetReplies(TweetReplies tweetReplies);
        bool Save();
        Task<bool> SaveAsync();
    }
}