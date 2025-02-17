using LeaveManagementSystem.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

//Program.cs konfigurira sve postavke koje su potrebne za pokretanje aplikacije!
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

//MIDDLEWARE:
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

//Koristimo routing:
app.UseHttpsRedirection();
app.UseRouting();

//Dodajemo autorizaciju:
app.UseAuthorization();

//Ovo nam omogućava insertanje skripti unutar Razor stranica iz wwwroot-a
app.MapStaticAssets();

//ROUTING MEHANIZAM:
app.MapControllerRoute(
    name: "default", //'default' je jedna routing konfiguracija
    pattern: "{controller=Home}/{action=Index}/{id?}") //u patternu je definirano da će Controller imati ime prema kojem onda tražimo Action
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

//Pokretanje aplikacije:
app.Run();
