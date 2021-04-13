namespace Application.Validation.TwitterUserFollowsTwitterUser
{
    using Application.Dtos.TwitterUserFollowsTwitterUser;
    using FluentValidation;

    public class TwitterUserFollowsTwitterUserForCreationDtoValidator: TwitterUserFollowsTwitterUserForManipulationDtoValidator<TwitterUserFollowsTwitterUserForCreationDto>
    {
        public TwitterUserFollowsTwitterUserForCreationDtoValidator()
        {

        }
    }
}