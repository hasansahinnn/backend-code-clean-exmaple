using Core.Data;

namespace Data.Models;

public class Comment : BaseEntity
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// The post associated with the comment.
    /// </summary>
    public virtual Post Post { get; set; }
    /// <summary>
    /// The user associated with the comment.
    /// </summary>
    public virtual User User { get; set; }

    public Comment() { }

    public Comment(int id, int postId, int userId,
        string text, DateTime createdDate)
    {
        Id = id;
        PostId = postId;
        UserId = userId;
        Text = text;
        CreatedDate = createdDate;
    }
}