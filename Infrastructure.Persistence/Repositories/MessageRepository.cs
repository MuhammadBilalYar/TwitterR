namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.Message;
    using Application.Interfaces.Message;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class MessageRepository : IMessageRepository
    {
        private TwittRDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public MessageRepository(TwittRDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Message>> GetMessagesAsync(MessageParametersDto messageParameters)
        {
            if (messageParameters == null)
            {
                throw new ArgumentNullException(nameof(messageParameters));
            }

            var collection = _context.Messages
                as IQueryable<Message>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = messageParameters.SortOrder ?? "MessageId",
                Filters = messageParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Message>.CreateAsync(collection,
                messageParameters.PageNumber,
                messageParameters.PageSize);
        }

        public async Task<Message> GetMessageAsync(int messageId)
        {
                     return await _context.Messages
                .FirstOrDefaultAsync(m => m.MessageId == messageId);
        }

        public Message GetMessage(int messageId)
        {
                     return _context.Messages
                .FirstOrDefault(m => m.MessageId == messageId);
        }

        public async Task AddMessage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(Message));
            }

            await _context.Messages.AddAsync(message);
        }

        public void DeleteMessage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(Message));
            }

            _context.Messages.Remove(message);
        }

        public void UpdateMessage(Message message)
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