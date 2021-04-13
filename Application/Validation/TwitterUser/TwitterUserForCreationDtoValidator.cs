namespace Application.Validation.TwitterUser
{
    using Application.Dtos.TwitterUser;
    using FluentValidation;

    public class TwitterUserForCreationDtoValidator: TwitterUserForManipulationDtoValidator<TwitterUserForCreationDto>
    {
        public TwitterUserForCreationDtoValidator()
        {

        }
    }
}