using Core.Data;

namespace Service.DTOs;

public class CreateCommentDto : IDto
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; }

    public CreateCommentDto()
    {

    }

    public CreateCommentDto(int postId, int userId, string text)
    {
        PostId = postId;
        UserId = userId;
        Text = text;
    }
}