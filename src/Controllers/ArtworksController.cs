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
        public async Task<ActionResult<List<ArtworkReadDto>>> GetAll(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var artworkList = await _artworkService.GetAllAsync(paginationOptions);
            var totalCount = await _artworkService.GetCountAsync();

            var artworkResponse = new { ArtworkList = artworkList, TotalCount = totalCount };
            return Ok(artworkResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ArtworkReadDto>> GetById([FromRoute] Guid id)
        {
            var artwork = await _artworkService.GetByIdAsync(id);
            return Ok(artwork);
        }

        [HttpGet("artist/{artistId:guid}")]
        public async Task<ActionResult<List<ArtworkReadDto>>> GetByArtistId(
            [FromRoute] Guid artistId
        )
        {
            var artwork = await _artworkService.GetByArtistIdAsync(artistId);
            return Ok(artwork);
        }

        [HttpPost]
        [Authorize(Roles = "Artist")]
        public async Task<ActionResult<ArtworkReadDto>> CreateOne(
            [FromBody] ArtworkCreateDto createDto
        )
        {
            var authenticateClaims = HttpContext.User;
            var userId = authenticateClaims
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!
                .Value;
            var userGuid = new Guid(userId);
            var createdArtwork = await _artworkService.CreateOneAsync(userGuid, createDto);
            return Ok(createdArtwork);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult> UpdateOne(
            [FromRoute] Guid id,
            [FromBody] ArtworkUpdateDTO updateDTO
        )
        {
            var artwork = await _artworkService.UpdateOneAsync(id, updateDTO);
            return Ok(artwork);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult> DeleteOne([FromRoute] Guid id)
        {
            await _artworkService.DeleteOneAsync(id);
            return NoContent();
        }
    }
}
