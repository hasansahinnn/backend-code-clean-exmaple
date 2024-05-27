using Core.Data;

namespace Service.DTOs;

public class CreatePostDto : IDto
{
    public int UserId { get; set; }
    public string Content { get; set; }

    public CreatePostDto()
    {
        UserId = 0;
        Content = string.Empty;
    }

    public CreatePostDto(int userId, string content)
    {
        UserId = userId;
        Content = content;
    }
}