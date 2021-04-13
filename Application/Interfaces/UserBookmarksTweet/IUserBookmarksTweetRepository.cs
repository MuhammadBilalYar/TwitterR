namespace Application.Interfaces.UserBookmarksTweet
{
    using System;
    using Application.Dtos.UserBookmarksTweet;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IUserBookmarksTweetRepository
    {
        Task<PagedList<UserBookmarksTweet>> GetUserBookmarksTweetsAsync(UserBookmarksTweetParametersDto UserBookmarksTweetParameters);
        Task<UserBookmarksTweet> GetUserBookmarksTweetAsync(int UserBookmarksTweetId);
        UserBookmarksTweet GetUserBookmarksTweet(int UserBookmarksTweetId);
        Task AddUserBookmarksTweet(UserBookmarksTweet userBookmarksTweet);
        void DeleteUserBookmarksTweet(UserBookmarksTweet userBookmarksTweet);
        void UpdateUserBookmarksTweet(UserBookmarksTweet userBookmarksTweet);
        bool Save();
        Task<bool> SaveAsync();
    }
}