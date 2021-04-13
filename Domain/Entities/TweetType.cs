namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("TweetType")]
    public class TweetType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int TweetTypeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string TweetTypeName { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string TweetTypeDescription { get; set; }
    }
}