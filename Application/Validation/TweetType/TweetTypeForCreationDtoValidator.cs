namespace Application.Validation.TweetType
{
    using Application.Dtos.TweetType;
    using FluentValidation;

    public class TweetTypeForCreationDtoValidator: TweetTypeForManipulationDtoValidator<TweetTypeForCreationDto>
    {
        public TweetTypeForCreationDtoValidator()
        {

        }
    }
}