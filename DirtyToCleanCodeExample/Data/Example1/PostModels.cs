using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Example1
{
    public enum PostStatus
    {
        Draft,
        Published,
        Archived,
        Deleted
    }

    public enum UserRole
    {
        Admin,
        User,
        Guest
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Role { get; set; }
        public int PostCount { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Content { get; set; }
        public PostStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual User User { get; set; }

        public static Post Create(int userId, string content, User user, PostStatus status = PostStatus.Draft)
        {
            user.PostCount++;
            return new Post
            {
                UserId = userId,
                Content = content,
                Status = status,
                CreatedDate = DateTime.Now,
                User = user
            };
        }
        public static void UpdatePostStatus(Post post, PostStatus status)
        {
            post.Status = status;
            post.UpdatedDate = DateTime.Now;
        }
        public static void DeletePostStatus(Post post)
        {
            post.Status = PostStatus.Deleted;
            post.UpdatedDate = DateTime.Now;
        }
    }

     public class Comment
     {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
            
        public static Comment Create(int postId,int userId,string commentText)
        {
            return new Comment
            {
                PostId = postId,
                UserId = userId,
                Text = commentText,
                CreatedDate= DateTime.Now
            };
        }
     }
}



