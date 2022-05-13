using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Services;
using AspNetCoreTodo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// builder.Services.AddSingleton<ITodoItemService, TodoItemService>();
builder.Services.AddScoped<ITodoItemService, TodoItemService>();


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


{
    // var testUsers = new List<ApplicationUser>
    // {
    //     new ApplicationUser { 
    //         Id = "agdasfda1", 
    //         // FirstName = "Admin", 
    //         // LastName = "User", 
    //         Email = "User@mail.com", 
    //         // Username = "admin", 
    //         PasswordHash = BCryptNet.HashPassword("admin"), 
    //         Role = Role.Admin 
    //     },
    //     new ApplicationUser { 
    //         Id = "2sagsdafasf", 
    //         // FirstName = "Normal", 
    //         // LastName = "User", 
    //         Email = "User2@mail.com", 
    //         // Username = "user", 
    //         PasswordHash = BCryptNet.HashPassword("user"), 
    //         Role = Role.User 
    //     }
    // };
    // var roleManager = services
    //             .GetRequiredService<RoleManager<IdentityRole>>();
    // testAdmin = new ApplicationUser
    // {
    //     UserName = "admin@todo.local",
    //     Email = "admin@todo.local"
    // };
    // await userManager.CreateAsync(
    //     testAdmin, "NotSecure123!!");
    // await userManager.AddToRoleAsync(
    //     testAdmin, Role.Admin);

    SeedData.InitializeAsync(app);


    // using var scope = app.Services.CreateScope();
    // var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();





    // dataContext.Users.AddRange(testUsers);
    // dataContext.SaveChanges();
}


app.Run();
