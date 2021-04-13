namespace Application.Dtos.TwitterUser
{
    using Application.Dtos.Shared;

    public class TwitterUserParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}