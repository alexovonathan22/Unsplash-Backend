using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Polly;
using System.IO;
using System.Linq;
using System.Reflection;
using Unsplash.Core.DataAccess;
using Unsplash.Core.Services;
using Unsplash.Core.Services.Interfaces;
using Unsplash.Core.Models;

namespace Unsplash.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var baseurl = Configuration["BaseFixerUrl"];
            var jwtSecret = Configuration["JwtSettings:Secret"];
            //TODo: datalayer
            

            
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ICategoryService, CategoryService>();

            #region Auth/Auth Setup

            services.AddAppAuthentication(jwtSecret);
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AuthorizedAdmin", policy =>

                policy.RequireRole(UserRoles.Admin));

                opt.AddPolicy("AuthorizedUser", policy =>

                policy.RequireRole(UserRoles.AppUser));

            });

            #endregion
            services.AddHttpContextAccessor();
            // Adding CORS allowing all origin for development purposes
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Unsplash.Api", Version = "v1" });
            });
            services.ConfigureMySqlContext(Configuration);
            services.AddControllers(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Unsplash.Api v1"));
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
