using Core.Data;
using Data.Models;

namespace Service.DTOs;

public class PostsListDto : IDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; }
    public PostStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public UserDto UserDto { get; set; }

    public PostsListDto()
    {

    }

    public PostsListDto(int id, int userId, string content, PostStatus status, DateTime createdDate, DateTime updatedDate, UserDto userDto)
    {
        Id = id;
        UserId = userId;
        Content = content;
        Status = status;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
        UserDto = userDto;
    }
}