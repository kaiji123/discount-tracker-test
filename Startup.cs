using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Serilog;



using MySql.Data;
using Microsoft.EntityFrameworkCore;
using System;


namespace CompressorBackend
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddControllers();
            services.AddMvc(); 
           

            services.AddDbContext<CompressContext>(options =>
            options.UseMySQL(Configuration.GetConnectionString("CompressMysqlConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<CompressContext>();
            services.AddHostedService<DiscountTrackingService>();
              
            services.AddHttpClient();
            services.AddAuthentication();
    
            services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.SlidingExpiration = true;
                });
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                // options.Password.RequireDigit = true;
                // options.Password.RequireLowercase = true;
                // options.Password.RequireNonAlphanumeric = true;
                // options.Password.RequireUppercase = true;
                // options.Password.RequiredLength = 6;
                // options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            // services.AddDbContext<CompressContext>(options =>
            // {
            //     options.UseSqlServer(Configuration.GetConnectionString("CompressDatabaseConnection"));
            // });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
            // app.MapControllerRoute(
            //     name: "default",
            //     pattern: "{controller=Home}/{action=Index}/{id?}");
            
            app.UseEndpoints(endpoints =>
            {   
                endpoints.MapRazorPages(); // this is for identity
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                });
            

                
            // Log.CloseAndFlush(); 
        }
        
    }
}
