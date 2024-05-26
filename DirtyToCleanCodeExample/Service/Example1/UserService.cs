using Core.Example1;
using Core.ReturnModel;
using Data;
using Data.Repository.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Service.Example1
{
    public class UserService : Service<User>
    {
        internal new readonly UserRepository repository;
        public UserService(UserRepository repository, IHttpContextAccessor httpContextAccessor) : base(repository, httpContextAccessor)
        {
            this.repository = repository;
        }

        public async Task<IReturn> UpdateMailAsync(int userId, string newEmail)
        {
            Console.WriteLine("Update User servisi çalıştı");
            var result = await repository.UpdateMailAsync(userId, newEmail);
            Console.WriteLine($"durumu = {result.Status} ");
            return result;
        }

        public async Task<IReturn> DeleteUserPosts(int userId)
        {
            Console.WriteLine("delete user posts servisi çalıştı");
            var result = await repository.DeleteUserPosts(userId);
            Console.WriteLine($"durumu = {result.Status} ");
            return result;
        }

        public async Task<IReturn> Delete(int userId)
        {
            Console.WriteLine("delete user servisi çalıştı");
            var result = await repository.DeleteAsync(userId);
            Console.WriteLine($"durumu = {result.Status} ");
            return result;
        }

    }
}

