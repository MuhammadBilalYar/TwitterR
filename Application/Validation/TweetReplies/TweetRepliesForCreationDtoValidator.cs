namespace Application.Validation.TweetReplies
{
    using Application.Dtos.TweetReplies;
    using FluentValidation;

    public class TweetRepliesForCreationDtoValidator: TweetRepliesForManipulationDtoValidator<TweetRepliesForCreationDto>
    {
        public TweetRepliesForCreationDtoValidator()
        {
                          }
    }
}