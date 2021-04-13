namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("TweetReplies")]
    public class TweetReplies
    {
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int TweetId { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int ReplyTweetId { get; set; }
    }
}