using System;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using Data;
using Microsoft.AspNetCore.Http;
using Data.Example2;
using System.Text;
using AutoMapper.Execution;
using Data.Example1;

namespace Service.Example2
{
	public class BookService
	{
        private readonly DataContext _dbContext;
        private readonly MemberService _memberService;
        private readonly EmailService _emailService;
        public BookService(DataContext dbContext, MemberService memberService, EmailService emailService)
        {
            _dbContext = dbContext;
            _memberService=memberService;
            _emailService=emailService;
        }

        public async Task<string> BorrowBooksAsync(int memberId, List<int> bookIds)
        {
            //Burada parametredeki id ye göre memberı çekiyoruz.MemberService de buna ait fonksiyon yazmamız lazım.
            var member =await _memberService.GetMemberByIdAsync(memberId);
            if (member == null)
            {
                return "Member not found.";
            }
            //istenen kitap id lerine göre bulunan ve bulunamayan kitapları getirdik.Eğer bulunamayan kitap varsa return le sonlandırıyoruz.
            var (books,NotFoundBooks)=await GetBooksByIdAsync(bookIds);
            if (NotFoundBooks.Any())
            {
                return $"Books Not Found. BooksId:{string.Join(',',NotFoundBooks)}";
            }
            //eğer tüm kitapları getirdiysek bu fonksiyonu çağırıp her kitap için record olusturuyoruz.
            //
            await CreateBorrowRecordAsync(memberId,books);

            member.BorrowCount++; 
            _dbContext.Members.Update(member);
            await _dbContext.SaveChangesAsync();
            await _emailService.SendEmailForBorrowsBookAsync(member.Email,books);
            return "Books borrowed and email sent successfully.";
        }

        public async Task SendEmailsForRecentBorrowersAsync(int daysAgo)
        {
            await foreach (var (report,email) in GetRecentBorrowersAsync(daysAgo))
            {
                //her üyeye, ödünç aldığı kitapların raporlarını mail olarak gönderiyoruz.
                await _emailService.SendEmailForBorrowsBookReportAsync(email, report);
            }
        }

        public async  IAsyncEnumerable<(string report,string email)> GetRecentBorrowersAsync(int daysAgo)
        {
            var startDate = DateTime.Now.AddDays(-daysAgo);


            for (int i = 0; i <= daysAgo; i++)
            {
                var targetDate = startDate.AddDays(i);
                //borrowRecordlarını tarihe göre getirme metodu
                var borrowRecords = await GetBorrowRecordsByDateAsync(targetDate.Date);

                var reportBuilder = new StringBuilder();
                foreach (var group in borrowRecords)
                {
                    var user = await _dbContext.Users.FindAsync(group.Key);
                    if (user == null) continue;
                    //ödünç alınan kitapların title larını getiriyoruz.
                    var bookTitles = await _dbContext.Books
                        .Where(b => group.Any(x=>x.BookId==b.Id))
                        .Select(b => b.Title)
                        .ToListAsync();
                    reportBuilder.AppendLine($"{user.Username}- {user.Email} - ({string.Join(", ", bookTitles)})");
                    yield return (reportBuilder.ToString(),user.Email);
                }
            }
        }
        public async Task<(List<Book>,List<int>)> GetBooksByIdAsync(List<int> bookIds)
        {
            //parametre olarak bookId leri alıyoruz ve bu id lere sahip kitapları döndüreceğiz.
            //eğer kitaplar bulunamazsa bulunamayan kitapların id sini döndürüp hatayı yazdırıyoruz.Id si eşleşen kitapları da döndürüyoruz.
            var books = new List<Book>();
            var notFoundBooksId = new List<int>();
            foreach (var bookId in bookIds)
            {
                var book = await GetBookByIdAsync(bookId);
                if (book == null)
                {
                    notFoundBooksId.Add(book.Id);
                }
                else
                {
                    books.Add(book);
                }
            }

            return (books,notFoundBooksId);
        }
        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(x => x.Id==id);
        }
        public async Task CreateBorrowRecordAsync(int memberId,List<Book> books)
        {
            //burada her book için borrow record olusturdum.
            var borrowRecords = books.Select(x => new BorrowRecord
            {
                BookId = x.Id,
                MemberId = memberId,
                BorrowDate = DateTime.Now
            });
            await _dbContext.BorrowRecords.AddRangeAsync(borrowRecords);
        }
        public async Task<List<IGrouping<int, BorrowRecord>>> GetBorrowRecordsByDateAsync(DateTime day)
        {
            //şarta uyan her memberın borrow recordunu getirir.
            //memberId ye göre grupladığımız için IGrouping kullandık geri dönüş tipi olarak
           return await _dbContext.BorrowRecords.Where(x=>x.BorrowDate.Date.Day==day.Date.Day).GroupBy(x=>x.MemberId).ToListAsync();
        }
    }
}

