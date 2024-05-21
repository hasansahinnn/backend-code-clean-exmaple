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
        public PostService(IHttpContextAccessor httpContextAccessor, DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Post>> GetPostsAndUsersAsync()
        {
            var posts = await _dbContext.Posts.ToListAsync();
            var postsWithUsers = new List<Post>();

            foreach (var post in posts)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == post.UserId);
                post.User = user;
                postsWithUsers.Add(post);
            }

            return postsWithUsers;
        }

        public async Task<string> CreatePostAsync(int userId, string content)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user.Role != 1) 
            {
                return "Only users can create posts.";
            }

            var post = new Post
            {
                UserId = userId,
                Content = content,
                Status = PostStatus.Draft, 
                CreatedDate = DateTime.Now
            };

            _dbContext.Posts.Add(post);

            user.PostCount++;
            _dbContext.Users.Update(user);

            await _dbContext.SaveChangesAsync();

            return "Post created successfully.";
        }

        public async Task<string> AddCommentAsync(int postId, int userId, string commentText)
        {
            var post = _dbContext.Posts.ToList().First(x => x.Id == postId);

            var comment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Text = commentText,
                CreatedDate = DateTime.Now
            };

            _dbContext.Comments.Add(comment);

            var allPosts = await _dbContext.Posts.ToListAsync();
            foreach (var p in allPosts)
            {
                if (p.CreatedDate < DateTime.Now.AddDays(-30))
                {
                    p.Status = PostStatus.Archived;
                }
            }

            await _dbContext.SaveChangesAsync();

            return "Comment added successfully.";
        }


        public async Task<string> PublishPostAsync(int postId, int status)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId);

            post.Status = (PostStatus)status;
            post.UpdatedDate = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return "Post published successfully.";
        }

        public async Task<string> Delete(int postId)
        {
            var post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == postId);

            post.Status = PostStatus.Deleted;
            post.UpdatedDate = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return "Post deleted.";
        }
    }
}

