namespace Application.Validation.TweetReplies
{
    using Application.Dtos.TweetReplies;
    using FluentValidation;

    public class TweetRepliesForUpdateDtoValidator: TweetRepliesForManipulationDtoValidator<TweetRepliesForUpdateDto>
    {
        public TweetRepliesForUpdateDtoValidator()
        {

        }
    }
}