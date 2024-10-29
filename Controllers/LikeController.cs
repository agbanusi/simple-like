using System.Security.Claims;
using LikeSystem.DTOs;
using LikeSystem.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LikeSimpleSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ArticlesController : ControllerBase
{
    private readonly ILikeService likeService;

    public ArticlesController(ILikeService _likeService)
    {
        likeService = _likeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetArticles([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await likeService.GetArticlesAsync(userId, page, pageSize);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);
            
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetArticle(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await likeService.GetArticleAsync(id, userId);
        
        if (!result.IsSuccess)
            return NotFound(result.Error);
            
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateArticle(CreateArticleDto dto)
    {
        var result = await likeService.CreateArticleAsync(dto);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);
            
        return CreatedAtAction(nameof(GetArticle), new { id = result.Data.Id }, result.Data);
    }

    [HttpPost("{id}/like")]
    public async Task<IActionResult> LikeArticle(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await likeService.LikeArticleAsync(id, userId);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);
            
        return Ok();
    }

    [HttpDelete("{id}/like")]
    public async Task<IActionResult> UnlikeArticle(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await likeService.UnlikeArticleAsync(id, userId);
        
        if (!result.IsSuccess)
            return BadRequest(result.Error);
            
        return Ok();
    }
}