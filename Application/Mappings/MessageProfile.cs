namespace Application.Mappings
{
    using Application.Dtos.Message;
    using AutoMapper;
    using Domain.Entities;

    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
                     CreateMap<Message, MessageDto>()
                .ReverseMap();
            CreateMap<MessageForCreationDto, Message>();
            CreateMap<MessageForUpdateDto, Message>()
                .ReverseMap();
        }
    }
}