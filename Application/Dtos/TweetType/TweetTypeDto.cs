namespace Application.Dtos.TweetType
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class TweetTypeDto 
    {
        public int TweetTypeId { get; set; }
        public string TweetTypeName { get; set; }
        public string TweetTypeDescription { get; set; }
    }
}