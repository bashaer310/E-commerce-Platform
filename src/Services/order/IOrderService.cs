using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.OrderDTO;

namespace Backend_Teamwork.src.Services.order
{
    public interface IOrderService
    {
        Task<List<OrderReadDto>> GetAllAsync(PaginationOptions paginationOptions);
        Task<List<OrderReadDto>> GetAllAsync(PaginationOptions paginationOptions, Guid id);
        Task<OrderReadDto> GetByIdAsync(Guid id);
        Task<int> GetCountAsync();
        Task<int> GetCountByCustomerAsync(Guid id);
        Task<OrderReadDto> CreateOneAsync(Guid id, OrderCreateDto createDto);
        Task DeleteOneAsync(Guid id);
        Task<OrderReadDto> UpdateOneAsync(Guid id, OrderUpdateDto updateDto);
    }
}
