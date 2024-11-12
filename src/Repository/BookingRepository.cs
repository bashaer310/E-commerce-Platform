using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class BookingRepository
    {
        private readonly DbSet<Booking> _booking;
        private readonly DatabaseContext _databaseContext;

        public BookingRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _booking = databaseContext.Set<Booking>();
        }

        public async Task<List<Booking>> GetAllAsync(PaginationOptions paginationOptions)
        {
            //  get all
            var booking = _booking
                .Include(b => b.User)
                .Include(b => b.Workshop)
                .ThenInclude(w => w.User)
                .AsQueryable();

            // get by status
            if (!string.IsNullOrEmpty(paginationOptions.Search))
            {
                booking = booking.Where(b =>
                    b.Status.ToString().ToLower().Contains(paginationOptions.Search)
                );
            }

            // sort by amount or date
            if (!string.IsNullOrEmpty(paginationOptions.SortOrder))
            {
                booking = paginationOptions.SortOrder switch
                {
                    "date_desc" => booking.OrderByDescending(b => b.CreatedAt),
                    _ => booking.OrderBy(b => b.CreatedAt)
                };
            }

            
            // apply pagination
            booking = booking
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize);

            return await booking.ToListAsync();
        }

        public async Task<List<Booking>> GetByUserIdAsync(
            Guid userId,
            PaginationOptions paginationOptions
        )
        {
            //  get all
            var booking = _booking.Where(b=>b.UserId == userId)
                .Include(b => b.User)
                .Include(b => b.Workshop)
                .ThenInclude(w => w.User)
                .AsQueryable();

            // get by status
            if (!string.IsNullOrEmpty(paginationOptions.Search))
            {
                booking = booking.Where(b =>
                    b.Status.ToString().ToLower().Contains(paginationOptions.Search)
                );
            }

            // sort by amount or date
            if (!string.IsNullOrEmpty(paginationOptions.SortOrder))
            {
                booking = paginationOptions.SortOrder switch
                {
                    "date_desc" => booking.OrderByDescending(b => b.CreatedAt),
                    _ => booking.OrderBy(b => b.CreatedAt)
                };
            }

            
            // apply pagination
            booking = booking
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize);

            return await booking.ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _booking
                .Include(b => b.User)
                .Include(b => b.Workshop)
                .ThenInclude(w => w.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> GetByUserIdAndWorkshopIdAsync(Guid userId, Guid workshopId)
        {
            return await _booking.AnyAsync(b => b.UserId == userId && b.WorkshopId == workshopId);
        }

        
        public async Task<int> GetCountAsync()
        {
            return await _booking.CountAsync();
        }

        public async Task<int> GetCountByUserIdAsync(Guid id)
        {
            return await _booking.CountAsync(b => b.UserId == id);
        }

        public async Task<Booking?> CreateAsync(Booking booking)
        {
            await _booking.AddAsync(booking);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(booking.Id);
        }

        public async Task<Booking?> UpdateAsync(Booking booking)
        {
            _booking.Update(booking);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(booking.Id);
        }
    }
}
