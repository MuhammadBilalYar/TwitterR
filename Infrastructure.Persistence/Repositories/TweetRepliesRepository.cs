namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.TweetReplies;
    using Application.Interfaces.TweetReplies;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class TweetRepliesRepository : ITweetRepliesRepository
    {
        private TwittRDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public TweetRepliesRepository(TwittRDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<TweetReplies>> GetTweetRepliessAsync(TweetRepliesParametersDto tweetRepliesParameters)
        {
            if (tweetRepliesParameters == null)
            {
                throw new ArgumentNullException(nameof(tweetRepliesParameters));
            }

            var collection = _context.TweetRepliess
                as IQueryable<TweetReplies>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = tweetRepliesParameters.SortOrder ?? "TweetId",
                Filters = tweetRepliesParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<TweetReplies>.CreateAsync(collection,
                tweetRepliesParameters.PageNumber,
                tweetRepliesParameters.PageSize);
        }

        public async Task<TweetReplies> GetTweetRepliesAsync(int tweetRepliesId)
        {
                     return await _context.TweetRepliess
                .FirstOrDefaultAsync(t => t.TweetId == tweetRepliesId);
        }

        public TweetReplies GetTweetReplies(int tweetRepliesId)
        {
                     return _context.TweetRepliess
                .FirstOrDefault(t => t.TweetId == tweetRepliesId);
        }

        public async Task AddTweetReplies(TweetReplies tweetReplies)
        {
            if (tweetReplies == null)
            {
                throw new ArgumentNullException(nameof(TweetReplies));
            }

            await _context.TweetRepliess.AddAsync(tweetReplies);
        }

        public void DeleteTweetReplies(TweetReplies tweetReplies)
        {
            if (tweetReplies == null)
            {
                throw new ArgumentNullException(nameof(TweetReplies));
            }

            _context.TweetRepliess.Remove(tweetReplies);
        }

        public void UpdateTweetReplies(TweetReplies tweetReplies)
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