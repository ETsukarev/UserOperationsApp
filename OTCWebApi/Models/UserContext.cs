using Microsoft.EntityFrameworkCore;

namespace OTCWebApi.Models
{
    public sealed class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User { Login = "admin", Password = "password", FirstName = "Иван",
                MiddleName = "Иванович", LastName = "Иванов", IsAdmin = true, Telephone = "7-777-7777777", Id = 1});
        }

    }
}
