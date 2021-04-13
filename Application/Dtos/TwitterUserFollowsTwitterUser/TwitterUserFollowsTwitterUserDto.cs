namespace Application.Dtos.TwitterUserFollowsTwitterUser
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class TwitterUserFollowsTwitterUserDto 
    {
        public int TwitterUserId { get; set; }
        public int FollowedTwitterUserId { get; set; }
    }
}