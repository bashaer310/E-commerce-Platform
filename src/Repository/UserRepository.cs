using Backend_Teamwork.src.Database;
using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;

namespace Backend_Teamwork.src.Repository
{
    public class UserRepository
    {
        private readonly DbSet<User> _user;
        private readonly DatabaseContext _databaseContext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _user = databaseContext.Set<User>();
        }

        public async Task<List<User>> GetAllAsync(PaginationOptions paginationOptions)
        {
            //get all
            var users = _user.ToList();

            //get by name, email, phoneNumber or role
            if (!string.IsNullOrEmpty(paginationOptions.Search))
            {
                users = users
                    .Where(u =>
                        u.Name.ToLower().Contains(paginationOptions.Search.ToLower())
                        || u.Email.ToLower().Contains(paginationOptions.Search.ToLower())
                        || u.PhoneNumber.Contains(paginationOptions.Search)
                        || u.Role.ToString().ToLower().Contains(paginationOptions.Search.ToLower())
                    )
                    .ToList();
            }

            //sort by name or email
            if (!string.IsNullOrEmpty(paginationOptions.SortOrder))
            {
                users = paginationOptions.SortOrder switch
                {
                    "name_desc" => users.OrderByDescending(a => a.Name).ToList(),
                    "email_desc" => users.OrderByDescending(a => a.Email).ToList(),
                    "email_asc" => users.OrderBy(a => a.Email).ToList(),
                    _ => users.OrderBy(a => a.Name).ToList(),
                };
            }

            //apply pagination
            users = users
                .Skip((paginationOptions.PageNumber - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .ToList();

            return users;
        }

        public async Task<int> GetCountAsync()
        {
            return await _user.CountAsync();
        }

        public async Task<User> CreateOneAsync(User user)
        {
            await _user.AddAsync(user);
            await _databaseContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _user.FindAsync(id);
        }

        public async Task DeleteOneAsync(User User)
        {
            _user.Remove(User);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<User?> UpdateOneAsync(User user)
        {
            _user.Update(user);
            await _databaseContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _user.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _user.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<User?> GetByNameAsync(string name)
        {
            return await _user.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
