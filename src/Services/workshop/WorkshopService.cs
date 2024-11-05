using AutoMapper;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Repository;
using Backend_Teamwork.src.Utils;
using static Backend_Teamwork.src.DTO.WorkshopDTO;

namespace Backend_Teamwork.src.Services.workshop
{
    public class WorkshopService : IWorkshopService
    {
        protected readonly WorkshopRepository _workshopRepo;
        protected readonly IMapper _mapper;

        public WorkshopService(WorkshopRepository workshopRepo, IMapper mapper)
        {
            _workshopRepo = workshopRepo;
            _mapper = mapper;
        }

        public async Task<WorkshopReadDTO> CreateOneAsync(
            Guid artistId,
            WorkshopCreateDTO createworkshopDto
        )
        {
            var workshop = _mapper.Map<WorkshopCreateDTO, Workshop>(createworkshopDto);
            workshop.UserId = artistId;
            var workshopCreated = await _workshopRepo.CreateOneAsync(workshop);
            return _mapper.Map<Workshop, WorkshopReadDTO>(workshopCreated);
        }

        public async Task<List<WorkshopReadDTO>> GetAllAsync()
        {
            var workshopList = await _workshopRepo.GetAllAsync();
            if (workshopList == null)
            {
                throw CustomException.NotFound("Workshops not found");
            }
            return _mapper.Map<List<Workshop>, List<WorkshopReadDTO>>(workshopList);
        }

        public async Task<List<WorkshopReadDTO>> GetAllAsync(PaginationOptions paginationOptions)
        {
            // Validate pagination options
            if (paginationOptions.PageSize <= 0)
            {
                throw CustomException.BadRequest("Page Size should be greater than 0");
            }

            if (paginationOptions.PageNumber <= 0)
            {
                throw CustomException.BadRequest("Page Number should be greater than 0");
            }

            var workshopList = await _workshopRepo.GetAllAsync(paginationOptions);
            if (workshopList == null)
            {
                throw CustomException.NotFound("No workshops found");
            }
            return _mapper.Map<List<Workshop>, List<WorkshopReadDTO>>(workshopList);
        }

        public async Task<WorkshopReadDTO> GetByIdAsync(Guid id)
        {
            var foundworkshop = await _workshopRepo.GetByIdAsync(id);
            if (foundworkshop == null)
            {
                throw CustomException.NotFound($"Workshop with ID {id} not found.");
            }
            return _mapper.Map<Workshop, WorkshopReadDTO>(foundworkshop);
        }

        public async Task<List<WorkshopReadDTO>> GetByArtistIdAsync(
            Guid id,
            PaginationOptions paginationOptions
        )
        {
            var workshopList = await _workshopRepo.GetByArtistIdAsync(id, paginationOptions);
            if (workshopList == null)
            {
                throw CustomException.NotFound("Workshops not found");
            }
            return _mapper.Map<List<Workshop>, List<WorkshopReadDTO>>(workshopList);
        }

        public async Task<int> GetCountAsync()
        {
            return await _workshopRepo.GetCountAsync();
        }

        public async Task<int> GetCountByArtistAsync(Guid id)
        {
            return await _workshopRepo.GetCountByArtistAsync(id);
        }

        public async Task DeleteOneAsync(Guid id)
        {
            var foundworkshop = await _workshopRepo.GetByIdAsync(id);
            if (foundworkshop == null)
            {
                throw CustomException.NotFound($"Workshop with ID {id} not found.");
            }
            await _workshopRepo.DeleteOneAsync(foundworkshop);
        }

        public async Task<WorkshopReadDTO> UpdateOneAsync(
            Guid id,
            WorkshopUpdateDTO workshopupdateDto
        )
        {
            var foundWorkshop = await _workshopRepo.GetByIdAsync(id);
            if (foundWorkshop == null)
            {
                throw CustomException.NotFound($"Workshop with ID {id} not found.");
            }
            _mapper.Map(workshopupdateDto, foundWorkshop);
            var createdWorkshop = await _workshopRepo.UpdateOneAsync(foundWorkshop);
            return _mapper.Map<Workshop, WorkshopReadDTO>(createdWorkshop);
        }
    }
}
