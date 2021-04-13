namespace Application.Dtos.TweetLikes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class TweetLikesDto 
    {
        public int TweetId { get; set; }
        public int LikerTwitterUserId { get; set; }
    }
}