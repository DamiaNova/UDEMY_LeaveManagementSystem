using LeaveManagementSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    /// <summary>
    /// View-model izvedena klasa koja se koristi SAMO za prikaz podataka iz data-modela
    /// </summary>
    public class LeaveTypeReadOnlyVM : BaseLeaveTypeVM
    {
        #region Properties
        /// <summary>
        /// Name of the leave type
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Number of days that the leave type has
        /// </summary>
        [Display(Name = "Maximum allocation of days")]
        public int Days { get; set; }
        #endregion
    }
}
