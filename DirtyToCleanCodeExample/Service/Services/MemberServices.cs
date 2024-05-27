using Core.Data.Repositories;
using Data.Contexts;
using Data.Models;
using Service.Rules;

namespace Service.Services;

public interface IMemberService
{
    /// <summary>
    /// Get member by id.
    /// Throws an exception if the member does not exist.
    /// </summary>
    /// <param name="memberId"></param>
    Task<Member> GetById(int memberId);
    /// <summary>
    /// Increases the borrow count of the member.
    /// Throws an exception if the member does not exist.
    /// </summary>
    /// <param name="memberId"></param>
    Task IncreaseBorrowCountAsync(int memberId);
}

public class MemberService : IMemberService
{
    private readonly IRepository<Member, DataContext> _repository;
    private readonly MemberBusinessRule _memberBusinessRule;

    public MemberService(IRepository<Member, DataContext> repository, MemberBusinessRule memberBusinessRule)
    {
        _repository = repository;
        _memberBusinessRule = memberBusinessRule;
    }

    /// <summary>
    /// Get member by id.
    /// Throws an exception if the member does not exist.
    /// </summary>
    /// <param name="memberId"></param>
    public async Task<Member> GetById(int memberId)
    {
        await _memberBusinessRule.IdShouldBeGreaterThanZero(memberId);
        var member = await _repository.GetSingleOrDefaultAsync(x => x.Id == memberId);
        await _memberBusinessRule.MemberShouldBeExists(member);
        return member!;
    }

    /// <summary>
    /// Increases the borrow count of the member.
    /// Throws an exception if the member does not exist.
    /// </summary>
    /// <param name="memberId"></param>
    public async Task IncreaseBorrowCountAsync(int memberId)
    {
        await _memberBusinessRule.IdShouldBeGreaterThanZero(memberId);
        var member = await _repository.GetSingleOrDefaultAsync(x => x.Id == memberId);
        await _memberBusinessRule.MemberShouldBeExists(member);

        member!.BorrowCount++;
        await _repository.SaveChangesAsync();
    }


}