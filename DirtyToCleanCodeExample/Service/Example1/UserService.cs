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
        private readonly PostService _postService;
        public UserService(DataContext dbContext, PostService postService)
        {
            _dbContext = dbContext;
            _postService=postService;
        }

        public async Task<string> Update(int userId, string newEmail)
        {
            // burada AsNoTracking kullanılamaz çünkü çektiğimiz data üzerinde değişiklikler yapıyoruz.
            //var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
            //Userları getirmek için fonksiyon yazmalıyız.Başka yerlerde de kullanabiliriz.
            var user=await GetUserByIdAsync(userId);
            //null kontrolü
            if (user == null) { return "User Not Found"; };
            //User sınıfına updateEmail diye static bir sınıf eklememiz lazım.
            User.UpdateEmail(user, newEmail);
            await _dbContext.SaveChangesAsync();
            return "User email updated successfully.";
        }

        public async Task<string> Delete(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if(user == null) { return "User Not Found";}
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            /* userın postlarını buradan silmemize gerek yok. DbContext üzerinden override edilmiş onModelCreating metodu ile
               Cascade Delete özelliğini kullanarak ilişkili bir varlık silindiğinde diğer tablolarda da bu özelliğe sahip verileri silinebilir.Entity Framework Core özelliğidir.
               modelBuilder.Entity<Post>().HasOne(x=>x.User).WithMany(p=>p.Posts).HasForeignKey(x=>x.UserId).OnDelete(DeleteBehavior.Cascade);
               Ama bizim modelimizde ilişki belirtilmediği için bunu yapamıycaz.
            */
            await DeletePost(userId);
            return "User deleted successfully.";
        }

        public async Task<bool> DeletePost(int userId)
        {
            //Parametreye geçilen userId ye sahip postları getirmek için PostService'e GetPostsByUserIdWithAsync fonksiyonu ekledim.
            var userPosts =await _postService.GetPostsByUserIdWithAsync(userId);
            if(userPosts.Count == 0) { return false; }
            
             //burada her post remove edildiğinde değişiklikler kalıcı olarak veri tabanına kaydediliyor ve performansı düşürüyor.Bide asenkron olarak kullanmalıyız.
             //SaveChanges metodu tek bir transaction içinde gerçekleştirilir.
            _dbContext.Posts.RemoveRange(userPosts);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
           return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}

