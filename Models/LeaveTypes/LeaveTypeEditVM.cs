﻿using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    /// <summary>
    /// View-model klasa za rad sa Edit viewom
    /// </summary>
    public class LeaveTypeEditVM : BaseLeaveTypeVM
    {
        #region Properties
        /// <summary>
        /// Name of the leave type
        /// </summary>
        [Required] //oznaka za obavezno polje
        [Length(5, 150, ErrorMessage = "Duljina naziva kategorije nije valjana!")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Number of days that the leave type has
        /// </summary>
        [Required] //oznaka za obavezno polje
        [Range(1, 90)] //min: 1 dan, max: 90 dana
        [Display(Name = "Maximum allocation of days")]
        public int NumberOfDays { get; set; }
        #endregion
    }
}
