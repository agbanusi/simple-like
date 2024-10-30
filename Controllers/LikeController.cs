using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LikeSystem.DTOs;
using LikeSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeSimpleSystem.Controllers;

[ApiController]
[Route("api/article")]
public class ArticlesController : ControllerBase
{
    private readonly ILikeService likeService;

    public ArticlesController(ILikeService _likeService)
    {
        likeService = _likeService;
    }

    // login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        // generate token for user
        var token =  await likeService.GenerateAccessToken(model.Email);
        // return access token for user's use
        return Ok(new { AccessToken = new JwtSecurityTokenHandler().WriteToken(token.Data)});

    }

    [HttpGet]
    public async Task<IActionResult> GetArticles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await likeService.GetArticlesAsync( page, pageSize);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);
            
        return Ok(result.Data);
    }

    [HttpGet("liked")]
    public async Task<IActionResult> GetLikedArticles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = likeService.GetRandomUserId();
        var result = await likeService.GetLikedArticlesAsync(userId, page, pageSize);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);
            
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize]

    public async Task<IActionResult> GetArticle(int id)
    {
        var result = await likeService.GetArticleAsync(id);
        
        if (!result.IsSuccess)
            return NotFound(result.Error);
            
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize]

    public async Task<IActionResult> CreateArticle(CreateArticleDto dto)
    {
        var result = await likeService.CreateArticleAsync(dto);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);
            
        return CreatedAtAction(nameof(GetArticle), new { id = result.Data.Id }, result.Data);
    }

    [HttpPost("{id}/like")]
    [Authorize]

    public async Task<IActionResult> LikeArticle(int id)
    {
        var userId = likeService.GetRandomUserId();
        var result = await likeService.LikeArticleAsync(id, userId);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);
            
        return Ok(result.Data);
    }

    [HttpPost("{id}/unlike")]
    [Authorize]

    public async Task<IActionResult> UnlikeArticle(int id)
    {
        var userId = likeService.GetRandomUserId();
        var result = await likeService.UnlikeArticleAsync(id, userId);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);
            
        return Ok(result.Data);
    }
}