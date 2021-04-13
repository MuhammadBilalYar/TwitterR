namespace Application.Dtos.TweetType
{
    using Application.Dtos.Shared;

    public class TweetTypeParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}