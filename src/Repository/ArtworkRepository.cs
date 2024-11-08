using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class ArtworkRepository
    {
        private readonly DbSet<Artwork> _artwork;
        private readonly DatabaseContext _databaseContext;

        public ArtworkRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _artwork = databaseContext.Set<Artwork>();
        }

        public async Task<List<Artwork>> GetAllAsync(PaginationOptions paginationOptions)
        {
            //get all
            var artworks = _artwork.Include(o => o.Category).Include(o => o.User).AsQueryable();

            // get by title, category or artist
            if (!string.IsNullOrEmpty(paginationOptions.Search))
            {
                artworks = artworks.Where(a =>
                    a.Title.ToLower().Contains(paginationOptions.Search.ToLower())
                    || a.Category.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                    || a.User.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                );
            }

            // get by low price
            if (paginationOptions.LowPrice.HasValue && paginationOptions.LowPrice > 0)
            {
                artworks = artworks.Where(a => a.Price >= paginationOptions.LowPrice);
            }

            // get by high price
            if (paginationOptions.HighPrice.HasValue && paginationOptions.HighPrice > 0)
            {
                artworks = artworks.Where(a => a.Price <= paginationOptions.HighPrice);
            }

            // sort by title, date, or price
            if (!string.IsNullOrEmpty(paginationOptions.SortOrder))
            {
                artworks = paginationOptions.SortOrder switch
                {
                    "title_desc" => artworks.OrderByDescending(a => a.Title),
                    "date" => artworks.OrderBy(a => a.CreatedAt),
                    "date_desc" => artworks.OrderByDescending(a => a.CreatedAt),
                    "price" => artworks.OrderBy(a => a.Price),
                    "price_desc" => artworks.OrderByDescending(a => a.Price),
                    _ => artworks.OrderBy(a => a.Title),
                };
            }

            //apply pagination
            artworks = artworks
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize);

            return await artworks.ToListAsync();
        }

        public async Task<Artwork?> GetByIdAsync(Guid id)
        {
            return await _artwork
                .Include(a => a.Category)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Artwork>> GetByArtistIdAsync(
            Guid id,
            PaginationOptions paginationOptions
        )
        {
            var artworks = _artwork
                .Include(a => a.Category)
                .Where(a => a.UserId == id)
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .OrderBy(a => a.Title);

            return await artworks.ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _artwork.CountAsync();
        }

        public async Task<int> GetCountByArtistAsync(Guid id)
        {
            return await _artwork.CountAsync(a => a.UserId == id);
        }

        public async Task<Artwork?> CreateOneAsync(Artwork artwork)
        {
            await _artwork.AddAsync(artwork);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(artwork.Id);
        }

        public async Task DeleteOneAsync(Artwork artwork)
        {
            _artwork.Remove(artwork);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<Artwork?> UpdateOneAsync(Artwork artwork)
        {
            _artwork.Update(artwork);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(artwork.Id);
        }
    }
}
