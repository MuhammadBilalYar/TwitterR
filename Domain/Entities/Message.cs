namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("Message")]
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int MessageId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int SenderUserId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int ReceiverUserId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string MessageContent { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime MessageSendDate { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string MessageMediaURL { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string MessagePublicId { get; set; }
    }
}