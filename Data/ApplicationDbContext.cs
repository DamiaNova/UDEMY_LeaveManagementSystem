using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

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
        /// Property data tipa koji ima entity tipa LeaveType
        /// Kreiramo prikaz seta entiteta tipa LeaveType
        /// Naziv tablice u bazi: LeaveTypes (plural)
        /// </summary>
        public DbSet<LeaveType> LeaveTypes { get; set; }
    }
}
