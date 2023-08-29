using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Post.Service.Models.Tables;

public partial class PostBlogsContext : DbContext
{
    public PostBlogsContext()
    {
    }

    public PostBlogsContext(DbContextOptions<PostBlogsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasKey(e => e.BlogPostId).HasName("PK_BlogPost");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
