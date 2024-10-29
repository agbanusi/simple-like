public class Article
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int LikesCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<ArticleLike> Likes { get; set; }
}

public class ArticleLike
{
    public int Id { get; set; }
    public int ArticleId { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Article Article { get; set; }
}