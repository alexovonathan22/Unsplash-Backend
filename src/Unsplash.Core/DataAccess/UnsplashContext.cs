using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Unsplash.Core.Models;


namespace Unsplash.Core.DataAccess
{
    public class UnsplashContext : DbContext
    {
        public UnsplashContext(DbContextOptions<UnsplashContext> options)
        : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> ImgCategories { get; set; }
        public DbSet<Photo> Image { get; set; }

    }
    
}
