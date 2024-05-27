using Core.Data;

namespace Service.DTOs;

public class UpdateUserEmailDto : IDto
{
    public int UserId { get; set; }
    public string Email { get; set; }

    public UpdateUserEmailDto()
    {
        UserId = 0;
        Email = string.Empty;
    }

    public UpdateUserEmailDto(int userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}