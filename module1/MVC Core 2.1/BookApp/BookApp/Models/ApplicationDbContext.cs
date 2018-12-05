using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        // to update or create DB create migration evry db change
        //In nuget manager type add-migration AddBookModel
        //After the migration created type update-database
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Book> Books { get; set; }
    }
}
