namespace Application.Validation.Message
{
    using Application.Dtos.Message;
    using FluentValidation;
    using System;

    public class MessageForManipulationDtoValidator<T> : AbstractValidator<T> where T : MessageForManipulationDto
    {
        public MessageForManipulationDtoValidator()
        {
                          }
    }
}