namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("TweetLikes")]
    public class TweetLikes
    {
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int TweetId { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int LikerTwitterUserId { get; set; }
    }
}