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

        public async Task<Workshop?> CreateOneAsync(Workshop newWorkshop)
        {
            await _workshops.AddAsync(newWorkshop);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(newWorkshop.Id);
        }

        public async Task<Workshop?> GetByIdAsync(Guid id)
        {
            return await _workshops.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task DeleteOneAsync(Workshop deleteWorkshop)
        {
            _workshops.Remove(deleteWorkshop);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<Workshop> UpdateOneAsync(Workshop updateWorkshop)
        {
            _workshops.Update(updateWorkshop);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(updateWorkshop.Id);
        }

        public async Task<List<Workshop>> GetAllAsync()
        {
            return await _workshops.Include(w => w.User).ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _workshops.CountAsync();
        }

        public async Task<List<Workshop>> GetAllAsync(PaginationOptions paginationOptions)
        {
            //get all
            var workshops = _workshops.Include(w => w.User).ToList();

            //get by name, location or artist
            if (!string.IsNullOrEmpty(paginationOptions.Search))
            {
                workshops = workshops
                    .Where(w =>
                        w.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                        || w.Location.ToLower().Contains(paginationOptions.Search.ToLower())
                        || w.User.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                    )
                    .ToList();
            }

            //get by availability
            if (paginationOptions.IsAvailable.HasValue && paginationOptions.IsAvailable == true)
            {
                workshops = workshops.Where(w => w.Availability == true).ToList();
            }

            // get by low price
            if (paginationOptions.LowPrice.HasValue && paginationOptions.LowPrice > 0)
            {
                workshops = workshops.Where(w => w.Price >= paginationOptions.LowPrice).ToList();
            }

            // get by high price
            if (paginationOptions.HighPrice.HasValue && paginationOptions.HighPrice > 0)
            {
                workshops = workshops.Where(w => w.Price <= paginationOptions.HighPrice).ToList();
            }

            // sort by name, location, price, date or capacity
            if (!string.IsNullOrEmpty(paginationOptions.SortOrder))
            {
                workshops = paginationOptions.SortOrder switch
                {
                    "name_desc" => workshops.OrderByDescending(w => w.Name).ToList(),
                    "location_asc" => workshops.OrderBy(w => w.Location).ToList(),
                    "location_desc" => workshops.OrderByDescending(w => w.Location).ToList(),
                    "price_desc" => workshops.OrderByDescending(w => w.Price).ToList(),
                    "price_asc" => workshops.OrderBy(w => w.Price).ToList(),
                    "start_time_desc" => workshops.OrderByDescending(w => w.StartTime).ToList(),
                    "start_time_asc" => workshops.OrderBy(w => w.StartTime).ToList(),
                    "end_time_desc" => workshops.OrderByDescending(w => w.EndTime).ToList(),
                    "end_time_asc" => workshops.OrderBy(w => w.EndTime).ToList(),
                    "date_desc" => workshops.OrderByDescending(w => w.CreatedAt).ToList(),
                    "date_asc" => workshops.OrderBy(w => w.CreatedAt).ToList(),
                    "capacity_desc" => workshops.OrderByDescending(w => w.Capacity).ToList(),
                    _ => workshops.OrderBy(a => a.Name).ToList(),
                };
            }

            //apply pagination
            workshops = workshops
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .ToList();

            return workshops;
        }

        public async Task<List<Workshop>> GetByArtistIdAsync(
            Guid id,
            PaginationOptions paginationOptions
        )
        {
            var workshops = _workshops.Where(w => w.UserId == id).ToList();

            return workshops
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .OrderBy(w => w.Name)
                .ToList();
        }

        public async Task<int> GetCountByArtistAsync(Guid id)
        {
            return _workshops.Where(a => a.UserId == id).ToList().Count();
        }
    }
}
