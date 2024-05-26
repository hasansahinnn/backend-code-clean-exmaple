using Core.Example1;
using Core.ReturnModel;
using Data.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Concrete
{
    public class UserRepository : Repository<User, DataContext>
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public async Task<IReturn> UpdateMailAsync(int userId, string newEmail)
        {
            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
            if(user != null)
            {
                user.Email = newEmail;
                user.UpdatedDate = DateTime.Now;

                context.Users.Update(user);

                return new SuccessReturn("Başarılı");
            }
            else
            {
                return new ErrorReturn("user yok");
            }
        }

        public async Task<IReturn> DeleteAsync(int userId)
        {
            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
            if(user != null)
            {
                return await DeleteAsync(user);
            }
            else
            {
                return new ErrorReturn();
            }
        }



        public async Task<IReturn> DeleteUserPosts(int userId)
        {
            var result = await context.Posts
                .TagWith("user posts delete işlemi için DeleteUserPosts")
                .Where(p => p.Status != PostStatus.Deleted && p.UserId == userId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.Status, x => PostStatus.Deleted));

            return new SuccessReturn($"User a ait {result} adet post silindi.");
        }





    }
}
