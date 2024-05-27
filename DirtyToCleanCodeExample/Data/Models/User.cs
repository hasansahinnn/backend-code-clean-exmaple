using Core.Data;

namespace Data.Models;

public class User : BaseEntity
{
    public string Username { get; set; }
    public int Role { get; set; }
    public int PostCount { get; set; }
    public string Email { get; set; }
    public DateTime UpdatedDate { get; set; }


    /// <summary>
    /// The collection of posts associated with the user.
    /// </summary>
    public virtual ICollection<Post> Posts { get; set; }
    /// <summary>
    /// The collection of comments associated with the user.
    /// </summary>
    public virtual ICollection<Comment> Comments { get; set; }

    public User()
    {
        Posts = new HashSet<Post>();
        Comments = new HashSet<Comment>();
    }

    public User(int id, string userName, int role, int postCount,
        string email, DateTime updatedDate)
    {
        Id = id;
        Username = userName;
        Role = role;
        PostCount = postCount;
        Email = email;
        UpdatedDate = updatedDate;
    }
}

public enum UserRole
{
    Admin,
    User,
    Guest
}