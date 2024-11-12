using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.OrderDTO;

namespace Backend_Teamwork.src.Services.order
{
    public interface IOrderService
    {
        Task<List<OrderReadDto>> GetAllAsync(PaginationOptions paginationOptions);
        Task<List<OrderReadDto>> GetAllByUserIdAsync(PaginationOptions paginationOptions, Guid id);
        Task<OrderReadDto> GetByIdAsync(Guid id);
        Task<int> GetCountAsync();
        Task<int> GetCountByUserIdAsync(Guid id);
        Task<OrderReadDto> CreateOneAsync(Guid id, OrderCreateDto createDto);
        Task<OrderReadDto> UpdateStatusToShippedAsync(Guid id);
        Task<OrderReadDto> UpdateStatusToDeliveredAsync(Guid id);
        Task<OrderReadDto> UpdateStatusToCanceledAsync(Guid id);
    }
}
