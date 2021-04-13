namespace WebApi.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using Application.Dtos.TwitterUser;
    using Application.Interfaces.TwitterUser;
    using Application.Validation.TwitterUser;
    using Domain.Entities;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;
    using Application.Wrappers;

    [ApiController]
    [Route("api/TwitterUsers")]
    [ApiVersion("1.0")]
    public class TwitterUsersController: Controller
    {
        private readonly ITwitterUserRepository _twitterUserRepository;
        private readonly IMapper _mapper;

        public TwitterUsersController(ITwitterUserRepository twitterUserRepository
            , IMapper mapper)
        {
            _twitterUserRepository = twitterUserRepository ??
                throw new ArgumentNullException(nameof(twitterUserRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        
                                                                                                                                                    [ProducesResponseType(typeof(Response<IEnumerable<TwitterUserDto>>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet(Name = "GetTwitterUsers")]
        public async Task<IActionResult> GetTwitterUsers([FromQuery] TwitterUserParametersDto twitterUserParametersDto)
        {
            var twitterUsersFromRepo = await _twitterUserRepository.GetTwitterUsersAsync(twitterUserParametersDto);

            var paginationMetadata = new
            {
                totalCount = twitterUsersFromRepo.TotalCount,
                pageSize = twitterUsersFromRepo.PageSize,
                currentPageSize = twitterUsersFromRepo.CurrentPageSize,
                currentStartIndex = twitterUsersFromRepo.CurrentStartIndex,
                currentEndIndex = twitterUsersFromRepo.CurrentEndIndex,
                pageNumber = twitterUsersFromRepo.PageNumber,
                totalPages = twitterUsersFromRepo.TotalPages,
                hasPrevious = twitterUsersFromRepo.HasPrevious,
                hasNext = twitterUsersFromRepo.HasNext
            };

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(paginationMetadata));

            var twitterUsersDto = _mapper.Map<IEnumerable<TwitterUserDto>>(twitterUsersFromRepo);
            var response = new Response<IEnumerable<TwitterUserDto>>(twitterUsersDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TwitterUserDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpGet("{twitterUserId}", Name = "GetTwitterUser")]
        public async Task<ActionResult<TwitterUserDto>> GetTwitterUser(int twitterUserId)
        {
            var twitterUserFromRepo = await _twitterUserRepository.GetTwitterUserAsync(twitterUserId);

            if (twitterUserFromRepo == null)
            {
                return NotFound();
            }

            var twitterUserDto = _mapper.Map<TwitterUserDto>(twitterUserFromRepo);
            var response = new Response<TwitterUserDto>(twitterUserDto);

            return Ok(response);
        }
        
                                      [ProducesResponseType(typeof(Response<TwitterUserDto>), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<TwitterUserDto>> AddTwitterUser([FromBody]TwitterUserForCreationDto twitterUserForCreation)
        {
            var validationResults = new TwitterUserForCreationDtoValidator().Validate(twitterUserForCreation);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            var twitterUser = _mapper.Map<TwitterUser>(twitterUserForCreation);
            await _twitterUserRepository.AddTwitterUser(twitterUser);
            var saveSuccessful = await _twitterUserRepository.SaveAsync();

            if(saveSuccessful)
            {
                var twitterUserFromRepo = await _twitterUserRepository.GetTwitterUserAsync(twitterUser.TwitterUserId);
                var twitterUserDto = _mapper.Map<TwitterUserDto>(twitterUserFromRepo);
                var response = new Response<TwitterUserDto>(twitterUserDto);
                
                return CreatedAtRoute("GetTwitterUser",
                    new { twitterUserDto.TwitterUserId },
                    response);
            }

            return StatusCode(500);
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpDelete("{twitterUserId}")]
        public async Task<ActionResult> DeleteTwitterUser(int twitterUserId)
        {
            var twitterUserFromRepo = await _twitterUserRepository.GetTwitterUserAsync(twitterUserId);

            if (twitterUserFromRepo == null)
            {
                return NotFound();
            }

            _twitterUserRepository.DeleteTwitterUser(twitterUserFromRepo);
            await _twitterUserRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        [HttpPut("{twitterUserId}")]
        public async Task<IActionResult> UpdateTwitterUser(int twitterUserId, TwitterUserForUpdateDto twitterUser)
        {
            var twitterUserFromRepo = await _twitterUserRepository.GetTwitterUserAsync(twitterUserId);

            if (twitterUserFromRepo == null)
            {
                return NotFound();
            }

            var validationResults = new TwitterUserForUpdateDtoValidator().Validate(twitterUser);
            validationResults.AddToModelState(ModelState, null);

            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
                         }

            _mapper.Map(twitterUser, twitterUserFromRepo);
            _twitterUserRepository.UpdateTwitterUser(twitterUserFromRepo);

            await _twitterUserRepository.SaveAsync();

            return NoContent();
        }
        
                                      [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPatch("{twitterUserId}")]
        public async Task<IActionResult> PartiallyUpdateTwitterUser(int twitterUserId, JsonPatchDocument<TwitterUserForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var existingTwitterUser = await _twitterUserRepository.GetTwitterUserAsync(twitterUserId);

            if (existingTwitterUser == null)
            {
                return NotFound();
            }

            var twitterUserToPatch = _mapper.Map<TwitterUserForUpdateDto>(existingTwitterUser); // map the twitterUser we got from the database to an updatable twitterUser model
            patchDoc.ApplyTo(twitterUserToPatch, ModelState); // apply patchdoc updates to the updatable twitterUser

            if (!TryValidateModel(twitterUserToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(twitterUserToPatch, existingTwitterUser); // apply updates from the updatable twitterUser to the db entity so we can apply the updates to the database
            _twitterUserRepository.UpdateTwitterUser(existingTwitterUser); // apply business updates to data if needed

            await _twitterUserRepository.SaveAsync(); // save changes in the database

            return NoContent();
        }
    }
}