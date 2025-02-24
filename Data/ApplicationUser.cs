using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Web.Data
{
    /// <summary>
    /// Data-model klasa koju ćemo koristiti za dodavanje dodatnih polja u tablicu AspNetUser
    /// Klasa je izvedena tako da naslijedi sva trenutna polja iz bazne klase
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Ime
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Prezime
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Datum rođenja
        /// </summary>
        public DateOnly DateOfBirth { get; set; }
    }
}
