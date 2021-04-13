namespace Application.Validation.TweetType
{
    using Application.Dtos.TweetType;
    using FluentValidation;
    using System;

    public class TweetTypeForManipulationDtoValidator<T> : AbstractValidator<T> where T : TweetTypeForManipulationDto
    {
        public TweetTypeForManipulationDtoValidator()
        {

        }
    }
}