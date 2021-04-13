namespace Application.Validation.TwitterUserFollowsTwitterUser
{
    using Application.Dtos.TwitterUserFollowsTwitterUser;
    using FluentValidation;
    using System;

    public class TwitterUserFollowsTwitterUserForManipulationDtoValidator<T> : AbstractValidator<T> where T : TwitterUserFollowsTwitterUserForManipulationDto
    {
        public TwitterUserFollowsTwitterUserForManipulationDtoValidator()
        {

        }
    }
}