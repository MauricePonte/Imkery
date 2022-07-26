using FluentValidation;
using Imkery.Data.Storage;
using Imkery.Data.Storage.Core;
using Imkery.Entities;
using Imkery.Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Imkery.Server.Data;
using Imkery.Server;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
})
   .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<ImkeryDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ImkerySqlConnection")));

builder.Services.AddImkeryRepositories();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
    {
        options.IdentityResources["openid"].UserClaims.Add("role");
        options.ApiResources.Single().UserClaims.Add("role");
    })
    .AddProfileService<ProfileService>();
builder.Services.AddAuthentication()
    .AddIdentityServerJwt();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IImkeryUserProvider, ImkeryUserProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    // Just fix with pull and re-comment when pushing :)
    var databaseContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    //databaseContext?.Database.EnsureDeleted();
    databaseContext?.Database.EnsureCreated();
    //databaseContext?.Database.Migrate();

    var databaseContextImkery = scope.ServiceProvider.GetService<ImkeryDbContext>();
    //databaseContextImkery?.Database.EnsureDeleted();
    databaseContextImkery?.Database.EnsureCreated();
    //databaseContextImkery?.Database.Migrate();
}

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
