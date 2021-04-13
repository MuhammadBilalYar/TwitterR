namespace Application.Validation.TweetRetweets
{
    using Application.Dtos.TweetRetweets;
    using FluentValidation;
    using System;

    public class TweetRetweetsForManipulationDtoValidator<T> : AbstractValidator<T> where T : TweetRetweetsForManipulationDto
    {
        public TweetRetweetsForManipulationDtoValidator()
        {

        }
    }
}