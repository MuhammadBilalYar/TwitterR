namespace Application.Dtos.TweetLikes
{
    using Application.Dtos.Shared;

    public class TweetLikesParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}