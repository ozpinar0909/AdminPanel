using AdminPanel.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AdminPanel.DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
