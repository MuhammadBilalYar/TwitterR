namespace Application.Interfaces.Message
{
    using System;
    using Application.Dtos.Message;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IMessageRepository
    {
        Task<PagedList<Message>> GetMessagesAsync(MessageParametersDto MessageParameters);
        Task<Message> GetMessageAsync(int MessageId);
        Message GetMessage(int MessageId);
        Task AddMessage(Message message);
        void DeleteMessage(Message message);
        void UpdateMessage(Message message);
        bool Save();
        Task<bool> SaveAsync();
    }
}