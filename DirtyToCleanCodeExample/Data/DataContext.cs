using System;
using Microsoft.EntityFrameworkCore;
using Core.Example1;
using Core.Example2;
using System.Reflection;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext() { } // repository kısmında TContext türü için gerekli olduğundan konuldu.
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }


        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BorrowRecord> BorrowRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //Configs içinde eklenen tüm ayarları otomatik alması için. Aslında `IEntityTypeConfiguration` miras alanları alıyor içine diyebilirim. 
        }
    
    }
}

