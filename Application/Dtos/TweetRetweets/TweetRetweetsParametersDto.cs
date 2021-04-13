namespace Application.Dtos.TweetRetweets
{
    using Application.Dtos.Shared;

    public class TweetRetweetsParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}