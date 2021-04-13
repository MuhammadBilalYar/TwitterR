namespace Application.Validation.TwitterUser
{
    using Application.Dtos.TwitterUser;
    using FluentValidation;
    using System;

    public class TwitterUserForManipulationDtoValidator<T> : AbstractValidator<T> where T : TwitterUserForManipulationDto
    {
        public TwitterUserForManipulationDtoValidator()
        {

        }
    }
}