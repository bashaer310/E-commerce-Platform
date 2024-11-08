using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.UserDTO;
using static Backend_Teamwork.src.Entities.User;

namespace Backend_Teamwork.src.Services.user
{
    public interface IUserService
    {
        Task<List<UserReadDto>> GetAllAsync(PaginationOptions paginationOptions);
        Task<UserReadDto> GetByIdAsync(Guid id);
        Task<int> GetCountAsync();
        Task<string> SignInAsync(UserSigninDto createDto);
        Task<UserReadDto> CreateOneAsync(UserCreateDto createDto, UserRole userRole);
        Task DeleteOneAsync(Guid id);
        Task<UserReadDto> UpdateOneAsync(Guid id, UserUpdateDto updateDto);
    }
}
