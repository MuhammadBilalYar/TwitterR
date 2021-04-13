namespace Application.Validation.UserBookmarksTweet
{
    using Application.Dtos.UserBookmarksTweet;
    using FluentValidation;

    public class UserBookmarksTweetForCreationDtoValidator: UserBookmarksTweetForManipulationDtoValidator<UserBookmarksTweetForCreationDto>
    {
        public UserBookmarksTweetForCreationDtoValidator()
        {

        }
    }
}