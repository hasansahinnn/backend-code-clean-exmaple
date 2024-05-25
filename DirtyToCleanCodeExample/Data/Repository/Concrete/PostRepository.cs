using Core.Example1;
using Core.ReturnModel;
using Data.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository.Concrete
{
    public class PostRepository : Repository<Post, DataContext>
    {
        public PostRepository(DataContext context) : base(context)
        {
        }

        public async Task<IReturn<List<Post>>> GetPostsAndUsersAsync()
        {
            var result = await context.Posts.AsNoTracking().TagWith("Post tipli GetPostsAndUsersAsync işlemi").Include(p => p.User).ToListAsync(); //kod iyileştirildi.
            return CheckIsNull(result);
        }

        public async Task<IReturn> CreatePostAsync(int userId, string content)
        {
            if (await context.Users.AsNoTracking().TagWith("User tipli CreatePostAsync->İf kontrol işlemi").AnyAsync(x => x.Id == userId && x.Role != UserRole.Guest)) // ayırarak hataları özelde dönebilrsin
            {
                context.Add(
                new Post
                {
                    UserId = userId,
                    Content = content,
                    Status = PostStatus.Draft,
                    CreatedDate = DateTime.Now
                });// burada işte eklenme başarılımı kontroller filan geçtim.
                return new SuccessReturn("Başarıyla eklendi.");
            }
            else
            {
                return new ErrorReturn("Misafirler yazı yazamaz veya User Yok");
            }
        }

        public async Task<IReturn> AddCommentAsync(int postId, int userId, string commentText)
        {
            if (
                await context.Posts.AsNoTracking().TagWith("Post tipli id kontrol işlemi AddCommentAsync").AnyAsync(p => p.Id == postId) 
                || 
                await context.Users.AsNoTracking().TagWith("User tipli id kontrol işlemi AddCommentAsync").AnyAsync(p => p.Id == userId))
            {
                return new ErrorReturn("Post veya User yok");
            }

            // commentText küfür argo kontrol olabilir.

            context.Add(new Comment
            {
                PostId = postId,
                UserId = userId,
                Text = commentText,
                CreatedDate = DateTime.Now
            });

            await ArchiveComment();

            return new SuccessReturn("Başarı ile ekledik");
        }

        public async Task<IReturn> ArchiveComment(int day = -30) // ayrı olsunki sonra handfire mı neydi onunla mesela oto ayarlarız
        {

            //int result = await context.Database.ExecuteSqlInterpolatedAsync($@"
            //UPDATE Posts
            //SET Status = {PostStatus.Archived}
            //WHERE CreatedDate < {DateTime.Now.AddDays(-30)}");
            //return new SuccessReturn($"{result} adet Post tablosundaki satırun Status kolonu {PostStatus.Archived} olarak güncellendi.");

            //test edilmeli aslında
            // kaynak https://www.reddit.com/r/dotnet/comments/y0crl8/updating_multiple_entities_properties_in_ef_core/
            
            var result = await context.Posts  
                .TagWith("Comment update işlemi arşiv için")
                .Where(p => p.Status != PostStatus.Archived && p.CreatedDate < DateTime.Now.AddDays(day))
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.Status, x => PostStatus.Archived));
            
            return new SuccessReturn($"{result} adet Post tablosundaki satırun Status kolonu {PostStatus.Archived} olarak güncellendi.");
        }

        private async Task<IReturn> UpdatePostStatusAsync(int postId, PostStatus status)
        {
           // success text error text filan gönderilebilir

            var result = await context.Posts
                .AsNoTracking()
                .TagWith($"Update Post Status new status = {status}")
                .FirstOrDefaultAsync(p => p.Id == postId);

            if(result != null)
            {
                result.Status = status;
                context.Posts.Update(result);
                return new SuccessReturn();
            }
            else
            {
                return new ErrorReturn();
            }

                
        }


        public async Task<IReturn> PublishPostAsync(int postId, int status)
        {

            return await UpdatePostStatusAsync(postId, (PostStatus)status);
        }

        public async Task<IReturn> DeleteAsync(int postId)
        {
            return await UpdatePostStatusAsync(postId, PostStatus.Deleted);
        }



    }
}
