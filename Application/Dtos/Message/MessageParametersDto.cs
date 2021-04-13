namespace Application.Dtos.Message
{
    using Application.Dtos.Shared;

    public class MessageParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}