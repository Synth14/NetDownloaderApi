using Microsoft.EntityFrameworkCore;
using NetDownloader.Entity.Models;
using Microsoft.Extensions.Configuration;

namespace NetDownloader.Entity.Context
{
    /// <summary>
    /// Represents the application's database context.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the collection of hosts in the database.
        /// </summary>
        public DbSet<Hosts> HostItems { get; set; }

        /// <summary>
        /// Gets or sets the collection of tags in the database.
        /// </summary>
        public DbSet<Tags> TagItems { get; set; }

        /// <summary>
        /// Gets or sets the collection of accounts in the database.
        /// </summary>
        public DbSet<Accounts> AccountItems { get; set; }

        /// <summary>
        /// Gets or sets the collection of links in the database.
        /// </summary>
        public DbSet<Links> LinkItems { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        public ApplicationDbContext() : base()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=../NetDownloader.Entity/Db/NetDownloader.db");
            }
        }

    }
}
