
using Data;
using Data.Example1;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Service.Example1
{
    public class UserService
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor, DataContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task<string> Update(int userId, string newEmail)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            //Null kontrolü yapılmalıydı
            if (user == null)
            {
                return "User not found.";
            }

            user.Email = newEmail;
            user.UpdatedDate = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return "User email updated successfully.";
        }

        public async Task<string> Delete(int userId)
        {   
            //
            var user = await _dbContext.Users
                .Include(u => u.Posts)//postları silmek için direkt olarak include ile çekmeliyiz
                .FirstOrDefaultAsync(x => x.Id == userId);
            
            //null kontrolü 
            if (user == null)
            {
                return "User not found.";
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            await DeletePosts(user.Posts);

            return "User deleted successfully.";
        }

        private async Task<bool> DeletePosts(IEnumerable<Post> posts)
        {
            _dbContext.Posts.RemoveRange(posts); //postları range olarak silebiliriz
            return await _dbContext.SaveChangesAsync() == 1;
        }
    }
}
