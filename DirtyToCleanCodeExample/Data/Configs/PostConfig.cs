using Core.Example1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configs
{
    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(e => e.Content)
                .HasColumnType("nvarchar(MAX)");  // => If not find, install Microsoft.EntityFrameworkCore.SqlServer or PostgreSQL version
            builder.Property(u => u.Status)
             .HasConversion<int>(); // Enum'ı int olarak kaydet
        }
    }
}
