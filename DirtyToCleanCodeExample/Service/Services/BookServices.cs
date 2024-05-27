using System.Text;
using Core.Data.Repositories;
using Core.Mailing;
using Data.Contexts;
using Data.Models;
using MimeKit;
using Service.DTOs;
using Service.Rules;
using static Service.Constants.Messages;

namespace Service.Services;

public interface IBookService
{
    Task<string> BorrowBooksAsync(CreateBorrowDto createBorrowDto);
    IAsyncEnumerable<string> GetRecentBorrowersAsync(int daysAgo);
    Task SendEmailsForRecentBorrowersAsync(int daysAgo);
}

public class BookService : IBookService
{
    private readonly IRepository<BorrowRecord, DataContext> _borrowRecordRepository;
    private readonly IRepository<Book, DataContext> _bookRepository;
    private readonly IRepository<User, DataContext> _userRepository;
    private readonly IRepository<Member, DataContext> _memberRepository;
    private readonly BookBusinessRule _bookBusinessRule;
    private readonly IMemberService _memberService;
    private readonly IMailService _mailService;
    public BookService
    (
        BookBusinessRule bookBusinessRule,
        IMemberService memberService,
        IMailService mailService,
        IRepository<BorrowRecord, DataContext> borrowRecordRepository,
        IRepository<User, DataContext> userRepository,
        IRepository<Book, DataContext> bookRepository,
        IRepository<Member, DataContext> memberRepository)
    {
        _bookBusinessRule = bookBusinessRule;
        _memberService = memberService;
        _mailService = mailService;
        _borrowRecordRepository = borrowRecordRepository;
        _userRepository = userRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
    }

    /// <summary>
    /// Borrow books for a member and send an email to the member
    /// </summary>
    /// <param name="createBorrowDto"></param>
    public async Task<string> BorrowBooksAsync(CreateBorrowDto createBorrowDto)
    {
        var member = await _memberService.GetById(createBorrowDto.MemberId);
        await _bookBusinessRule.ThereMustBeAtLeastOneBook(createBorrowDto.BookIds);

        var books = await GetBooksAsync(createBorrowDto.BookIds);
        await AddBorrowRecordsAsync(books, member.Id);
        await IncreaseBorrowCountAsync(member, books);

        var textBody = string.Format(BorrowEmailTextBodyTemplate, string.Join("\n", books.Select(b => b.Title)));
        var htmlBody = "";
        List<MailboxAddress> mailboxAddresses = new List<MailboxAddress>
        {
            new (member.Name, member.Email)
        };
        await _mailService.SendEmailAsync(new Mail(BorrowEmailSubject, textBody, htmlBody, mailboxAddresses));

        return BooksBorrowedAndEmailSentSuccessfully;
    }

    /// <summary>
    /// Increase the borrow count of a member
    /// </summary>
    /// <param name="member"></param>
    /// <param name="books"></param>
    private async Task IncreaseBorrowCountAsync(Member member, List<Book> books)
    {
        member!.BorrowCount += books.Count;
        await _memberRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Send emails to the recent borrowers
    /// </summary>
    /// <param name="daysAgo"></param>
    public async Task SendEmailsForRecentBorrowersAsync(int daysAgo)
    {
        await foreach (var report in GetRecentBorrowersAsync(daysAgo))
        {
            var textBody = report;
            var htmlBody = "";
            var userName = UserRole.Admin.ToString();
            var address = "alp704r@gmail.com";
            List<MailboxAddress> mailboxAddresses = new List<MailboxAddress>
            {
                new (userName, address )
            };
            await _mailService.SendEmailAsync(new Mail(BorrowEmailSubject, textBody, htmlBody, mailboxAddresses));
        }
    }

    /// <summary>
    /// Get the recent borrowers
    /// </summary>
    /// <param name="daysAgo"></param>
    public async IAsyncEnumerable<string> GetRecentBorrowersAsync(int daysAgo)
    {

        await _bookBusinessRule.TheNumberOfDaysAgoCannotBeNegative(daysAgo);

        var startDate = DateTime.UtcNow.AddDays(-daysAgo);
        var groups = await GetBorrowRecordsGroupedByDateAsync(startDate, daysAgo);
        var reports = await GenerateReportsAsync(groups);

        foreach (var report in reports)
            yield return report;
    }

    /// <summary>
    /// Get the borrow records grouped by date
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="days"></param>
    private async Task<IEnumerable<BorrowRecordGroupDto>> GetBorrowRecordsGroupedByDateAsync(DateTime startDate, int days)
    {
        var groups = new List<BorrowRecordGroupDto>();

        for (int i = 0; i < days; i++)
        {
            var targetDate = startDate.AddDays(i);
            var borrowRecords = await _borrowRecordRepository.GetGroupedListAsync(
                predicate: br => br.BorrowDate.Date == targetDate.Date,
                groupBy: br => br.MemberId);

            if (borrowRecords.Count > 0)
                groups.Add(new BorrowRecordGroupDto(targetDate, borrowRecords));
        }

        if (groups.Count < 1)
            throw new InvalidOperationException(NoBorrowRecordGroupsFound);

        return groups;
    }

    /// <summary>
    /// Generate reports for the borrow records
    /// </summary>
    /// <param name="groups"></param>
    private async Task<IEnumerable<string>> GenerateReportsAsync(IEnumerable<BorrowRecordGroupDto> groups)
    {
        var reports = new List<string>();
        foreach (var group in groups)
        {
            var reportBuilder = new StringBuilder();
            foreach (var record in group.BorrowRecords)
            {
                var user = await _userRepository.FindAsync(record.Key);
                var bookTitles = _userRepository
                    .Query()
                    .Where(b => record
                        .Select(br => br.BookId)
                        .Contains(b.Id)
                    );

                reportBuilder.AppendLine($"{user.Username} - ({string.Join(", ", bookTitles)})");
            }
            reports.Add(reportBuilder.ToString());
        }

        return reports;
    }

    /// <summary>
    /// Get the books with the given ids
    /// </summary>
    /// <param name="bookIds"></param>
    private async Task<List<Book>> GetBooksAsync(List<int> bookIds)
    {
        var booksList = await _bookRepository.GetListAsync();
        var filteredBooks = booksList.Where(book => bookIds.Contains(book.Id)).ToList();
        await _bookBusinessRule.TheBookListCannotBeEmpty(filteredBooks);
        return filteredBooks;
    }

    /// <summary>
    /// Add borrow records for the given books and member
    /// </summary>
    /// <param name="books"></param>
    /// <param name="memberId"></param>
    private async Task AddBorrowRecordsAsync(List<Book> books, int memberId)
    {
        var borrowRecords = books.Select(book => new BorrowRecord
        {
            BookId = book.Id,
            MemberId = memberId,
            BorrowDate = DateTime.UtcNow
        }).ToList();

        await _borrowRecordRepository.AddRangeAsync(borrowRecords);
    }
}