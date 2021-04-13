namespace Application.Validation.NotificationType
{
    using Application.Dtos.NotificationType;
    using FluentValidation;

    public class NotificationTypeForCreationDtoValidator: NotificationTypeForManipulationDtoValidator<NotificationTypeForCreationDto>
    {
        public NotificationTypeForCreationDtoValidator()
        {
                          }
    }
}