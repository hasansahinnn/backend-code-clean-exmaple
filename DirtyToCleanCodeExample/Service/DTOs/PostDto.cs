using Core.Data;
using Data.Models;

namespace Service.DTOs;

public class PostDto : IDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public PostStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public PostDto()
    {
        Id = 0;
        UserId = 0;
        Content = string.Empty;
        Status = default;
        CreatedDate = DateTime.MinValue;
        UpdatedDate = DateTime.MinValue;
    }

    public PostDto(int id, int userId, string content, PostStatus status, DateTime createdDate, DateTime updatedDate)
    {
        Id = id;
        UserId = userId;
        Content = content;
        Status = status;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
    }
}