
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DatingAppDb(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users {get; set;}
    }
}