namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("UserBookmarksTweet")]
    public class UserBookmarksTweet
    {
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int TwitterUserId { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int TweetId { get; set; }
    }
}