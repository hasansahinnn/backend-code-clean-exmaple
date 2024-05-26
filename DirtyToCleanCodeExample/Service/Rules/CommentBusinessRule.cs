using Core.Data.Repositories;
using Core.Rules;
using Data.Contexts;
using Data.Models;

namespace Service.Rules;

/// <summary>
/// Business rules for the Comment entity.
/// </summary>
public class CommentBusinessRule : BaseBusinessRules
{
    private readonly IRepository<Comment, DataContext> _repository;

    public CommentBusinessRule(IRepository<Comment, DataContext> repository)
    {
        _repository = repository;
    }
}