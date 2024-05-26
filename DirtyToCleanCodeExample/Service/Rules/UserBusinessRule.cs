using Core.Data.Repositories;
using Core.Rules;
using Data.Contexts;
using Data.Models;
using static Service.Constants.Messages;

namespace Service.Rules;

/// <summary>
/// Business rules for the User entity.
/// </summary>
public class UserBusinessRule : BaseBusinessRules
{
    private readonly IRepository<User, DataContext> _repository;

    public UserBusinessRule(IRepository<User, DataContext> repository)
    {
        _repository = repository;
    }

    public async Task UserIdShouldExistWhenSelected(int userId)
    {
        var user = await _repository.GetFirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) throw new Exception(UserNotFound);
    }

    public async Task<User> CanUserCreatePost(int userId)
    {
        var user = await _repository.GetSingleOrDefaultAsync(x => x.Id == userId);
        if (user == null) throw new Exception(UserNotFound);
        if (user.Role != 1) throw new Exception(OnlyUsersCanCreatePost);

        return user;
    }

    public Task UserShouldBeExists(User? user)
    {
        if (user == null) throw new Exception(UserNotFound);
        return Task.CompletedTask;
    }

    public Task EmailShouldBeValid(string email)
    {
        if (!email.Contains("@")) throw new Exception(InvalidEmail);
        return Task.CompletedTask;
    }

    public Task IdShouldBeGreaterThanZero(int id)
    {
        if (id < 1) throw new Exception(IdMustBeGreaterThanZero);
        return Task.CompletedTask;
    }

}