namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.TweetRetweets;
    using Application.Interfaces.TweetRetweets;
    using Application.Validation.TweetRetweets;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/TweetRetweetss")]
    [ApiVersion("1.0")]
    public class TweetRetweetssController: Controller
    {
        private readonly ITweetRetweetsRepository _tweetRetweetsRepository;
        private readonly IMapper _mapper;

        public TweetRetweetssController(ITweetRetweetsRepository tweetRetweetsRepository
            , IMapper mapper)
        {
            _tweetRetweetsRepository = tweetRetweetsRepository ??
                throw new ArgumentNullException(nameof(tweetRetweetsRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        
                                                                                                                                                    [ProducesResponseType(typeof(Response<IEnumerable<TweetRetweetsDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetTweetRetweetss")]
        public async Task<IActionResult> GetTweetRetweetss([FromQuery] TweetRetweetsParametersDto tweetRetweetsParametersDto)
        {
            var tweetRetweetssFromRepo = await _tweetRetweetsRepository.GetTweetRetweetssAsync(tweetRetweetsParametersDto);

            var paginationMetadata = new
            {
                totalCount = tweetRetweetssFromRepo.TotalCount,
                pageSize = tweetRetweetssFromRepo.PageSize,
                currentPageSize = tweetRetweetssFromRepo.CurrentPageSize,
                currentStartIndex = tweetRetweetssFromRepo.CurrentStartIndex,
                currentEndIndex = tweetRetweetssFromRepo.CurrentEndIndex,
                pageNumber = tweetRetweetssFromRepo.PageNumber,
                totalPages = tweetRetweetssFromRepo.TotalPages,
                hasPrevious = tweetRetweetssFromRepo.HasPrevious,
                hasNext = tweetRetweetssFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var tweetRetweetssDto = _mapper.Map<IEnumerable<TweetRetweetsDto>>(tweetRetweetssFromRepo);
            var response = new Response<IEnumerable<TweetRetweetsDto>>(tweetRetweetssDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TweetRetweetsDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{tweetRetweetsId}", Name = "GetTweetRetweets")]
        public async Task<ActionResult<TweetRetweetsDto>> GetTweetRetweets(int tweetRetweetsId)
        {
            var tweetRetweetsFromRepo = await _tweetRetweetsRepository.GetTweetRetweetsAsync(tweetRetweetsId);

            if (tweetRetweetsFromRepo == null)
            {
                return NotFound();
            }

            var tweetRetweetsDto = _mapper.Map<TweetRetweetsDto>(tweetRetweetsFromRepo);
            var response = new Response<TweetRetweetsDto>(tweetRetweetsDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TweetRetweetsDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<TweetRetweetsDto>> AddTweetRetweets([FromBody]TweetRetweetsForCreationDto tweetRetweetsForCreation)
        {
            var validationResults = new TweetRetweetsForCreationDtoValidator().Validate(tweetRetweetsForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            var tweetRetweets = _mapper.Map<TweetRetweets>(tweetRetweetsForCreation);
            await _tweetRetweetsRepository.AddTweetRetweets(tweetRetweets);
            var saveSuccessful = await _tweetRetweetsRepository.SaveAsync();

            if(saveSuccessful)
            {
                var tweetRetweetsFromRepo = await _tweetRetweetsRepository.GetTweetRetweetsAsync(tweetRetweets.TweetId);
                var tweetRetweetsDto = _mapper.Map<TweetRetweetsDto>(tweetRetweetsFromRepo);
                var response = new Response<TweetRetweetsDto>(tweetRetweetsDto);
                
                return CreatedAtRoute("GetTweetRetweets",
                    new { tweetRetweetsDto.TweetId },
                    response);
            }

            return StatusCode(500);
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{tweetRetweetsId}")]
        public async Task<ActionResult> DeleteTweetRetweets(int tweetRetweetsId)
        {
            var tweetRetweetsFromRepo = await _tweetRetweetsRepository.GetTweetRetweetsAsync(tweetRetweetsId);

            if (tweetRetweetsFromRepo == null)
            {
                return NotFound();
            }

            _tweetRetweetsRepository.DeleteTweetRetweets(tweetRetweetsFromRepo);
            await _tweetRetweetsRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{tweetRetweetsId}")]
        public async Task<IActionResult> UpdateTweetRetweets(int tweetRetweetsId, TweetRetweetsForUpdateDto tweetRetweets)
        {
            var tweetRetweetsFromRepo = await _tweetRetweetsRepository.GetTweetRetweetsAsync(tweetRetweetsId);

            if (tweetRetweetsFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new TweetRetweetsForUpdateDtoValidator().Validate(tweetRetweets);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            _mapper.Map(tweetRetweets, tweetRetweetsFromRepo);
            _tweetRetweetsRepository.UpdateTweetRetweets(tweetRetweetsFromRepo);

            await _tweetRetweetsRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{tweetRetweetsId}")]
        public async Task<IActionResult> PartiallyUpdateTweetRetweets(int tweetRetweetsId, JsonPatchDocument<TweetRetweetsForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingTweetRetweets = await _tweetRetweetsRepository.GetTweetRetweetsAsync(tweetRetweetsId);

            if (existingTweetRetweets == null)
            {
                return NotFound();
            }

            var tweetRetweetsToPatch = _mapper.Map<TweetRetweetsForUpdateDto>(existingTweetRetweets); // map the tweetRetweets we got from the database to an updatable tweetRetweets model
            patchDoc.ApplyTo(tweetRetweetsToPatch, ModelState); // apply patchdoc updates to the updatable tweetRetweets

            if (!TryValidateModel(tweetRetweetsToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(tweetRetweetsToPatch, existingTweetRetweets); // apply updates from the updatable tweetRetweets to the db entity so we can apply the updates to the database
            _tweetRetweetsRepository.UpdateTweetRetweets(existingTweetRetweets); // apply business updates to data if needed

            await _tweetRetweetsRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}