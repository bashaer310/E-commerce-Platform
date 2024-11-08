using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class WorkshopRepository
    {
        private readonly DbSet<Workshop> _workshops;
        private readonly DatabaseContext _databaseContext;

        public WorkshopRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _workshops = databaseContext.Set<Workshop>();
        }

        public async Task<List<Workshop>> GetAllAsync(PaginationOptions paginationOptions)
        {
            //get all
            var workshops = _workshops.Include(w => w.User).AsQueryable();

            //get by name, location or artist
            if (!string.IsNullOrEmpty(paginationOptions.Search))
            {
                workshops = workshops.Where(w =>
                    w.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                    || w.Location.ToLower().Contains(paginationOptions.Search.ToLower())
                    || w.User.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                );
            }

            //get by availability
            if (paginationOptions.IsAvailable.HasValue && paginationOptions.IsAvailable == true)
            {
                workshops = workshops.Where(w => w.Availability == true);
            }

            // get by low price
            if (paginationOptions.LowPrice.HasValue && paginationOptions.LowPrice > 0)
            {
                workshops = workshops.Where(w => w.Price >= paginationOptions.LowPrice);
            }

            // get by high price
            if (paginationOptions.HighPrice.HasValue && paginationOptions.HighPrice > 0)
            {
                workshops = workshops.Where(w => w.Price <= paginationOptions.HighPrice);
            }

            // sort by name, location, price, date or capacity
            if (!string.IsNullOrEmpty(paginationOptions.SortOrder))
            {
                workshops = paginationOptions.SortOrder switch
                {
                    "name_desc" => workshops.OrderByDescending(w => w.Name),
                    "location_asc" => workshops.OrderBy(w => w.Location),
                    "location_desc" => workshops.OrderByDescending(w => w.Location),
                    "price_desc" => workshops.OrderByDescending(w => w.Price),
                    "price_asc" => workshops.OrderBy(w => w.Price),
                    "start_time_desc" => workshops.OrderByDescending(w => w.StartTime),
                    "start_time_asc" => workshops.OrderBy(w => w.StartTime),
                    "end_time_desc" => workshops.OrderByDescending(w => w.EndTime),
                    "end_time_asc" => workshops.OrderBy(w => w.EndTime),
                    "date_desc" => workshops.OrderByDescending(w => w.CreatedAt),
                    "date_asc" => workshops.OrderBy(w => w.CreatedAt),
                    "capacity_desc" => workshops.OrderByDescending(w => w.Capacity),
                    _ => workshops.OrderBy(a => a.Name),
                };
            }

            //apply pagination
            workshops = workshops
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize);

            return await workshops.ToListAsync();
        }

        public async Task<List<Workshop>> GetByArtistIdAsync(
            Guid id,
            PaginationOptions paginationOptions
        )
        {
            var workshops = _workshops
                .Where(w => w.UserId == id)
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .OrderBy(w => w.Name);

            return await workshops.ToListAsync();
        }

        public async Task<Workshop?> GetByIdAsync(Guid id)
        {
            return await _workshops.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<int> GetCountByArtistAsync(Guid id)
        {
            return await _workshops.CountAsync(a => a.UserId == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _workshops.CountAsync();
        }

        public async Task<Workshop?> CreateOneAsync(Workshop workshop)
        {
            await _workshops.AddAsync(workshop);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(workshop.Id);
        }

        public async Task DeleteOneAsync(Workshop workshop)
        {
            _workshops.Remove(workshop);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<Workshop?> UpdateOneAsync(Workshop workshop)
        {
            _workshops.Update(workshop);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(workshop.Id);
        }
    }
}
