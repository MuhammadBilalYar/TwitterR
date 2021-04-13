namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.NotificationType;
    using Application.Interfaces.NotificationType;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class NotificationTypeRepository : INotificationTypeRepository
    {
        private TwittRDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public NotificationTypeRepository(TwittRDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<NotificationType>> GetNotificationTypesAsync(NotificationTypeParametersDto notificationTypeParameters)
        {
            if (notificationTypeParameters == null)
            {
                throw new ArgumentNullException(nameof(notificationTypeParameters));
            }

            var collection = _context.NotificationTypes
                as IQueryable<NotificationType>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = notificationTypeParameters.SortOrder ?? "NotificationTypeId",
                Filters = notificationTypeParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<NotificationType>.CreateAsync(collection,
                notificationTypeParameters.PageNumber,
                notificationTypeParameters.PageSize);
        }

        public async Task<NotificationType> GetNotificationTypeAsync(int notificationTypeId)
        {
                     return await _context.NotificationTypes
                .FirstOrDefaultAsync(n => n.NotificationTypeId == notificationTypeId);
        }

        public NotificationType GetNotificationType(int notificationTypeId)
        {
                     return _context.NotificationTypes
                .FirstOrDefault(n => n.NotificationTypeId == notificationTypeId);
        }

        public async Task AddNotificationType(NotificationType notificationType)
        {
            if (notificationType == null)
            {
                throw new ArgumentNullException(nameof(NotificationType));
            }

            await _context.NotificationTypes.AddAsync(notificationType);
        }

        public void DeleteNotificationType(NotificationType notificationType)
        {
            if (notificationType == null)
            {
                throw new ArgumentNullException(nameof(NotificationType));
            }

            _context.NotificationTypes.Remove(notificationType);
        }

        public void UpdateNotificationType(NotificationType notificationType)
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