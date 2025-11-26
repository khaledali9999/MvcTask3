using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MvcTask3.DataAccses;
using MvcTask3.Models;
using MvcTask3.Repos;
using MvcTask3.Utilites;

namespace ECommerce
{
    public static class AppConfiguration
    {
        public static void RegisterConfig(this IServiceCollection services, string connection)
        {
            services.AddDbContext<ApplicationDbContext>(option =>
            {
                //option.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
                //option.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
                option.UseSqlServer(connection);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.User.RequireUniqueEmail = true;
                option.Password.RequiredLength = 8;
                option.Password.RequireNonAlphanumeric = false;
                option.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login"; // Default login path
                options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Default access denied path
            });

            //services.AddTransient<IEmailSender, EmailSender>();

            //services.AddScoped<IRepository<Category>, Repository<Category>>();
            //services.AddScoped<IRepository<Cinema>, Repository<Cinema>>();
            //services.AddScoped<IRepository<Movie>, Repository<Movie>>();
            ////services.AddScoped<IRepository<ProductSubImage>, Repository<ProductSubImage>>();
            ////services.AddScoped<IMovieRepository, MovieRepository>();
            ////services.AddScoped<IProductColorRepository, ProductColorRepository>();
            ////services.AddScoped<IRepository<ApplicationUserOTP>, Repository<ApplicationUserOTP>>();
            //services.AddScoped<IRepository<Cart>, Repository<Cart>>();
            ////services.AddScoped<IRepository<Promotion>, Repository<Promotion>>();

            ////services.AddScoped<IdBInitializer, DBInItializer>();
        }
    }
}
