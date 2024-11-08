using System.Text.Json;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Services.category;
using Backend_Teamwork.src.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Backend_Teamwork.src.DTO.CategoryDTO;

namespace Backend_Teamwork.src.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryReadDto>>> GetCategories(
            [FromQuery] PaginationOptions paginationOptions
        )
        {
            var categoryList = await _categoryService.GetAllAsync(paginationOptions);
            var totalCount = await _categoryService.GetCountAsync();

            var categoryResponse = new { CategoryList = categoryList, TotalCount = totalCount };
            return Ok(categoryResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryReadDto>> GetCategoryById([FromRoute] Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryReadDto>> CreateCategory(
            [FromBody] CategoryCreateDto categoryDTO
        )
        {
            var category = await _categoryService.CreateAsync(categoryDTO);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCategory([FromRoute] Guid id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryReadDto>> UpdateCategory(
            [FromRoute] Guid id,
            [FromBody] CategoryUpdateDto categoryDTO
        )
        {
            var category = await _categoryService.UpdateAsync(id, categoryDTO);
            return Ok(category);
        }
    }
}
