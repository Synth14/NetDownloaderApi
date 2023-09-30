using Microsoft.EntityFrameworkCore;
using NetDownloader.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDownloader.Entity.Context
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Hosts> HostItems { get; set; }
        public DbSet<Tags> TagItems { get; set; }
        public DbSet<Accounts> AccountItems { get; set; }
        public DbSet<Links> LinkItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

    }
}
