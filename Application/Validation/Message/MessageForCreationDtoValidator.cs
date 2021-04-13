namespace Application.Validation.Message
{
    using Application.Dtos.Message;
    using FluentValidation;

    public class MessageForCreationDtoValidator: MessageForManipulationDtoValidator<MessageForCreationDto>
    {
        public MessageForCreationDtoValidator()
        {
                          }
    }
}