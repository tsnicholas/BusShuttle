using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Service;
using Database.Service;
using Database;
namespace App;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddAuthorization(options => options.AddPolicy("IsActivated", 
            policyBuilder => policyBuilder.RequireClaim("activated", "True")));
        builder.Services.Configure<IdentityOptions>(options => {
            options.User.RequireUniqueEmail = true;
        });
        builder.Services.AddControllersWithViews();
        builder.Services.AddScoped<UserManager<IdentityUser>>();
        builder.Services.AddScoped<BusShuttleContext>();
        builder.Services.AddScoped<IDatabaseService, DatabaseService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );
        app.MapControllerRoute(
            name: "Bus Manager",
            pattern: "{controller}/{action=Index}/{id?}",
            defaults: new { controller = "BusManager"}
        );
        app.MapControllerRoute(
            name: "Driver Manager",
            pattern: "{controller=DriverManager}/{action=Index}/{id?}",
            defaults: new { controller = "DriverManager"}
        );
        app.MapControllerRoute(
            name: "Stop Manager",
            pattern: "{controller=StopManager}/{action=Index}/{id?}",
            defaults: new { controller = "StopManager"}
        );
        app.MapControllerRoute(
            name: "Route Manager",
            pattern: "{controller=RouteManager}/{action=Index}/{id?}",
            defaults: new { controller = "RouteManager"}
        );
        app.MapControllerRoute(
            name: "Loop Manager",
            pattern: "{controller=LoopManager}/{action=Index}/{id?}",
            defaults: new { controller = "LoopManager"}
        );
        app.MapControllerRoute(
            name: "Entry Manager",
            pattern: "{controller=EntryManager}/{action=Index}/{id?}",
            defaults: new { controller = "EntryManager"}
        );
        app.MapControllerRoute(
            name: "Loop Driver",
            pattern: "{controller}/{action=Index}",
            defaults: new { controller = "Driver"}
        );
        app.MapRazorPages();
        
        // Seed the roles
        using(var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new []{"Manager", "Driver"};
            foreach(var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        app.Run();
    }
}
