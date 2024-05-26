using Core.Data;

namespace Service.DTOs;

public class UsersListDto : IDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int Role { get; set; }
    public int PostCount { get; set; }
    public string Email { get; set; }
    public DateTime UpdatedDate { get; set; }

    public UsersListDto()
    {
        Id = 0;
        Username = string.Empty;
        Role = 0;
        PostCount = 0;
        Email = string.Empty;
        UpdatedDate = DateTime.MinValue;
    }

    public UsersListDto(int id, string username, int role, int postCount, string email, DateTime updatedDate)
    {
        Id = id;
        Username = username;
        Role = role;
        PostCount = postCount;
        Email = email;
        UpdatedDate = updatedDate;
    }
}