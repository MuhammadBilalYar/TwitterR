namespace Application.Interfaces.TwitterUserFollowsTwitterUser
{
    using System;
    using Application.Dtos.TwitterUserFollowsTwitterUser;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface ITwitterUserFollowsTwitterUserRepository
    {
        Task<PagedList<TwitterUserFollowsTwitterUser>> GetTwitterUserFollowsTwitterUsersAsync(TwitterUserFollowsTwitterUserParametersDto TwitterUserFollowsTwitterUserParameters);
        Task<TwitterUserFollowsTwitterUser> GetTwitterUserFollowsTwitterUserAsync(int TwitterUserFollowsTwitterUserId);
        TwitterUserFollowsTwitterUser GetTwitterUserFollowsTwitterUser(int TwitterUserFollowsTwitterUserId);
        Task AddTwitterUserFollowsTwitterUser(TwitterUserFollowsTwitterUser twitterUserFollowsTwitterUser);
        void DeleteTwitterUserFollowsTwitterUser(TwitterUserFollowsTwitterUser twitterUserFollowsTwitterUser);
        void UpdateTwitterUserFollowsTwitterUser(TwitterUserFollowsTwitterUser twitterUserFollowsTwitterUser);
        bool Save();
        Task<bool> SaveAsync();
    }
}