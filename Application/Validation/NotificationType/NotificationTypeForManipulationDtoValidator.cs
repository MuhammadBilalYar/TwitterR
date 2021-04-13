namespace Application.Validation.NotificationType
{
    using Application.Dtos.NotificationType;
    using FluentValidation;
    using System;

    public class NotificationTypeForManipulationDtoValidator<T> : AbstractValidator<T> where T : NotificationTypeForManipulationDto
    {
        public NotificationTypeForManipulationDtoValidator()
        {
                          }
    }
}