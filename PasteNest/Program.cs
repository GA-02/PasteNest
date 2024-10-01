using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using PasteNest.Areas.Identity.Data;
using PasteNest.Data;
using Microsoft.IdentityModel.Tokens;
using PasteNest.Middleware;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PasteNestContextConnection") ?? throw new InvalidOperationException("Connection string 'PasteNestContextConnection' not found.");

builder.Services.AddDbContext<PasteNestContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<PasteNestUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedAccount = true;
}).AddEntityFrameworkStores<PasteNestContext>();

builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
});

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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Connect Middleware
app.UseMiddleware<AuthMiddleware>();

app.Run();
