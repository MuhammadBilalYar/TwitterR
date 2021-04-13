namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.TwitterUserFollowsTwitterUser;
    using Application.Interfaces.TwitterUserFollowsTwitterUser;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class TwitterUserFollowsTwitterUserRepository : ITwitterUserFollowsTwitterUserRepository
    {
        private TwittRDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public TwitterUserFollowsTwitterUserRepository(TwittRDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<TwitterUserFollowsTwitterUser>> GetTwitterUserFollowsTwitterUsersAsync(TwitterUserFollowsTwitterUserParametersDto twitterUserFollowsTwitterUserParameters)
        {
            if (twitterUserFollowsTwitterUserParameters == null)
            {
                throw new ArgumentNullException(nameof(twitterUserFollowsTwitterUserParameters));
            }

            var collection = _context.TwitterUserFollowsTwitterUsers
                as IQueryable<TwitterUserFollowsTwitterUser>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = twitterUserFollowsTwitterUserParameters.SortOrder ?? "TwitterUserId",
                Filters = twitterUserFollowsTwitterUserParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<TwitterUserFollowsTwitterUser>.CreateAsync(collection,
                twitterUserFollowsTwitterUserParameters.PageNumber,
                twitterUserFollowsTwitterUserParameters.PageSize);
        }

        public async Task<TwitterUserFollowsTwitterUser> GetTwitterUserFollowsTwitterUserAsync(int twitterUserFollowsTwitterUserId)
        {
                     return await _context.TwitterUserFollowsTwitterUsers
                .FirstOrDefaultAsync(t => t.TwitterUserId == twitterUserFollowsTwitterUserId);
        }

        public TwitterUserFollowsTwitterUser GetTwitterUserFollowsTwitterUser(int twitterUserFollowsTwitterUserId)
        {
                     return _context.TwitterUserFollowsTwitterUsers
                .FirstOrDefault(t => t.TwitterUserId == twitterUserFollowsTwitterUserId);
        }

        public async Task AddTwitterUserFollowsTwitterUser(TwitterUserFollowsTwitterUser twitterUserFollowsTwitterUser)
        {
            if (twitterUserFollowsTwitterUser == null)
            {
                throw new ArgumentNullException(nameof(TwitterUserFollowsTwitterUser));
            }

            await _context.TwitterUserFollowsTwitterUsers.AddAsync(twitterUserFollowsTwitterUser);
        }

        public void DeleteTwitterUserFollowsTwitterUser(TwitterUserFollowsTwitterUser twitterUserFollowsTwitterUser)
        {
            if (twitterUserFollowsTwitterUser == null)
            {
                throw new ArgumentNullException(nameof(TwitterUserFollowsTwitterUser));
            }

            _context.TwitterUserFollowsTwitterUsers.Remove(twitterUserFollowsTwitterUser);
        }

        public void UpdateTwitterUserFollowsTwitterUser(TwitterUserFollowsTwitterUser twitterUserFollowsTwitterUser)
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