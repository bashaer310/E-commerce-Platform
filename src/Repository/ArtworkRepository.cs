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
            var artworks = _artwork.Include(o => o.Category).Include(o => o.User).ToList();

            // get by title, category or artist
            if (!string.IsNullOrEmpty(paginationOptions.Search))
            {
                artworks = artworks
                    .Where(a =>
                        a.Title.ToLower().Contains(paginationOptions.Search.ToLower())
                        || a.Category.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                        || a.User.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                    )
                    .ToList();
            }

            // get by low price
            if (paginationOptions.LowPrice.HasValue && paginationOptions.LowPrice > 0)
            {
                artworks = artworks.Where(a => a.Price >= paginationOptions.LowPrice).ToList();
            }

            // get by high price
            if (paginationOptions.HighPrice.HasValue && paginationOptions.HighPrice > 0)
            {
                artworks = artworks.Where(a => a.Price <= paginationOptions.HighPrice).ToList();
            }

            // sort by title, date, or price
            if (!string.IsNullOrEmpty(paginationOptions.SortOrder))
            {
                artworks = paginationOptions.SortOrder switch
                {
                    "title_desc" => artworks.OrderByDescending(a => a.Title).ToList(),
                    "date" => artworks.OrderBy(a => a.CreatedAt).ToList(),
                    "date_desc" => artworks.OrderByDescending(a => a.CreatedAt).ToList(),
                    "price" => artworks.OrderBy(a => a.Price).ToList(),
                    "price_desc" => artworks.OrderByDescending(a => a.Price).ToList(),
                    _ => artworks.OrderBy(a => a.Title).ToList(),
                };
            }

            //apply pagination
            artworks = artworks
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .ToList();

            return artworks;
        }

        public async Task<Artwork?> GetByIdAsync(Guid id)
        {
            return await _artwork
                .Include(a => a.Category)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Artwork>> GetByArtistIdAsync(Guid id)
        {
            return await _artwork.Include(a => a.Category).Where(a => a.UserId == id).ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _artwork.CountAsync();
        }

        public async Task<Artwork?> CreateOneAsync(Artwork newArtwork)
        {
            await _artwork.AddAsync(newArtwork);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(newArtwork.Id);
        }

        public async Task DeleteOneAsync(Artwork artwork)
        {
            _artwork.Remove(artwork);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<Artwork?> UpdateOneAsync(Artwork updateArtwork)
        {
            _artwork.Update(updateArtwork);
            await _databaseContext.SaveChangesAsync();
            return await GetByIdAsync(updateArtwork.Id);
        }
    }
}
