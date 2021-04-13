namespace Application.Dtos.NotificationType
{
    using Application.Dtos.Shared;

    public class NotificationTypeParametersDto : BasePaginationParameters
    {
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}