namespace Application.Validation.Message
{
    using Application.Dtos.Message;
    using FluentValidation;

    public class MessageForUpdateDtoValidator: MessageForManipulationDtoValidator<MessageForUpdateDto>
    {
        public MessageForUpdateDtoValidator()
        {
                          }
    }
}