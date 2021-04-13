namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.TweetLikes;
    using Application.Interfaces.TweetLikes;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class TweetLikesRepository : ITweetLikesRepository
    {
        private TwittRDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public TweetLikesRepository(TwittRDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<TweetLikes>> GetTweetLikessAsync(TweetLikesParametersDto tweetLikesParameters)
        {
            if (tweetLikesParameters == null)
            {
                throw new ArgumentNullException(nameof(tweetLikesParameters));
            }

            var collection = _context.TweetLikess
                as IQueryable<TweetLikes>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = tweetLikesParameters.SortOrder ?? "TweetId",
                Filters = tweetLikesParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<TweetLikes>.CreateAsync(collection,
                tweetLikesParameters.PageNumber,
                tweetLikesParameters.PageSize);
        }

        public async Task<TweetLikes> GetTweetLikesAsync(int tweetLikesId)
        {
                     return await _context.TweetLikess
                .FirstOrDefaultAsync(t => t.TweetId == tweetLikesId);
        }

        public TweetLikes GetTweetLikes(int tweetLikesId)
        {
                     return _context.TweetLikess
                .FirstOrDefault(t => t.TweetId == tweetLikesId);
        }

        public async Task AddTweetLikes(TweetLikes tweetLikes)
        {
            if (tweetLikes == null)
            {
                throw new ArgumentNullException(nameof(TweetLikes));
            }

            await _context.TweetLikess.AddAsync(tweetLikes);
        }

        public void DeleteTweetLikes(TweetLikes tweetLikes)
        {
            if (tweetLikes == null)
            {
                throw new ArgumentNullException(nameof(TweetLikes));
            }

            _context.TweetLikess.Remove(tweetLikes);
        }

        public void UpdateTweetLikes(TweetLikes tweetLikes)
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