using Microsoft.EntityFrameworkCore;

using BooksAPI.Models;

namespace BooksAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Read> Reads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("books")
                .HasMany(b => b.Reads)
                .WithOne(r => r.book)
                .HasForeignKey(r => r.book_id);
            modelBuilder.Entity<User>().ToTable("users")
                .HasMany(u => u.Reads)
                .WithOne(r => r.user)
                .HasForeignKey(r => r.user_id);
            modelBuilder.Entity<Read>().ToTable("reads")
                .HasOne(r => r.book)
                .WithMany(b => b.Reads)
                .HasForeignKey(r => r.book_id);
        }
    }
}
