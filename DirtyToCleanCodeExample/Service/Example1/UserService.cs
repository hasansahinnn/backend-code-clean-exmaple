using System;
using Data;
using Data.Example1;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Service.Example1
{
	public class UserService
	{
        private readonly DataContext _dbContext;
        public UserService(IHttpContextAccessor httpContextAccessor, DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> Update(int userId, string newEmail)
        {
            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);

            user.Email = newEmail;
            user.UpdatedDate = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return "User email updated successfully.";
        }

        public async Task<string> Delete(int userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            await DeletePost(userId);

            return "User deleted successfully.";
        }

        public async Task<bool> DeletePost(int userId)
        {
            var userPosts = _dbContext.Posts.Where(x => x.UserId == userId).ToList();
            foreach (var post in userPosts)
            {
               _dbContext.Posts.Remove(post);
               _dbContext.SaveChanges();
            }

            return true;
        }

    }
}

