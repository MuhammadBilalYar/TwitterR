namespace Application.Interfaces.TwitterUser
{
    using System;
    using Application.Dtos.TwitterUser;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface ITwitterUserRepository
    {
        Task<PagedList<TwitterUser>> GetTwitterUsersAsync(TwitterUserParametersDto TwitterUserParameters);
        Task<TwitterUser> GetTwitterUserAsync(int TwitterUserId);
        TwitterUser GetTwitterUser(int TwitterUserId);
        Task AddTwitterUser(TwitterUser twitterUser);
        void DeleteTwitterUser(TwitterUser twitterUser);
        void UpdateTwitterUser(TwitterUser twitterUser);
        bool Save();
        Task<bool> SaveAsync();
    }
}