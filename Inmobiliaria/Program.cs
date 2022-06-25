using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Data;
using Inmobiliaria.Services;
using Inmobiliaria.Models;
// using Inmobiliaria.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>{
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddScoped<IBackupRestore, BackupRestore>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

SeedData.InitializeAsync(app);

// Digito verificador
app.Use(async (context, next) => {
    using var scope = app.Services.CreateScope();
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    string path = context.Request.Path.ToString();
    if (
        !path.Equals("/ManageUsers/DatabaseCorruption") &&
        !path.Equals("/Identity/Account/AccessDenied") &&
        !path.Equals("/Identity/Account/Login") &&
        !path.Equals("/ManageUsers/DatabaseCorruptionPost") &&
        !path.Equals("/Identity/Account/Logout") &&
        new CheckDigitService(dataContext).DbCorrupta()
    ) {
        context.Response.Redirect("/ManageUsers/DatabaseCorruption");
    } else {
        await next.Invoke();
    }
});


app.Run();
