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
            var users = await _userService.GetAllAsync(paginationOptions);
            return Ok(users);
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
        public async Task<ActionResult<UserReadDto>> GetInformationById()
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var convertedUserId = new Guid(userId);
            var user = await _userService.GetByIdAsync(convertedUserId);
            return Ok(user);
        }

        [HttpGet("email/{email:alpha}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReadDto>> GetByEmail([FromRoute] string email)
        {
            var user = await _userService.GetByEmailAsync(email);
            return Ok(user);
        }

        [HttpGet("count")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> GetTotalUsersCount()
        {
            var count = await _userService.GetTotalUsersCountAsync();
            return Ok(count);
        }

        [HttpPost("artist/signup")]
        public async Task<ActionResult<UserReadDto>> SignUpArtist(
            [FromBody] UserCreateDto createDto
        )
        {
            var user = await _userService.CreateOneAsync(createDto, UserRole.Artist);
            return CreatedAtAction(nameof(SignUpArtist), new { id = user.Id }, user);
        }

        [HttpPost("customer/signup")]
        public async Task<ActionResult<UserReadDto>> SignUpCustomer(
            [FromBody] UserCreateDto createDto
        )
        {
            var user = await _userService.CreateOneAsync(createDto, UserRole.Customer);
            return CreatedAtAction(nameof(SignUpCustomer), new { id = user.Id }, user);
        }

        [HttpPost("create-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserReadDto>> CreateAdmin([FromBody] UserCreateDto createDto)
        {
            var user = await _userService.CreateOneAsync(createDto, UserRole.Admin);
            return CreatedAtAction(nameof(CreateAdmin), new { id = user.Id }, user);
        }

        [HttpPost("signin")]
        public async Task<ActionResult<string>> SignIn([FromBody] UserSigninDto signinDto)
        {
            var token = await _userService.SignInAsync(signinDto);
            return Ok(token);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UpdateUser(
            [FromRoute] Guid id,
            [FromBody] UserUpdateDto updateDto
        )
        {
            await _userService.UpdateOneAsync(id, updateDto);
            return NoContent();
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<ActionResult<bool>> UpdateProfileInformation(
            [FromBody] UserUpdateDto updateDto
        )
        {
            var authClaims = HttpContext.User;
            var userId = authClaims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var convertedUserId = new Guid(userId);
            await _userService.UpdateOneAsync(convertedUserId, updateDto);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> DeleteUser([FromRoute] Guid id)
        {
            await _userService.DeleteOneAsync(id);
            return NoContent();
        }
    }
}
