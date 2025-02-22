using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    /// <summary>
    /// BAZNA apstraktna view-model klasa za rad s viewovima koji koriste data-model LeaveType.cs
    /// </summary>
    public abstract class BaseLeaveTypeVM
    {
        #region Properties
        /// <summary>
        /// Primary key: ID
        /// </summary>
        [Required]
        public int Id { get; set; }
        #endregion
    }
}
