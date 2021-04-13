namespace Application.Dtos.UserBookmarksTweet
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class UserBookmarksTweetDto 
    {
        public int TwitterUserId { get; set; }
        public int TweetId { get; set; }

         }
}