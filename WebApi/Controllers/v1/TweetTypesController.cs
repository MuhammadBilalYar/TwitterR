namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.TweetType;
    using Application.Interfaces.TweetType;
    using Application.Validation.TweetType;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/TweetTypes")]
    [ApiVersion("1.0")]
    public class TweetTypesController: Controller
    {
        private readonly ITweetTypeRepository _tweetTypeRepository;
        private readonly IMapper _mapper;

        public TweetTypesController(ITweetTypeRepository tweetTypeRepository
            , IMapper mapper)
        {
            _tweetTypeRepository = tweetTypeRepository ??
                throw new ArgumentNullException(nameof(tweetTypeRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        
                                                                                                                                                    [ProducesResponseType(typeof(Response<IEnumerable<TweetTypeDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetTweetTypes")]
        public async Task<IActionResult> GetTweetTypes([FromQuery] TweetTypeParametersDto tweetTypeParametersDto)
        {
            var tweetTypesFromRepo = await _tweetTypeRepository.GetTweetTypesAsync(tweetTypeParametersDto);

            var paginationMetadata = new
            {
                totalCount = tweetTypesFromRepo.TotalCount,
                pageSize = tweetTypesFromRepo.PageSize,
                currentPageSize = tweetTypesFromRepo.CurrentPageSize,
                currentStartIndex = tweetTypesFromRepo.CurrentStartIndex,
                currentEndIndex = tweetTypesFromRepo.CurrentEndIndex,
                pageNumber = tweetTypesFromRepo.PageNumber,
                totalPages = tweetTypesFromRepo.TotalPages,
                hasPrevious = tweetTypesFromRepo.HasPrevious,
                hasNext = tweetTypesFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var tweetTypesDto = _mapper.Map<IEnumerable<TweetTypeDto>>(tweetTypesFromRepo);
            var response = new Response<IEnumerable<TweetTypeDto>>(tweetTypesDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TweetTypeDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{tweetTypeId}", Name = "GetTweetType")]
        public async Task<ActionResult<TweetTypeDto>> GetTweetType(int tweetTypeId)
        {
            var tweetTypeFromRepo = await _tweetTypeRepository.GetTweetTypeAsync(tweetTypeId);

            if (tweetTypeFromRepo == null)
            {
                return NotFound();
            }

            var tweetTypeDto = _mapper.Map<TweetTypeDto>(tweetTypeFromRepo);
            var response = new Response<TweetTypeDto>(tweetTypeDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TweetTypeDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<TweetTypeDto>> AddTweetType([FromBody]TweetTypeForCreationDto tweetTypeForCreation)
        {
            var validationResults = new TweetTypeForCreationDtoValidator().Validate(tweetTypeForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            var tweetType = _mapper.Map<TweetType>(tweetTypeForCreation);
            await _tweetTypeRepository.AddTweetType(tweetType);
            var saveSuccessful = await _tweetTypeRepository.SaveAsync();

            if(saveSuccessful)
            {
                var tweetTypeFromRepo = await _tweetTypeRepository.GetTweetTypeAsync(tweetType.TweetTypeId);
                var tweetTypeDto = _mapper.Map<TweetTypeDto>(tweetTypeFromRepo);
                var response = new Response<TweetTypeDto>(tweetTypeDto);
                
                return CreatedAtRoute("GetTweetType",
                    new { tweetTypeDto.TweetTypeId },
                    response);
            }

            return StatusCode(500);
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{tweetTypeId}")]
        public async Task<ActionResult> DeleteTweetType(int tweetTypeId)
        {
            var tweetTypeFromRepo = await _tweetTypeRepository.GetTweetTypeAsync(tweetTypeId);

            if (tweetTypeFromRepo == null)
            {
                return NotFound();
            }

            _tweetTypeRepository.DeleteTweetType(tweetTypeFromRepo);
            await _tweetTypeRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{tweetTypeId}")]
        public async Task<IActionResult> UpdateTweetType(int tweetTypeId, TweetTypeForUpdateDto tweetType)
        {
            var tweetTypeFromRepo = await _tweetTypeRepository.GetTweetTypeAsync(tweetTypeId);

            if (tweetTypeFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new TweetTypeForUpdateDtoValidator().Validate(tweetType);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            _mapper.Map(tweetType, tweetTypeFromRepo);
            _tweetTypeRepository.UpdateTweetType(tweetTypeFromRepo);

            await _tweetTypeRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{tweetTypeId}")]
        public async Task<IActionResult> PartiallyUpdateTweetType(int tweetTypeId, JsonPatchDocument<TweetTypeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingTweetType = await _tweetTypeRepository.GetTweetTypeAsync(tweetTypeId);

            if (existingTweetType == null)
            {
                return NotFound();
            }

            var tweetTypeToPatch = _mapper.Map<TweetTypeForUpdateDto>(existingTweetType); // map the tweetType we got from the database to an updatable tweetType model
            patchDoc.ApplyTo(tweetTypeToPatch, ModelState); // apply patchdoc updates to the updatable tweetType

            if (!TryValidateModel(tweetTypeToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(tweetTypeToPatch, existingTweetType); // apply updates from the updatable tweetType to the db entity so we can apply the updates to the database
            _tweetTypeRepository.UpdateTweetType(existingTweetType); // apply business updates to data if needed

            await _tweetTypeRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}