namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.TwitterUserFollowsTwitterUser;
    using Application.Interfaces.TwitterUserFollowsTwitterUser;
    using Application.Validation.TwitterUserFollowsTwitterUser;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/TwitterUserFollowsTwitterUsers")]
    [ApiVersion("1.0")]
    public class TwitterUserFollowsTwitterUsersController: Controller
    {
        private readonly ITwitterUserFollowsTwitterUserRepository _twitterUserFollowsTwitterUserRepository;
        private readonly IMapper _mapper;

        public TwitterUserFollowsTwitterUsersController(ITwitterUserFollowsTwitterUserRepository twitterUserFollowsTwitterUserRepository
            , IMapper mapper)
        {
            _twitterUserFollowsTwitterUserRepository = twitterUserFollowsTwitterUserRepository ??
                throw new ArgumentNullException(nameof(twitterUserFollowsTwitterUserRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        
                                                                                                                                                    [ProducesResponseType(typeof(Response<IEnumerable<TwitterUserFollowsTwitterUserDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetTwitterUserFollowsTwitterUsers")]
        public async Task<IActionResult> GetTwitterUserFollowsTwitterUsers([FromQuery] TwitterUserFollowsTwitterUserParametersDto twitterUserFollowsTwitterUserParametersDto)
        {
            var twitterUserFollowsTwitterUsersFromRepo = await _twitterUserFollowsTwitterUserRepository.GetTwitterUserFollowsTwitterUsersAsync(twitterUserFollowsTwitterUserParametersDto);

            var paginationMetadata = new
            {
                totalCount = twitterUserFollowsTwitterUsersFromRepo.TotalCount,
                pageSize = twitterUserFollowsTwitterUsersFromRepo.PageSize,
                currentPageSize = twitterUserFollowsTwitterUsersFromRepo.CurrentPageSize,
                currentStartIndex = twitterUserFollowsTwitterUsersFromRepo.CurrentStartIndex,
                currentEndIndex = twitterUserFollowsTwitterUsersFromRepo.CurrentEndIndex,
                pageNumber = twitterUserFollowsTwitterUsersFromRepo.PageNumber,
                totalPages = twitterUserFollowsTwitterUsersFromRepo.TotalPages,
                hasPrevious = twitterUserFollowsTwitterUsersFromRepo.HasPrevious,
                hasNext = twitterUserFollowsTwitterUsersFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var twitterUserFollowsTwitterUsersDto = _mapper.Map<IEnumerable<TwitterUserFollowsTwitterUserDto>>(twitterUserFollowsTwitterUsersFromRepo);
            var response = new Response<IEnumerable<TwitterUserFollowsTwitterUserDto>>(twitterUserFollowsTwitterUsersDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TwitterUserFollowsTwitterUserDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{twitterUserFollowsTwitterUserId}", Name = "GetTwitterUserFollowsTwitterUser")]
        public async Task<ActionResult<TwitterUserFollowsTwitterUserDto>> GetTwitterUserFollowsTwitterUser(int twitterUserFollowsTwitterUserId)
        {
            var twitterUserFollowsTwitterUserFromRepo = await _twitterUserFollowsTwitterUserRepository.GetTwitterUserFollowsTwitterUserAsync(twitterUserFollowsTwitterUserId);

            if (twitterUserFollowsTwitterUserFromRepo == null)
            {
                return NotFound();
            }

            var twitterUserFollowsTwitterUserDto = _mapper.Map<TwitterUserFollowsTwitterUserDto>(twitterUserFollowsTwitterUserFromRepo);
            var response = new Response<TwitterUserFollowsTwitterUserDto>(twitterUserFollowsTwitterUserDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TwitterUserFollowsTwitterUserDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<TwitterUserFollowsTwitterUserDto>> AddTwitterUserFollowsTwitterUser([FromBody]TwitterUserFollowsTwitterUserForCreationDto twitterUserFollowsTwitterUserForCreation)
        {
            var validationResults = new TwitterUserFollowsTwitterUserForCreationDtoValidator().Validate(twitterUserFollowsTwitterUserForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            var twitterUserFollowsTwitterUser = _mapper.Map<TwitterUserFollowsTwitterUser>(twitterUserFollowsTwitterUserForCreation);
            await _twitterUserFollowsTwitterUserRepository.AddTwitterUserFollowsTwitterUser(twitterUserFollowsTwitterUser);
            var saveSuccessful = await _twitterUserFollowsTwitterUserRepository.SaveAsync();

            if(saveSuccessful)
            {
                var twitterUserFollowsTwitterUserFromRepo = await _twitterUserFollowsTwitterUserRepository.GetTwitterUserFollowsTwitterUserAsync(twitterUserFollowsTwitterUser.TwitterUserId);
                var twitterUserFollowsTwitterUserDto = _mapper.Map<TwitterUserFollowsTwitterUserDto>(twitterUserFollowsTwitterUserFromRepo);
                var response = new Response<TwitterUserFollowsTwitterUserDto>(twitterUserFollowsTwitterUserDto);
                
                return CreatedAtRoute("GetTwitterUserFollowsTwitterUser",
                    new { twitterUserFollowsTwitterUserDto.TwitterUserId },
                    response);
            }

            return StatusCode(500);
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{twitterUserFollowsTwitterUserId}")]
        public async Task<ActionResult> DeleteTwitterUserFollowsTwitterUser(int twitterUserFollowsTwitterUserId)
        {
            var twitterUserFollowsTwitterUserFromRepo = await _twitterUserFollowsTwitterUserRepository.GetTwitterUserFollowsTwitterUserAsync(twitterUserFollowsTwitterUserId);

            if (twitterUserFollowsTwitterUserFromRepo == null)
            {
                return NotFound();
            }

            _twitterUserFollowsTwitterUserRepository.DeleteTwitterUserFollowsTwitterUser(twitterUserFollowsTwitterUserFromRepo);
            await _twitterUserFollowsTwitterUserRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{twitterUserFollowsTwitterUserId}")]
        public async Task<IActionResult> UpdateTwitterUserFollowsTwitterUser(int twitterUserFollowsTwitterUserId, TwitterUserFollowsTwitterUserForUpdateDto twitterUserFollowsTwitterUser)
        {
            var twitterUserFollowsTwitterUserFromRepo = await _twitterUserFollowsTwitterUserRepository.GetTwitterUserFollowsTwitterUserAsync(twitterUserFollowsTwitterUserId);

            if (twitterUserFollowsTwitterUserFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new TwitterUserFollowsTwitterUserForUpdateDtoValidator().Validate(twitterUserFollowsTwitterUser);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            _mapper.Map(twitterUserFollowsTwitterUser, twitterUserFollowsTwitterUserFromRepo);
            _twitterUserFollowsTwitterUserRepository.UpdateTwitterUserFollowsTwitterUser(twitterUserFollowsTwitterUserFromRepo);

            await _twitterUserFollowsTwitterUserRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{twitterUserFollowsTwitterUserId}")]
        public async Task<IActionResult> PartiallyUpdateTwitterUserFollowsTwitterUser(int twitterUserFollowsTwitterUserId, JsonPatchDocument<TwitterUserFollowsTwitterUserForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingTwitterUserFollowsTwitterUser = await _twitterUserFollowsTwitterUserRepository.GetTwitterUserFollowsTwitterUserAsync(twitterUserFollowsTwitterUserId);

            if (existingTwitterUserFollowsTwitterUser == null)
            {
                return NotFound();
            }

            var twitterUserFollowsTwitterUserToPatch = _mapper.Map<TwitterUserFollowsTwitterUserForUpdateDto>(existingTwitterUserFollowsTwitterUser); // map the twitterUserFollowsTwitterUser we got from the database to an updatable twitterUserFollowsTwitterUser model
            patchDoc.ApplyTo(twitterUserFollowsTwitterUserToPatch, ModelState); // apply patchdoc updates to the updatable twitterUserFollowsTwitterUser

            if (!TryValidateModel(twitterUserFollowsTwitterUserToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(twitterUserFollowsTwitterUserToPatch, existingTwitterUserFollowsTwitterUser); // apply updates from the updatable twitterUserFollowsTwitterUser to the db entity so we can apply the updates to the database
            _twitterUserFollowsTwitterUserRepository.UpdateTwitterUserFollowsTwitterUser(existingTwitterUserFollowsTwitterUser); // apply business updates to data if needed

            await _twitterUserFollowsTwitterUserRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}