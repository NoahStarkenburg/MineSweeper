using Microsoft.EntityFrameworkCore;
using MineSweeper.Models;
using MineSweeper.Models.DAOs;

namespace MineSweeper.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
} 