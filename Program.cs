using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

//Program.cs konfigurira sve postavke koje su potrebne za pokretanje aplikacije!
var builder = WebApplication.CreateBuilder(args);

//ConnectionString 'DefaultConnection' je definiran u appsettings.json fileu:
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//DEPENDENCY INJECTION CONTAINER:
//Dohvaćanje DatabaseContexta --> klase koja sadrži model naše baze podataka:
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Dodavanje Servicea za Automapper (Assembly.GetExecutingAssembly() pretražuje cijeli projekt za Automapper profilom):
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//Registracija service-layera za tablicu LEAVE_TYPE (prvo: contract, drugo: implementation)
builder.Services.AddScoped<ILeaveTypesService, LeaveTypesService>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//Svi services moraju biti postavljeni prije izvođenja ove linije koda:
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
