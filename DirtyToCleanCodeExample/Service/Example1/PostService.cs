using Core.Example1;
using Core.ReturnModel;
using Data;
using Data.Repository.Abstract;
using Data.Repository.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Service.Example1
{
	public class PostService : Service<Post>
	{
        internal new readonly PostRepository repository;
        public PostService(PostRepository repository, IHttpContextAccessor httpContextAccessor) : base(repository, httpContextAccessor)
        {
            this.repository = repository;
        }

        public async Task<IReturn<List<Post>>> GetPostsAndUsersAsync()
        {
            Console.WriteLine("GetPostsAndUsersAsync servisi çalıştı");
            var result = await repository.GetPostsAndUsersAsync();
            Console.WriteLine($"veri geldi durumu = {result.Status} ");
            return result;
        }

        public async Task<IReturn> CreatePostAsync(int userId, string content)
        {
            Console.WriteLine("Post ekleniyor.");
            var result = await repository.CreatePostAsync(userId, content);
            Console.WriteLine($"post ekleme durumu {result.Status}");
            return result;
        }

        public async Task<IReturn> AddCommentAsync(int postId, int userId, string commentText)
        {
            Console.WriteLine("AddCommentAsync çalıştı.");
            var result = await repository.AddCommentAsync(postId, userId, commentText);
            Console.WriteLine($" durumu {result.Status}");
            return result;
        }


        public async Task<IReturn> PublishPostAsync(int postId, int status)
        {
            Console.WriteLine("PublishPostAsync çalıştı.");
            var result = await repository.PublishPostAsync(postId, status);
            Console.WriteLine($"durumu {result.Status}");
            return result;
        }

        public async Task<IReturn> Delete(int postId)
        {
            Console.WriteLine("Delete çalıştı.");
            var result = await repository.DeleteAsync(postId);
            Console.WriteLine($"durumu {result.Status}");
            return result;
        }
    }
}

