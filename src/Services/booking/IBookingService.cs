using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.BookingDTO;

namespace Backend_Teamwork.src.Services.booking
{
    public interface IBookingService
    {
        Task<List<BookingReadDto>> GetAllAsync(PaginationOptions paginationOptions);
        Task<List<BookingReadDto>> GetAllByUserIdAsync(
            PaginationOptions paginationOptions,
            Guid userId
        );
        Task<int> GetCountAsync();
        Task<int> GetCountByUserIdAsync(Guid id);

        Task<BookingReadDto> GetByIdAsync(Guid id, Guid userId, string userRole);
        Task<BookingReadDto> CreateAsync(BookingCreateDto booking, Guid userId);
        Task<BookingReadDto> UpdateStatusToConfirmedAsync(Guid id);
        Task<BookingReadDto> UpdateStatusToCanceledAsync(Guid id, Guid userId);
    }
}
