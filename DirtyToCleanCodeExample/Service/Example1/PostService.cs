using System;
using System.Net.NetworkInformation;
using Data;
using Data.Example1;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Service.Example1
{
	public class PostService
	{
        private readonly DataContext _dbContext;
        private readonly UserService _userService;
        public PostService(DataContext dbContext,UserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public async Task<List<Post>> GetPostsAndUsersAsync()
        {
            /*Bu fonksiyonda user ile posts beraber getirilmek isteniyor.
            Eager loading kullanarak fonksiyonu daha performanslı yapabilirz.*/

            return await _dbContext.Posts.Include(p => p.User).ToListAsync();
        }

        public async Task<string> CreatePostAsync(int userId, string content)
        {
            //user servisine bunun metodunu yazdım.Başka yerlerde de lazım olabilir.
            var user = await _userService.GetUserByIdAsync(userId);
            //user null kontolü
            if(user == null) { return "User not found";}
            //userrole böyle kontrol etmek daha doğru.
            if (user.Role == (int)UserRole.Admin) 
            {
                return "Only users can create posts.";
            }
            // var post=new post(){ } yapmak yerine post sınıfına statik bir create fonksiyonu ekledim.
            var post=Post.Create(userId, content,user);
            _dbContext.Posts.Add(post);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return "Post created successfully.";
        }

        public async Task<string> AddCommentAsync(int postId, int userId, string commentText)
        {
            //önce sorgu yapıp sonra verileri çekmek daha performanslı.
            // ama burada buna gerek yok cünkü kullanmıycaz.
            //var post = await _dbContext.Posts.Where(x => x.Id==postId).FirstOrDefaultAsync();
            
            //var comment=new comment(){} yapmak yerine static bir create fonksiyonu
            var comment = Comment.Create(postId,userId,commentText);
            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            return "Comment added successfully.";
            /*
            Bunu burada yapmamıza gerek yok.Başka bir fonksiyon olarak ekleyebiliriz.
            var allPosts = await _dbContext.Posts.ToListAsync();
            foreach (var p in allPosts)
            {
                if (p.CreatedDate < DateTime.Now.AddDays(-30))
                {
                    p.Status = PostStatus.Archived;
                }
            }
            */
        }
        public async Task<string> PublishPostAsync(int postId,int status)
        {
            //getPostById metodu bekledim çünkü her yerde kullanabilirm ihtiyacım olabilir.
            var post = await GetPostByIdAsync(postId);
            //post null kontrolü
            if(post == null) { return "Post not found."; }
            //static updateStatus metodu ekledim.
            Post.UpdatePostStatus(post,(PostStatus)status);

            await _dbContext.SaveChangesAsync();
            return "Post published successfully.";
        }

        public async Task<string> Delete(int postId)
        {
            var post = await GetPostByIdAsync(postId);
            //null kontrolü
            if(post == null) { return "Post Not Found"; }

            await _dbContext.SaveChangesAsync();
            return "Post deleted.";
        }
        public async Task<bool> ChangeStatusPostsToArchive()
        {
            var allPosts = await _dbContext.Posts.ToListAsync();
            foreach (var p in allPosts)
            {
                if (p.CreatedDate < DateTime.Now.AddDays(-30))
                {
                    p.Status = PostStatus.Archived;
                }
            }
            return true;
        }
        public async Task<Post> GetPostByIdAsync(int postId)
        {
            return await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId);
        }
        public async Task<List<Post>> GetPostsByUserIdWithAsync(int userId)
        {
            return await _dbContext.Posts.Where(x=>x.UserId == userId).ToListAsync();
        }


    }
}

