using Backend_Teamwork.src.Entities;
using Backend_Teamwork.src.Utils;
using Microsoft.EntityFrameworkCore;
using static Backend_Teamwork.src.Entities.User;

namespace Backend_Teamwork.src.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<Artwork> Artwork { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetails> OrderDetail { get; set; }
        public DbSet<Workshop> Workshop { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Booking> Booking { get; set; }

        public DatabaseContext(DbContextOptions option)
            : base(option) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<UserRole>();
            modelBuilder.HasPostgresEnum<BookingStatus>();
            modelBuilder.HasPostgresEnum<OrderStatus>();


            modelBuilder.Entity<User>().HasIndex(x => x.PhoneNumber).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();

            modelBuilder
                .Entity<User>()
                .HasData(
                    new User
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin",
                        PhoneNumber = "+966563034770",
                        Email = "admin@artify.com",
                        Password = PasswordUtils.HashPassword(
                            "12345678",
                            out string hashedPassword,
                            out byte[] salt
                        ),
                        Role = UserRole.Admin,
                        Salt = salt,
                    }
                );
        }
    }
}
