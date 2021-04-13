namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.UserBookmarksTweet;
    using Application.Interfaces.UserBookmarksTweet;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class UserBookmarksTweetRepository : IUserBookmarksTweetRepository
    {
        private TwittRDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public UserBookmarksTweetRepository(TwittRDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<UserBookmarksTweet>> GetUserBookmarksTweetsAsync(UserBookmarksTweetParametersDto userBookmarksTweetParameters)
        {
            if (userBookmarksTweetParameters == null)
            {
                throw new ArgumentNullException(nameof(userBookmarksTweetParameters));
            }

            var collection = _context.UserBookmarksTweets
                as IQueryable<UserBookmarksTweet>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = userBookmarksTweetParameters.SortOrder ?? "TwitterUserId",
                Filters = userBookmarksTweetParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<UserBookmarksTweet>.CreateAsync(collection,
                userBookmarksTweetParameters.PageNumber,
                userBookmarksTweetParameters.PageSize);
        }

        public async Task<UserBookmarksTweet> GetUserBookmarksTweetAsync(int userBookmarksTweetId)
        {
                     return await _context.UserBookmarksTweets
                .FirstOrDefaultAsync(u => u.TwitterUserId == userBookmarksTweetId);
        }

        public UserBookmarksTweet GetUserBookmarksTweet(int userBookmarksTweetId)
        {
                     return _context.UserBookmarksTweets
                .FirstOrDefault(u => u.TwitterUserId == userBookmarksTweetId);
        }

        public async Task AddUserBookmarksTweet(UserBookmarksTweet userBookmarksTweet)
        {
            if (userBookmarksTweet == null)
            {
                throw new ArgumentNullException(nameof(UserBookmarksTweet));
            }

            await _context.UserBookmarksTweets.AddAsync(userBookmarksTweet);
        }

        public void DeleteUserBookmarksTweet(UserBookmarksTweet userBookmarksTweet)
        {
            if (userBookmarksTweet == null)
            {
                throw new ArgumentNullException(nameof(UserBookmarksTweet));
            }

            _context.UserBookmarksTweets.Remove(userBookmarksTweet);
        }

        public void UpdateUserBookmarksTweet(UserBookmarksTweet userBookmarksTweet)
        {
                 }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}