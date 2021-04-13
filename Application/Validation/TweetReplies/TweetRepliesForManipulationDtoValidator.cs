namespace Application.Validation.TweetReplies
{
    using Application.Dtos.TweetReplies;
    using FluentValidation;
    using System;

    public class TweetRepliesForManipulationDtoValidator<T> : AbstractValidator<T> where T : TweetRepliesForManipulationDto
    {
        public TweetRepliesForManipulationDtoValidator()
        {
                          }
    }
}