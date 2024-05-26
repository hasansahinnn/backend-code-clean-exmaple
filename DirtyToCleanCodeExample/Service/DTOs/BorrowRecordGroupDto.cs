using Core.Data;
using Data.Models;

namespace Service.DTOs;

public class BorrowRecordGroupDto : IDto
{
    public DateTime Date { get; }
    public IEnumerable<IGrouping<int, BorrowRecord>> BorrowRecords { get; }

    public BorrowRecordGroupDto(DateTime date, IEnumerable<IGrouping<int, BorrowRecord>> borrowRecords)
    {
        Date = date;
        BorrowRecords = borrowRecords;
    }
}