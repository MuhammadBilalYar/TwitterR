namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.TweetReplies;
    using Application.Interfaces.TweetReplies;
    using Application.Validation.TweetReplies;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/TweetRepliess")]
    [ApiVersion("1.0")]
    public class TweetRepliessController: Controller
    {
        private readonly ITweetRepliesRepository _tweetRepliesRepository;
        private readonly IMapper _mapper;

        public TweetRepliessController(ITweetRepliesRepository tweetRepliesRepository
            , IMapper mapper)
        {
            _tweetRepliesRepository = tweetRepliesRepository ??
                throw new ArgumentNullException(nameof(tweetRepliesRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        
                                                                                                                                                    [ProducesResponseType(typeof(Response<IEnumerable<TweetRepliesDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetTweetRepliess")]
        public async Task<IActionResult> GetTweetRepliess([FromQuery] TweetRepliesParametersDto tweetRepliesParametersDto)
        {
            var tweetRepliessFromRepo = await _tweetRepliesRepository.GetTweetRepliessAsync(tweetRepliesParametersDto);

            var paginationMetadata = new
            {
                totalCount = tweetRepliessFromRepo.TotalCount,
                pageSize = tweetRepliessFromRepo.PageSize,
                currentPageSize = tweetRepliessFromRepo.CurrentPageSize,
                currentStartIndex = tweetRepliessFromRepo.CurrentStartIndex,
                currentEndIndex = tweetRepliessFromRepo.CurrentEndIndex,
                pageNumber = tweetRepliessFromRepo.PageNumber,
                totalPages = tweetRepliessFromRepo.TotalPages,
                hasPrevious = tweetRepliessFromRepo.HasPrevious,
                hasNext = tweetRepliessFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var tweetRepliessDto = _mapper.Map<IEnumerable<TweetRepliesDto>>(tweetRepliessFromRepo);
            var response = new Response<IEnumerable<TweetRepliesDto>>(tweetRepliessDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TweetRepliesDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{tweetRepliesId}", Name = "GetTweetReplies")]
        public async Task<ActionResult<TweetRepliesDto>> GetTweetReplies(int tweetRepliesId)
        {
            var tweetRepliesFromRepo = await _tweetRepliesRepository.GetTweetRepliesAsync(tweetRepliesId);

            if (tweetRepliesFromRepo == null)
            {
                return NotFound();
            }

            var tweetRepliesDto = _mapper.Map<TweetRepliesDto>(tweetRepliesFromRepo);
            var response = new Response<TweetRepliesDto>(tweetRepliesDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TweetRepliesDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<TweetRepliesDto>> AddTweetReplies([FromBody]TweetRepliesForCreationDto tweetRepliesForCreation)
        {
            var validationResults = new TweetRepliesForCreationDtoValidator().Validate(tweetRepliesForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            var tweetReplies = _mapper.Map<TweetReplies>(tweetRepliesForCreation);
            await _tweetRepliesRepository.AddTweetReplies(tweetReplies);
            var saveSuccessful = await _tweetRepliesRepository.SaveAsync();

            if(saveSuccessful)
            {
                var tweetRepliesFromRepo = await _tweetRepliesRepository.GetTweetRepliesAsync(tweetReplies.TweetId);
                var tweetRepliesDto = _mapper.Map<TweetRepliesDto>(tweetRepliesFromRepo);
                var response = new Response<TweetRepliesDto>(tweetRepliesDto);
                
                return CreatedAtRoute("GetTweetReplies",
                    new { tweetRepliesDto.TweetId },
                    response);
            }

            return StatusCode(500);
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{tweetRepliesId}")]
        public async Task<ActionResult> DeleteTweetReplies(int tweetRepliesId)
        {
            var tweetRepliesFromRepo = await _tweetRepliesRepository.GetTweetRepliesAsync(tweetRepliesId);

            if (tweetRepliesFromRepo == null)
            {
                return NotFound();
            }

            _tweetRepliesRepository.DeleteTweetReplies(tweetRepliesFromRepo);
            await _tweetRepliesRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{tweetRepliesId}")]
        public async Task<IActionResult> UpdateTweetReplies(int tweetRepliesId, TweetRepliesForUpdateDto tweetReplies)
        {
            var tweetRepliesFromRepo = await _tweetRepliesRepository.GetTweetRepliesAsync(tweetRepliesId);

            if (tweetRepliesFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new TweetRepliesForUpdateDtoValidator().Validate(tweetReplies);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            _mapper.Map(tweetReplies, tweetRepliesFromRepo);
            _tweetRepliesRepository.UpdateTweetReplies(tweetRepliesFromRepo);

            await _tweetRepliesRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{tweetRepliesId}")]
        public async Task<IActionResult> PartiallyUpdateTweetReplies(int tweetRepliesId, JsonPatchDocument<TweetRepliesForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingTweetReplies = await _tweetRepliesRepository.GetTweetRepliesAsync(tweetRepliesId);

            if (existingTweetReplies == null)
            {
                return NotFound();
            }

            var tweetRepliesToPatch = _mapper.Map<TweetRepliesForUpdateDto>(existingTweetReplies); // map the tweetReplies we got from the database to an updatable tweetReplies model
            patchDoc.ApplyTo(tweetRepliesToPatch, ModelState); // apply patchdoc updates to the updatable tweetReplies

            if (!TryValidateModel(tweetRepliesToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(tweetRepliesToPatch, existingTweetReplies); // apply updates from the updatable tweetReplies to the db entity so we can apply the updates to the database
            _tweetRepliesRepository.UpdateTweetReplies(existingTweetReplies); // apply business updates to data if needed

            await _tweetRepliesRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}