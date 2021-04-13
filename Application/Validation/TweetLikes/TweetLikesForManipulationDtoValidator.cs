namespace Application.Validation.TweetLikes
{
    using Application.Dtos.TweetLikes;
    using FluentValidation;
    using System;

    public class TweetLikesForManipulationDtoValidator<T> : AbstractValidator<T> where T : TweetLikesForManipulationDto
    {
        public TweetLikesForManipulationDtoValidator()
        {
                          }
    }
}