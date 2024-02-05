using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApi;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme)
    .AddCookie(IdentityConstants.ApplicationScheme, o => { o.Cookie.Name = "accessToken"; });

builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<AccountDbContent>(x => x.UseSqlite("DataSource=app.db"));

builder.Services
    .AddIdentityCore<User>()
    .AddTokenProvider<PasswordlessTokenProvider>(PasswordlessTokenProvider.ProviderName)
    .AddApiEndpoints();

builder.Services.AddTransient<IUserStore<User>, UserStore>();
builder.Services.AddTransient<IUserEmailStore<User>, UserStore>();


var app = builder.Build();

app.MapControllers();

app.MapGet("/", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}")
    .RequireAuthorization();

app.Run();


public class AccountDbContent(DbContextOptions<AccountDbContent> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("MyUsers");
    }
}