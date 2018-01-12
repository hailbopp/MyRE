using System.IO;
using MyRE.Data;
using MyRE.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRE.Core.Models.Data;
using MyRE.Core.Repositories;
using MyRE.Core.Services;
using MyRE.Data.Repositories;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace MyRE.Web
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
            services.AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info() {Title = "MyRE", Version = "v1"}); });

            services.AddDbContext<MyREContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => {}));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyREContext>()
                .AddDefaultTokenProviders();

            // IoC Binding
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAppInstanceRepository, AppInstanceRepository>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IProjectService, ProjectService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, MyREContext db)
        {
            if (env.IsDevelopment() || true)
            {
                app.UseDeveloperExceptionPage();
            }

            if (Configuration.GetValue<bool>("MigrateDatabaseToLatestVersionOnAppStart"))
            {
                db.Database.Migrate();
            }


            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyRE API v1"); });

            app.Use(async (context, next) => {
                await next();

                // This feels a bit dirty. Normally I'd be doing this through nginx, but... whatever.
                // The point here is to redirect non-API requests back to the client application for front-end HTML5 routing.
                if (!context.Request.Path.ToString().StartsWith("/api") && !context.Request.Path.ToString().StartsWith("/swagger") && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
