using Microsoft.EntityFrameworkCore;
using LikeSystem.Models;

namespace LikeSystem.Database
{
  public class ApplicationDbContext : DbContext
  {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
      {}

      public DbSet<Article> Articles { get; set; }
      public DbSet<ArticleLike> ArticleLikes { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
        // Configure the unique index for ArticleLike
        modelBuilder.Entity<ArticleLike>()
            .HasIndex(al => new { al.ArticleId, al.UserId })
            .IsUnique();

        // Configure the relationship between Article and ArticleLike
        modelBuilder.Entity<ArticleLike>()
            .HasOne(al => al.Article)
            .WithMany(a => a.Likes)
            .HasForeignKey(al => al.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
      }
  }
}