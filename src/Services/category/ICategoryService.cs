using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.CategoryDTO;

namespace Backend_Teamwork.src.Services.category
{
    public interface ICategoryService
    {
        Task<List<CategoryReadDto>> GetAllAsync(PaginationOptions paginationOptions);
        Task<CategoryReadDto> GetByIdAsync(Guid id);
        Task<int> GetCountAsync();
        Task<CategoryReadDto> CreateAsync(CategoryCreateDto category);
        Task DeleteAsync(Guid id);
        Task<CategoryReadDto> UpdateAsync(Guid id, CategoryUpdateDto category);
    }
}
