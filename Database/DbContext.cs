public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {}

    public DbSet<Article> Articles { get; set; }
    public DbSet<ArticleLike> ArticleLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ArticleLike>()
            .HasIndex(al => new { al.ArticleId, al.UserId })
            .IsUnique();
    }
}