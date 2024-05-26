using Core.Data.Repositories;
using Core.Rules;
using Data.Contexts;
using Data.Models;
using static Service.Constants.Messages;

namespace Service.Rules;

/// <summary>
/// Business rules for the Book entity.
/// </summary>
public class BookBusinessRule : BaseBusinessRules
{
    private readonly IRepository<Book, DataContext> _repository;

    public BookBusinessRule(IRepository<Book, DataContext> repository)
    {
        _repository = repository;
    }

    public async Task BookIdShouldExistWhenSelected(int bookId)
    {
        var book = await _repository.GetSingleOrDefaultAsync(x => x.Id == bookId);
        if (book == null) throw new Exception(BookNotFound);
    }

    public Task BookShouldBeExists(Book? book)
    {
        if (book == null) throw new Exception(BookNotFound);
        return Task.CompletedTask;
    }

    public Task ThereMustBeAtLeastOneBook(List<int> bookIds)
    {
        if (bookIds.Count <= 0) throw new Exception(BookIdMustBeEntered);
        return Task.CompletedTask;
    }

    public Task TheBookListCannotBeEmpty(List<Book> books)
    {
        if (books.Count <= 0) throw new Exception(BookNotFound);
        return Task.CompletedTask;
    }

    public Task TheNumberOfDaysAgoCannotBeNegative(int daysAgo)
    {
        if (daysAgo < 0) throw new Exception(DaysAgoMustBeZeroOrGreater);
        return Task.CompletedTask;
    }
}