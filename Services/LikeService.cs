using LikeSystem.Database;
using LikeSystem.DTOs;
using LikeSystem.Interface;
using LikeSystem.Models;
using Microsoft.EntityFrameworkCore;

public class LikeService : ILikeService
{
    private readonly ApplicationDbContext context;
    private readonly ILogger<LikeService> logger;

    public LikeService(ApplicationDbContext _context, ILogger<LikeService> _logger)
    {
        context = _context;
        logger = _logger;
    }

    public async Task<Result<ArticleDto>> GetArticleAsync(int articleId, string userId)
    {
        try
        {
            var article = await context.Articles
                .AsNoTracking()
                .Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    LikesCount = a.LikesCount,
                    CreatedAt = a.CreatedAt,
                    IsLikedByCurrentUser = a.Likes.Any(l => l.UserId == userId)
                })
                .FirstOrDefaultAsync(a => a.Id == articleId);

            if (article == null)
                return Result<ArticleDto>.Failure("Article not found");

            return Result<ArticleDto>.Success(article);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving article {ArticleId}", articleId);
            return Result<ArticleDto>.Failure("An error occurred while retrieving the article");
        }
    }

    public async Task<Result<List<ArticleDto>>> GetArticlesAsync(string userId, int page = 1, int pageSize = 10)
    {
        try
        {
            var articles = await context.Articles
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    LikesCount = a.LikesCount,
                    CreatedAt = a.CreatedAt,
                    IsLikedByCurrentUser = a.Likes.Any(l => l.UserId == userId)
                })
                .ToListAsync();

            return Result<List<ArticleDto>>.Success(articles);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving articles");
            return Result<List<ArticleDto>>.Failure("An error occurred while retrieving articles");
        }
    }

    public async Task<Result<bool>> LikeArticleAsync(int articleId, string userId)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var article = await context.Articles.FindAsync(articleId);
            if (article == null)
                return Result<bool>.Failure("Article not found");

            var existingLike = await context.ArticleLikes
                .FirstOrDefaultAsync(l => l.ArticleId == articleId && l.UserId == userId);

            if (existingLike != null)
                return Result<bool>.Failure("Article already liked");

            var like = new ArticleLike
            {
                ArticleId = articleId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            context.ArticleLikes.Add(like);
            article.LikesCount++;
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Error liking article {ArticleId}", articleId);
            return Result<bool>.Failure("An error occurred while liking the article");
        }
    }

    public async Task<Result<bool>> UnlikeArticleAsync(int articleId, string userId)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var article = await context.Articles.FindAsync(articleId);
            if (article == null)
                return Result<bool>.Failure("Article not found");

            var like = await context.ArticleLikes
                .FirstOrDefaultAsync(l => l.ArticleId == articleId && l.UserId == userId);

            if (like == null)
                return Result<bool>.Failure("Article not liked");

            context.ArticleLikes.Remove(like);
            article.LikesCount--;
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Error unliking article {ArticleId}", articleId);
            return Result<bool>.Failure("An error occurred while unliking the article");
        }
    }

    public async Task<Result<ArticleDto>> CreateArticleAsync(CreateArticleDto dto)
    {
        try
        {
            var article = new Article
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                LikesCount = 0
            };

            context.Articles.Add(article);
            await context.SaveChangesAsync();

            var articleDto = new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                LikesCount = 0,
                CreatedAt = article.CreatedAt,
                IsLikedByCurrentUser = false
            };

            return Result<ArticleDto>.Success(articleDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating article");
            return Result<ArticleDto>.Failure("An error occurred while creating the article");
        }
    }
}
