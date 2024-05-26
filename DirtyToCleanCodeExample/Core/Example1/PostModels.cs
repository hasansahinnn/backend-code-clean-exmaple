using Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Example1
{
    // Data katmanında ne olmalı tam hatırlamıyorum ama bunlar core katmanında olmalı
    // normalde her obje ayrı dosyada yapardım ama gerçek bir proje olmadığından üşendim tek dosya devam edeceğim buradan

    public enum PostStatus
    {
        Draft,
        Published,
        Archived,
        Deleted
    }

    public enum UserRole
    {
        Admin = 1,
        User = 2,
        Guest = 3
    }

    public class User : IModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!; // null olamaz
        public UserRole Role { get; set; }
        public string Email { get; set; } = null!; // null olamaz
        public DateTime UpdatedDate { get; set; }
    }

    public class Post : IModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        //[Column(TypeName = "nvarchar(MAX)")] // => Data.Configs.PostConfig 
        public string Content { get; set; } = null!;
        public PostStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual User User { get; set; } = null!;
    }

    public class Comment : IModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }

}

