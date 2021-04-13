namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.UserBookmarksTweet;
    using Application.Interfaces.UserBookmarksTweet;
    using Application.Validation.UserBookmarksTweet;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/UserBookmarksTweets")]
    [ApiVersion("1.0")]
    public class UserBookmarksTweetsController: Controller
    {
        private readonly IUserBookmarksTweetRepository _userBookmarksTweetRepository;
        private readonly IMapper _mapper;

        public UserBookmarksTweetsController(IUserBookmarksTweetRepository userBookmarksTweetRepository
            , IMapper mapper)
        {
            _userBookmarksTweetRepository = userBookmarksTweetRepository ??
                throw new ArgumentNullException(nameof(userBookmarksTweetRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        
                                                                                                                                                    [ProducesResponseType(typeof(Response<IEnumerable<UserBookmarksTweetDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetUserBookmarksTweets")]
        public async Task<IActionResult> GetUserBookmarksTweets([FromQuery] UserBookmarksTweetParametersDto userBookmarksTweetParametersDto)
        {
            var userBookmarksTweetsFromRepo = await _userBookmarksTweetRepository.GetUserBookmarksTweetsAsync(userBookmarksTweetParametersDto);

            var paginationMetadata = new
            {
                totalCount = userBookmarksTweetsFromRepo.TotalCount,
                pageSize = userBookmarksTweetsFromRepo.PageSize,
                currentPageSize = userBookmarksTweetsFromRepo.CurrentPageSize,
                currentStartIndex = userBookmarksTweetsFromRepo.CurrentStartIndex,
                currentEndIndex = userBookmarksTweetsFromRepo.CurrentEndIndex,
                pageNumber = userBookmarksTweetsFromRepo.PageNumber,
                totalPages = userBookmarksTweetsFromRepo.TotalPages,
                hasPrevious = userBookmarksTweetsFromRepo.HasPrevious,
                hasNext = userBookmarksTweetsFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var userBookmarksTweetsDto = _mapper.Map<IEnumerable<UserBookmarksTweetDto>>(userBookmarksTweetsFromRepo);
            var response = new Response<IEnumerable<UserBookmarksTweetDto>>(userBookmarksTweetsDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<UserBookmarksTweetDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{userBookmarksTweetId}", Name = "GetUserBookmarksTweet")]
        public async Task<ActionResult<UserBookmarksTweetDto>> GetUserBookmarksTweet(int userBookmarksTweetId)
        {
            var userBookmarksTweetFromRepo = await _userBookmarksTweetRepository.GetUserBookmarksTweetAsync(userBookmarksTweetId);

            if (userBookmarksTweetFromRepo == null)
            {
                return NotFound();
            }

            var userBookmarksTweetDto = _mapper.Map<UserBookmarksTweetDto>(userBookmarksTweetFromRepo);
            var response = new Response<UserBookmarksTweetDto>(userBookmarksTweetDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<UserBookmarksTweetDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<UserBookmarksTweetDto>> AddUserBookmarksTweet([FromBody]UserBookmarksTweetForCreationDto userBookmarksTweetForCreation)
        {
            var validationResults = new UserBookmarksTweetForCreationDtoValidator().Validate(userBookmarksTweetForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            var userBookmarksTweet = _mapper.Map<UserBookmarksTweet>(userBookmarksTweetForCreation);
            await _userBookmarksTweetRepository.AddUserBookmarksTweet(userBookmarksTweet);
            var saveSuccessful = await _userBookmarksTweetRepository.SaveAsync();

            if(saveSuccessful)
            {
                var userBookmarksTweetFromRepo = await _userBookmarksTweetRepository.GetUserBookmarksTweetAsync(userBookmarksTweet.TwitterUserId);
                var userBookmarksTweetDto = _mapper.Map<UserBookmarksTweetDto>(userBookmarksTweetFromRepo);
                var response = new Response<UserBookmarksTweetDto>(userBookmarksTweetDto);
                
                return CreatedAtRoute("GetUserBookmarksTweet",
                    new { userBookmarksTweetDto.TwitterUserId },
                    response);
            }

            return StatusCode(500);
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{userBookmarksTweetId}")]
        public async Task<ActionResult> DeleteUserBookmarksTweet(int userBookmarksTweetId)
        {
            var userBookmarksTweetFromRepo = await _userBookmarksTweetRepository.GetUserBookmarksTweetAsync(userBookmarksTweetId);

            if (userBookmarksTweetFromRepo == null)
            {
                return NotFound();
            }

            _userBookmarksTweetRepository.DeleteUserBookmarksTweet(userBookmarksTweetFromRepo);
            await _userBookmarksTweetRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{userBookmarksTweetId}")]
        public async Task<IActionResult> UpdateUserBookmarksTweet(int userBookmarksTweetId, UserBookmarksTweetForUpdateDto userBookmarksTweet)
        {
            var userBookmarksTweetFromRepo = await _userBookmarksTweetRepository.GetUserBookmarksTweetAsync(userBookmarksTweetId);

            if (userBookmarksTweetFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new UserBookmarksTweetForUpdateDtoValidator().Validate(userBookmarksTweet);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            _mapper.Map(userBookmarksTweet, userBookmarksTweetFromRepo);
            _userBookmarksTweetRepository.UpdateUserBookmarksTweet(userBookmarksTweetFromRepo);

            await _userBookmarksTweetRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{userBookmarksTweetId}")]
        public async Task<IActionResult> PartiallyUpdateUserBookmarksTweet(int userBookmarksTweetId, JsonPatchDocument<UserBookmarksTweetForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingUserBookmarksTweet = await _userBookmarksTweetRepository.GetUserBookmarksTweetAsync(userBookmarksTweetId);

            if (existingUserBookmarksTweet == null)
            {
                return NotFound();
            }

            var userBookmarksTweetToPatch = _mapper.Map<UserBookmarksTweetForUpdateDto>(existingUserBookmarksTweet); // map the userBookmarksTweet we got from the database to an updatable userBookmarksTweet model
            patchDoc.ApplyTo(userBookmarksTweetToPatch, ModelState); // apply patchdoc updates to the updatable userBookmarksTweet

            if (!TryValidateModel(userBookmarksTweetToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userBookmarksTweetToPatch, existingUserBookmarksTweet); // apply updates from the updatable userBookmarksTweet to the db entity so we can apply the updates to the database
            _userBookmarksTweetRepository.UpdateUserBookmarksTweet(existingUserBookmarksTweet); // apply business updates to data if needed

            await _userBookmarksTweetRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}