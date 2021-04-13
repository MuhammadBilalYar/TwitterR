namespace Application.Validation.TweetType
{
    using Application.Dtos.TweetType;
    using FluentValidation;

    public class TweetTypeForUpdateDtoValidator: TweetTypeForManipulationDtoValidator<TweetTypeForUpdateDto>
    {
        public TweetTypeForUpdateDtoValidator()
        {

        }
    }
}