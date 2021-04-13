namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("TwitterUserFollowsTwitterUser")]
    public class TwitterUserFollowsTwitterUser
    {
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int TwitterUserId { get; set; }

        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int FollowedTwitterUserId { get; set; }
    }
}