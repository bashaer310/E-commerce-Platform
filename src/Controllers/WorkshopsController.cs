using System.Security.Claims;
using Backend_Teamwork.src.Services.workshop;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.WorkshopDTO;

namespace sda_3_online_Backend_Teamwork.src.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WorkshopsController : ControllerBase
    {
        private readonly IWorkshopService _workshopService;

        public WorkshopsController(IWorkshopService service)
        {
            _workshopService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkshopReadDTO>>> GetWorkshop()
        {
            var workshops = await _workshopService.GetAllAsync();
            return Ok(workshops);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<WorkshopReadDTO>> GetWorkshopById([FromRoute] Guid id)
        {
            var workshop = await _workshopService.GetByIdAsync(id);
            return Ok(workshop);
        }

        [HttpGet("page")]
        public async Task<ActionResult<WorkshopReadDTO>> GetWorkShopByPage(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var workshopList = await _workshopService.GetAllAsync(paginationOptions);
            var totalCount = await _workshopService.GetCountAsync();

            var workshopResponse = new { WorkshopList = workshopList, TotalCount = totalCount };
            return Ok(workshopResponse);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult<WorkshopReadDTO>> CreateWorkshop(
            [FromBody] WorkshopCreateDTO createDto
        )
        {
            var authenticateClaims = HttpContext.User;
            var userId = authenticateClaims
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!
                .Value;
            var userGuid = new Guid(userId);

            var workshopCreated = await _workshopService.CreateOneAsync(userGuid, createDto);
            return CreatedAtAction(
                nameof(CreateWorkshop),
                new { id = workshopCreated.Id },
                workshopCreated
            );
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult> DeleteWorkshop([FromRoute] Guid id)
        {
            await _workshopService.DeleteOneAsync(id);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Artist")]
        public async Task<ActionResult<WorkshopReadDTO>> UpdateWorkshop(
            Guid id,
            [FromBody] WorkshopUpdateDTO updateDto
        )
        {
            var updateWorkshop = await _workshopService.UpdateOneAsync(id, updateDto);
            return Ok(updateWorkshop);
        }
    }
}
