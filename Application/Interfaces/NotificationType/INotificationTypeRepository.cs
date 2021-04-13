namespace Application.Interfaces.NotificationType
{
    using System;
    using Application.Dtos.NotificationType;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface INotificationTypeRepository
    {
        Task<PagedList<NotificationType>> GetNotificationTypesAsync(NotificationTypeParametersDto NotificationTypeParameters);
        Task<NotificationType> GetNotificationTypeAsync(int NotificationTypeId);
        NotificationType GetNotificationType(int NotificationTypeId);
        Task AddNotificationType(NotificationType notificationType);
        void DeleteNotificationType(NotificationType notificationType);
        void UpdateNotificationType(NotificationType notificationType);
        bool Save();
        Task<bool> SaveAsync();
    }
}