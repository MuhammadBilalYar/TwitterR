namespace Application.Mappings
{
    using Application.Dtos.NotificationType;
    using AutoMapper;
    using Domain.Entities;

    public class NotificationTypeProfile : Profile
    {
        public NotificationTypeProfile()
        {
                     CreateMap<NotificationType, NotificationTypeDto>()
                .ReverseMap();
            CreateMap<NotificationTypeForCreationDto, NotificationType>();
            CreateMap<NotificationTypeForUpdateDto, NotificationType>()
                .ReverseMap();
        }
    }
}