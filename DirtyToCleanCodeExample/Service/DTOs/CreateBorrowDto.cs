using Core.Data;

namespace Service.DTOs;

public class CreateBorrowDto : IDto
{
    public int MemberId { get; set; }
    public List<int> BookIds { get; set; }

    public CreateBorrowDto()
    {
        BookIds = new List<int>();
    }

    public CreateBorrowDto(int memberId, List<int> bookIds)
    {
        MemberId = memberId;
        BookIds = bookIds;
    }
}