
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        //Note every edit to the datacontext should always execute the command for migration
        //dotnet ef migrations add AddedUserEntity
        //dotnet ef database update
        public DataContext(DbContextOptions<DataContext> option) : base(option)
        {
        }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }

    }
}