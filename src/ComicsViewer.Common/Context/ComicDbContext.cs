using ComicsViewer.Common.Resources;
using Microsoft.EntityFrameworkCore;

namespace ComicsViewer.Common.Context
{
    public class ComicDbContext : DbContext
    {
        private readonly string _connetionString;
        public ComicDbContext()
        {
            _connetionString = System.Environment.GetEnvironmentVariable("ConnectionString");
        }
        public ComicDbContext(string connectionString = null)
        {
            if (connectionString == null)
            {
                _connetionString = System.Environment.GetEnvironmentVariable("ConnectionString");
            }
            else
            {
                _connetionString = connectionString;
            }
        }

        public DbSet<Comic> Comics { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<IssuePicture> IssuePicture { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connetionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comic>()
                .HasKey(i => i.Id);
            modelBuilder.Entity<Issue>()
                .HasKey(i => i.Id);
            modelBuilder.Entity<Comic>()
                .HasMany(i => i.Issues)
                .WithOne(i => i.Comic);
            modelBuilder.Entity<Issue>()
                .HasMany(i => i.IssuePictureLinks)
                .WithOne(i => i.Issue);
        }
    }
}
