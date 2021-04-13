namespace Application.Dtos.TweetReplies
{
    using Application.Dtos.Shared;

    public class TweetRepliesParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}