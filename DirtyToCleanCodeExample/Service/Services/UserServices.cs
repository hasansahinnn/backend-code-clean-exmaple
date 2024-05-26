using Core.Data.Repositories;
using Data.Contexts;
using Data.Models;
using Service.DTOs;
using Service.Rules;
using static Service.Constants.Messages;

namespace Service.Services;

public interface IUserService
{
    /// <summary>
    /// Deletes the user with the given id.
    /// </summary>
    /// <param name="userId"></param>
    Task<string> DeleteAsync(int userId);
    /// <summary>
    /// Updates the email of the user with the given id.
    /// Throws an exception if the user does not exist.
    /// Returns a success message if the email is updated successfully.
    /// </summary>
    /// <param name="updateUserEmailDto"></param>
    Task<string> UpdateUserEmail(UpdateUserEmailDto updateUserEmailDto);
}

public class UserService : IUserService
{
    private readonly IRepository<User, DataContext> _repository;
    private readonly UserBusinessRule _userBusinessRule;
    private readonly IPostService _postService;

    public UserService
    (
        IRepository<User, DataContext> repository,
        UserBusinessRule userBusinessRule,
        IPostService postService)
    {
        _repository = repository;
        _userBusinessRule = userBusinessRule;
        _postService = postService;
    }

    /// <summary>
    /// Updates the email of the user with the given id.
    /// Throws an exception if the user does not exist.
    /// Returns a success message if the email is updated successfully.
    /// </summary>
    /// <param name="updateUserEmailDto"></param>
    public async Task<string> UpdateUserEmail(UpdateUserEmailDto updateUserEmailDto)
    {
        await _userBusinessRule.EmailShouldBeValid(updateUserEmailDto.Email);
        await _userBusinessRule.IdShouldBeGreaterThanZero(updateUserEmailDto.UserId);

        var user = await _repository.GetSingleOrDefaultAsync(x => x.Id == updateUserEmailDto.UserId);
        await _userBusinessRule.UserShouldBeExists(user);

        // Update the email of the user.
        user!.Email = updateUserEmailDto.Email;
        user.UpdatedDate = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
        return UserEmailUpdatedSuccessfully;
    }

    /// <summary>
    /// Deletes the user with the given id.
    /// </summary>
    /// <param name="userId"></param>
    public async Task<string> DeleteAsync(int userId)
    {
        await _userBusinessRule.IdShouldBeGreaterThanZero(userId);

        var user = await _repository.GetSingleOrDefaultAsync(x => x.Id == userId);
        await _userBusinessRule.UserShouldBeExists(user);

        await _repository.DeleteAsync(user!);
        await _postService.DeletePostAsync(user!.Id);
        return UserDeletedSuccessfully;
    }
}