namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.NotificationType;
    using Application.Interfaces.NotificationType;
    using Application.Validation.NotificationType;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/NotificationTypes")]
    [ApiVersion("1.0")]
    public class NotificationTypesController: Controller
    {
        private readonly INotificationTypeRepository _notificationTypeRepository;
        private readonly IMapper _mapper;

        public NotificationTypesController(INotificationTypeRepository notificationTypeRepository
            , IMapper mapper)
        {
            _notificationTypeRepository = notificationTypeRepository ??
                throw new ArgumentNullException(nameof(notificationTypeRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        
                                                                                                                                                    [ProducesResponseType(typeof(Response<IEnumerable<NotificationTypeDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetNotificationTypes")]
        public async Task<IActionResult> GetNotificationTypes([FromQuery] NotificationTypeParametersDto notificationTypeParametersDto)
        {
            var notificationTypesFromRepo = await _notificationTypeRepository.GetNotificationTypesAsync(notificationTypeParametersDto);

            var paginationMetadata = new
            {
                totalCount = notificationTypesFromRepo.TotalCount,
                pageSize = notificationTypesFromRepo.PageSize,
                currentPageSize = notificationTypesFromRepo.CurrentPageSize,
                currentStartIndex = notificationTypesFromRepo.CurrentStartIndex,
                currentEndIndex = notificationTypesFromRepo.CurrentEndIndex,
                pageNumber = notificationTypesFromRepo.PageNumber,
                totalPages = notificationTypesFromRepo.TotalPages,
                hasPrevious = notificationTypesFromRepo.HasPrevious,
                hasNext = notificationTypesFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var notificationTypesDto = _mapper.Map<IEnumerable<NotificationTypeDto>>(notificationTypesFromRepo);
            var response = new Response<IEnumerable<NotificationTypeDto>>(notificationTypesDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<NotificationTypeDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{notificationTypeId}", Name = "GetNotificationType")]
        public async Task<ActionResult<NotificationTypeDto>> GetNotificationType(int notificationTypeId)
        {
            var notificationTypeFromRepo = await _notificationTypeRepository.GetNotificationTypeAsync(notificationTypeId);

            if (notificationTypeFromRepo == null)
            {
                return NotFound();
            }

            var notificationTypeDto = _mapper.Map<NotificationTypeDto>(notificationTypeFromRepo);
            var response = new Response<NotificationTypeDto>(notificationTypeDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<NotificationTypeDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<NotificationTypeDto>> AddNotificationType([FromBody]NotificationTypeForCreationDto notificationTypeForCreation)
        {
            var validationResults = new NotificationTypeForCreationDtoValidator().Validate(notificationTypeForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            var notificationType = _mapper.Map<NotificationType>(notificationTypeForCreation);
            await _notificationTypeRepository.AddNotificationType(notificationType);
            var saveSuccessful = await _notificationTypeRepository.SaveAsync();

            if(saveSuccessful)
            {
                var notificationTypeFromRepo = await _notificationTypeRepository.GetNotificationTypeAsync(notificationType.NotificationTypeId);
                var notificationTypeDto = _mapper.Map<NotificationTypeDto>(notificationTypeFromRepo);
                var response = new Response<NotificationTypeDto>(notificationTypeDto);
                
                return CreatedAtRoute("GetNotificationType",
                    new { notificationTypeDto.NotificationTypeId },
                    response);
            }

            return StatusCode(500);
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{notificationTypeId}")]
        public async Task<ActionResult> DeleteNotificationType(int notificationTypeId)
        {
            var notificationTypeFromRepo = await _notificationTypeRepository.GetNotificationTypeAsync(notificationTypeId);

            if (notificationTypeFromRepo == null)
            {
                return NotFound();
            }

            _notificationTypeRepository.DeleteNotificationType(notificationTypeFromRepo);
            await _notificationTypeRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{notificationTypeId}")]
        public async Task<IActionResult> UpdateNotificationType(int notificationTypeId, NotificationTypeForUpdateDto notificationType)
        {
            var notificationTypeFromRepo = await _notificationTypeRepository.GetNotificationTypeAsync(notificationTypeId);

            if (notificationTypeFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new NotificationTypeForUpdateDtoValidator().Validate(notificationType);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            _mapper.Map(notificationType, notificationTypeFromRepo);
            _notificationTypeRepository.UpdateNotificationType(notificationTypeFromRepo);

            await _notificationTypeRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{notificationTypeId}")]
        public async Task<IActionResult> PartiallyUpdateNotificationType(int notificationTypeId, JsonPatchDocument<NotificationTypeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingNotificationType = await _notificationTypeRepository.GetNotificationTypeAsync(notificationTypeId);

            if (existingNotificationType == null)
            {
                return NotFound();
            }

            var notificationTypeToPatch = _mapper.Map<NotificationTypeForUpdateDto>(existingNotificationType); // map the notificationType we got from the database to an updatable notificationType model
            patchDoc.ApplyTo(notificationTypeToPatch, ModelState); // apply patchdoc updates to the updatable notificationType

            if (!TryValidateModel(notificationTypeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(notificationTypeToPatch, existingNotificationType); // apply updates from the updatable notificationType to the db entity so we can apply the updates to the database
            _notificationTypeRepository.UpdateNotificationType(existingNotificationType); // apply business updates to data if needed

            await _notificationTypeRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}