using BLL.Configuration;
using lab_3.Data;
using lab_3.Models;
using lab_3.Profiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace lab_3
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<UserDbContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("RailwayTicketingEF")));

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                
                options.User.AllowedUserNameCharacters = null;
            }).AddEntityFrameworkStores<UserDbContext>() // использует UserDbContext дл€ хранени€ пользователей и ролей
            .AddDefaultTokenProviders(); // добавл€ет токены дл€ сброса парол€

            // доступ ко всем страницам будет требовать авторизации
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
            });

            // маршрут дл€ перенаправлени€ по умолчанию
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Access/Index";
            });

            // дл€ tag helper'а
            //builder.Services.AddHttpContextAccessor();

            // Add services
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Middleware дл€ аутентификации и авторизации
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // routing setting
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            // db initialize
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userContext = services.GetRequiredService<UserDbContext>();
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await DbInitializer.InitializeAsync(userContext, userManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "ќшибка при инициализации базы данных.");
                }
            }

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string efConnectionString = configuration.GetConnectionString("RailwayTicketingEF");
            services.ConfigureEfServices(); // config dal for ef
            services.ConfigureBLL(efConnectionString);

            // добавление профилей дл€ automapper
            services.AddAutoMapper(
                typeof(PassengerProfile),
                typeof(TicketProfile),
                typeof(TrainProfile),
                typeof(UserProfile)
            );
        }
    }
}
