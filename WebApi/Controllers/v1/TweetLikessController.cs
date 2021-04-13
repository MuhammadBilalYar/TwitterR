namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.TweetLikes;
    using Application.Interfaces.TweetLikes;
    using Application.Validation.TweetLikes;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/TweetLikess")]
    [ApiVersion("1.0")]
    public class TweetLikessController: Controller
    {
        private readonly ITweetLikesRepository _tweetLikesRepository;
        private readonly IMapper _mapper;

        public TweetLikessController(ITweetLikesRepository tweetLikesRepository
            , IMapper mapper)
        {
            _tweetLikesRepository = tweetLikesRepository ??
                throw new ArgumentNullException(nameof(tweetLikesRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        
                                                                                                                                                    [ProducesResponseType(typeof(Response<IEnumerable<TweetLikesDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetTweetLikess")]
        public async Task<IActionResult> GetTweetLikess([FromQuery] TweetLikesParametersDto tweetLikesParametersDto)
        {
            var tweetLikessFromRepo = await _tweetLikesRepository.GetTweetLikessAsync(tweetLikesParametersDto);

            var paginationMetadata = new
            {
                totalCount = tweetLikessFromRepo.TotalCount,
                pageSize = tweetLikessFromRepo.PageSize,
                currentPageSize = tweetLikessFromRepo.CurrentPageSize,
                currentStartIndex = tweetLikessFromRepo.CurrentStartIndex,
                currentEndIndex = tweetLikessFromRepo.CurrentEndIndex,
                pageNumber = tweetLikessFromRepo.PageNumber,
                totalPages = tweetLikessFromRepo.TotalPages,
                hasPrevious = tweetLikessFromRepo.HasPrevious,
                hasNext = tweetLikessFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var tweetLikessDto = _mapper.Map<IEnumerable<TweetLikesDto>>(tweetLikessFromRepo);
            var response = new Response<IEnumerable<TweetLikesDto>>(tweetLikessDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TweetLikesDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{tweetLikesId}", Name = "GetTweetLikes")]
        public async Task<ActionResult<TweetLikesDto>> GetTweetLikes(int tweetLikesId)
        {
            var tweetLikesFromRepo = await _tweetLikesRepository.GetTweetLikesAsync(tweetLikesId);

            if (tweetLikesFromRepo == null)
            {
                return NotFound();
            }

            var tweetLikesDto = _mapper.Map<TweetLikesDto>(tweetLikesFromRepo);
            var response = new Response<TweetLikesDto>(tweetLikesDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TweetLikesDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<TweetLikesDto>> AddTweetLikes([FromBody]TweetLikesForCreationDto tweetLikesForCreation)
        {
            var validationResults = new TweetLikesForCreationDtoValidator().Validate(tweetLikesForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            var tweetLikes = _mapper.Map<TweetLikes>(tweetLikesForCreation);
            await _tweetLikesRepository.AddTweetLikes(tweetLikes);
            var saveSuccessful = await _tweetLikesRepository.SaveAsync();

            if(saveSuccessful)
            {
                var tweetLikesFromRepo = await _tweetLikesRepository.GetTweetLikesAsync(tweetLikes.TweetId);
                var tweetLikesDto = _mapper.Map<TweetLikesDto>(tweetLikesFromRepo);
                var response = new Response<TweetLikesDto>(tweetLikesDto);
                
                return CreatedAtRoute("GetTweetLikes",
                    new { tweetLikesDto.TweetId },
                    response);
            }

            return StatusCode(500);
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{tweetLikesId}")]
        public async Task<ActionResult> DeleteTweetLikes(int tweetLikesId)
        {
            var tweetLikesFromRepo = await _tweetLikesRepository.GetTweetLikesAsync(tweetLikesId);

            if (tweetLikesFromRepo == null)
            {
                return NotFound();
            }

            _tweetLikesRepository.DeleteTweetLikes(tweetLikesFromRepo);
            await _tweetLikesRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{tweetLikesId}")]
        public async Task<IActionResult> UpdateTweetLikes(int tweetLikesId, TweetLikesForUpdateDto tweetLikes)
        {
            var tweetLikesFromRepo = await _tweetLikesRepository.GetTweetLikesAsync(tweetLikesId);

            if (tweetLikesFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new TweetLikesForUpdateDtoValidator().Validate(tweetLikes);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            _mapper.Map(tweetLikes, tweetLikesFromRepo);
            _tweetLikesRepository.UpdateTweetLikes(tweetLikesFromRepo);

            await _tweetLikesRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{tweetLikesId}")]
        public async Task<IActionResult> PartiallyUpdateTweetLikes(int tweetLikesId, JsonPatchDocument<TweetLikesForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingTweetLikes = await _tweetLikesRepository.GetTweetLikesAsync(tweetLikesId);

            if (existingTweetLikes == null)
            {
                return NotFound();
            }

            var tweetLikesToPatch = _mapper.Map<TweetLikesForUpdateDto>(existingTweetLikes); // map the tweetLikes we got from the database to an updatable tweetLikes model
            patchDoc.ApplyTo(tweetLikesToPatch, ModelState); // apply patchdoc updates to the updatable tweetLikes

            if (!TryValidateModel(tweetLikesToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(tweetLikesToPatch, existingTweetLikes); // apply updates from the updatable tweetLikes to the db entity so we can apply the updates to the database
            _tweetLikesRepository.UpdateTweetLikes(existingTweetLikes); // apply business updates to data if needed

            await _tweetLikesRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}