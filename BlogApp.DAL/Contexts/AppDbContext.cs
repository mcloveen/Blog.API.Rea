using System;
using System.Reflection;
using BlogApp.Core.Entities;
using BlogApp.DAL.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.DAL.Contexts;

public class AppDbContext:IdentityDbContext<AppUser>
{
	public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

	public DbSet<Category> Categories { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogCategory> BlogCategories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<BlogLike> BlogLikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}



