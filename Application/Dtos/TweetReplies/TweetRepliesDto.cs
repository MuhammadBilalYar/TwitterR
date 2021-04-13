namespace Application.Dtos.TweetReplies
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class TweetRepliesDto 
    {
        public int TweetId { get; set; }
        public int ReplyTweetId { get; set; }
    }
}