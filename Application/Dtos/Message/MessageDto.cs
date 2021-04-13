namespace Application.Dtos.Message
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public  class MessageDto 
    {
        public int MessageId { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string MessageContent { get; set; }
        public DateTime MessageSendDate { get; set; }
        public string MessageMediaURL { get; set; }
        public string MessagePublicId { get; set; }
    }
}