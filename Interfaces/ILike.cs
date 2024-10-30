using System.IdentityModel.Tokens.Jwt;
using LikeSystem.DTOs;

namespace LikeSystem.Interface
{
    public interface ILikeService
    {
        Task<Result<ArticleDto>> GetArticleAsync(int articleId);
        Task<Result<List<ArticleDto>>> GetArticlesAsync(int page = 1, int pageSize = 10);
        Task<Result<List<ArticleDto>>> GetLikedArticlesAsync(string userId, int page = 1, int pageSize = 10);
        Task<Result<ArticleDto>> CreateArticleAsync(CreateArticleDto dto);
        Task<Result<bool>> LikeArticleAsync(int articleId, string userId);
        Task<Result<bool>> UnlikeArticleAsync(int articleId, string userId);
        Task<Result<JwtSecurityToken>> GenerateAccessToken(string email);
        string GetRandomUserId();
    }
}