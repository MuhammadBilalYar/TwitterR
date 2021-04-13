namespace Application.Validation.TweetLikes
{
    using Application.Dtos.TweetLikes;
    using FluentValidation;

    public class TweetLikesForUpdateDtoValidator: TweetLikesForManipulationDtoValidator<TweetLikesForUpdateDto>
    {
        public TweetLikesForUpdateDtoValidator()
        {
                          }
    }
}