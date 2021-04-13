namespace Application.Dtos.NotificationType
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class NotificationTypeDto 
    {
        public int NotificationTypeId { get; set; }
        public string NotificationTypeTitle { get; set; }
        public string NotificationTypeDescription { get; set; }
    }
}