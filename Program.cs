using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        var connectionString = builder.Configuration.GetConnectionString("Default");

        builder.Services.AddDbContext<ECommContext>(options =>

        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        });
        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ECommContext>()
        .AddDefaultTokenProviders();
        var jwt = builder.Configuration.GetSection("JWT");
        var secretKey = jwt["SECRET_KEY"];
        var issuer = jwt["Issuer"];
        var audience = jwt["Audience"];

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(Options =>
        {
            Options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "JWT:Issuer",
                ValidAudience = "JWT:Audience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt[secretKey]))
            };
        }
        );

        // Add services to the container.
        builder.Services.AddControllersWithViews();


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

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}