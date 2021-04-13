namespace Application.Validation.UserBookmarksTweet
{
    using Application.Dtos.UserBookmarksTweet;
    using FluentValidation;
    using System;

    public class UserBookmarksTweetForManipulationDtoValidator<T> : AbstractValidator<T> where T : UserBookmarksTweetForManipulationDto
    {
        public UserBookmarksTweetForManipulationDtoValidator()
        {

        }
    }
}