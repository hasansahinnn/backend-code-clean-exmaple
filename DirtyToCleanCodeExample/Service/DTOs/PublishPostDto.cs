using Core.Data;

namespace Service.DTOs;

public class PublishPostDto : IDto
{
    public int PostId { get; set; }
    public int Status { get; set; }

    public PublishPostDto()
    {
        PostId = 0;
        Status = 0;
    }

    public PublishPostDto(int postId, int status)
    {
        PostId = postId;
        Status = status;
    }
}