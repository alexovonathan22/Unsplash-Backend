using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Unsplash.Core.Models;
using Unsplash.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Unsplash.Core.DataAccess
{
    public static class ModelBuilderExtensions
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UnsplashContext(
                serviceProvider.GetRequiredService<DbContextOptions<UnsplashContext>>()))
            {
                // Look for any board games.
                if (await context.Users.AnyAsync())
                {
                    return;   // Data was already seeded
                }
                var pwd = "012345";

                AuthUtil.CreatePasswordHash(pwd, out byte[] passwordHash, out byte[] passwordSalt);
                context.AddRange(
                    new User
                    {
                        ID= 1,
                        Username = "adminovo",
                        CreatedAt = DateTime.Now,
                        Email = "avo.nathan@gmail.com",
                        Role = UserRoles.Admin,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt
                    },
                   new User
                   {
                       ID = 2,
                       Username = "usernew",
                       CreatedAt = DateTime.Now,
                       Email = "nee.nathan@gmail.com",
                       Role = UserRoles.AppUser,
                       PasswordHash = passwordHash,
                       PasswordSalt = passwordSalt
                   },
                   new Category
                   {
                       ID = 1,
                       CategoryName="Cars",
                       Description="For all your automobile photos.",
                       CreatedAt = DateTime.Now,
                       ModifiedAt=DateTime.Now,
                   }
                );

                context.SaveChanges();
            }

        }
    }
}

/*
 public class DataGenerator
{
    
    }
}*/