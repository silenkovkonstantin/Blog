using AutoMapper;
using Blog.Data;
using Blog.Data.Repository;
using Blog.Extensions;
using Blog.Models.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog
{
    public class Startup
    {
        static IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //string connection = Configuration.GetConnectionString("DefaultConnecton");
            var mapperConfig = new MapperConfiguration((v) =>
            {
                v.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper)
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data Source={_env.ContentRootPath}/BlogDb.db"))
                .AddUnitOfWork()
                .AddCustomRepository<Post, PostsRepository>()
                .AddCustomRepository<Comment, CommentsRepository>()
                .AddCustomRepository<Tag, TagsRepository>()
                .AddIdentity<User, IdentityRole>(opts =>
                {
                    opts.Password.RequiredLength = 5;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireDigit = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    //opts.ClaimsIdentit
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(options => options.DefaultScheme = "Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = RedirectContext =>
                        {
                            RedirectContext.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
