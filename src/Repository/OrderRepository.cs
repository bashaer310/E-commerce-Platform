using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class OrderRepository
    {
        private readonly DbSet<Order> _order;
        private readonly DatabaseContext _databaseContext;

        public OrderRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _order = databaseContext.Set<Order>();
        }

        public async Task<List<Order>> GetAllAsync(PaginationOptions paginationOptions)
        {
            //  get all
            var order = _order
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Artwork)
                .ThenInclude(a => a.Category)
                .AsQueryable();

            // get by status or address
            if (!string.IsNullOrEmpty(paginationOptions.Search))
            {
                order = order
                    .Where(o =>
                        o.ShippingAddress.ToLower().Contains(paginationOptions.Search)
                        || o.Status.ToString().ToLower().Contains(paginationOptions.Search)
                    );
            }

            // sort by amount or date
            if (!string.IsNullOrEmpty(paginationOptions.SortOrder))
            {
                order = paginationOptions.SortOrder switch
                {
                    "amount_desc" => order.OrderByDescending(o => o.TotalAmount),
                    "amount_asc" => order.OrderBy(o => o.TotalAmount),
                    "date_desc" => order.OrderByDescending(o => o.CreatedAt),
                    "date_asc" => order.OrderBy(o => o.CreatedAt),
                    _ => order.OrderBy(o => o.ShippingAddress),
                };
            }

            // apply pagination
            order = order
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize);

            return await order.ToListAsync();
        }

        public async Task<List<Order>> GetByUserAsync(
            PaginationOptions paginationOptions,
            Guid userId
        )
        {
            //  get all
            var order = _order
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Artwork)
                .ThenInclude(a => a.Category)
                .Where(order => order.UserId == userId)
                .AsQueryable();

            // get by status or address
            if (!string.IsNullOrEmpty(paginationOptions.Search))
            {
                order = order
                    .Where(o =>
                        o.ShippingAddress.ToLower().Contains(paginationOptions.Search)
                        || o.Status.ToString().ToLower().Contains(paginationOptions.Search)
                    );
            }

            // sort by amount or date
            if (!string.IsNullOrEmpty(paginationOptions.SortOrder))
            {
                order = paginationOptions.SortOrder switch
                {
                    "amount_desc" => order.OrderByDescending(o => o.TotalAmount),
                    "amount_asc" => order.OrderBy(o => o.TotalAmount),
                    "date_desc" => order.OrderByDescending(o => o.CreatedAt),
                    "date_asc" => order.OrderBy(o => o.CreatedAt),
                    _ => order.OrderBy(o => o.ShippingAddress),
                };
            }

            // apply pagination
            order = order
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize);

            return await order.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _order
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Artwork)
                .ThenInclude(a => a.Category)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _order.CountAsync();
        }

        public async Task<int> GetCountByUserIdAsync(Guid id)
        {
            return await _order.CountAsync(a => a.UserId == id);
        }

        public async Task<Order?> CreateOneAsync(Order order)
        {
            await _order.AddAsync(order);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(order.Id);
        }

        public async Task<Order?> UpdateOneAsync(Order order)
        {
            _order.Update(order);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(order.Id);
        }
    }
}
