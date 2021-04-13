namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.TweetRetweets;
    using Application.Interfaces.TweetRetweets;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class TweetRetweetsRepository : ITweetRetweetsRepository
    {
        private TwittRDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public TweetRetweetsRepository(TwittRDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<TweetRetweets>> GetTweetRetweetssAsync(TweetRetweetsParametersDto tweetRetweetsParameters)
        {
            if (tweetRetweetsParameters == null)
            {
                throw new ArgumentNullException(nameof(tweetRetweetsParameters));
            }

            var collection = _context.TweetRetweetss
                as IQueryable<TweetRetweets>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = tweetRetweetsParameters.SortOrder ?? "TweetId",
                Filters = tweetRetweetsParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<TweetRetweets>.CreateAsync(collection,
                tweetRetweetsParameters.PageNumber,
                tweetRetweetsParameters.PageSize);
        }

        public async Task<TweetRetweets> GetTweetRetweetsAsync(int tweetRetweetsId)
        {
                     return await _context.TweetRetweetss
                .FirstOrDefaultAsync(t => t.TweetId == tweetRetweetsId);
        }

        public TweetRetweets GetTweetRetweets(int tweetRetweetsId)
        {
                     return _context.TweetRetweetss
                .FirstOrDefault(t => t.TweetId == tweetRetweetsId);
        }

        public async Task AddTweetRetweets(TweetRetweets tweetRetweets)
        {
            if (tweetRetweets == null)
            {
                throw new ArgumentNullException(nameof(TweetRetweets));
            }

            await _context.TweetRetweetss.AddAsync(tweetRetweets);
        }

        public void DeleteTweetRetweets(TweetRetweets tweetRetweets)
        {
            if (tweetRetweets == null)
            {
                throw new ArgumentNullException(nameof(TweetRetweets));
            }

            _context.TweetRetweetss.Remove(tweetRetweets);
        }

        public void UpdateTweetRetweets(TweetRetweets tweetRetweets)
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