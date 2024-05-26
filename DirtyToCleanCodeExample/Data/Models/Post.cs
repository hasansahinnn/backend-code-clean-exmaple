using Core.Data;

namespace Data.Models;

public class Post : BaseEntity
{
    public int UserId { get; set; }
    public string Content { get; set; }
    public PostStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    /// <summary>
    /// The user associated with the post.
    /// </summary>
    public virtual User User { get; set; }
    /// <summary>
    /// The collection of comments associated with the post.
    /// </summary>
    public virtual ICollection<Comment> Comments { get; set; }

    public Post()
    {
        Comments = new HashSet<Comment>();
    }

    public Post(int id, int userId, string content, PostStatus status,
        DateTime createdDate, DateTime updatedDate)
    {
        Id = id;
        UserId = userId;
        Content = content;
        Status = status;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
    }
}

public enum PostStatus
{
    Draft,
    Published,
    Archived,
    Deleted
}