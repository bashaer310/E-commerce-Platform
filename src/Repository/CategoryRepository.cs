using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class CategoryRepository
    {
        private readonly DbSet<Category> _category;
        private readonly DatabaseContext _databaseContext;

        public CategoryRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _category = databaseContext.Set<Category>();
        }

        public async Task<List<Category>> GetAllAsync(PaginationOptions paginationOptions)
        {
            //get all
            var categories = _category.AsQueryable();

            //get by name
            if (!string.IsNullOrEmpty(paginationOptions.Search))
            {
                categories = categories.Where(c =>
                    c.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                );
            }

            categories = _category
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .OrderBy(c => c.Name);

            return await categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _category.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> GetByNameAsync(string name)
        {
            return await _category.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<int> GetCountAsync()
        {
            return await _category.CountAsync();
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _category.AddAsync(category);
            await _databaseContext.SaveChangesAsync();
            return category;
        }

        public async Task DeleteAsync(Category category)
        {
            _category.Remove(category);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _category.Update(category);
            await _databaseContext.SaveChangesAsync();
            return category;
        }
    }
}
