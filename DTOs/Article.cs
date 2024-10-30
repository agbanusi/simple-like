using System.ComponentModel.DataAnnotations;

namespace LikeSystem.DTOs
{
  public class ArticleDto
  {
      public int Id { get; set; }
      public string Title { get; set; }
      public string Content { get; set; }
      public int LikesCount { get; set; }
      public DateTime CreatedAt { get; set; }
      public bool IsLikedByCurrentUser { get; set; }
  }

  public class CreateArticleDto
  {
      [Required]
      [MinLength(3)]
      public string Title { get; set; }
      
      [Required]
      public string Content { get; set; }
  }

  public class LoginModel
  {
      [Required(ErrorMessage = "Username is required.")]
      public string Email { get; set; }

      [Required(ErrorMessage = "Password is required.")]
      [DataType(DataType.Password)]
      public string Password { get; set; }
  }
}