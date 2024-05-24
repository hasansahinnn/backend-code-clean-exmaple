using Data;
using Data.Example2;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Example2
{
    public class MemberService
    {
        private readonly DataContext _dbcontext;

        public MemberService(DataContext dbcontext)
        {
            _dbcontext=dbcontext;
        }
        public async Task<List<Member>> GetMembersAsync()
        {
            return await _dbcontext.Members.ToListAsync();
        }
        public async Task<Member> GetMemberByIdAsync(int id)
        {
            return await _dbcontext.Members.FirstOrDefaultAsync(x=>x.Id==id);
        }
    }
}
