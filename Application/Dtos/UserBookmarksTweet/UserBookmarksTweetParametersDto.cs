namespace Application.Dtos.UserBookmarksTweet
{
    using Application.Dtos.Shared;

    public class UserBookmarksTweetParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}