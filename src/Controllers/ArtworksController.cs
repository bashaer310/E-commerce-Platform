using System.Security.Claims;
using Backend_Teamwork.src.DTO;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Services.artwork;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.ArtworkDTO;

namespace Backend_Teamwork.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ArtworksController : ControllerBase
    {
        private readonly IArtworkService _artworkService;

        public ArtworksController(IArtworkService service)
        {
            _artworkService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ArtworkReadDto>>> GetArtworks(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var artworkList = await _artworkService.GetAllAsync(paginationOptions);
            var totalCount = await _artworkService.GetCountAsync();

            var artworkResponse = new { ArtworkList = artworkList, TotalCount = totalCount };
            return Ok(artworkResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ArtworkReadDto>> GetArtworkById([FromRoute] Guid id)
        {
            var artwork = await _artworkService.GetByIdAsync(id);
            return Ok(artwork);
        }

        [HttpGet("artist")]
        [Authorize]
        public async Task<ActionResult<List<ArtworkReadDto>>> GetArtworksByArtistId(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var authenticateClaims = HttpContext.User;
            var userId = authenticateClaims
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!
                .Value;
            var userGuid = new Guid(userId);
            var artworkList = await _artworkService.GetByArtistIdAsync(userGuid, paginationOptions);
            var totalCount = await _artworkService.GetCountByArtistAsync(userGuid);

            var artworkResponse = new { ArtworkList = artworkList, TotalCount = totalCount };
            return Ok(artworkResponse);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult<ArtworkReadDto>> CreateArtwork(
            [FromBody] ArtworkCreateDto createDto
        )
        {
            var authenticateClaims = HttpContext.User;
            var userId = authenticateClaims
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!
                .Value;
            var userGuid = new Guid(userId);
            var artwork = await _artworkService.CreateOneAsync(userGuid, createDto);
            return CreatedAtAction(nameof(GetArtworkById), new { id = artwork.Id }, artwork);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult> DeleteArtwork([FromRoute] Guid id)
        {
            await _artworkService.DeleteOneAsync(id);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult<ArtworkReadDto>> UpdateArtwork(
            [FromRoute] Guid id,
            [FromBody] ArtworkUpdateDTO updateDTO
        )
        {
            var artwork = await _artworkService.UpdateOneAsync(id, updateDTO);
            return Ok(artwork);
        }
    }
}
