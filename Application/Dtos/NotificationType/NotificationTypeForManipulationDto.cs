namespace Application.Dtos.NotificationType
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class NotificationTypeForManipulationDto 
    {
        public string NotificationTypeTitle { get; set; }
        public string NotificationTypeDescription { get; set; }
    }
}