using System.Security.Claims;
using Backend_Teamwork.src.Services.user;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.UserDTO;
using static Backend_Teamwork.src.Entities.User;

namespace Backend_Teamwork.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService service)
        {
            _userService = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserReadDto>>> GetUsers(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var userList = await _userService.GetAllAsync(paginationOptions);
            var totalCount = await _userService.GetCountAsync();
            var userResponse = new { UserList = userList, TotalCount = totalCount };

            return Ok(userResponse);
        }

        [HttpGet("artists")]
        public async Task<ActionResult<List<UserReadDto>>> GetArtists(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            paginationOptions.Search = UserRole.Artist.ToString();
            var artistList = await _userService.GetAllAsync(paginationOptions);
            var totalCount = await _userService.GetCountAsync();

            var artistResponse = new { ArtistList = artistList, TotalCount = totalCount };
            return Ok(artistResponse);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReadDto>> GetUserById([FromRoute] Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<UserReadDto>> GetProfileInformation()
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var convertedUserId = new Guid(userId);
            var user = await _userService.GetByIdAsync(convertedUserId);
            return Ok(user);
        }

        [HttpPost("artist")]
        public async Task<ActionResult<UserReadDto>> SignUpArtist(
            [FromBody] UserCreateDto createDto
        )
        {
            var user = await _userService.CreateOneAsync(createDto, UserRole.Artist);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPost("customer")]
        public async Task<ActionResult<UserReadDto>> SignUpCustomer(
            [FromBody] UserCreateDto createDto
        )
        {
            var user = await _userService.CreateOneAsync(createDto, UserRole.Customer);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPost("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReadDto>> CreateAdmin([FromBody] UserCreateDto createDto)
        {
            var user = await _userService.CreateOneAsync(createDto, UserRole.Admin);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpPost("signin")]
        public async Task<ActionResult<string>> SignIn([FromBody] UserSigninDto signinDto)
        {
            var token = await _userService.SignInAsync(signinDto);
            return Ok(token);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
        {
            await _userService.DeleteOneAsync(id);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReadDto>> UpdateUser(
            [FromRoute] Guid id,
            [FromBody] UserUpdateDto updateDto
        )
        {
            var user = await _userService.UpdateOneAsync(id, updateDto);
            return Ok(user);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<ActionResult<UserReadDto>> UpdateProfileInformation(
            [FromBody] UserUpdateDto updateDto
        )
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var convertedUserId = new Guid(userId);
            var updatedUser = await _userService.UpdateOneAsync(convertedUserId, updateDto);
            return Ok(updatedUser);
        }
    }
}
