namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.Message;
    using Application.Interfaces.Message;
    using Application.Validation.Message;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/Messages")]
    [ApiVersion("1.0")]
    public class MessagesController: Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessagesController(IMessageRepository messageRepository
            , IMapper mapper)
        {
            _messageRepository = messageRepository ??
                throw new ArgumentNullException(nameof(messageRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        
                                                                                                                                                    [ProducesResponseType(typeof(Response<IEnumerable<MessageDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetMessages")]
        public async Task<IActionResult> GetMessages([FromQuery] MessageParametersDto messageParametersDto)
        {
            var messagesFromRepo = await _messageRepository.GetMessagesAsync(messageParametersDto);

            var paginationMetadata = new
            {
                totalCount = messagesFromRepo.TotalCount,
                pageSize = messagesFromRepo.PageSize,
                currentPageSize = messagesFromRepo.CurrentPageSize,
                currentStartIndex = messagesFromRepo.CurrentStartIndex,
                currentEndIndex = messagesFromRepo.CurrentEndIndex,
                pageNumber = messagesFromRepo.PageNumber,
                totalPages = messagesFromRepo.TotalPages,
                hasPrevious = messagesFromRepo.HasPrevious,
                hasNext = messagesFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var messagesDto = _mapper.Map<IEnumerable<MessageDto>>(messagesFromRepo);
            var response = new Response<IEnumerable<MessageDto>>(messagesDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<MessageDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{messageId}", Name = "GetMessage")]
        public async Task<ActionResult<MessageDto>> GetMessage(int messageId)
        {
            var messageFromRepo = await _messageRepository.GetMessageAsync(messageId);

            if (messageFromRepo == null)
            {
                return NotFound();
            }

            var messageDto = _mapper.Map<MessageDto>(messageFromRepo);
            var response = new Response<MessageDto>(messageDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<MessageDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<MessageDto>> AddMessage([FromBody]MessageForCreationDto messageForCreation)
        {
            var validationResults = new MessageForCreationDtoValidator().Validate(messageForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            var message = _mapper.Map<Message>(messageForCreation);
            await _messageRepository.AddMessage(message);
            var saveSuccessful = await _messageRepository.SaveAsync();

            if(saveSuccessful)
            {
                var messageFromRepo = await _messageRepository.GetMessageAsync(message.MessageId);
                var messageDto = _mapper.Map<MessageDto>(messageFromRepo);
                var response = new Response<MessageDto>(messageDto);
                
                return CreatedAtRoute("GetMessage",
                    new { messageDto.MessageId },
                    response);
            }

            return StatusCode(500);
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{messageId}")]
        public async Task<ActionResult> DeleteMessage(int messageId)
        {
            var messageFromRepo = await _messageRepository.GetMessageAsync(messageId);

            if (messageFromRepo == null)
            {
                return NotFound();
            }

            _messageRepository.DeleteMessage(messageFromRepo);
            await _messageRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{messageId}")]
        public async Task<IActionResult> UpdateMessage(int messageId, MessageForUpdateDto message)
        {
            var messageFromRepo = await _messageRepository.GetMessageAsync(messageId);

            if (messageFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new MessageForUpdateDtoValidator().Validate(message);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            _mapper.Map(message, messageFromRepo);
            _messageRepository.UpdateMessage(messageFromRepo);

            await _messageRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{messageId}")]
        public async Task<IActionResult> PartiallyUpdateMessage(int messageId, JsonPatchDocument<MessageForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingMessage = await _messageRepository.GetMessageAsync(messageId);

            if (existingMessage == null)
            {
                return NotFound();
            }

            var messageToPatch = _mapper.Map<MessageForUpdateDto>(existingMessage); // map the message we got from the database to an updatable message model
            patchDoc.ApplyTo(messageToPatch, ModelState); // apply patchdoc updates to the updatable message

            if (!TryValidateModel(messageToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(messageToPatch, existingMessage); // apply updates from the updatable message to the db entity so we can apply the updates to the database
            _messageRepository.UpdateMessage(existingMessage); // apply business updates to data if needed

            await _messageRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}