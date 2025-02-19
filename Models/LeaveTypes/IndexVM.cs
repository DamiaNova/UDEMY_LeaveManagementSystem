using LeaveManagementSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    /// <summary>
    /// View-model klasa koja se koristi isključivo za Index view model
    /// </summary>
    public class IndexVM
    {
        #region Properties
        /// <summary>
        /// Primary key: ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the leave type
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Number of days that the leave type has
        /// </summary>
        public int Days { get; set; }
        #endregion
    }
}
