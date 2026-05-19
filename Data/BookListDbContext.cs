using Microsoft.EntityFrameworkCore;
using boklista_api.Models;

namespace boklista_api.Data;

public class BookListDbContext(DbContextOptions<BookListDbContext> options) : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Quote> Quotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(u => u.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Book>()
            .Property(b => b.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Quote>()
            .Property(q => q.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<Book>()
            .HasOne(b => b.User)
            .WithMany(u => u.Books)
            .HasForeignKey(b => b.UserId)
            .IsRequired();

        modelBuilder.Entity<Quote>()
            .HasOne(q => q.User)
            .WithMany(u => u.Quotes)
            .HasForeignKey(q => q.UserId)
            .IsRequired();

        modelBuilder.Entity<Quote>()
            .HasOne(q => q.Book)
            .WithMany(u => u.Quotes)
            .HasForeignKey(q => q.BookId)
            .IsRequired();
    }
}

