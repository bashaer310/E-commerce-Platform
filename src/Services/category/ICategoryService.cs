using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.CategoryDTO;

namespace Backend_Teamwork.src.Services.category
{
    public interface ICategoryService
    {
        Task<List<CategoryReadDto>> GetAllAsync();
        Task<CategoryReadDto> GetByIdAsync(Guid id);
        Task<List<CategoryReadDto>> GetWithPaginationAsync(PaginationOptions paginationOptions);
        Task<CategoryReadDto> CreateAsync(CategoryCreateDto category);
        Task<CategoryReadDto> UpdateAsync(Guid id, CategoryUpdateDto category);
        Task DeleteAsync(Guid id);
    }
}
