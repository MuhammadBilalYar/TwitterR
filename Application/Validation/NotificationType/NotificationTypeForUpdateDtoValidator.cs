namespace Application.Validation.NotificationType
{
    using Application.Dtos.NotificationType;
    using FluentValidation;

    public class NotificationTypeForUpdateDtoValidator: NotificationTypeForManipulationDtoValidator<NotificationTypeForUpdateDto>
    {
        public NotificationTypeForUpdateDtoValidator()
        {
                          }
    }
}