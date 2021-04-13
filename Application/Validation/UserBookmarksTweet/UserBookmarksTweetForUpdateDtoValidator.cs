namespace Application.Validation.UserBookmarksTweet
{
    using Application.Dtos.UserBookmarksTweet;
    using FluentValidation;

    public class UserBookmarksTweetForUpdateDtoValidator: UserBookmarksTweetForManipulationDtoValidator<UserBookmarksTweetForUpdateDto>
    {
        public UserBookmarksTweetForUpdateDtoValidator()
        {

        }
    }
}