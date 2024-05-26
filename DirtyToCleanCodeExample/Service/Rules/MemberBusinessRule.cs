using Core.Data.Repositories;
using Core.Rules;
using Data.Contexts;
using Data.Models;
using static Service.Constants.Messages;

namespace Service.Rules;

/// <summary>
/// Business rules for the Member entity.
/// </summary>
public class MemberBusinessRule : BaseBusinessRules
{
    private readonly IRepository<Member, DataContext> _repository;

    public MemberBusinessRule(IRepository<Member, DataContext> repository)
    {
        _repository = repository;
    }

    public async Task MemberIdShouldExistWhenSelected(int userId)
    {
        var member = await _repository.GetSingleOrDefaultAsync(x => x.Id == userId);
        if (member == null) throw new Exception(MemberNotFound);
    }

    public Task MemberShouldBeExists(Member? member)
    {
        if (member == null) throw new Exception(MemberNotFound);
        return Task.CompletedTask;
    }

    public Task IdShouldBeGreaterThanZero(int id)
    {
        if (id < 1) throw new Exception(IdMustBeGreaterThanZero);
        return Task.CompletedTask;
    }
}