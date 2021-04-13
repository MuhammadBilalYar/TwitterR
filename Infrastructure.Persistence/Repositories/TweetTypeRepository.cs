namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.TweetType;
    using Application.Interfaces.TweetType;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class TweetTypeRepository : ITweetTypeRepository
    {
        private TwittRDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public TweetTypeRepository(TwittRDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<TweetType>> GetTweetTypesAsync(TweetTypeParametersDto tweetTypeParameters)
        {
            if (tweetTypeParameters == null)
            {
                throw new ArgumentNullException(nameof(tweetTypeParameters));
            }

            var collection = _context.TweetTypes
                as IQueryable<TweetType>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = tweetTypeParameters.SortOrder ?? "TweetTypeId",
                Filters = tweetTypeParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<TweetType>.CreateAsync(collection,
                tweetTypeParameters.PageNumber,
                tweetTypeParameters.PageSize);
        }

        public async Task<TweetType> GetTweetTypeAsync(int tweetTypeId)
        {
                     return await _context.TweetTypes
                .FirstOrDefaultAsync(t => t.TweetTypeId == tweetTypeId);
        }

        public TweetType GetTweetType(int tweetTypeId)
        {
                     return _context.TweetTypes
                .FirstOrDefault(t => t.TweetTypeId == tweetTypeId);
        }

        public async Task AddTweetType(TweetType tweetType)
        {
            if (tweetType == null)
            {
                throw new ArgumentNullException(nameof(TweetType));
            }

            await _context.TweetTypes.AddAsync(tweetType);
        }

        public void DeleteTweetType(TweetType tweetType)
        {
            if (tweetType == null)
            {
                throw new ArgumentNullException(nameof(TweetType));
            }

            _context.TweetTypes.Remove(tweetType);
        }

        public void UpdateTweetType(TweetType tweetType)
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