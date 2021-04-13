namespace Application.Validation.TweetLikes
{
    using Application.Dtos.TweetLikes;
    using FluentValidation;

    public class TweetLikesForCreationDtoValidator: TweetLikesForManipulationDtoValidator<TweetLikesForCreationDto>
    {
        public TweetLikesForCreationDtoValidator()
        {
                          }
    }
}