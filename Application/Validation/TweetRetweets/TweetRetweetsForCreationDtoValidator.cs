namespace Application.Validation.TweetRetweets
{
    using Application.Dtos.TweetRetweets;
    using FluentValidation;

    public class TweetRetweetsForCreationDtoValidator: TweetRetweetsForManipulationDtoValidator<TweetRetweetsForCreationDto>
    {
        public TweetRetweetsForCreationDtoValidator()
        {

        }
    }
}