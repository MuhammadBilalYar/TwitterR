namespace Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("NotificationType")]
    public class NotificationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int NotificationTypeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string NotificationTypeTitle { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string NotificationTypeDescription { get; set; }
    }
}