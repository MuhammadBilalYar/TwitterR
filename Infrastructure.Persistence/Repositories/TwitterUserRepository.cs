namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.TwitterUser;
    using Application.Interfaces.TwitterUser;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class TwitterUserRepository : ITwitterUserRepository
    {
        private TwittRDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public TwitterUserRepository(TwittRDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<TwitterUser>> GetTwitterUsersAsync(TwitterUserParametersDto twitterUserParameters)
        {
            if (twitterUserParameters == null)
            {
                throw new ArgumentNullException(nameof(twitterUserParameters));
            }

            var collection = _context.TwitterUsers
                as IQueryable<TwitterUser>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = twitterUserParameters.SortOrder ?? "TwitterUserId",
                Filters = twitterUserParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<TwitterUser>.CreateAsync(collection,
                twitterUserParameters.PageNumber,
                twitterUserParameters.PageSize);
        }

        public async Task<TwitterUser> GetTwitterUserAsync(int twitterUserId)
        {
                     return await _context.TwitterUsers
                .FirstOrDefaultAsync(t => t.TwitterUserId == twitterUserId);
        }

        public TwitterUser GetTwitterUser(int twitterUserId)
        {
                     return _context.TwitterUsers
                .FirstOrDefault(t => t.TwitterUserId == twitterUserId);
        }

        public async Task AddTwitterUser(TwitterUser twitterUser)
        {
            if (twitterUser == null)
            {
                throw new ArgumentNullException(nameof(TwitterUser));
            }

            await _context.TwitterUsers.AddAsync(twitterUser);
        }

        public void DeleteTwitterUser(TwitterUser twitterUser)
        {
            if (twitterUser == null)
            {
                throw new ArgumentNullException(nameof(TwitterUser));
            }

            _context.TwitterUsers.Remove(twitterUser);
        }

        public void UpdateTwitterUser(TwitterUser twitterUser)
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