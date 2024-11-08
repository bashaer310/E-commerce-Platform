using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.WorkshopDTO;

namespace Backend_Teamwork.src.Services.workshop
{
    public interface IWorkshopService
    {
        Task<List<WorkshopReadDTO>> GetAllAsync(PaginationOptions paginationOptions);
        Task<List<WorkshopReadDTO>> GetByArtistIdAsync(
            Guid id,
            PaginationOptions paginationOptions
        );
        Task<WorkshopReadDTO?> GetByIdAsync(Guid id);
        Task<int> GetCountAsync();
        Task<int> GetCountByArtistAsync(Guid id);
        Task<WorkshopReadDTO?> CreateOneAsync(Guid artistId, WorkshopCreateDTO createworkshopDto);
        Task DeleteOneAsync(Guid id);
        Task<WorkshopReadDTO?> UpdateOneAsync(Guid id, WorkshopUpdateDTO updateworkshopDto);
    }
}
