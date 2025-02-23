using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Xml;

namespace LeaveManagementSystem.Web.Data
{
    /// <summary>
    /// Izvedena klasa iz bazne klase IdentityDbContext.cs
    /// Pristup toj baznoj klasi imamo zahvaljujući instalaciji odgovarajućeg NuGet paketa
    /// Nasljeđujemo od IdentityDbContext klase zato što samo za Authentification type odabrali 'Individual accounts'
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// Custom konstruktor koji prosljeđuje ulazni parametar konstruktoru bazne kase
        /// </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Ne znam zašto mi ne prolazi database-update za migraciju ispod (seeding roles and users)
        /// Tekst greške koji se javlja: The model for context 'ApplicationDbContext' changes each time it is built. This is usually caused by dynamic values used in a 'HasData' call
        /// Uglavnom, konfigurirala sam da sustav jednostavno ignorira tu grešku
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        /// <summary>
        /// Data-seeding proces postavljanja custom-uloga za korisnike (data-model: IdentityRole)
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Base-call ne smije obrisati:
            base.OnModelCreating(builder);

            //Kôd koji omogućuje data-seeding proces za dodavanje inicijalnih uloga:
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "6b64a0a3-b68f-4222-9244-4e70dd5a681d", //DbContext dozvoljava INSERT id-eva zato što se radi o data-seeding procesu
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new IdentityRole
                {
                    Id = "42a465c4-e747-4314-8c06-e7f0d0efff98", //korišten je GUID generator
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"
                },
                new IdentityRole
                {
                    Id = "939a4b2f-fa07-46c3-b7d1-538e247a5055", //korišten je GUID generator
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
             );

            //NE RADI korištenje hashera zato što javlja grešku:
            //"The model for context 'ApplicationDbContext' changes each time it is built. This is usually caused by dynamic values used in a 'HasData' call"
            //var hasher = new PasswordHasher<IdentityUser>();
            //var lozinka = hasher.HashPassword(null, "Password1@");

            //Data-seeding proces sa postavljanje inicijalnog korisnika (insanca klase IdentityUser):
            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "a0c89e17-6dae-4d39-b35c-0d54a8dad74e",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    UserName = "admin@localhost.com",
                    PasswordHash = "AQAAAAIAAYagAAAAEEr1h1mY9FjUylM1az+zXrrcHcPZ5g/NC/6j1smENaJgPqIjH1KPjoU0MepWy9lHxA==", // hardkodirani hash
                    EmailConfirmed = true
                }
            );

            //Konfiguracija: povezivanje defaultnog ADMIN usera sa ADMINISTRATOR ulogom
            builder.Entity<IdentityUserRole<string>>().HasData
            (
                new IdentityUserRole<string>
                {
                    RoleId = "939a4b2f-fa07-46c3-b7d1-538e247a5055", //id administrating uloge
                    UserId = "a0c89e17-6dae-4d39-b35c-0d54a8dad74e" //id admin korisnika
                }
            );
        }

        /// <summary>
        /// Property data tipa koji ima entity tipa LeaveType
        /// Kreiramo prikaz seta entiteta tipa LeaveType
        /// Naziv tablice u bazi: LeaveTypes (plural)
        /// </summary>
        public DbSet<LeaveType> LeaveTypes { get; set; }
    }
}
