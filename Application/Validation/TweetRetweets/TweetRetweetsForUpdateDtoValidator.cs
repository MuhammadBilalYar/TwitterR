namespace Application.Validation.TweetRetweets
{
    using Application.Dtos.TweetRetweets;
    using FluentValidation;

    public class TweetRetweetsForUpdateDtoValidator: TweetRetweetsForManipulationDtoValidator<TweetRetweetsForUpdateDto>
    {
        public TweetRetweetsForUpdateDtoValidator()
        {

        }
    }
}