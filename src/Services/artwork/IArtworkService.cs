using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.ArtworkDTO;

namespace Backend_Teamwork.src.Services.artwork
{
    public interface IArtworkService
    {
        Task<List<ArtworkReadDto>> GetAllAsync(PaginationOptions paginationOptions);
        Task<ArtworkReadDto> GetByIdAsync(Guid id);
        Task<List<ArtworkReadDto>> GetByArtistIdAsync(Guid id);
        Task<int> GetCountAsync();
        Task<ArtworkReadDto> CreateOneAsync(Guid userId, ArtworkCreateDto artwork);
        Task DeleteOneAsync(Guid id);
        Task<ArtworkReadDto> UpdateOneAsync(Guid id, ArtworkUpdateDTO updateArtwork);
    }
}
