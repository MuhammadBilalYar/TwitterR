namespace Application.Dtos.TweetType
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class TweetTypeForManipulationDto 
    {
        public string TweetTypeName { get; set; }
        public string TweetTypeDescription { get; set; }
    }
}