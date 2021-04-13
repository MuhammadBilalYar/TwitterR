namespace Application.Validation.TwitterUser
{
    using Application.Dtos.TwitterUser;
    using FluentValidation;

    public class TwitterUserForUpdateDtoValidator: TwitterUserForManipulationDtoValidator<TwitterUserForUpdateDto>
    {
        public TwitterUserForUpdateDtoValidator()
        {

        }
    }
}