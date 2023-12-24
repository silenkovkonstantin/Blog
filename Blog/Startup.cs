using AutoMapper;
using Blog.Data;
using Blog.Data.Repository;
using Blog.Extensions;
using Blog.Data.Models.Db;
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
            services.AddControllersWithViews();

            //string connection = Configuration.GetConnectionString("DefaultConnecton");
            var mapperConfig = new MapperConfiguration((v) =>
            {
                v.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite($"Data Source={_env.ContentRootPath}/BlogDb.db"));
            services.AddUnitOfWork();
            services.AddCustomRepository<Post, PostsRepository>();
            services.AddCustomRepository<Comment, CommentsRepository>();
            services.AddCustomRepository<Tag, TagsRepository>();
            services.AddIdentity<User, Role>(opts =>
                {
                    opts.Password.RequiredLength = 5;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireDigit = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.SignIn.RequireConfirmedAccount = false;
                    opts.SignIn.RequireConfirmedEmail = false;
                    //opts.ClaimsIdentit
                })
                //.AddRoles<IdentityRole>()
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
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
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
