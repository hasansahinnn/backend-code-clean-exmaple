using System;
using Core.Data;

namespace Service.DTOs;

public class UserDto : IDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public int Role { get; set; }
    public int PostCount { get; set; }
    public string Email { get; set; }
    public DateTime UpdatedDate { get; set; }

    public UserDto()
    {

    }

    public UserDto(int id, string username, int role, int postCount, string email, DateTime updatedDate)
    {
        Id = id;
        Username = username;
        Role = role;
        PostCount = postCount;
        Email = email;
        UpdatedDate = updatedDate;
    }
}
