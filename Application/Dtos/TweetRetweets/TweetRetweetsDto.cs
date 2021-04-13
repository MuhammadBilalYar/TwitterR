namespace Application.Dtos.TweetRetweets
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class TweetRetweetsDto 
    {
        public int TweetId { get; set; }
        public int TweetRetweetId { get; set; }
    }
}