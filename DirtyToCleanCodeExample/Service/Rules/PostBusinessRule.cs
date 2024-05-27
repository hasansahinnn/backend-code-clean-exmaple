using Core.Data.Repositories;
using Core.Rules;
using Data.Contexts;
using Data.Models;
using Service.Constants;

namespace Service.Rules;


/// <summary>
/// Business rules for the Post entity.
/// </summary>
public class PostBusinessRule : BaseBusinessRules
{
    private readonly IRepository<Post, DataContext> _repository;

    public PostBusinessRule(IRepository<Post, DataContext> repository)
    {
        _repository = repository;
    }

    public async Task PostIdShouldExistWhenSelected(int postId)
    {
        var post = await _repository.GetFirstOrDefaultAsync(x => x.Id == postId);
        if (post == null) throw new Exception(Messages.PostNotFound);
    }

    public Task PostCannotBeNull(Post? post)
    {
        if (post == null) throw new Exception(Messages.PostCannotBeNull);
        return Task.CompletedTask;
    }

    public Task ThereMustBeAnElementInThePostList(List<Post>? posts)
    {
        if (posts == null || posts.Count <= 0) throw new Exception(Messages.PostNotFound);
        return Task.CompletedTask;
    }
    
    public Task ValidateId(int id)
    {
        if (id < 1) throw new ArgumentException(Messages.IdMustBeGreaterThanZero);
        return Task.CompletedTask;
    }
    
    public Task ValidateStatus(int status)
    {
        if (status < 1) throw new ArgumentException(Messages.StatusMustBeGreaterThanZero);
        return Task.CompletedTask;
    }


    public Task ValidateContent(string content)
    {
        if (string.IsNullOrEmpty(content) || content.Length < 3) throw new ArgumentException(Messages.ContentCannotBeEmpty);
        return Task.CompletedTask;
    }

}