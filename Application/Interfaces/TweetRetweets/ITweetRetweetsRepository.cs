namespace Application.Interfaces.TweetRetweets
{
    using System;
    using Application.Dtos.TweetRetweets;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface ITweetRetweetsRepository
    {
        Task<PagedList<TweetRetweets>> GetTweetRetweetssAsync(TweetRetweetsParametersDto TweetRetweetsParameters);
        Task<TweetRetweets> GetTweetRetweetsAsync(int TweetRetweetsId);
        TweetRetweets GetTweetRetweets(int TweetRetweetsId);
        Task AddTweetRetweets(TweetRetweets tweetRetweets);
        void DeleteTweetRetweets(TweetRetweets tweetRetweets);
        void UpdateTweetRetweets(TweetRetweets tweetRetweets);
        bool Save();
        Task<bool> SaveAsync();
    }
}