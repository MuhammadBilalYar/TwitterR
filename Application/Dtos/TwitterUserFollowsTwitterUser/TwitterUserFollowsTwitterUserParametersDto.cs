namespace Application.Dtos.TwitterUserFollowsTwitterUser
{
    using Application.Dtos.Shared;

    public class TwitterUserFollowsTwitterUserParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}